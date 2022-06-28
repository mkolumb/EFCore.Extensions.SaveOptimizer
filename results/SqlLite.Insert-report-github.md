``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-EBGRBP : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |    Error |   StdDev |      Min |      Max |   Median |
|------------ |---------------- |----- |---------:|---------:|---------:|---------:|---------:|---------:|
| **InsertAsync** |       **Optimized** |    **1** | **438.6 μs** | **18.29 μs** | **36.94 μs** | **396.2 μs** | **607.8 μs** | **428.0 μs** |
| **InsertAsync** | **OptimizedDapper** |    **1** | **447.4 μs** | **25.77 μs** | **52.05 μs** | **384.7 μs** | **602.2 μs** | **433.9 μs** |
| **InsertAsync** |          **EfCore** |    **1** | **441.2 μs** | **24.28 μs** | **49.05 μs** | **371.1 μs** | **574.2 μs** | **424.6 μs** |
