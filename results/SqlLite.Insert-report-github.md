``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-LWDXGF : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |    Error |    StdDev |   Median |      Min |        Max |
|------------ |---------------- |----- |---------:|---------:|----------:|---------:|---------:|-----------:|
| **InsertAsync** |       **Optimized** |    **1** | **474.2 μs** | **52.69 μs** | **106.44 μs** | **442.9 μs** | **399.8 μs** | **1,118.3 μs** |
| **InsertAsync** |       **Optimized** |   **10** | **570.2 μs** | **68.24 μs** | **137.85 μs** | **517.5 μs** | **434.5 μs** | **1,139.5 μs** |
| **InsertAsync** | **OptimizedDapper** |    **1** | **486.2 μs** | **47.77 μs** |  **96.50 μs** | **443.8 μs** | **384.0 μs** |   **777.5 μs** |
| **InsertAsync** | **OptimizedDapper** |   **10** | **527.9 μs** | **56.35 μs** | **113.84 μs** | **498.4 μs** | **420.5 μs** | **1,168.5 μs** |
| **InsertAsync** |          **EfCore** |    **1** | **445.7 μs** | **36.06 μs** |  **72.84 μs** | **422.2 μs** | **373.3 μs** |   **733.8 μs** |
| **InsertAsync** |          **EfCore** |   **10** | **563.9 μs** | **46.45 μs** |  **93.83 μs** | **538.6 μs** | **478.6 μs** | **1,052.3 μs** |
