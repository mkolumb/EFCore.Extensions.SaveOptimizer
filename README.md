# EFCore.Extensions.SaveOptimizer
Save optimizer extension for EF Core.  
It supports multiple EF Core providers and brings serious performance benefits for many scenarios without big effort.

Currently in BETA

[![GitHub build](https://github.com/mkolumb/EFCore.Extensions.SaveOptimizer/actions/workflows/build.yml/badge.svg "GitHub build")](https://github.com/mkolumb/EFCore.Extensions.SaveOptimizer/actions/workflows/build.yml) [![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.1-4baaaa.svg)](code_of_conduct.md)


| Package | NuGet |
|---|---|
| [EFCore.Extensions.SaveOptimizer](https://www.nuget.org/packages/EFCore.Extensions.SaveOptimizer) | [![NuGet SaveOptimizer](https://img.shields.io/nuget/v/EFCore.Extensions.SaveOptimizer "NuGet SaveOptimizer")](https://www.nuget.org/packages/EFCore.Extensions.SaveOptimizer) |
| [EFCore.Extensions.SaveOptimizer.Dapper](https://www.nuget.org/packages/EFCore.Extensions.SaveOptimizer.Dapper) | [![NuGet SaveOptimizer.Dapper](https://img.shields.io/nuget/v/EFCore.Extensions.SaveOptimizer.Dapper "NuGet SaveOptimizer.Dapper")](https://www.nuget.org/packages/EFCore.Extensions.SaveOptimizer.Dapper) |

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

### Optimized

```csharp
using EFCore.Extensions.SaveOptimizer;

await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
await context.AddAsync(entity);
await context.SaveChangesOptimizedAsync();
await transaction.CommitAsync(cancellationToken);
```

### Optimized Dapper

```csharp
using EFCore.Extensions.SaveOptimizer.Dapper;

await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
await context.AddAsync(entity);
await context.SaveChangesDapperOptimizedAsync();
await transaction.CommitAsync(cancellationToken);
```

## How it works

When you execute SaveChangesOptimized usually the following sequence happens:
1. Get entries from ChangeTracker
2. Build property changes dictionary for each entry
3. Group changes as much as possible
4. Generate SQL
5. Execute
6. Accept changes

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
- [Configuration](#configuration)
- Concurrency token support

## What to do next

- Support for
  - Different cultures
  - Interceptors
  - Value converters
  - Custom attributes
  - Ignore
  - Shadow properties
  - Tables without primary key
  - Auto-generated values
- Optimize data retrieval from Change Tracker
- Add unchecked when possible
- Low level performance optimizations
- Configure await
- Tests
  - Value generated on add
  - Auto increment primary key
  - Explicitly set primary key
  - No primary key
  - Various type primary keys
  - Sequences
  - Value converter
  - Hierarchical operations
  - Data types precision (date, decimal etc.)
  - Update concurrency tokens
  - Different configuration types
  - Owned entities
  - Keyless entities
  - Value comparers
  - Table splitting

## Limitations

### Refresh data after save

SaveOptimizer approach makes almost impossible refresh data after save, it is on your side. If you will use auto increment primary key it will not retrieve this key from db. I recommend to generate values for primary keys in code, not in db. This will make much easier refresh data after save if necessary, you will be able to use this values for query. Also DatabaseValues for entry will not be retrieved from db when ConcurrencyTokenException is thrown.

Basically - after save you should not use this context anymore as it could be invalid, you should use new context for another operation. However if you need you can experiment with AfterSaveBehavior.

## Known issues

### Oracle serializable transaction

It looks like serializable transaction produces many errors during execution, especially during insert (e.g. ORA-08177 & ORA-06512). This is something to investigate, maybe this is dockerized Oracle Express issue. I don't recommend using this library with Oracle in production environment without strong testing. Sometimes decrease batch size for insert could help.

### Firebird provider

This is not a SaveOptimizer issue, however I experienced some problems with Firebird provider. It looks model builder sometimes build different model than other providers.

| Issue | Workaround |
|---|---|
| Precision lost for decimal column | Use `HasColumnType("DECIMAL(PRECISION,SCALE)")` |
| Auto increment column not created | Use `HasAnnotation(FbAnnotationNames.ValueGenerationStrategy, FbValueGenerationStrategy.IdentityColumn)` |

## Q&A

1. Why you wrote query builder instead of using SqlKata?
   - The reason behind is performance. I noticed few issues and created [pull request](https://github.com/sqlkata/querybuilder/pull/548). When I started working on this library my pull request was in review without response for few months. Initially I decided to create [.NET 6 fork](https://www.nuget.org/packages/SqlKata.Net6/) and performance improvements were really great. Later I realized I don't need most of SqlKata features so it's better to write something optimized from scratch - this will bring performance & freedom boost. Currently there is reference to SqlKata in test project, just for verify my builder.
2. Why you wrote query executor?
   - I noticed a bug with ExecuteSqlRaw from RelationalExtensions. It looks it cuts precision for decimals. So I created something lightweight using some EF Core features.
3. What is EFCore.Extensions.SaveOptimizer.Dapper package purpose?
   - The default execution use provider described in previous question. Someone could prefer execution using Dapper. You can compare performance in your case. From my experience results usually are similar.
4. Which EF Core version do you support?
   - I have plan to support only current release and latest LTS version. As there is only one required dependency (Microsoft.EntityFrameworkCore.Relational) you should be able to quickly prepare version for older EF if you need. Maybe some small changes in DataContextModelWrapper would be required.

## Configuration

| Parameter | Description | Provider default value |
|---|---|---|
| Batch size | Default batch size  | _All - 1000_ |
| Insert batch size | When defined override batch size for insert operations | _Firebird - 500_ |
|  |  | _Oracle - 50_ |
|  |  | _Other - NULL_ |
| Update batch size | When defined override batch size for update operations | _All - NULL_ |
| Delete batch size | When defined override batch size for delete operations | _All - NULL_ |
| Parameters limit | Limit parameters for statement, when exceeded batch size decreased for operation to prevent exceptions | _SqlServer - 1024_ |
|  |  | _Firebird - 2048_ |
|  |  | _SqLite - 512_ |
|  |  | _Postgres - 31768_ |
|  |  | _Other - 15384_ |
| Concurrency token behavior | When concurrency token is defined for entity it is included in update / delete statements. When flag is set to throws exception it will throws exception when statements affected less / more rows than expected. | _All - throw exception_ |
| Auto transaction enabled | If enabled it will start transaction when no transaction attached to DbContext | _All - true_ |
| After save behavior | It will behavior after successful save, possible values (ClearChanges, AcceptChanges, DetachSaved, DoNothing) | _All - ClearChanges_ |
| Auto transaction isolation level | Isolation level for auto transaction | _All - serializable_ |
| Builder configuration -> case type | Case type used when building statements, if normal it will not change case to upper / lower | _All - normal_ |
| Builder configuration -> optimize parameters | Optimize parameters usage in statements, sometimes can lead to unexpected exception in db | _All - true_ |

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

All benchmarks uses serializable isolation level and run databases within containers.

``` ini
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

EvaluateOverhead=True  OutlierMode=RemoveUpper
InvocationCount=1  IterationCount=20  LaunchCount=3
RunStrategy=ColdStart  UnrollFactor=1  WarmupCount=0

```

### Running

```cmd
powershell -File run_benchmarks.ps1
```

```powershell
.\run_benchmarks.ps1
```

### Results

#### Legend
- **Optimized** - *Using EFCore.Extensions.SaveOptimizer package*
- **Optimized Dapper** - *Using EFCore.Extensions.SaveOptimizer.Dapper package*
- **EF Core** - *Using pure EF Core*

#### CockroachDB - single docker node

##### [INSERT](/results/Cockroach.Insert-report-github.md)

[![CockroachDB - single docker node INSERT](/results/Cockroach.Insert-barplot-small.png "CockroachDB - single docker node INSERT")](/results/Cockroach.Insert-report-github.md)

##### [UPDATE](/results/Cockroach.Update-report-github.md)

[![CockroachDB - single docker node UPDATE](/results/Cockroach.Update-barplot-small.png "CockroachDB - single docker node UPDATE")](/results/Cockroach.Update-report-github.md)

##### [DELETE](/results/Cockroach.Delete-report-github.md)

[![CockroachDB - single docker node DELETE](/results/Cockroach.Delete-barplot-small.png "CockroachDB - single docker node DELETE")](/results/Cockroach.Delete-report-github.md)

#### CockroachDB - nine docker nodes

##### [INSERT](/results/CockroachMulti.Insert-report-github.md)

[![CockroachDB - nine docker nodes INSERT](/results/CockroachMulti.Insert-barplot-small.png "CockroachDB - nine docker nodes INSERT")](/results/CockroachMulti.Insert-report-github.md)

##### [UPDATE](/results/CockroachMulti.Update-report-github.md)

[![CockroachDB - nine docker nodes UPDATE](/results/CockroachMulti.Update-barplot-small.png "CockroachDB - nine docker nodes UPDATE")](/results/CockroachMulti.Update-report-github.md)

##### [DELETE](/results/CockroachMulti.Delete-report-github.md)

[![CockroachDB - nine docker nodes DELETE](/results/CockroachMulti.Delete-barplot-small.png "CockroachDB - nine docker nodes DELETE")](/results/CockroachMulti.Delete-report-github.md)

#### SQLite

##### [INSERT](/results/Sqlite.Insert-report-github.md)

[![SQLite INSERT](/results/Sqlite.Insert-barplot-small.png "SQLite INSERT")](/results/Sqlite.Insert-report-github.md)

##### [UPDATE](/results/Sqlite.Update-report-github.md)

[![SQLite UPDATE](/results/Sqlite.Update-barplot-small.png "SQLite UPDATE")](/results/Sqlite.Update-report-github.md)

##### [DELETE](/results/Sqlite.Delete-report-github.md)

[![SQLite DELETE](/results/Sqlite.Delete-barplot-small.png "SQLite DELETE")](/results/Sqlite.Delete-report-github.md)

#### SQL Server

##### [INSERT](/results/SqlServer.Insert-report-github.md)

[![SQL Server INSERT](/results/SqlServer.Insert-barplot-small.png "SQL Server INSERT")](/results/SqlServer.Insert-report-github.md)

##### [UPDATE](/results/SqlServer.Update-report-github.md)

[![SQL Server UPDATE](/results/SqlServer.Update-barplot-small.png "SQL Server UPDATE")](/results/SqlServer.Update-report-github.md)

##### [DELETE](/results/SqlServer.Delete-report-github.md)

[![SQL Server DELETE](/results/SqlServer.Delete-barplot-small.png "SQL Server DELETE")](/results/SqlServer.Delete-report-github.md)

#### Oracle Express

##### [INSERT](/results/Oracle.Insert-report-github.md)

[![Oracle Express INSERT](/results/Oracle.Insert-barplot-small.png "Oracle Express INSERT")](/results/Oracle.Insert-report-github.md)

##### [UPDATE](/results/Oracle.Update-report-github.md)

[![Oracle Express UPDATE](/results/Oracle.Update-barplot-small.png "Oracle Express UPDATE")](/results/Oracle.Update-report-github.md)

##### [DELETE](/results/Oracle.Delete-report-github.md)

[![Oracle Express DELETE](/results/Oracle.Delete-barplot-small.png "Oracle Express DELETE")](/results/Oracle.Delete-report-github.md)

#### MySQL

##### [INSERT](/results/PomeloMySql.Insert-report-github.md)

[![MySQL INSERT](/results/PomeloMySql.Insert-barplot-small.png "MySQL INSERT")](/results/PomeloMySql.Insert-report-github.md)

##### [UPDATE](/results/PomeloMySql.Update-report-github.md)

[![MySQL UPDATE](/results/PomeloMySql.Update-barplot-small.png "MySQL UPDATE")](/results/PomeloMySql.Update-report-github.md)

##### [DELETE](/results/PomeloMySql.Delete-report-github.md)

[![MySQL DELETE](/results/PomeloMySql.Delete-barplot-small.png "MySQL DELETE")](/results/PomeloMySql.Delete-report-github.md)

#### MariaDB

##### [INSERT](/results/PomeloMariaDb.Insert-report-github.md)

[![MariaDB INSERT](/results/PomeloMariaDb.Insert-barplot-small.png "MariaDB INSERT")](/results/PomeloMariaDb.Insert-report-github.md)

##### [UPDATE](/results/PomeloMariaDb.Update-report-github.md)

[![MariaDB UPDATE](/results/PomeloMariaDb.Update-barplot-small.png "MariaDB UPDATE")](/results/PomeloMariaDb.Update-report-github.md)

##### [DELETE](/results/PomeloMariaDb.Delete-report-github.md)

[![MariaDB DELETE](/results/PomeloMariaDb.Delete-barplot-small.png "MariaDB DELETE")](/results/PomeloMariaDb.Delete-report-github.md)

#### PostgreSQL

##### [INSERT](/results/Postgres.Insert-report-github.md)

[![PostgreSQL INSERT](/results/Postgres.Insert-barplot-small.png "PostgreSQL INSERT")](/results/Postgres.Insert-report-github.md)

##### [UPDATE](/results/Postgres.Update-report-github.md)

[![PostgreSQL UPDATE](/results/Postgres.Update-barplot-small.png "PostgreSQL UPDATE")](/results/Postgres.Update-report-github.md)

##### [DELETE](/results/Postgres.Delete-report-github.md)

[![PostgreSQL DELETE](/results/Postgres.Delete-barplot-small.png "PostgreSQL DELETE")](/results/Postgres.Delete-report-github.md)

#### Firebird 3

##### [INSERT](/results/Firebird3.Insert-report-github.md)

[![Firebird 3 INSERT](/results/Firebird3.Insert-barplot-small.png "Firebird 3 INSERT")](/results/Firebird3.Insert-report-github.md)

##### [UPDATE](/results/Firebird3.Update-report-github.md)

[![Firebird 3 UPDATE](/results/Firebird3.Update-barplot-small.png "Firebird 3 UPDATE")](/results/Firebird3.Update-report-github.md)

##### [DELETE](/results/Firebird3.Delete-report-github.md)

[![Firebird 3 DELETE](/results/Firebird3.Delete-barplot-small.png "Firebird 3 DELETE")](/results/Firebird3.Delete-report-github.md)

#### Firebird 4

##### [INSERT](/results/Firebird4.Insert-report-github.md)

[![Firebird 4 INSERT](/results/Firebird4.Insert-barplot-small.png "Firebird 4 INSERT")](/results/Firebird4.Insert-report-github.md)

##### [UPDATE](/results/Firebird4.Update-report-github.md)

[![Firebird 4 UPDATE](/results/Firebird4.Update-barplot-small.png "Firebird 4 UPDATE")](/results/Firebird4.Update-report-github.md)

##### [DELETE](/results/Firebird4.Delete-report-github.md)

[![Firebird 4 DELETE](/results/Firebird4.Delete-barplot-small.png "Firebird 4 DELETE")](/results/Firebird4.Delete-report-github.md)

## Buy Me A Coffee

[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/mkolumb)
