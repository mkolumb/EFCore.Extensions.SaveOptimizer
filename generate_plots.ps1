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

# solution
Set-Location $workingDir

$exportDir = [System.IO.Path]::Combine($workingDir, "results")

if (!(Test-Path -Path $exportDir -PathType Container)) {
    New-Item -ItemType Directory -Path $exportDir
}

Set-Location $exportDir
$exportDir = $(Get-Location).Path

# get csv

Set-Location $workingDir

$extensions = @('.csv', '.md')

Get-ChildItem -File -Recurse | ForEach-Object {
    $fileInfo = [System.IO.FileInfo]::new("\\?\$($_.FullName)")
    
    if ($fileInfo.FullName.Contains("BenchmarkDotNet.Artifacts") -and $extensions.Contains($fileInfo.Extension)) {
        $newPath = [System.IO.Path]::Combine($exportDir, $fileInfo.Name)

        $newPath = $newPath.Replace("EFCore.Extensions.SaveOptimizer.", "")
        
        $newPath = $newPath.Replace("Benchmark.Standard.", "")
        
        $newPath = $newPath.Replace("Benchmark-", "-")

        $fileInfo.CopyTo($newPath, $true)
    }
}

Set-Location $workingDir

# fix measurements
Set-Location $exportDir
Get-ChildItem -Filter "*report.csv" | Remove-Item

$regex = 'Variant=([A-z]*)&Rows=([0-9]*);'
$replacer = '$1;$2;$1 ($2);'

$titleRegex = '([A-z]*)Benchmark\.([A-z]*)Async;EFCore\.Extensions\.SaveOptimizer\.([A-z]*)\.Benchmark\.Standard;([A-z]*)Benchmark;([A-z]*)Async'
$titleReplacer = '$1;$3;$1;$1'

Get-ChildItem -Filter "*measurements.csv" | ForEach-Object {
    $item = $_

    (Get-Content $item.FullName) `
        -replace $regex, $replacer `
        -replace $titleRegex, $titleReplacer `
        -replace "Job_Display;Params", "Job_Display;SaveVariant;SavedRows;Params" `
        -replace "OptimizedDapper", "Optimized Dapper" `
        -replace "EfCore", "EF Core" |
    Out-File $item.FullName -Encoding ascii
}

Get-ChildItem -Filter "*report-*.md" | ForEach-Object {
    $item = $_

    $newName = $item.FullName.Replace(".md", ".csv")

    $content = (Get-Content $item.FullName) `
        -replace "OptimizedDapper", "Optimized Dapper" `
        -replace "EfCore", "EF Core";

    $newContent = [System.Collections.Generic.List[string]]::new()

    $content | ForEach-Object {
        $line = $_

        if ($line.StartsWith("|") -and !$line.StartsWith("|-")) {
            $lineBuilder = [System.Text.StringBuilder]::new()

            $line.Split("|") | ForEach-Object {
                $column = $_

                $column = $column.Replace("*", "").Replace("-", "").Replace("Async", "").Trim()
                
                if ($column.EndsWith("ns")) {
                    $sValue = $column.Replace("ns", "").Replace(",", "").Replace(".", ",").Trim();

                    $value = [System.Decimal]::Parse($sValue)

                    $column = $value.ToString("F0")
                }
                elseif ($column.EndsWith("μs")) {
                    $sValue = $column.Replace("μs", "").Replace(",", "").Replace(".", ",").Trim();

                    $value = [System.Decimal]::Parse($sValue)

                    $value = $value *= 1000;

                    $column = $value.ToString("F0")
                }
                elseif ($column.EndsWith("ms")) {
                    $sValue = $column.Replace("ms", "").Replace(",", "").Replace(".", ",").Trim();

                    $value = [System.Decimal]::Parse($sValue)

                    $value = $value *= 1000;

                    $value = $value *= 1000;

                    $column = $value.ToString("F0")
                }
                elseif ($column.EndsWith("sec")) {
                    $sValue = $column.Replace("sec", "").Replace(",", "").Replace(".", ",").Trim();

                    $value = [System.Decimal]::Parse($sValue)

                    $value = $value *= 1000;

                    $value = $value *= 1000;

                    $value = $value *= 1000;

                    $column = $value.ToString("F0")
                }

                if ($column.Length -gt 0) {
                    $lineBuilder = $lineBuilder.Append($column)
                    $lineBuilder = $lineBuilder.Append(";")
                }
            }

            $newLine = $lineBuilder.ToString()

            if (!$newLine.Contains("NA;")) {
                $newContent.Add($newLine)
            }
        }
    }

    $newContent | Out-File $newName -Encoding utf8
}

Get-ChildItem -Filter "*report-*.md" | ForEach-Object {
    $item = $_

    (Get-Content $item.FullName) `
        -replace "Async", "" `
        -replace "OptimizedDapper", "Optimized Dapper" `
        -replace "EfCore", "EF Core" |
    Out-File $item.FullName -Encoding utf8
}

# generate plots
Set-Location $workingDir
Copy-Item -Path "BuildPlots.R" -Destination "$($exportDir)/BuildPlots.R"
Set-Location $exportDir
Rscript.exe BuildPlots.R 

# cleanup
Set-Location $exportDir
# Get-ChildItem -Filter "*.csv" | Remove-Item
Get-ChildItem -Filter "*.R" | Remove-Item
