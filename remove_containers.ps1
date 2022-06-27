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

Set-Location .\EFCore.Extensions.SaveOptimizer\Containers

docker compose --file cockroach.yml down

docker compose --file cockroach_multi.yml down

docker compose --file sqlserver.yml down

docker compose --file postgres.yml down

docker compose --file mysql_pomelo.yml down

docker compose --file mariadb_pomelo.yml down

docker volume prune --force

Set-Location $workingDir
