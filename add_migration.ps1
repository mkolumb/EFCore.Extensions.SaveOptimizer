param (
    [string]$name
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
dotnet build

# SqlServer
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.SqlServer
dotnet ef migrations add $name --no-build

# Oracle
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.Oracle
dotnet ef migrations add $name --no-build

# Sqlite
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.Sqlite
dotnet ef migrations add $name --no-build

# Postgres
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.Postgres
dotnet ef migrations add $name --no-build

# PomeloMySql
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.PomeloMySql
dotnet ef migrations add $name --no-build

# Firebird3
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.Firebird3
dotnet ef migrations add $name --no-build

# Firebird4
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.Firebird4
dotnet ef migrations add $name --no-build

# PomeloMariaDb
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.PomeloMariaDb
dotnet ef migrations add $name --no-build

# Cockroach
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.Cockroach
dotnet ef migrations add $name --no-build

# CockroachMulti
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Model.CockroachMulti
dotnet ef migrations add $name --no-build
