# Copyright (c) Thomas Nieto - All Rights Reserved
# You may use, distribute and modify this code under the
# terms of the MIT license.

using module AnyPackage
using namespace AnyPackage.Provider

[PackageProvider('Programs')]
class ProgramsProvider : PackageProvider {
    ProgramsProvider() : base('0ff8ce6e-eead-493d-ad2d-2ff885dcecb6') { }
}

[PackageProviderManager]::RegisterProvider([ProgramsProvider], $MyInvocation.MyCommand.ScriptBlock.Module)

$MyInvocation.MyCommand.ScriptBlock.Module.OnRemove = {
    [PackageProviderManager]::UnregisterProvider([ProgramsProvider])
}
