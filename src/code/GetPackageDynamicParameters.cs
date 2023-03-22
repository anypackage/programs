// Copyright (c) Thomas Nieto - All Rights Reserved
// You may use, distribute and modify this code under the
// terms of the MIT license.

using System.Management.Automation;

namespace AnyPackage.Provider.Programs
{
    public sealed class GetPackageDynamicParameters
    {
        [Parameter]
        [Alias("IncludeSystemComponent")]
        public SwitchParameter SystemComponent { get; set; }

        [Parameter]
        [Alias("IncludeWindowsInstaller")]
        public SwitchParameter WindowsInstaller { get; set; }
    }
}
