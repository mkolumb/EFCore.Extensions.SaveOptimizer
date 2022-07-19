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

$benchmarks = @(
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.SqlServer.Benchmark', 
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Oracle21.Benchmark',
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Sqlite.Benchmark',
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Postgres.Benchmark',
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.PomeloMySql.Benchmark',
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Firebird3.Benchmark',
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Firebird4.Benchmark',
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.PomeloMariaDb.Benchmark',
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Cockroach.Benchmark', 
    '.\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.CockroachMulti.Benchmark'
)

foreach($benchmark in $benchmarks) {
    # Run
    Set-Location $workingDir
    Set-Location $benchmark
    .\run.ps1
    
    # plots & logs
    Set-Location $workingDir
    .\preserve_logs.ps1
    .\generate_plots.ps1
}

# cleanup
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer
git clean -fdX
