param (
    [string]$ExportDir
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

Set-Location ..

Set-Location "Containers"

docker compose --file sqlserver.yml up --detach

Set-Location $workingDir

dotnet build -c release

dotnet run -c release

Set-Location ..

Set-Location "Containers"

docker compose --file sqlserver.yml down

Set-Location $workingDir
