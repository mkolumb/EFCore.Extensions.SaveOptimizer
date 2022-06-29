``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-LFETAU : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |       Mean |      Error |     StdDev |     Median |        Min |        Max |
|------------ |---------------- |------ |-----------:|-----------:|-----------:|-----------:|-----------:|-----------:|
| **InsertAsync** |       **Optimized** |     **1** |   **3.464 ms** |  **0.2861 ms** |  **0.5779 ms** |   **3.278 ms** |   **2.653 ms** |   **5.527 ms** |
| **InsertAsync** |       **Optimized** |    **10** |   **3.985 ms** |  **0.3870 ms** |  **0.7817 ms** |   **3.893 ms** |   **3.007 ms** |   **7.431 ms** |
| **InsertAsync** |       **Optimized** |    **25** |   **4.009 ms** |  **0.4441 ms** |  **0.8971 ms** |   **3.692 ms** |   **2.861 ms** |   **6.310 ms** |
| **InsertAsync** |       **Optimized** |    **50** |   **3.624 ms** |  **0.2784 ms** |  **0.5624 ms** |   **3.478 ms** |   **3.113 ms** |   **6.618 ms** |
| **InsertAsync** |       **Optimized** |   **100** |   **4.451 ms** |  **0.2156 ms** |  **0.4355 ms** |   **4.376 ms** |   **3.881 ms** |   **5.896 ms** |
| **InsertAsync** |       **Optimized** |  **1000** |  **16.346 ms** |  **1.4173 ms** |  **2.8630 ms** |  **15.559 ms** |  **12.420 ms** |  **23.981 ms** |
| **InsertAsync** |       **Optimized** | **10000** | **150.149 ms** |  **9.9580 ms** | **20.1157 ms** | **144.615 ms** | **120.848 ms** | **216.298 ms** |
| **InsertAsync** | **OptimizedDapper** |     **1** |   **2.179 ms** |  **0.1223 ms** |  **0.2470 ms** |   **2.140 ms** |   **1.784 ms** |   **3.356 ms** |
| **InsertAsync** | **OptimizedDapper** |    **10** |   **2.385 ms** |  **0.1282 ms** |  **0.2589 ms** |   **2.292 ms** |   **1.919 ms** |   **3.093 ms** |
| **InsertAsync** | **OptimizedDapper** |    **25** |   **3.098 ms** |  **0.5656 ms** |  **1.1425 ms** |   **2.638 ms** |   **2.143 ms** |   **7.595 ms** |
| **InsertAsync** | **OptimizedDapper** |    **50** |   **2.991 ms** |  **0.2359 ms** |  **0.4766 ms** |   **2.856 ms** |   **2.387 ms** |   **4.512 ms** |
| **InsertAsync** | **OptimizedDapper** |   **100** |   **3.590 ms** |  **0.1735 ms** |  **0.3504 ms** |   **3.531 ms** |   **3.020 ms** |   **4.848 ms** |
| **InsertAsync** | **OptimizedDapper** |  **1000** |  **15.868 ms** |  **1.5332 ms** |  **3.0971 ms** |  **14.824 ms** |  **12.404 ms** |  **24.100 ms** |
| **InsertAsync** | **OptimizedDapper** | **10000** | **191.083 ms** | **16.4268 ms** | **33.1830 ms** | **179.792 ms** | **152.891 ms** | **308.261 ms** |
| **InsertAsync** |          **EfCore** |     **1** |   **2.644 ms** |  **0.1272 ms** |  **0.2570 ms** |   **2.587 ms** |   **2.310 ms** |   **3.534 ms** |
| **InsertAsync** |          **EfCore** |    **10** |   **6.008 ms** |  **1.0328 ms** |  **2.0864 ms** |   **7.034 ms** |   **2.438 ms** |   **8.477 ms** |
| **InsertAsync** |          **EfCore** |    **25** |   **6.354 ms** |  **1.0379 ms** |  **2.0966 ms** |   **7.214 ms** |   **3.058 ms** |   **9.491 ms** |
| **InsertAsync** |          **EfCore** |    **50** |   **8.047 ms** |  **1.2245 ms** |  **2.4736 ms** |   **8.438 ms** |   **4.818 ms** |  **20.100 ms** |
| **InsertAsync** |          **EfCore** |   **100** |  **10.571 ms** |  **1.3497 ms** |  **2.7264 ms** |  **10.956 ms** |   **6.830 ms** |  **23.855 ms** |
| **InsertAsync** |          **EfCore** |  **1000** |  **60.884 ms** |  **2.6366 ms** |  **5.3261 ms** |  **60.319 ms** |  **51.398 ms** |  **74.814 ms** |
| **InsertAsync** |          **EfCore** | **10000** | **644.266 ms** | **22.8232 ms** | **46.1039 ms** | **639.566 ms** | **580.672 ms** | **739.186 ms** |
