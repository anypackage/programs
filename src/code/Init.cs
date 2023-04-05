// Copyright (c) Thomas Nieto - All Rights Reserved
// You may use, distribute and modify this code under the
// terms of the MIT license.

using System;
using System.Management.Automation;
using static AnyPackage.Provider.PackageProviderManager;

namespace AnyPackage.Provider.Programs
{
    public sealed class Init : IModuleAssemblyInitializer, IModuleAssemblyCleanup
    {
        private readonly Guid _id = new Guid("4100e661-4a03-4e2a-855a-b9d17ed18b46");
        
        public void OnImport()
        {
            RegisterProvider(_id, typeof(ProgramsProvider), "AnyPackage.Programs");
        }

        public void OnRemove(PSModuleInfo psModuleInfo)
        {
            UnregisterProvider(_id);
        }
    }
}
