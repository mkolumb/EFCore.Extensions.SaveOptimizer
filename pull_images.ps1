$images = @(
    'cockroachdb/cockroach:latest-v22.1',
    'jacobalberty/firebird:3.0',
    'jacobalberty/firebird:v4.0',
    'mariadb:10',
    'mysql:8-oracle',
    'postgres:14.4',
    'mcr.microsoft.com/mssql/server:2019-latest',
    'ghcr.io/mkolumb/oracledb_pre:11.2-xe',
    'ghcr.io/mkolumb/oracledb_pre:21.3-xe'
)

$images | ForEach-Object -ThrottleLimit 15 -Parallel {
    $img = $_

    Write-Host "Pulling $($img)"

    docker pull $img

    Write-Host "Done pulling $($img)"
}
