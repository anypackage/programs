// Copyright (c) Thomas Nieto - All Rights Reserved
// You may use, distribute and modify this code under the
// terms of the MIT license.

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;

namespace AnyPackage.Provider.Programs
{
    [PackageProvider("Programs")]
    public sealed class ProgramsProvider : PackageProvider, IGetPackage, IUninstallPackage
    {
        private const string _uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        private const string _uninstallString = "UninstallString";
        private const string _quietUninstallString = "QuietUninstallString";

        protected override object? GetDynamicParameters(string commandName)
        {
            switch (commandName)
            {
                case "Get-Package": return new GetPackageDynamicParameters();
                default: return null;
            }
        }

        public void GetPackage(PackageRequest request)
        {
            if (Environment.Is64BitOperatingSystem)
            {
                using var hklm64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                                              .OpenSubKey(_uninstallKey);
                GetPackage(hklm64, request);
            }

            using var hklm32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
                                          .OpenSubKey(_uninstallKey);
            GetPackage(hklm32, request);

            using var hkcu = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default)
                                        .OpenSubKey(_uninstallKey);
            GetPackage(hkcu, request);
        }

        public void UninstallPackage(PackageRequest request)
        {
            if (request.Package is not null)
            {
                UninstallPackage(request.Package, request);
            }
            else
            {
                using var powershell = PowerShell.Create();
                powershell.AddCommand("Get-Package").AddParameter("Name", request.Name);

                if (request.Version is not null)
                {
                    powershell.AddParameter("Version", request.Version);
                }

                foreach (var package in powershell.Invoke<PackageInfo>())
                {
                    UninstallPackage(package, request);
                }
            }
        }

        private void GetPackage(RegistryKey key, PackageRequest request)
        {
            var dynamicParameters = request.DynamicParameters as GetPackageDynamicParameters;

            foreach (var subKeyName in key.GetSubKeyNames())
            {
                Dictionary<string, object> keyValues = new Dictionary<string, object>();
                var subKey = key.OpenSubKey(subKeyName);

                if (subKey is null) { continue; }

                foreach (var subKeyValueName in subKey.GetValueNames())
                {
                    keyValues.Add(subKeyValueName, subKey.GetValue(subKeyValueName).ToString());
                }

                if (keyValues.ContainsKey("SystemComponent")
                    && keyValues["SystemComponent"].ToString() == "1"
                    && (dynamicParameters is not null
                    && !dynamicParameters.SystemComponent
                    || dynamicParameters is null))
                {
                    continue;
                }

                if (!keyValues.ContainsKey("DisplayName")
                    || string.IsNullOrWhiteSpace(keyValues["DisplayName"].ToString()))
                {
                    continue;
                }

                var name = keyValues["DisplayName"].ToString();

                PackageSourceInfo? source;
                if (keyValues.ContainsKey("InstallLocation")
                    && !string.IsNullOrWhiteSpace(keyValues["InstallLocation"].ToString()))
                {
                    source = new PackageSourceInfo((string)keyValues["InstallLocation"],
                                                   (string)keyValues["InstallLocation"],
                                                   ProviderInfo);
                }
                else
                {
                    source = null;
                }

                var comment = keyValues.ContainsKey("Comments") ? keyValues["Comments"].ToString() : "";

                PackageInfo package;
                if (keyValues.ContainsKey("DisplayVersion")
                    && request.IsMatch(name, (string)keyValues["DisplayVersion"]))
                {
                    package = new PackageInfo(name, (string)keyValues["DisplayVersion"], source, comment, null, keyValues, ProviderInfo);
                    request.WritePackage(package);
                }
                else if (!keyValues.ContainsKey("DisplayVersion")
                         && request.IsMatch(name)
                         && (request.Version is null || request.Version.ToString() == "*"))
                {
                    package = new PackageInfo(name, null, source, comment, null, keyValues, ProviderInfo);
                    request.WritePackage(package);
                }
            }
        }

        private void UninstallPackage(PackageInfo package, PackageRequest request)
        {
            string uninstallString;

            if (package.Metadata.ContainsKey(_quietUninstallString))
            {
                request.WriteVerbose("Quiet uninstall string found.");
                uninstallString = package.Metadata[_quietUninstallString].ToString();
            }
            else if (package.Metadata.ContainsKey(_uninstallString))
            {
                request.WriteVerbose("Uninstall string found.");
                uninstallString = package.Metadata[_uninstallString].ToString();
            }
            else
            {
                throw new InvalidOperationException($"Package '{package.Name}' with version '{package.Version} cannot find uninstall program.");
            }

            using var process = GetProcess(uninstallString);
            process.Start();
            process.WaitForExit();

            request.WritePackage(package);
        }

        private Process GetProcess(string text)
        {
            bool quoted, found;
            int i, position;

            quoted = found = false;
            i = position = 0;

            if (text[0] == '"')
            {
                quoted = true;
            }

            for (i = quoted ? 1 : 0; i < text.Length; i++)
            {
                if (text[i] == ' ' && !quoted)
                {
                    position = i;
                    found = true;
                    break;
                }
                else if (text[i] == '"' && quoted)
                {
                    position = i;
                    found = true;
                    break;
                }
            }

            var process = new Process();
            
            if (found && quoted)
            {
                process.StartInfo.FileName = text.Substring(0, position + 1).Replace("\"", "");
                process.StartInfo.Arguments = text.Substring(position + 1, text.Length - position - 1).Trim();
            }
            else if (found)
            {
                process.StartInfo.FileName = text.Substring(0, position);
                process.StartInfo.Arguments = text.Substring(position, text.Length - position).Trim();
            }
            else
            {
                process.StartInfo.FileName = text;
            }

            return process;
        }
    }
}
