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

./remove_containers.ps1

# start all

Set-Location .\EFCore.Extensions.SaveOptimizer\Containers

docker compose --file cockroach.yml up --detach

docker compose --file cockroach_multi.yml up --detach

docker compose --file sqlserver.yml up --detach

docker compose --file postgres.yml up --detach

docker compose --file mysql_pomelo.yml up --detach

docker compose --file mariadb_pomelo.yml up --detach

docker compose --file firebird_3.yml up --detach

docker compose --file firebird_4.yml up --detach

docker compose --file oracle.yml up --detach

docker exec -it optimizerroachmulti11 ./cockroach init --insecure

Set-Location $workingDir
