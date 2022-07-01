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

Write-Host 'Stop container'
Write-Host 'docker compose --file mysql_pomelo.yml down'
docker compose --file mysql_pomelo.yml down
Start-Sleep -Seconds 5
docker volume prune --force
Start-Sleep -Seconds 5
Write-Host 'Finished stop container'
