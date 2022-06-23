# EFCore.Extensions.SaveOptimizer
Save optimizer extension for EF Core. 
It supports multiple EF Core providers and brings serious performance benefits for many scenarios without big effort.

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
await transaction.CommitAsync(cancellationToken);
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
- Tests
  - Value generated on add
  - Auto increment primary key
  - Value converter
  - Mixed statements
  - Hierarchical operations

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

My advice is to compare results in your real environment. Honestly - the best choice for pure performance is leave EF Core for write operations at all and then write statements from scratch. But this library could bring serious performance benefits in many scenarios without big effort. 

All benchmarks uses serializable isolation level.

``` ini
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.300
  [Host]     : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```

### Running

```cmd
powershell -File run_benchmarks.ps1
```

```powershell
.\run_benchmarks.ps1
```

### Results

#### CockroachDB - single docker node

TBD

#### CockroachDB - nine docker nodes

| Method      | Variant   | Rows  | Mean         | Median       | Min          | Max         |
|-------------|-----------|-------|--------------|--------------|--------------|-------------|
| InsertAsync | Optimized | 1     | 5.356 ms     | 4.551 ms     | 3.342 ms     | 16.64 ms    |
| InsertAsync | Optimized | 10    | 7.624 ms     | 6.233 ms     | 4.129 ms     | 17.57 ms    |
| InsertAsync | Optimized | 25    | 8.699 ms     | 8.076 ms     | 5.226 ms     | 15.15 ms    |
| InsertAsync | Optimized | 50    | 8.970 ms     | 8.597 ms     | 5.202 ms     | 15.43 ms    |
| InsertAsync | Optimized | 100   | 9.832 ms     | 8.241 ms     | 5.858 ms     | 31.13 ms    |
| InsertAsync | Optimized | 1000  | 28.068 ms    | 25.366 ms    | 19.536 ms    | 71.41 ms    |
| InsertAsync | Optimized | 10000 | 523.521 ms   | 440.253 ms   | 208.526 ms   | 1,227.85 ms |
| InsertAsync | EfCore    | 1     | 7.055 ms     | 6.290 ms     | 3.112 ms     | 16.95 ms    |
| InsertAsync | EfCore    | 10    | 14.405 ms    | 12.831 ms    | 6.908 ms     | 41.59 ms    |
| InsertAsync | EfCore    | 25    | 15.329 ms    | 12.899 ms    | 8.381 ms     | 32.20 ms    |
| InsertAsync | EfCore    | 50    | 19.469 ms    | 16.914 ms    | 10.609 ms    | 48.96 ms    |
| InsertAsync | EfCore    | 100   | 27.260 ms    | 23.934 ms    | 15.071 ms    | 54.78 ms    |
| InsertAsync | EfCore    | 1000  | 312.206 ms   | 238.954 ms   | 137.904 ms   | 1,154.99 ms |
| InsertAsync | EfCore    | 10000 | 2,002.225 ms | 1,773.687 ms | 1,174.984 ms | 5,738.30 ms |

| Method      | Variant   | Rows  | Mean         | Median       | Min          | Max          |
|-------------|-----------|-------|--------------|--------------|--------------|--------------|
| UpdateAsync | Optimized | 1     | 13.608 ms    | 6.932 ms     | 3.139 ms     | 71.969 ms    |
| UpdateAsync | Optimized | 10    | 5.908 ms     | 4.829 ms     | 3.421 ms     | 19.469 ms    |
| UpdateAsync | Optimized | 25    | 5.562 ms     | 5.398 ms     | 3.374 ms     | 14.500 ms    |
| UpdateAsync | Optimized | 50    | 5.599 ms     | 5.224 ms     | 3.473 ms     | 9.365 ms     |
| UpdateAsync | Optimized | 100   | 6.352 ms     | 5.990 ms     | 4.743 ms     | 9.333 ms     |
| UpdateAsync | Optimized | 1000  | 50.638 ms    | 48.831 ms    | 19.209 ms    | 143.633 ms   |
| UpdateAsync | Optimized | 10000 | 431.288 ms   | 416.436 ms   | 174.064 ms   | 954.876 ms   |
| UpdateAsync | EfCore    | 1     | 10.074 ms    | 6.946 ms     | 3.317 ms     | 37.710 ms    |
| UpdateAsync | EfCore    | 10    | 10.743 ms    | 10.290 ms    | 5.677 ms     | 20.844 ms    |
| UpdateAsync | EfCore    | 25    | 18.045 ms    | 16.216 ms    | 9.275 ms     | 49.263 ms    |
| UpdateAsync | EfCore    | 50    | 22.440 ms    | 20.958 ms    | 13.561 ms    | 38.646 ms    |
| UpdateAsync | EfCore    | 100   | 68.385 ms    | 53.593 ms    | 22.961 ms    | 285.637 ms   |
| UpdateAsync | EfCore    | 1000  | 661.244 ms   | 590.635 ms   | 291.719 ms   | 1,800.184 ms |
| UpdateAsync | EfCore    | 10000 | 3,178.785 ms | 3,129.773 ms | 1,140.467 ms | 7,029.094 ms |

| Method      | Variant   | Rows  | Mean         | Median       | Max          | Min        |
|-------------|-----------|-------|--------------|--------------|--------------|------------|
| DeleteAsync | Optimized | 1     | 4.584 ms     | 4.254 ms     | 11.590 ms    | 3.142 ms   |
| DeleteAsync | Optimized | 10    | 4.437 ms     | 3.960 ms     | 8.541 ms     | 2.997 ms   |
| DeleteAsync | Optimized | 25    | 4.561 ms     | 4.247 ms     | 8.867 ms     | 3.189 ms   |
| DeleteAsync | Optimized | 50    | 4.962 ms     | 4.706 ms     | 7.742 ms     | 3.485 ms   |
| DeleteAsync | Optimized | 100   | 5.929 ms     | 5.621 ms     | 8.820 ms     | 4.502 ms   |
| DeleteAsync | Optimized | 1000  | 24.705 ms    | 23.936 ms    | 41.615 ms    | 18.086 ms  |
| DeleteAsync | Optimized | 10000 | 182.846 ms   | 180.678 ms   | 261.666 ms   | 118.888 ms |
| DeleteAsync | EfCore    | 1     | 6.010 ms     | 4.858 ms     | 19.899 ms    | 2.996 ms   |
| DeleteAsync | EfCore    | 10    | 8.795 ms     | 7.619 ms     | 18.777 ms    | 4.705 ms   |
| DeleteAsync | EfCore    | 25    | 11.669 ms    | 11.010 ms    | 23.525 ms    | 5.851 ms   |
| DeleteAsync | EfCore    | 50    | 16.202 ms    | 15.363 ms    | 29.180 ms    | 7.241 ms   |
| DeleteAsync | EfCore    | 100   | 32.802 ms    | 30.629 ms    | 71.012 ms    | 13.730 ms  |
| DeleteAsync | EfCore    | 1000  | 581.582 ms   | 521.002 ms   | 1,798.104 ms | 113.653 ms |
| DeleteAsync | EfCore    | 10000 | 2,970.546 ms | 2,762.116 ms | 5,334.432 ms | 927.011 ms |

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

## Remarks

Currently this package instead of using SqlKata directly from original authors uses my own fork. The reason behind is performance - there is something wrong with current version, especially when generate multi-row insert statements. I created [pull request](https://github.com/sqlkata/querybuilder/pull/548), but unfortunately it is under review since January. When it will be approved I will switch to using package from NuGet.
