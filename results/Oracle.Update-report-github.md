``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-HMWWNW : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |       Mean |     Error |    StdDev |     Median |        Min |        Max |
|------------ |---------------- |----- |-----------:|----------:|----------:|-----------:|-----------:|-----------:|
| **UpdateAsync** |       **Optimized** |    **1** |   **6.662 ms** | **0.3245 ms** | **0.6555 ms** |   **6.528 ms** |   **5.343 ms** |   **8.251 ms** |
| **UpdateAsync** |       **Optimized** |   **10** |   **6.523 ms** | **0.3687 ms** | **0.7448 ms** |   **6.355 ms** |   **5.381 ms** |   **9.975 ms** |
| **UpdateAsync** |       **Optimized** |   **25** |   **6.359 ms** | **0.2863 ms** | **0.5784 ms** |   **6.322 ms** |   **5.582 ms** |   **7.978 ms** |
| **UpdateAsync** |       **Optimized** |   **50** |   **6.525 ms** | **0.2062 ms** | **0.4165 ms** |   **6.551 ms** |   **5.578 ms** |   **7.891 ms** |
| **UpdateAsync** |       **Optimized** |  **100** |   **5.828 ms** | **0.3164 ms** | **0.6392 ms** |   **5.630 ms** |   **4.244 ms** |   **7.298 ms** |
| **UpdateAsync** |       **Optimized** | **1000** |   **9.698 ms** | **0.6758 ms** | **1.3651 ms** |   **9.430 ms** |   **7.915 ms** |  **14.624 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** |   **4.806 ms** | **0.2367 ms** | **0.4781 ms** |   **4.682 ms** |   **4.046 ms** |   **6.251 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **10** |   **4.794 ms** | **0.2563 ms** | **0.5178 ms** |   **4.688 ms** |   **4.166 ms** |   **7.446 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **25** |   **4.673 ms** | **0.1789 ms** | **0.3613 ms** |   **4.633 ms** |   **4.196 ms** |   **5.664 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **50** |   **4.952 ms** | **0.2310 ms** | **0.4667 ms** |   **4.830 ms** |   **4.269 ms** |   **6.236 ms** |
| **UpdateAsync** | **OptimizedDapper** |  **100** |   **5.350 ms** | **0.1818 ms** | **0.3673 ms** |   **5.350 ms** |   **4.532 ms** |   **6.709 ms** |
| **UpdateAsync** | **OptimizedDapper** | **1000** |  **23.011 ms** | **1.5943 ms** | **3.2205 ms** |  **22.199 ms** |  **19.224 ms** |  **34.306 ms** |
| **UpdateAsync** |          **EfCore** |    **1** |   **5.394 ms** | **0.3705 ms** | **0.7484 ms** |   **5.258 ms** |   **4.493 ms** |   **7.998 ms** |
| **UpdateAsync** |          **EfCore** |   **10** |   **6.208 ms** | **0.2872 ms** | **0.5801 ms** |   **6.083 ms** |   **4.911 ms** |   **7.806 ms** |
| **UpdateAsync** |          **EfCore** |   **25** |   **8.650 ms** | **0.3890 ms** | **0.7857 ms** |   **8.560 ms** |   **7.284 ms** |  **11.382 ms** |
| **UpdateAsync** |          **EfCore** |   **50** |  **13.395 ms** | **1.2450 ms** | **2.5150 ms** |  **12.347 ms** |  **10.326 ms** |  **19.695 ms** |
| **UpdateAsync** |          **EfCore** |  **100** |  **18.354 ms** | **0.9821 ms** | **1.9838 ms** |  **17.808 ms** |  **15.870 ms** |  **26.038 ms** |
| **UpdateAsync** |          **EfCore** | **1000** | **128.740 ms** | **4.2187 ms** | **8.5219 ms** | **125.493 ms** | **117.813 ms** | **148.215 ms** |
