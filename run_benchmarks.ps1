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

$exportDir = [System.IO.Path]::Combine($workingDir, "export")

if (!(Test-Path -Path $exportDir -PathType Container)) {
    New-Item -ItemType Directory -Path $exportDir
}

Set-Location $exportDir
$exportDir = $(Get-Location).Path

# Postgres
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Postgres.Benchmark
.\benchmark.ps1 -ExportDir $exportDir

# PomeloMySql
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.PomeloMySql.Benchmark
.\benchmark.ps1 -ExportDir $exportDir

# PomeloMariaDb
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.PomeloMariaDb.Benchmark
.\benchmark.ps1 -ExportDir $exportDir

# SqlLite
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.SqlLite.Benchmark
.\benchmark.ps1 -ExportDir $exportDir

# SqlServer
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.SqlServer.Benchmark
.\benchmark.ps1 -ExportDir $exportDir

# Oracle
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Oracle.Benchmark
.\benchmark.ps1 -ExportDir $exportDir

# Firebird3
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Firebird3.Benchmark
.\benchmark.ps1 -ExportDir $exportDir

# Firebird4
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Firebird4.Benchmark
.\benchmark.ps1 -ExportDir $exportDir

# Cockroach
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.Cockroach.Benchmark
.\benchmark.ps1 -ExportDir $exportDir

# Cockroach Multi
Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer\EFCore.Extensions.SaveOptimizer.CockroachMulti.Benchmark
.\benchmark.ps1 -ExportDir $exportDir

# get csv

Set-Location $workingDir

$extensions = @('.csv', '.md')

Get-ChildItem -File -Recurse | ForEach-Object {
    $fileInfo = [System.IO.FileInfo]::new("\\?\$($_.FullName)")
    
    if ($fileInfo.FullName.Contains("BenchmarkDotNet.Artifacts") -and $extensions.Contains($fileInfo.Extension)) {
        $newPath = [System.IO.Path]::Combine($exportDir, $fileInfo.Name)

        $newPath = $newPath.Replace("EFCore.Extensions.SaveOptimizer.", "")
        
        $newPath = $newPath.Replace("Benchmark.Standard.", "")
        
        $newPath = $newPath.Replace("Benchmark-", "-")

        $fileInfo.CopyTo($newPath, $true)
    }
}

Set-Location $workingDir

# fix measurements
Set-Location $exportDir
Get-ChildItem -Filter "*report.csv" | Remove-Item

$regex = 'Variant=([A-z]*)&Rows=([0-9]*);'
$replacer = '$1 ($2);'

$titleRegex = '([A-z]*)Benchmark\.([A-z]*)Async;EFCore\.Extensions\.SaveOptimizer\.([A-z]*)\.Benchmark\.Standard;([A-z]*)Benchmark;([A-z]*)Async'
$titleReplacer = '$1;$3;$1;$1'

Get-ChildItem -Filter "*measurements.csv" | ForEach-Object {
    $item = $_

    (Get-Content $item.FullName) `
        -replace $regex, $replacer `
        -replace $titleRegex, $titleReplacer |
    Out-File $item.FullName -Encoding ascii
}

# generate plots
Set-Location $workingDir
Copy-Item -Path "BuildPlots.R" -Destination "$($exportDir)/BuildPlots.R"
Set-Location $exportDir
Rscript.exe BuildPlots.R 

# cleanup
Set-Location $exportDir
Get-ChildItem -Filter "*.csv" | Remove-Item
Get-ChildItem -Filter "*.R" | Remove-Item

Set-Location $workingDir
Set-Location .\EFCore.Extensions.SaveOptimizer
git clean -fdX
