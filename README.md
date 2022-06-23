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
  Job-CZYATU : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT

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

|      Method |  Rows |   Variant |       Mean |      Error |     StdDev |     Median |        Min |        Max |
|------------ |------ |---------- |-----------:|-----------:|-----------:|-----------:|-----------:|-----------:|
| **DeleteAsync** |     **1** | **Optimized** |   **2.927 ms** |  **0.2771 ms** |  **0.5597 ms** |   **2.752 ms** |   **2.310 ms** |   **5.110 ms** |
| **DeleteAsync** |     **1** |    **EfCore** |   **3.197 ms** |  **0.3965 ms** |  **0.8009 ms** |   **2.897 ms** |   **2.524 ms** |   **7.423 ms** |
| **DeleteAsync** |    **10** | **Optimized** |   **3.309 ms** |  **0.5512 ms** |  **1.1135 ms** |   **2.977 ms** |   **2.663 ms** |   **8.181 ms** |
| **DeleteAsync** |    **10** |    **EfCore** |   **3.646 ms** |  **0.2964 ms** |  **0.5988 ms** |   **3.488 ms** |   **3.000 ms** |   **6.140 ms** |
| **DeleteAsync** |    **25** | **Optimized** |   **2.772 ms** |  **0.1797 ms** |  **0.3630 ms** |   **2.664 ms** |   **2.422 ms** |   **4.476 ms** |
| **DeleteAsync** |    **25** |    **EfCore** |   **6.514 ms** |  **0.7550 ms** |  **1.5252 ms** |   **7.007 ms** |   **3.698 ms** |  **10.042 ms** |
| **DeleteAsync** |    **50** | **Optimized** |   **4.397 ms** |  **0.2892 ms** |  **0.5842 ms** |   **4.148 ms** |   **3.793 ms** |   **5.945 ms** |
| **DeleteAsync** |    **50** |    **EfCore** |   **7.594 ms** |  **0.8135 ms** |  **1.6434 ms** |   **7.984 ms** |   **4.604 ms** |  **12.072 ms** |
| **DeleteAsync** |   **100** | **Optimized** |   **5.906 ms** |  **1.0273 ms** |  **2.0752 ms** |   **6.649 ms** |   **2.870 ms** |  **11.222 ms** |
| **DeleteAsync** |   **100** |    **EfCore** |  **10.040 ms** |  **0.8523 ms** |  **1.7216 ms** |  **10.397 ms** |   **6.939 ms** |  **15.079 ms** |
| **DeleteAsync** |  **1000** | **Optimized** |  **32.839 ms** | **20.7675 ms** | **41.9514 ms** |  **11.922 ms** |   **9.237 ms** | **127.819 ms** |
| **DeleteAsync** |  **1000** |    **EfCore** |  **56.890 ms** |  **1.4738 ms** |  **2.9772 ms** |  **56.569 ms** |  **49.141 ms** |  **65.179 ms** |
| **DeleteAsync** | **10000** | **Optimized** | **138.275 ms** | **13.3974 ms** | **27.0635 ms** | **132.216 ms** | **107.835 ms** | **219.907 ms** |
| **DeleteAsync** | **10000** |    **EfCore** | **673.574 ms** | **43.5720 ms** | **88.0175 ms** | **658.231 ms** | **571.703 ms** | **893.884 ms** |

|      Method |  Rows |   Variant |       Mean |      Error |     StdDev |     Median |        Min |          Max |
|------------ |------ |---------- |-----------:|-----------:|-----------:|-----------:|-----------:|-------------:|
| **UpdateAsync** |     **1** | **Optimized** |   **2.122 ms** |  **0.1337 ms** |  **0.2701 ms** |   **2.037 ms** |   **1.865 ms** |     **3.262 ms** |
| **UpdateAsync** |     **1** |    **EfCore** |   **2.264 ms** |  **0.1720 ms** |  **0.3474 ms** |   **2.187 ms** |   **1.959 ms** |     **3.884 ms** |
| **UpdateAsync** |    **10** | **Optimized** |   **2.268 ms** |  **0.1496 ms** |  **0.3022 ms** |   **2.195 ms** |   **1.941 ms** |     **3.305 ms** |
| **UpdateAsync** |    **10** |    **EfCore** |   **3.239 ms** |  **0.4675 ms** |  **0.9444 ms** |   **2.896 ms** |   **2.502 ms** |     **8.185 ms** |
| **UpdateAsync** |    **25** | **Optimized** |   **2.615 ms** |  **0.5420 ms** |  **1.0948 ms** |   **2.328 ms** |   **2.102 ms** |     **7.062 ms** |
| **UpdateAsync** |    **25** |    **EfCore** |   **6.397 ms** |  **0.7742 ms** |  **1.5639 ms** |   **7.015 ms** |   **3.335 ms** |     **9.263 ms** |
| **UpdateAsync** |    **50** | **Optimized** |   **2.684 ms** |  **0.2195 ms** |  **0.4434 ms** |   **2.569 ms** |   **2.282 ms** |     **5.023 ms** |
| **UpdateAsync** |    **50** |    **EfCore** |   **8.074 ms** |  **0.9692 ms** |  **1.9578 ms** |   **8.246 ms** |   **4.716 ms** |    **13.517 ms** |
| **UpdateAsync** |   **100** | **Optimized** |   **2.946 ms** |  **0.2242 ms** |  **0.4530 ms** |   **2.876 ms** |   **2.575 ms** |     **5.879 ms** |
| **UpdateAsync** |   **100** |    **EfCore** |  **11.144 ms** |  **1.0832 ms** |  **2.1882 ms** |  **11.040 ms** |   **8.293 ms** |    **20.163 ms** |
| **UpdateAsync** |  **1000** | **Optimized** |  **69.628 ms** | **38.0141 ms** | **76.7903 ms** |  **13.612 ms** |  **10.747 ms** |   **210.438 ms** |
| **UpdateAsync** |  **1000** |    **EfCore** |  **71.832 ms** |  **1.9889 ms** |  **4.0177 ms** |  **71.654 ms** |  **64.219 ms** |    **80.661 ms** |
| **UpdateAsync** | **10000** | **Optimized** | **164.625 ms** | **14.8428 ms** | **29.9833 ms** | **158.891 ms** | **125.295 ms** |   **259.686 ms** |
| **UpdateAsync** | **10000** |    **EfCore** | **837.938 ms** | **33.1046 ms** | **66.8729 ms** | **811.320 ms** | **767.117 ms** | **1,044.190 ms** |


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
