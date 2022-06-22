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

Please note it is not working exactly as SaveChanges, so You should verify it works in your case as expected.

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

### Running

```cmd
powershell -File run_benchmarks.ps1
```

```powershell
.\run_benchmarks.ps1
```

### SqlLite

|       Method | Rows |   Variant |      Mean |     Error |    StdDev |
|------------- |----- |---------- |----------:|----------:|----------:|
| ExecuteAsync |    1 | EF Core   |  41.81 ms |  6.438 ms |  18.98 ms |
| ExecuteAsync |    1 | Optimized |  52.59 ms |  5.774 ms |  17.02 ms |
| ExecuteAsync |   10 | EF Core   | 192.00 ms | 28.277 ms |  83.38 ms |
| ExecuteAsync |   10 | Optimized | 256.63 ms | 31.335 ms |  92.39 ms |
| ExecuteAsync |  100 | EF Core   | 414.46 ms | 69.008 ms | 201.30 ms |
| ExecuteAsync |  100 | Optimized | 379.45 ms | 52.489 ms | 154.76 ms |

## Remarks

Currently this package instead of using SqlKata directly from NuGet uses my own fork. The reason behind is performance - there is something wrong with current version. I created [pull request](https://github.com/sqlkata/querybuilder/pull/548), but unfortunately it is under review since January. When it will be approved I will switch to using package from NuGet.
