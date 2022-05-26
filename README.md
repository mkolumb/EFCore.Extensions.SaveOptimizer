# EFCore.Extensions.SaveOptimizer
Save optimizer extension for EF Core

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
- Other
  - Concurrency token
  - Computed properties
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
