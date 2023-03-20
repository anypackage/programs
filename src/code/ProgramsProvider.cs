using System;

namespace AnyPackage.Provider.Programs
{
    [PackageProvider("Programs")]
    public sealed class ProgramsProvider : PackageProvider
    {
        private readonly static Guid s_id = new Guid("4100e661-4a03-4e2a-855a-b9d17ed18b46");

        public ProgramsProvider() : base(s_id) { }
    }
}
