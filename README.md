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

I considered few solutions (e.g. [EFCore.BulkExtensions](https://github.com/borisdj/EFCore.BulkExtensions), [linq2db](https://linq2db.github.io)) but all of them had some disadvantages during this time. We needed something with support for CockroachDB and concurrency tokens. Also we want to avoid business logic rewrite, more code changes than replacing SaveChanges execution could be big problem. As there were no good choice I wrote something simple from scratch. For sql statements generation I used excellent [SqlKata](https://sqlkata.com/) library. Now I decided to rewrite whole solution as something more efficient and better integrated into EF. 

[EFCore.BulkExtensions](https://github.com/borisdj/EFCore.BulkExtensions) is good choice for most cases as it has BulkSaveChanges method and currently supports few different databases (SQL Server, PostgreSQL and SQLite). Unfortunately it looks like API they used for PostgreSQL is incompatible with CockroachDB.

Main difference is approach - [EFCore.BulkExtensions](https://github.com/borisdj/EFCore.BulkExtensions) uses copy tools / statements under the hood, SaveOptimizer uses batched INSERT / UPDATE / DELETE statements. You need to choose which would be better in your case. Likely copy would be faster in most cases (it's good to measure this), but SaveOptimizer approach would bring more databases support. SaveOptimizer is simple library so you should verify it with your requirements.

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
4. Generate SQL
5. Execute
   - *if there is no transaction started then will start serializable transaction*
6. Mark all saved entities as detached 

Please note it is not working exactly as SaveChanges, so you should verify it works in your case as expected. 

## Features
- Providers support
  - [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer)
  - [Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite)
  - [Npgsql.EntityFrameworkCore.PostgreSQL](https://www.nuget.org/packages/Npgsql.EntityFrameworkCore.PostgreSQL)
  - [Oracle.EntityFrameworkCore](https://www.nuget.org/packages/Oracle.EntityFrameworkCore/)
  - [Pomelo.EntityFrameworkCore.MySql](https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql)
  - [FirebirdSql.EntityFrameworkCore.Firebird](https://www.nuget.org/packages/FirebirdSql.EntityFrameworkCore.Firebird/)
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
- Add unchecked when possible
- Low level performance optimizations
- Batch parameters when multi insert
- GitHub actions
- Test databases within container
- Tests
  - Value generated on add
  - Auto increment primary key
  - Value converter
  - Mixed statements
  - Hierarchical operations
  - Data types precision (date, decimal etc.)
  - Update concurrency tokens

## Limitations

### Refresh data after save

SaveOptimizer approach makes almost impossible refresh data after save, it is on your side. 
I recommend to generate values for primary keys in code, not in db. 
This will make much easier refresh data after save if necessary, you will be able to use this values for query. 

### Firebird insert

Basically query builders are prepared for multi row statements, but it looks there are some issues when executing via DbCommand. Currently for this provider batch is working only on update and delete.

### Oracle serializable transaction

It looks like serializable transaction produces many errors during execution. This is something to investigate. Currently I don't recommend using this library with Oracle in production environment.

## Q&A

1. Why you wrote query builder instead of using SqlKata?
   - The reason behind is performance. I noticed few issues and created [pull request](https://github.com/sqlkata/querybuilder/pull/548). When I started working on this library my pull request was in review without response for few months. Initially I decided to create [.NET 6 fork](https://www.nuget.org/packages/SqlKata.Net6/) and performance improvements were really great. Later I realized I don't need most of SqlKata features so it's better to write something optimized from scratch - this will bring performance & freedom boost. Currently there is reference to SqlKata in test project, just for verify my builder.
2. Why you wrote query executor?
   - I noticed a bug with ExecuteSqlRaw from RelationalExtensions. It looks it cuts precision for decimals. So I created something lightweight using some EF Core features.
3. Which EF Core version do you support?
   - I have plan to support only current release and latest LTS version. As there is only one required dependency (Microsoft.EntityFrameworkCore.Relational) you should be able to quickly prepare version for older EF if you need. Maybe some small changes in DataContextModelWrapper would be required.

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

My advice is to compare results in your real environment. Honestly - the best choice for pure performance is leave EF Core for write operations at all and then write statements from scratch for your scenarios. But this library could bring serious performance benefits in many scenarios without big effort. 

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

TBD

#### SqlLite

TBD

#### SqlServer

TBD

#### Oracle

TBD

#### MySql

TBD

#### MariaDB

TBD

#### PostgreSQL

TBD

#### Firebird 3

TBD

#### Firebird 4

TBD
