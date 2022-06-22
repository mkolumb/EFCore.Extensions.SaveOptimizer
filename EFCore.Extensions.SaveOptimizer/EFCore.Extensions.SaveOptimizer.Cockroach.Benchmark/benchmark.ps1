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

Set-Location ..

Set-Location "Containers"

docker compose --file cockroach.yml up --detach

Set-Location $workingDir

dotnet build -c release

dotnet run -c release

Set-Location ..

Set-Location "Containers"

docker compose --file cockroach.yml down

Set-Location $workingDir
