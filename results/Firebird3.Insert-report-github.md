``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-YLGLZT : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |      Mean |     Error |    StdDev |       Min |       Max |    Median |
|------------ |---------------- |----- |----------:|----------:|----------:|----------:|----------:|----------:|
| **InsertAsync** |       **Optimized** |    **1** |  **6.169 ms** | **0.3426 ms** | **0.6920 ms** |  **5.447 ms** |  **9.739 ms** |  **5.992 ms** |
| **InsertAsync** |       **Optimized** |   **10** | **13.403 ms** | **0.5220 ms** | **1.0546 ms** | **11.754 ms** | **16.838 ms** | **13.229 ms** |
| **InsertAsync** | **OptimizedDapper** |    **1** |  **6.477 ms** | **0.3708 ms** | **0.7490 ms** |  **5.487 ms** |  **9.074 ms** |  **6.431 ms** |
| **InsertAsync** | **OptimizedDapper** |   **10** | **13.406 ms** | **0.5067 ms** | **1.0235 ms** | **11.954 ms** | **16.802 ms** | **13.025 ms** |
| **InsertAsync** |          **EfCore** |    **1** |  **6.597 ms** | **0.3024 ms** | **0.6109 ms** |  **5.678 ms** |  **9.366 ms** |  **6.511 ms** |
| **InsertAsync** |          **EfCore** |   **10** | **13.679 ms** | **0.4150 ms** | **0.8384 ms** | **12.172 ms** | **16.560 ms** | **13.647 ms** |
