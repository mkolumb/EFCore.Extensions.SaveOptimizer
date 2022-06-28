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

# stop all

Set-Location .\EFCore.Extensions.SaveOptimizer\Containers

docker compose --file cockroach.yml down

docker compose --file cockroach_multi.yml down

docker compose --file sqlserver.yml down

docker compose --file postgres.yml down

docker compose --file mysql_pomelo.yml down

docker compose --file mariadb_pomelo.yml down

docker compose --file firebird_3.yml down

docker compose --file firebird_4.yml down

docker compose --file oracle.yml down

# solution
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer
dotnet build -c Release

# Postgres
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Postgres.Benchmark
.\benchmark.ps1

# PomeloMySql
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.PomeloMySql.Benchmark
.\benchmark.ps1

# PomeloMariaDb
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.PomeloMariaDb.Benchmark
.\benchmark.ps1

# SqlLite
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.SqlLite.Benchmark
.\benchmark.ps1

# SqlServer
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.SqlServer.Benchmark
.\benchmark.ps1

# Oracle
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Oracle.Benchmark
.\benchmark.ps1

# Firebird3
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Firebird3.Benchmark
.\benchmark.ps1

# Firebird4
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Firebird4.Benchmark
.\benchmark.ps1

# Cockroach
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Cockroach.Benchmark
.\benchmark.ps1

# Cockroach Multi
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.CockroachMulti.Benchmark
.\benchmark.ps1
