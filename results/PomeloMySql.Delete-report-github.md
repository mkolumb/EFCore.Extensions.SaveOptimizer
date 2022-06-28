``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-ZPTBWN : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |   StdDev |   Median |      Min |      Max |
|------------ |---------------- |----- |---------:|----------:|---------:|---------:|---------:|---------:|
| **DeleteAsync** |       **Optimized** |    **1** | **7.836 ms** | **1.0908 ms** | **2.203 ms** | **7.206 ms** | **5.355 ms** | **16.94 ms** |
| **DeleteAsync** |       **Optimized** |   **10** | **8.430 ms** | **1.1518 ms** | **2.327 ms** | **7.528 ms** | **5.297 ms** | **13.98 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **7.880 ms** | **0.7976 ms** | **1.611 ms** | **7.438 ms** | **5.948 ms** | **15.01 ms** |
| **DeleteAsync** | **OptimizedDapper** |   **10** | **7.945 ms** | **0.8633 ms** | **1.744 ms** | **7.357 ms** | **5.757 ms** | **13.09 ms** |
| **DeleteAsync** |          **EfCore** |    **1** | **7.669 ms** | **0.5625 ms** | **1.136 ms** | **7.500 ms** | **6.056 ms** | **11.48 ms** |
| **DeleteAsync** |          **EfCore** |   **10** | **8.450 ms** | **0.8213 ms** | **1.659 ms** | **7.952 ms** | **6.591 ms** | **12.97 ms** |
