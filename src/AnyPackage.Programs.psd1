@{
    RootModule = 'ProgramsProvider.dll'
    ModuleVersion = '0.2.2'
    CompatiblePSEditions = @('Desktop', 'Core')
    GUID = '84cf5334-85e0-4263-8471-60394099cefb'
    Author = 'Thomas Nieto'
    Copyright = '(c) 2023 Thomas Nieto. All rights reserved.'
    Description = 'Windows programs provider for AnyPackage.'
    PowerShellVersion = '5.1'
    RequiredModules = @(
        @{ ModuleName = 'AnyPackage'; ModuleVersion = '0.5.0' })
    FunctionsToExport = @()
    CmdletsToExport = @()
    AliasesToExport = @()
    PrivateData = @{
        AnyPackage = @{
            Providers = 'Programs'
        }
        PSData = @{
            Tags = @('AnyPackage', 'Provider', 'Programs', 'Windows')
            LicenseUri = 'https://github.com/anypackage/programs/blob/main/LICENSE'
            ProjectUri = 'https://github.com/anypackage/programs'
        }
    }
    HelpInfoURI = 'https://go.anypackage.dev/help'
}
