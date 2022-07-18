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

function DisableProvider {
    $dir = $workingDir

    while ($null -ne $dir -And !(Test-Path([System.IO.Path]::Combine($dir, "environment.settings.json")))) {
        $di = [System.IO.DirectoryInfo]::new($dir)

        $dir = $di.Parent.FullName;
    }

    $path = [System.IO.Path]::Combine($dir, "environment.settings.json")

    $item = Get-Content $path | ConvertFrom-Json
    
    $providersData = $item.TEST_DISABLED_PROVIDERS

    $providers = $providersData.Split(";")

    $exists = $false

    foreach($provider in $providers) {
        if ($provider.Trim() -ne '' -And $workingDir.Contains($provider)) {
            $exists = $true

            Write-Host "$($provider) disabled"
        }
    }

    return $exists
}

if (DisableProvider) {
    Exit;
}

Write-Host 'Start container'
Write-Host 'docker compose --file cockroach_multi.yml up --detach'
docker compose --file cockroach_multi.yml up --detach
Start-Sleep -Seconds 5
Write-Host 'docker compose --file cockroach_multi.yml exec optimizerroachmulti11 /cockroach/cockroach init --insecure'
docker compose --file cockroach_multi.yml exec optimizerroachmulti11 /cockroach/cockroach init --insecure
Start-Sleep -Seconds 5
Write-Host 'Finished start container'
