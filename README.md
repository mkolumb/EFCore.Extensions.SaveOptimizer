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

Main difference is approach - [EFCore.BulkExtensions](https://github.com/borisdj/EFCore.BulkExtensions) uses copy tools / statements under the hood, SaveOptimizer uses batched INSERT / UPDATE / DELETE statements generated with [SqlKata](https://sqlkata.com/) help. You need to choose which would be better in your case. Likely copy would be faster in most cases (it's good to measure this), but SaveOptimizer approach would result in more features and databases support.

## How it works

1. SaveChangesOptimizerInterceptor receives info about saving changes
2. IDataPrepareService prepare SQL statements based on ChangeTracker detection
3. IExecutionService executes SQL statements
4. SaveChangesOptimizerInterceptor marks saved entities
5. EF Core continue with other entities in normal way

## Migration command

### SqlLite
```
cd .\EFCore.Extensions.SaveOptimizer.Model.SqlLite
dotnet ef migrations add [NAME] 
```

## Roadmap

### 0.1
- Providers
  - SQL Server
  - SqlLite
  - PostgreSQL
- Primary keys
  - Simple
  - Composed
  - None
- Clauses
  - Insert
  - Update
  - Delete
- Dependency injection
  - .NET DI container
- Other
  - Concurrency token
  - Computed properties
  - Value converters
  - Test with different cultures

### 0.2
- Providers
  - Oracle
  - MySQL
  - Firebird
- Configuration
  - Exclude entity from optimization
  - Concurrency token behavior (skip versus exception)
  - Batch size
