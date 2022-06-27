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

# SqlServer
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.SqlServer
dotnet ef migrations add $name

# SqlLite
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.SqlLite
dotnet ef migrations add $name

# Postgres
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.Postgres
dotnet ef migrations add $name

# PomeloMySql
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.PomeloMySql
dotnet ef migrations add $name

# PomeloMariaDb
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.PomeloMariaDb
dotnet ef migrations add $name

# Cockroach
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.Cockroach
dotnet ef migrations add $name

# CockroachMulti
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.CockroachMulti
dotnet ef migrations add $name
