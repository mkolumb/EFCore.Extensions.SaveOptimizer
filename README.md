# EFCore.Extensions.SaveOptimizer
Save optimizer extension for EF Core

## Why another library for batch save?

#### Short version
Because I needed

#### Long version
Idea came from one of my commercial projects. 

We were working with CockroachDB (excellent database for multi-region environments) and initially used Entity Framework Core 3.1. It worked fine, but in multi-region configuration save performance was not so good. 

Reason? As many people knows EF generates multiple INSERT / UPDATE / DELETE single row statements instead of lower amount of multiple row statements. 

I considered few solutions (e.g. [EFCore.BulkExtensions](https://github.com/borisdj/EFCore.BulkExtensions), [linq2db](https://linq2db.github.io)) but all of them had some disadvantages during this time. We needed something with support for CockroachDB and concurrency tokens. Also we want to avoid business logic rewrite, more code changes than replacing SaveChanges execution could be big problem. As there were no good choice I wrote something simple from scratch. Now I decided to rewrite this as something more integrated into EF. 

[EFCore.BulkExtensions](https://github.com/borisdj/EFCore.BulkExtensions) is good choice for most cases as it has BulkSaveChanges method and currently supports few different databases (SQL Server, PostgreSQL and SQLite). Unfortunately it looks like API they used for PostgreSQL is incompatible with CockroachDB.

Main difference is approach - [EFCore.BulkExtensions](https://github.com/borisdj/EFCore.BulkExtensions) uses copy tools / statements under the hood, SaveOptimizer uses batched INSERT / UPDATE / DELETE statements generated with [SqlKata](https://sqlkata.com/) help. You need to choose which would be better in your case. Likely copy would be faster in most cases (it's good to measure this), but SaveOptimizer approach would bring more databases support. SaveOptimizer is simple library so you should verify it with your requirements.

## How to use

Just replace SaveChanges() / SaveChangesAsync() :)

```csharp
await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
await context.AddAsync(entity);
await context.SaveChangesOptimizedAsync();
await transaction.RollbackAsync(cancellationToken);
```

## How it works

When you execute SaveChangesOptimized the following sequence happens:
1. Get entries from ChangeTracker
2. Build property changes dictionary for each entry
3. Group changes as much as possible
4. Generate SQL using SqlKata
5. ExecuteRawSql
   - *if there is no transaction started then will start serializable transaction*
6. Mark all saved entities as detached 

Please note it is not working exactly as SaveChanges, so you should verify it works in your case as expected. 

## Limitations

### Refresh data after save

SaveOptimizer approach makes almost impossible refresh data after save, it is on your side. 
I recommend to generate values for primary keys in code, not in db. 
This will make much easier refresh data after save if necessary, you will be able to use this values for query. 

## Features
- Providers support
  - SQL Server
  - SqlLite
  - PostgreSQL
  - Oracle
  - MySQL
  - Firebird
- Primary keys
  - Simple
  - Composed
- Statements
  - Insert
  - Update
  - Delete
- Other
  - Concurrency token

## What to do next

- Configuration
  - Concurrency token behavior
  - Batch size
  - Auto transaction behavior
- Support for
  - Different cultures
  - Interceptors
  - Value converters
  - Custom attributes
  - Ignore
  - Shadow properties
  - Tables without primary key
  - Auto-generated values
- Dependencies lifecycle optimizations
- Optimize data retrieval from Change Tracker
- GitHub actions
- Test databases within container

## Migration command

```cmd
powershell -Command ".\add_migration.ps1 -name [NAME]"
```

```powershell
$name = "[NAME]"

.\add_migration.ps1 -name $name
```

## Benchmarks

### Summary

SaveOptimizer is not always better than pure EF Core methods. EF Core contains tons of optimizations so for small operations and simple workloads likely is better. 
I noticed that range 1-100 for INSERT is unpredictable - almost each benchmark run produces different results. 

My advice is to compare results in your real environment. Honestly - the best choice for pure performance is leave EF Core for write operations at all and then write statements from scratch. But this library could bring serious performance benefits in many scenarios without big effort. 

All benchmarks uses serializable isolation level.

```
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.300
  [Host]     : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT

InvocationCount=10, IterationCount=10, LaunchCount=5, RunStrategy=Monitoring, UnrollFactor=1, WarmupCount=2
```

### Running

```cmd
powershell -File run_benchmarks.ps1
```

```powershell
.\run_benchmarks.ps1
```

### INSERT

#### SqlLite

TBD

#### SqlServer

TBD

#### Oracle

TBD

#### MySql

TBD

#### PostgreSQL

TBD

#### Firebird

TBD

#### CockroachDB - single docker node

|      Method |  Rows |   Variant |        Mean |      Error |       StdDev |     StdErr |       Median |          Min |          Max |
|------------ |------ |---------- |------------:|-----------:|-------------:|-----------:|-------------:|-------------:|-------------:|
| InsertAsync |     1 | Optimized |    10.17 ms |   0.652 ms |     1.317 ms |   0.186 ms |     9.840 ms |     8.095 ms |     14.14 ms |
| InsertAsync |     1 | EF Core   |    11.71 ms |   0.995 ms |     2.010 ms |   0.284 ms |    11.357 ms |     9.340 ms |     18.26 ms |
| InsertAsync |    10 | Optimized |    14.46 ms |   0.864 ms |     1.746 ms |   0.247 ms |    14.119 ms |    12.077 ms |     19.87 ms |
| InsertAsync |    10 | EF Core   |    60.71 ms |   0.815 ms |     1.645 ms |   0.233 ms |    60.493 ms |    55.334 ms |     65.75 ms |
| InsertAsync |    25 | Optimized |    21.93 ms |   1.119 ms |     2.261 ms |   0.320 ms |    21.954 ms |    17.452 ms |     25.74 ms |
| InsertAsync |    25 | EF Core   |    46.27 ms |   3.228 ms |     6.522 ms |   0.922 ms |    46.950 ms |    30.909 ms |     59.11 ms |
| InsertAsync |    50 | Optimized |    31.96 ms |   2.328 ms |     4.704 ms |   0.665 ms |    31.650 ms |    23.543 ms |     42.29 ms |
| InsertAsync |    50 | EF Core   |    65.48 ms |   4.262 ms |     8.609 ms |   1.217 ms |    65.888 ms |    49.199 ms |     82.25 ms |
| InsertAsync |   100 | Optimized |    58.07 ms |   7.676 ms |    15.505 ms |   2.193 ms |    55.675 ms |    32.708 ms |     97.82 ms |
| InsertAsync |   100 | EF Core   |   102.57 ms |   7.028 ms |    14.197 ms |   2.008 ms |   101.253 ms |    79.008 ms |    140.81 ms |
| InsertAsync |  1000 | Optimized |   460.28 ms |  63.296 ms |   127.861 ms |  18.082 ms |   454.208 ms |   257.897 ms |    671.97 ms |
| InsertAsync |  1000 | EF Core   |   682.31 ms |  58.695 ms |   118.567 ms |  16.768 ms |   688.391 ms |   488.458 ms |    885.48 ms |
| InsertAsync | 10000 | Optimized | 4,640.29 ms | 694.274 ms | 1,402.468 ms | 198.339 ms | 4,615.951 ms | 2,443.311 ms |  7,358.03 ms |
| InsertAsync | 10000 | EF Core   | 7,630.47 ms | 785.283 ms | 1,586.310 ms | 224.338 ms | 7,009.984 ms | 5,799.234 ms | 11,372.97 ms |

#### CockroachDB - nine docker nodes

|      Method |  Rows |    ariant |         Mean |        Error |       StdDev |     StdErr |          Min |          Max |       Median |
|------------ |------ |---------- |-------------:|-------------:|-------------:|-----------:|-------------:|-------------:|-------------:|
| InsertAsync |     1 | Optimized |     21.16 ms |     1.885 ms |     3.807 ms |   0.538 ms |     16.18 ms |     33.57 ms |     20.31 ms |
| InsertAsync |     1 | EF Core   |     21.15 ms |     1.394 ms |     2.817 ms |   0.398 ms |     15.88 ms |     32.41 ms |     20.41 ms |
| InsertAsync |    10 | Optimized |     25.17 ms |     1.223 ms |     2.471 ms |   0.349 ms |     21.08 ms |     31.57 ms |     24.52 ms |
| InsertAsync |    10 | EF Core   |     76.67 ms |     1.772 ms |     3.580 ms |   0.506 ms |     71.14 ms |     87.05 ms |     76.18 ms |
| InsertAsync |    25 | Optimized |     34.18 ms |     2.214 ms |     4.472 ms |   0.632 ms |     24.85 ms |     49.15 ms |     33.07 ms |
| InsertAsync |    25 | EF Core   |     93.90 ms |     4.585 ms |     9.262 ms |   1.310 ms |     78.24 ms |    126.30 ms |     93.01 ms |
| InsertAsync |    50 | Optimized |     46.73 ms |     4.402 ms |     8.893 ms |   1.258 ms |     35.10 ms |     77.57 ms |     45.99 ms |
| InsertAsync |    50 | EF Core   |    117.21 ms |     5.161 ms |    10.426 ms |   1.474 ms |     90.53 ms |    146.95 ms |    118.13 ms |
| InsertAsync |   100 | Optimized |     63.66 ms |     6.524 ms |    13.179 ms |   1.864 ms |     42.38 ms |     87.64 ms |     62.56 ms |
| InsertAsync |   100 | EF Core   |    196.49 ms |     8.679 ms |    17.532 ms |   2.479 ms |    166.72 ms |    241.54 ms |    196.59 ms |
| InsertAsync |  1000 | Optimized |    606.18 ms |    76.693 ms |   154.924 ms |  21.909 ms |    333.27 ms |    869.72 ms |    587.11 ms |
| InsertAsync |  1000 | EF Core   |  1,993.56 ms |   225.990 ms |   456.511 ms |  64.560 ms |  1,284.95 ms |  3,014.77 ms |  1,951.14 ms |
| InsertAsync | 10000 | Optimized |  5,516.29 ms | 2,842.006 ms | 1,879.811 ms | 594.448 ms |  3,016.59 ms |  8,488.42 ms |  5,412.93 ms |
| InsertAsync | 10000 | EF Core   | 20,122.79 ms | 1,633.778 ms | 3,300.313 ms | 466.735 ms | 13,500.72 ms | 27,940.97 ms | 20,256.04 ms |

## Remarks

Currently this package instead of using SqlKata directly from original authors uses my own fork. The reason behind is performance - there is something wrong with current version, especially when generate multi-row insert statements. I created [pull request](https://github.com/sqlkata/querybuilder/pull/548), but unfortunately it is under review since January. When it will be approved I will switch to using package from NuGet.
