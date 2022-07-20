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

$yamls = @(
    'cockroach.yml',
    'cockroach_multi.yml',
    'sqlserver.yml',
    'postgres.yml', 
    'mysql_pomelo.yml', 
    'mariadb_pomelo.yml', 
    'firebird_3.yml', 
    'firebird_4.yml', 
    'oracle_21.yml'
)

$ErrorActionPreference = 'SilentlyContinue'

$yamls | Foreach-Object -ThrottleLimit 15 -Parallel {
    $workingDir = $using:workingDir
    $item = $_
    
    Set-Location $workingDir
    Set-Location .\EFCore.Extensions.SaveOptimizer\Containers
    $cmd = "docker compose --file $($item) down"

    Write-Host $cmd

    cmd /c $cmd

    Write-Host "$($item) finished"
}

$ErrorActionPreference = "Stop"

docker volume prune --force

Start-Sleep -Seconds 5

Set-Location $workingDir
