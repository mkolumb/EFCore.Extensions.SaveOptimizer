$images = @('cockroachdb/cockroach:latest-v22.1',
    'jacobalberty/firebird:3.0',
    'jacobalberty/firebird:v4.0',
    'mariadb:10',
    'mysql:8-oracle',
    'container-registry.oracle.com/database/express:latest',
    'postgres:14.4',
    'mcr.microsoft.com/mssql/server:2019-latest')

$images | ForEach-Object -ThrottleLimit 10 -Parallel {
    $img = $_

    Write-Host "Pulling $($img)"

    docker pull $img
}
