@{
    RootModule = ''
    ModuleVersion = '0.1.0'
    CompatiblePSEditions = @('Desktop', 'Core')
    GUID = '84cf5334-85e0-4263-8471-60394099cefb'
    Author = 'Thomas Nieto'
    Copyright = '(c) 2023 Thomas Nieto. All rights reserved.'
    Description = 'Windows programs provider for AnyPackage.'
    PowerShellVersion = '5.1'
    RequiredModules = @(
        @{ ModuleName = 'AnyPackage'; ModuleVersion = '0.4.0' })
    FunctionsToExport = @()
    CmdletsToExport = @()
    AliasesToExport = @()
    PrivateData = @{
        PSData = @{
            Tags = @('AnyPackage', 'Provider', 'Programs', 'Windows')
            LicenseUri = 'https://github.com/AnyPackage/AnyPackage.Programs/blob/main/LICENSE'
            ProjectUri = 'https://github.com/AnyPackage/AnyPackage.Programs'
        }
    }
    HelpInfoURI = 'https://go.anypackage.dev/help'
}
