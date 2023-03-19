#Requires -Modules AnyPackage.Programs

Describe Uninstall-Package {
    Context 'with -Name parameter' {
        It 'should uninstall' -ForEach '' -Skip {
            { Uninstall-Package -Name $_ } |
            Should -Not -Throw
        }
    }

    Context 'with -Version parameter' { }
}
