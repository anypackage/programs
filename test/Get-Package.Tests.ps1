#Requires -Modules AnyPackage.Programs

Describe Get-Package {
    Context 'with no parameters' {
        It 'should return results' -Skip {
            Get-Package |
            Should -Not -BeNullOrEmpty
        }
    }

    Context 'with -Name parameter' {
        It 'should return <_> package' -ForEach '' -Skip {
            Get-Package -Name $_ |
            Should -Not -BeNullOrEmpty
        }
    }
}
