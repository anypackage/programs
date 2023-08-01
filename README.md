# AnyPackage.Programs

[![gallery-image]][gallery-site]
[![build-image]][build-site]
[![cf-image]][cf-site]

[gallery-image]: https://img.shields.io/powershellgallery/dt/AnyPackage.Programs
[build-image]: https://img.shields.io/github/actions/workflow/status/anypackage/programs/ci.yml
[cf-image]: https://img.shields.io/codefactor/grade/github/anypackage/programs
[gallery-site]: https://www.powershellgallery.com/packages/AnyPackage.Programs
[build-site]: https://github.com/anypackage/programs/actions/workflows/ci.yml
[cf-site]: https://www.codefactor.io/repository/github/anypackage/programs

AnyPackage.Programs is a Windows programs provider for AnyPackage.
It shows applications that appear in Add/Remove Programs.

## Install AnyPackage.Programs

```PowerShell
Install-PSResource AnyPackage.Programs
```

## Import AnyPackage.Programs

```PowerShell
Import-Module AnyPackage.Programs
```

## Sample usages

### Get list of installed packages

```PowerShell
Get-Package -Name *7zip*
```

### Uninstall package

```PowerShell
Get-Package -Name *7zip* | Uninstall-Package
```
