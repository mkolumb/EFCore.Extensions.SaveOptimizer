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
| **UpdateAsync** |       **Optimized** |    **1** | **436.7 μs** | **31.66 μs** |  **63.96 μs** | **422.4 μs** | **349.3 μs** |   **680.4 μs** |
| **UpdateAsync** |       **Optimized** |   **10** | **483.2 μs** | **34.70 μs** |  **70.10 μs** | **465.7 μs** | **412.3 μs** |   **828.3 μs** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **454.6 μs** | **33.14 μs** |  **66.95 μs** | **420.2 μs** | **369.1 μs** |   **605.1 μs** |
| **UpdateAsync** | **OptimizedDapper** |   **10** | **473.0 μs** | **30.02 μs** |  **60.64 μs** | **456.9 μs** | **372.3 μs** |   **698.8 μs** |
| **UpdateAsync** |          **EfCore** |    **1** | **408.8 μs** | **32.90 μs** |  **66.46 μs** | **388.5 μs** | **347.0 μs** |   **777.9 μs** |
| **UpdateAsync** |          **EfCore** |   **10** | **523.4 μs** | **66.43 μs** | **134.19 μs** | **495.8 μs** | **411.9 μs** | **1,341.0 μs** |
