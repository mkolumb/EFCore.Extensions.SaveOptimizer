param (
    [string]$name
)


Clear-Host

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

git clean -fdX

# Cockroach
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Cockroach.Benchmark
.\benchmark.ps1

# SqlLite
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.SqlLite.Benchmark
.\benchmark.ps1
