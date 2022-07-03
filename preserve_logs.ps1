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
Set-Location ..

$logsDir = [System.IO.Path]::Combine($(Get-Location).Path, "logs")

if (!(Test-Path -Path $logsDir -PathType Container)) {
    New-Item -ItemType Directory -Path $logsDir
}

Set-Location $logsDir
$logsDir = $(Get-Location).Path

# get csv

Set-Location $workingDir

$extensions = @('.log')

Get-ChildItem -File -Recurse | ForEach-Object {
    $fileInfo = [System.IO.FileInfo]::new("\\?\$($_.FullName)")
    
    if ($fileInfo.FullName.Contains("BenchmarkDotNet.Artifacts") -and $extensions.Contains($fileInfo.Extension)) {
        $newPath = [System.IO.Path]::Combine($logsDir, $fileInfo.Name)

        $fileInfo.CopyTo($newPath, $true)
    }
}

Set-Location $workingDir
