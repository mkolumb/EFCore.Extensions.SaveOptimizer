``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-JLACHD : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |      Mean |     Error |    StdDev |   Median |      Min |       Max |
|------------ |---------------- |----- |----------:|----------:|----------:|---------:|---------:|----------:|
| **UpdateAsync** |       **Optimized** |    **1** |  **5.474 ms** | **0.3779 ms** | **0.7635 ms** | **5.312 ms** | **4.526 ms** |  **8.727 ms** |
| **UpdateAsync** |       **Optimized** |   **10** |  **5.538 ms** | **0.4160 ms** | **0.8403 ms** | **5.194 ms** | **4.741 ms** |  **8.688 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** |  **6.183 ms** | **0.5880 ms** | **1.1878 ms** | **5.901 ms** | **4.622 ms** | **10.397 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **10** |  **5.609 ms** | **0.3678 ms** | **0.7430 ms** | **5.471 ms** | **4.678 ms** |  **8.404 ms** |
| **UpdateAsync** |          **EfCore** |    **1** |  **6.458 ms** | **0.5465 ms** | **1.1040 ms** | **6.049 ms** | **5.469 ms** | **11.366 ms** |
| **UpdateAsync** |          **EfCore** |   **10** | **10.210 ms** | **0.5055 ms** | **1.0212 ms** | **9.833 ms** | **8.819 ms** | **13.788 ms** |
