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

Main difference is approach - [EFCore.BulkExtensions](https://github.com/borisdj/EFCore.BulkExtensions) uses copy tools / statements under the hood, SaveOptimizer uses batched INSERT / UPDATE / DELETE statements generated with [SqlKata](https://sqlkata.com/) help. You need to choose which would be better in your case. Likely copy would be faster in most cases (it's good to measure this), but SaveOptimizer approach would bring more databases support. SaveOptimizer is simple library so verify more advanced features if needed.

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

Please note it is not working exactly as SaveChanges, so you should verify it works in your case as expected. 

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
  - None
- Statements
  - Insert
  - Update
  - Delete
- Other
  - Concurrency token
  - Auto-generated values

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
- Dependencies lifecycle optimizations
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

SaveOptimizer is not always better than pure EF Core methods. EF Core contains tons of optimizations so for small operations likely is better. If you usually update more than 10 rows then SaveOptimizer likely is better choice. 

My advice is to compare results in your real environment. Honestly - the best choice for pure performance is leave EF Core for write operations at all and then write statements from scratch. But this library could bring performance benefits without big effort. 

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

|       Method |  Rows |   Variant |        Mean |      Error |       StdDev |
|------------- |------ |---------- |------------:|-----------:|-------------:|
| ExecuteAsync |     1 | EF Core   |    29.36 ms |   2.877 ms |     8.482 ms |
| ExecuteAsync |     1 | Optimized |    30.76 ms |   3.275 ms |     9.503 ms |
| ExecuteAsync |    10 | EF Core   |    63.57 ms |   1.302 ms |     3.671 ms |
| ExecuteAsync |    10 | Optimized |   117.82 ms |  15.827 ms |    46.666 ms |
| ExecuteAsync |   100 | EF Core   |   279.59 ms |  35.761 ms |   105.441 ms |
| ExecuteAsync |   100 | Optimized |    51.50 ms |   4.782 ms |    14.099 ms |
| ExecuteAsync |  1000 | EF Core   |   754.31 ms |  50.281 ms |   148.255 ms |
| ExecuteAsync |  1000 | Optimized |   408.67 ms |  46.751 ms |   137.846 ms |
| ExecuteAsync | 10000 | EF Core   | 7,588.40 ms | 577.382 ms | 1,702.424 ms |
| ExecuteAsync | 10000 | Optimized | 4,358.31 ms | 436.266 ms | 1,286.341 ms |

#### CockroachDB - nine docker nodes

TBD

## Remarks

Currently this package instead of using SqlKata directly from NuGet uses my own fork. The reason behind is performance - there is something wrong with current version. I created [pull request](https://github.com/sqlkata/querybuilder/pull/548), but unfortunately it is under review since January. When it will be approved I will switch to using package from NuGet.
