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

docker compose --file cockroach_multi.yml up --detach

Start-Sleep -Seconds 10

docker exec -it optimizerroachmulti11 ./cockroach init --insecure

Set-Location $workingDir

dotnet build -c release

dotnet run -c release

Set-Location ..

Set-Location "Containers"

docker compose --file cockroach.yml down

Set-Location $workingDir
