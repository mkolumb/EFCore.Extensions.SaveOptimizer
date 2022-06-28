``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-EBGRBP : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |    Error |    StdDev |      Min |        Max |   Median |
|------------ |---------------- |----- |---------:|---------:|----------:|---------:|-----------:|---------:|
| **UpdateAsync** |       **Optimized** |    **1** | **435.5 μs** | **27.46 μs** |  **55.47 μs** | **335.3 μs** |   **549.9 μs** | **433.8 μs** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **489.2 μs** | **70.54 μs** | **142.50 μs** | **382.9 μs** | **1,338.0 μs** | **463.0 μs** |
| **UpdateAsync** |          **EfCore** |    **1** | **398.2 μs** | **11.72 μs** |  **23.67 μs** | **354.5 μs** |   **442.1 μs** | **398.7 μs** |
