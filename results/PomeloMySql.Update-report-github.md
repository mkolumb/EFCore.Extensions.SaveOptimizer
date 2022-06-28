``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-ZPTBWN : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |   Median |      Min |       Max |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|----------:|
| **UpdateAsync** |       **Optimized** |    **1** | **7.405 ms** | **0.7605 ms** | **1.5362 ms** | **6.751 ms** | **5.617 ms** | **11.246 ms** |
| **UpdateAsync** |       **Optimized** |   **10** | **7.364 ms** | **0.5775 ms** | **1.1666 ms** | **7.199 ms** | **6.182 ms** | **14.451 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **7.136 ms** | **0.2942 ms** | **0.5944 ms** | **7.021 ms** | **6.130 ms** |  **9.067 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **10** | **7.189 ms** | **0.3358 ms** | **0.6783 ms** | **7.120 ms** | **6.063 ms** |  **9.403 ms** |
| **UpdateAsync** |          **EfCore** |    **1** | **7.418 ms** | **0.8709 ms** | **1.7592 ms** | **6.758 ms** | **5.745 ms** | **13.427 ms** |
| **UpdateAsync** |          **EfCore** |   **10** | **8.466 ms** | **0.7466 ms** | **1.5082 ms** | **8.030 ms** | **6.427 ms** | **13.710 ms** |
