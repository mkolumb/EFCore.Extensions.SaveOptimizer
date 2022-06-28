``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-IBCNHC : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |       Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|----------:|---------:|
| **UpdateAsync** |       **Optimized** |    **1** | **6.231 ms** | **0.2849 ms** | **0.5754 ms** | **5.345 ms** |  **8.099 ms** | **6.159 ms** |
| **UpdateAsync** |       **Optimized** |   **10** | **6.109 ms** | **0.3233 ms** | **0.6531 ms** | **4.680 ms** |  **7.518 ms** | **6.025 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **5.963 ms** | **0.6450 ms** | **1.3030 ms** | **4.573 ms** | **10.314 ms** | **5.507 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **10** | **5.632 ms** | **0.5138 ms** | **1.0379 ms** | **4.442 ms** |  **8.951 ms** | **5.265 ms** |
| **UpdateAsync** |          **EfCore** |    **1** | **6.172 ms** | **0.8295 ms** | **1.6755 ms** | **4.280 ms** | **13.216 ms** | **5.808 ms** |
| **UpdateAsync** |          **EfCore** |   **10** | **6.616 ms** | **0.2748 ms** | **0.5551 ms** | **5.609 ms** |  **7.989 ms** | **6.555 ms** |
