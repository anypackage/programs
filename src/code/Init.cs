using System.Management.Automation;
using static AnyPackage.Provider.PackageProviderManager;

namespace AnyPackage.Provider.Programs
{
    public sealed class Init : IModuleAssemblyInitializer, IModuleAssemblyCleanup
    {
        public void OnImport()
        {
            RegisterProvider(typeof(ProgramsProvider), "AnyPackage.Programs");
        }

        public void OnRemove(PSModuleInfo psModuleInfo)
        {
            UnregisterProvider(typeof(ProgramsProvider));
        }
    }
}
