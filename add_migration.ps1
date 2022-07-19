param (
    [string]$name,
    [switch]$clear
)

$ErrorActionPreference = "Stop"

# Ensure that is being run from dir where script locates (helpful when running on remote machine)

$workingDir = $(Get-Location).Path

Write-Host "Working directory:" $workingDir
if ($workingDir -ne $PSScriptRoot) {
    Set-Location $PSScriptRoot
    Write-Host "Changed working directory to script root:" $(Get-Location).Path
}

$workingDir = $(Get-Location).Path

# script
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer

if ($clear) {
    Get-ChildItem "Migrations" -Directory -Recurse | Foreach-Object -ThrottleLimit 10 -Parallel {
        $item = $_

        $item | Remove-Item -Force -Recurse
    }
}

dotnet build

$migrations = @(
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.SqlServer', 
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.Oracle21',
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.Sqlite',
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.Postgres',
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.PomeloMySql',
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.Firebird3',
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.Firebird4',
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.PomeloMariaDb',
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.Cockroach', 
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.CockroachMulti'
)

$migrations | Foreach-Object -ThrottleLimit 10 -Parallel {
    $workingDir = $using:workingDir
    $name = $using:name
    $item = $_
    Set-Location $workingDir
    Set-Location $item
    dotnet ef migrations add $name --no-build
}
