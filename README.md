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

| Method      | Rows  | Variant   | Median     | Mean       | Min        | Max          |
|-------------|-------|-----------|------------|------------|------------|--------------|
| DeleteAsync | 1     | Optimized | 2.752 ms   | 2.927 ms   | 2.310 ms   | 5.110 ms     |
| DeleteAsync | 1     | EfCore    | 2.897 ms   | 3.197 ms   | 2.524 ms   | 7.423 ms     |
| DeleteAsync | 10    | Optimized | 2.977 ms   | 3.309 ms   | 2.663 ms   | 8.181 ms     |
| DeleteAsync | 10    | EfCore    | 3.488 ms   | 3.646 ms   | 3.000 ms   | 6.140 ms     |
| DeleteAsync | 25    | Optimized | 2.664 ms   | 2.772 ms   | 2.422 ms   | 4.476 ms     |
| DeleteAsync | 25    | EfCore    | 7.007 ms   | 6.514 ms   | 3.698 ms   | 10.042 ms    |
| DeleteAsync | 50    | Optimized | 4.148 ms   | 4.397 ms   | 3.793 ms   | 5.945 ms     |
| DeleteAsync | 50    | EfCore    | 7.984 ms   | 7.594 ms   | 4.604 ms   | 12.072 ms    |
| DeleteAsync | 100   | Optimized | 6.649 ms   | 5.906 ms   | 2.870 ms   | 11.222 ms    |
| DeleteAsync | 100   | EfCore    | 10.397 ms  | 10.040 ms  | 6.939 ms   | 15.079 ms    |
| DeleteAsync | 1000  | Optimized | 11.922 ms  | 32.839 ms  | 9.237 ms   | 127.819 ms   |
| DeleteAsync | 1000  | EfCore    | 56.569 ms  | 56.890 ms  | 49.141 ms  | 65.179 ms    |
| DeleteAsync | 10000 | Optimized | 132.216 ms | 138.275 ms | 107.835 ms | 219.907 ms   |
| DeleteAsync | 10000 | EfCore    | 658.231 ms | 673.574 ms | 571.703 ms | 893.884 ms   |
|             |       |           |            |            |            |              |
| UpdateAsync | 1     | Optimized | 2.037 ms   | 2.122 ms   | 1.865 ms   | 3.262 ms     |
| UpdateAsync | 1     | EfCore    | 2.187 ms   | 2.264 ms   | 1.959 ms   | 3.884 ms     |
| UpdateAsync | 10    | Optimized | 2.195 ms   | 2.268 ms   | 1.941 ms   | 3.305 ms     |
| UpdateAsync | 10    | EfCore    | 2.896 ms   | 3.239 ms   | 2.502 ms   | 8.185 ms     |
| UpdateAsync | 25    | Optimized | 2.328 ms   | 2.615 ms   | 2.102 ms   | 7.062 ms     |
| UpdateAsync | 25    | EfCore    | 7.015 ms   | 6.397 ms   | 3.335 ms   | 9.263 ms     |
| UpdateAsync | 50    | Optimized | 2.569 ms   | 2.684 ms   | 2.282 ms   | 5.023 ms     |
| UpdateAsync | 50    | EfCore    | 8.246 ms   | 8.074 ms   | 4.716 ms   | 13.517 ms    |
| UpdateAsync | 100   | Optimized | 2.876 ms   | 2.946 ms   | 2.575 ms   | 5.879 ms     |
| UpdateAsync | 100   | EfCore    | 11.040 ms  | 11.144 ms  | 8.293 ms   | 20.163 ms    |
| UpdateAsync | 1000  | Optimized | 13.612 ms  | 69.628 ms  | 10.747 ms  | 210.438 ms   |
| UpdateAsync | 1000  | EfCore    | 71.654 ms  | 71.832 ms  | 64.219 ms  | 80.661 ms    |
| UpdateAsync | 10000 | Optimized | 158.891 ms | 164.625 ms | 125.295 ms | 259.686 ms   |
| UpdateAsync | 10000 | EfCore    | 811.320 ms | 837.938 ms | 767.117 ms | 1,044.190 ms |
|             |       |           |            |            |            |              |
| InsertAsync | 1     | Optimized | 2.536 ms   | 2.635 ms   | 2.149 ms   | 3.617 ms     |
| InsertAsync | 1     | EfCore    | 2.601 ms   | 2.753 ms   | 2.162 ms   | 4.950 ms     |
| InsertAsync | 10    | Optimized | 2.488 ms   | 2.633 ms   | 2.068 ms   | 5.705 ms     |
| InsertAsync | 10    | EfCore    | 6.816 ms   | 6.090 ms   | 2.424 ms   | 8.893 ms     |
| InsertAsync | 25    | Optimized | 3.030 ms   | 3.131 ms   | 2.526 ms   | 4.701 ms     |
| InsertAsync | 25    | EfCore    | 7.159 ms   | 6.410 ms   | 2.905 ms   | 10.001 ms    |
| InsertAsync | 50    | Optimized | 3.314 ms   | 3.512 ms   | 2.715 ms   | 6.098 ms     |
| InsertAsync | 50    | EfCore    | 8.448 ms   | 7.781 ms   | 4.024 ms   | 10.883 ms    |
| InsertAsync | 100   | Optimized | 4.006 ms   | 4.329 ms   | 3.418 ms   | 9.301 ms     |
| InsertAsync | 100   | EfCore    | 10.572 ms  | 10.155 ms  | 6.051 ms   | 16.999 ms    |
| InsertAsync | 1000  | Optimized | 17.706 ms  | 18.799 ms  | 14.202 ms  | 30.998 ms    |
| InsertAsync | 1000  | EfCore    | 49.253 ms  | 52.206 ms  | 43.953 ms  | 67.410 ms    |
| InsertAsync | 10000 | Optimized | 158.693 ms | 163.356 ms | 141.316 ms | 234.643 ms   |
| InsertAsync | 10000 | EfCore    | 484.974 ms | 500.430 ms | 449.271 ms | 678.444 ms   |

#### CockroachDB - nine docker nodes

TBD

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
