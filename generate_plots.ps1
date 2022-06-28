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

# solution
Set-Location $workingDir

$exportDir = [System.IO.Path]::Combine($workingDir, "results")

if (!(Test-Path -Path $exportDir -PathType Container)) {
    New-Item -ItemType Directory -Path $exportDir
}

Set-Location $exportDir
$exportDir = $(Get-Location).Path

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
        -replace $titleRegex, $titleReplacer `
        -replace "OptimizedDapper", "Dapper" `
        -replace "Optimized", "Optimized" `
        -replace "EfCore", "EF Core" |
    Out-File $item.FullName -Encoding ascii
}

# generate plots
Set-Location $workingDir
Copy-Item -Path "BuildPlots.R" -Destination "$($exportDir)/BuildPlots.R"
Set-Location $exportDir
Rscript.exe BuildPlots.R 

# cleanup
Set-Location $exportDir
# Get-ChildItem -Filter "*.csv" | Remove-Item
Get-ChildItem -Filter "*.R" | Remove-Item
