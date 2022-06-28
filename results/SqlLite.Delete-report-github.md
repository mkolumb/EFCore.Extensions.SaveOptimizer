``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-LWDXGF : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |    Error |    StdDev |      Min |        Max |   Median |
|------------ |---------------- |----- |---------:|---------:|----------:|---------:|-----------:|---------:|
| **DeleteAsync** |       **Optimized** |    **1** | **450.4 μs** | **30.60 μs** |  **61.82 μs** | **380.1 μs** |   **768.4 μs** | **429.5 μs** |
| **DeleteAsync** |       **Optimized** |   **10** | **518.5 μs** | **48.85 μs** |  **98.68 μs** | **430.1 μs** | **1,129.1 μs** | **505.1 μs** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **451.9 μs** | **39.57 μs** |  **79.93 μs** | **377.8 μs** |   **928.7 μs** | **432.4 μs** |
| **DeleteAsync** | **OptimizedDapper** |   **10** | **509.2 μs** | **25.75 μs** |  **52.02 μs** | **413.1 μs** |   **620.2 μs** | **502.7 μs** |
| **DeleteAsync** |          **EfCore** |    **1** | **439.4 μs** | **32.04 μs** |  **64.72 μs** | **325.5 μs** |   **665.2 μs** | **419.4 μs** |
| **DeleteAsync** |          **EfCore** |   **10** | **531.7 μs** | **72.16 μs** | **145.77 μs** | **423.4 μs** | **1,439.8 μs** | **500.3 μs** |
