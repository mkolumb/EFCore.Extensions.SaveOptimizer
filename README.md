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

## Known issues

### Firebird batch insert problem

Basically all query builders are prepared for multi row statements, but it looks there are some issues when executing DbCommand with Firebird provider. I think there is something with .NET Firebird client. Currently for this providers batch is working only on update and delete. Insert is working in the same manner as when using EF Core without extensions as it has InsertBatchSize 1 by default for this provider.

### Oracle serializable transaction

It looks like serializable transaction produces many errors during execution, especially during insert (e.g. ORA-08177 & ORA-06512). This is something to investigate. I don't recommend using this library with Oracle in production environment. Sometimes decrease batch size for insert could help. This is the reason why it has InsertBatchSize 1 by default for this provider.

## Q&A

1. Why you wrote query builder instead of using SqlKata?
   - The reason behind is performance. I noticed few issues and created [pull request](https://github.com/sqlkata/querybuilder/pull/548). When I started working on this library my pull request was in review without response for few months. Initially I decided to create [.NET 6 fork](https://www.nuget.org/packages/SqlKata.Net6/) and performance improvements were really great. Later I realized I don't need most of SqlKata features so it's better to write something optimized from scratch - this will bring performance & freedom boost. Currently there is reference to SqlKata in test project, just for verify my builder.
2. Why you wrote query executor?
   - I noticed a bug with ExecuteSqlRaw from RelationalExtensions. It looks it cuts precision for decimals. So I created something lightweight using some EF Core features.
3. What is EFCore.Extensions.SaveOptimizer.Dapper package purpose?
   - The default execution use provider described in previous question. Someone could prefer execution using Dapper. You can compare performance in your case. From my experience results usually are similar.
4. Which EF Core version do you support?
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

All benchmarks uses serializable isolation level and run databases within containers.

``` ini
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=12  IterationCount=12  LaunchCount=8  
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

#### Legend
- **Optimized** - *Using EFCore.Extensions.SaveOptimizer package*
- **Optimized Dapper** - *Using EFCore.Extensions.SaveOptimizer.Dapper package*
- **EF Core** - *Using pure EF Core*

#### CockroachDB - single docker node

##### [INSERT](/results/Cockroach.Insert-report-github.md)

[![CockroachDB - single docker node INSERT](/results/Cockroach.Insert-barplot.png "CockroachDB - single docker node INSERT")](/results/Cockroach.Insert-report-github.md)

##### [UPDATE](/results/Cockroach.Update-report-github.md)

[![CockroachDB - single docker node UPDATE](/results/Cockroach.Update-barplot.png "CockroachDB - single docker node UPDATE")](/results/Cockroach.Update-report-github.md)

##### [DELETE](/results/Cockroach.Delete-report-github.md)

[![CockroachDB - single docker node DELETE](/results/Cockroach.Delete-barplot.png "CockroachDB - single docker node DELETE")](/results/Cockroach.Delete-report-github.md)

#### CockroachDB - nine docker nodes

##### [INSERT](/results/CockroachMulti.Insert-report-github.md)

[![CockroachDB - nine docker nodes INSERT](/results/CockroachMulti.Insert-barplot.png "CockroachDB - nine docker nodes INSERT")](/results/CockroachMulti.Insert-report-github.md)

##### [UPDATE](/results/CockroachMulti.Update-report-github.md)

[![CockroachDB - nine docker nodes UPDATE](/results/CockroachMulti.Update-barplot.png "CockroachDB - nine docker nodes UPDATE")](/results/CockroachMulti.Update-report-github.md)

##### [DELETE](/results/CockroachMulti.Delete-report-github.md)

[![CockroachDB - nine docker nodes DELETE](/results/CockroachMulti.Delete-barplot.png "CockroachDB - nine docker nodes DELETE")](/results/CockroachMulti.Delete-report-github.md)

#### SQLite

##### [INSERT](/results/Sqlite.Insert-report-github.md)

[![SQLite INSERT](/results/Sqlite.Insert-barplot.png "SQLite INSERT")](/results/Sqlite.Insert-report-github.md)

##### [UPDATE](/results/Sqlite.Update-report-github.md)

[![SQLite UPDATE](/results/Sqlite.Update-barplot.png "SQLite UPDATE")](/results/Sqlite.Update-report-github.md)

##### [DELETE](/results/Sqlite.Delete-report-github.md)

[![SQLite DELETE](/results/Sqlite.Delete-barplot.png "SQLite DELETE")](/results/Sqlite.Delete-report-github.md)

#### SQL Server

##### [INSERT](/results/SqlServer.Insert-report-github.md)

[![SQL Server INSERT](/results/SqlServer.Insert-barplot.png "SQL Server INSERT")](/results/SqlServer.Insert-report-github.md)

##### [UPDATE](/results/SqlServer.Update-report-github.md)

[![SQL Server UPDATE](/results/SqlServer.Update-barplot.png "SQL Server UPDATE")](/results/SqlServer.Update-report-github.md)

##### [DELETE](/results/SqlServer.Delete-report-github.md)

[![SQL Server DELETE](/results/SqlServer.Delete-barplot.png "SQL Server DELETE")](/results/SqlServer.Delete-report-github.md)

#### Oracle Express

##### [INSERT](/results/Oracle.Insert-report-github.md)

[![Oracle Express INSERT](/results/Oracle.Insert-barplot.png "Oracle Express INSERT")](/results/Oracle.Insert-report-github.md)

##### [UPDATE](/results/Oracle.Update-report-github.md)

[![Oracle Express UPDATE](/results/Oracle.Update-barplot.png "Oracle Express UPDATE")](/results/Oracle.Update-report-github.md)

##### [DELETE](/results/Oracle.Delete-report-github.md)

[![Oracle Express DELETE](/results/Oracle.Delete-barplot.png "Oracle Express DELETE")](/results/Oracle.Delete-report-github.md)

#### MySQL

##### [INSERT](/results/PomeloMySql.Insert-report-github.md)

[![MySQL INSERT](/results/PomeloMySql.Insert-barplot.png "MySQL INSERT")](/results/PomeloMySql.Insert-report-github.md)

##### [UPDATE](/results/PomeloMySql.Update-report-github.md)

[![MySQL UPDATE](/results/PomeloMySql.Update-barplot.png "MySQL UPDATE")](/results/PomeloMySql.Update-report-github.md)

##### [DELETE](/results/PomeloMySql.Delete-report-github.md)

[![MySQL DELETE](/results/PomeloMySql.Delete-barplot.png "MySQL DELETE")](/results/PomeloMySql.Delete-report-github.md)

#### MariaDB

##### [INSERT](/results/PomeloMariaDb.Insert-report-github.md)

[![MariaDB INSERT](/results/PomeloMariaDb.Insert-barplot.png "MariaDB INSERT")](/results/PomeloMariaDb.Insert-report-github.md)

##### [UPDATE](/results/PomeloMariaDb.Update-report-github.md)

[![MariaDB UPDATE](/results/PomeloMariaDb.Update-barplot.png "MariaDB UPDATE")](/results/PomeloMariaDb.Update-report-github.md)

##### [DELETE](/results/PomeloMariaDb.Delete-report-github.md)

[![MariaDB DELETE](/results/PomeloMariaDb.Delete-barplot.png "MariaDB DELETE")](/results/PomeloMariaDb.Delete-report-github.md)

#### PostgreSQL

##### [INSERT](/results/Postgres.Insert-report-github.md)

[![PostgreSQL INSERT](/results/Postgres.Insert-barplot.png "PostgreSQL INSERT")](/results/Postgres.Insert-report-github.md)

##### [UPDATE](/results/Postgres.Update-report-github.md)

[![PostgreSQL UPDATE](/results/Postgres.Update-barplot.png "PostgreSQL UPDATE")](/results/Postgres.Update-report-github.md)

##### [DELETE](/results/Postgres.Delete-report-github.md)

[![PostgreSQL DELETE](/results/Postgres.Delete-barplot.png "PostgreSQL DELETE")](/results/Postgres.Delete-report-github.md)

#### Firebird 3

##### [INSERT](/results/Firebird3.Insert-report-github.md)

[![Firebird 3 INSERT](/results/Firebird3.Insert-barplot.png "Firebird 3 INSERT")](/results/Firebird3.Insert-report-github.md)

##### [UPDATE](/results/Firebird3.Update-report-github.md)

[![Firebird 3 UPDATE](/results/Firebird3.Update-barplot.png "Firebird 3 UPDATE")](/results/Firebird3.Update-report-github.md)

##### [DELETE](/results/Firebird3.Delete-report-github.md)

[![Firebird 3 DELETE](/results/Firebird3.Delete-barplot.png "Firebird 3 DELETE")](/results/Firebird3.Delete-report-github.md)

#### Firebird 4

##### [INSERT](/results/Firebird4.Insert-report-github.md)

[![Firebird 4 INSERT](/results/Firebird4.Insert-barplot.png "Firebird 4 INSERT")](/results/Firebird4.Insert-report-github.md)

##### [UPDATE](/results/Firebird4.Update-report-github.md)

[![Firebird 4 UPDATE](/results/Firebird4.Update-barplot.png "Firebird 4 UPDATE")](/results/Firebird4.Update-report-github.md)

##### [DELETE](/results/Firebird4.Delete-report-github.md)

[![Firebird 4 DELETE](/results/Firebird4.Delete-barplot.png "Firebird 4 DELETE")](/results/Firebird4.Delete-report-github.md)
