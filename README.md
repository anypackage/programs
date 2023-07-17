# AnyPackage.Programs

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
