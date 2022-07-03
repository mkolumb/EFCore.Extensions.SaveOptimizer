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

# stop all

./remove_containers.ps1

# solution
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer
dotnet build -c Release

# Cockroach Multi
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.CockroachMulti.Benchmark
.\run.ps1

# plots & logs
Set-Location $workingDir
.\preserve_logs.ps1
.\generate_plots.ps1

# Cockroach
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Cockroach.Benchmark
.\run.ps1

# plots & logs
Set-Location $workingDir
.\preserve_logs.ps1
.\generate_plots.ps1

# Oracle
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Oracle.Benchmark
.\run.ps1

# plots & logs
Set-Location $workingDir
.\preserve_logs.ps1
.\generate_plots.ps1

# Firebird4
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Firebird4.Benchmark
.\run.ps1

# plots & logs
Set-Location $workingDir
.\preserve_logs.ps1
.\generate_plots.ps1

# Firebird3
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Firebird3.Benchmark
.\run.ps1

# plots & logs
Set-Location $workingDir
.\preserve_logs.ps1
.\generate_plots.ps1

# PomeloMySql
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.PomeloMySql.Benchmark
.\run.ps1

# plots & logs
Set-Location $workingDir
.\preserve_logs.ps1
.\generate_plots.ps1

# PomeloMariaDb
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.PomeloMariaDb.Benchmark
.\run.ps1

# plots & logs
Set-Location $workingDir
.\preserve_logs.ps1
.\generate_plots.ps1

# Postgres
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Postgres.Benchmark
.\run.ps1

# plots & logs
Set-Location $workingDir
.\preserve_logs.ps1
.\generate_plots.ps1

# Sqlite
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Sqlite.Benchmark
.\run.ps1

# plots & logs
Set-Location $workingDir
.\preserve_logs.ps1
.\generate_plots.ps1

# SqlServer
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.SqlServer.Benchmark
.\run.ps1

# plots & logs
Set-Location $workingDir
.\preserve_logs.ps1
.\generate_plots.ps1

# cleanup
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer
git clean -fdX
