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

Set-Location ..

Set-Location "Containers"

Write-Host 'Start container'
Write-Host 'docker compose --file mariadb_pomelo.yml up --detach'
docker compose --file mariadb_pomelo.yml up --detach
Start-Sleep -Seconds 5
Write-Host 'Finished start container'
