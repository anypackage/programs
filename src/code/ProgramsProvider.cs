// Copyright (c) Thomas Nieto - All Rights Reserved
// You may use, distribute and modify this code under the
// terms of the MIT license.

using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace AnyPackage.Provider.Programs
{
    [PackageProvider("Programs")]
    public sealed class ProgramsProvider : PackageProvider, IGetPackage
    {
        private readonly static Guid s_id = new Guid("4100e661-4a03-4e2a-855a-b9d17ed18b46");
        private const string _uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        public ProgramsProvider() : base(s_id) { }

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

        private void GetPackage(RegistryKey key, PackageRequest request)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            var dynamicParameters = request.DynamicParameters as GetPackageDynamicParameters;
            PackageSourceInfo? source;
            PackageVersion packageVersion;
            string name, version, location, comment;

            foreach (var subKeyName in key.GetSubKeyNames())
            {
                keyValues.Clear();
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

                if (keyValues.ContainsKey("WindowsInstaller")
                    && keyValues["WindowsInstaller"].ToString() == "1"
                    && (dynamicParameters is not null
                    && !dynamicParameters.WindowsInstaller
                    || dynamicParameters is null))
                {
                    continue;
                }

                if (!keyValues.ContainsKey("DisplayName")
                    || string.IsNullOrWhiteSpace(keyValues["DisplayName"].ToString()))
                {
                    continue;
                }

                name = keyValues["DisplayName"].ToString();

                if (keyValues.ContainsKey("InstallLocation")
                    && !string.IsNullOrWhiteSpace(keyValues["InstallLocation"].ToString()))
                {
                    location = keyValues["InstallLocation"].ToString();
                    source = request.NewSourceInfo(location, location);
                }
                else
                {
                    source = null;
                }

                if (keyValues.ContainsKey("DisplayVersion")
                    && !string.IsNullOrWhiteSpace(keyValues["DisplayVersion"].ToString()))
                {
                    version = keyValues["DisplayVersion"].ToString();
                }
                else
                {
                    request.WriteVerbose($"Package '{name}' does not have a version, changing to '0'.");
                    version = "0";
                }

                packageVersion = new PackageVersion(version);

                comment = keyValues.ContainsKey("Comments") ? keyValues["Comments"].ToString() : "";

                if (request.IsMatch(name, packageVersion))
                {
                    request.WritePackage(name,
                                     new PackageVersion(version),
                                     comment,
                                     source);
                }
            }
        }
    }
}
