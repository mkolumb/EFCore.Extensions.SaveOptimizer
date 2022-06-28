``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-QNAEUH : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |       Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|----------:|---------:|
| **UpdateAsync** |       **Optimized** |    **1** | **6.761 ms** | **0.3027 ms** | **0.6114 ms** | **5.733 ms** |  **8.989 ms** | **6.663 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **7.021 ms** | **0.3521 ms** | **0.7112 ms** | **5.849 ms** |  **9.380 ms** | **6.967 ms** |
| **UpdateAsync** |          **EfCore** |    **1** | **6.925 ms** | **0.4854 ms** | **0.9805 ms** | **5.750 ms** | **10.571 ms** | **6.670 ms** |
