``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-WAPFFE : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |       Mean |      Error |     StdDev |        Min |        Max |     Median |
|------------ |---------------- |------ |-----------:|-----------:|-----------:|-----------:|-----------:|-----------:|
| **DeleteAsync** |       **Optimized** |     **1** |   **6.351 ms** |  **0.5871 ms** |  **1.1861 ms** |   **5.008 ms** |  **10.826 ms** |   **5.941 ms** |
| **DeleteAsync** |       **Optimized** |    **10** |   **5.782 ms** |  **0.3081 ms** |  **0.6224 ms** |   **5.008 ms** |   **8.063 ms** |   **5.652 ms** |
| **DeleteAsync** |       **Optimized** |    **25** |   **5.922 ms** |  **0.2274 ms** |  **0.4593 ms** |   **5.016 ms** |   **7.452 ms** |   **5.860 ms** |
| **DeleteAsync** |       **Optimized** |    **50** |   **6.088 ms** |  **0.2878 ms** |  **0.5814 ms** |   **5.167 ms** |   **7.995 ms** |   **5.947 ms** |
| **DeleteAsync** |       **Optimized** |   **100** |   **6.425 ms** |  **0.2759 ms** |  **0.5573 ms** |   **5.267 ms** |   **7.977 ms** |   **6.315 ms** |
| **DeleteAsync** |       **Optimized** |  **1000** |  **10.608 ms** |  **0.5218 ms** |  **1.0541 ms** |   **9.308 ms** |  **15.426 ms** |  **10.454 ms** |
| **DeleteAsync** |       **Optimized** | **10000** |  **60.845 ms** |  **4.0571 ms** |  **8.1956 ms** |  **54.608 ms** | **109.857 ms** |  **59.535 ms** |
| **DeleteAsync** | **OptimizedDapper** |     **1** |   **5.587 ms** |  **0.1987 ms** |  **0.4013 ms** |   **4.638 ms** |   **6.448 ms** |   **5.592 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **10** |   **6.490 ms** |  **0.6899 ms** |  **1.3936 ms** |   **4.749 ms** |  **10.417 ms** |   **6.115 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **25** |   **8.377 ms** |  **0.3968 ms** |  **0.8016 ms** |   **6.379 ms** |   **9.856 ms** |   **8.497 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **50** |   **8.257 ms** |  **0.3941 ms** |  **0.7961 ms** |   **7.024 ms** |  **11.489 ms** |   **8.222 ms** |
| **DeleteAsync** | **OptimizedDapper** |   **100** |   **8.777 ms** |  **0.4924 ms** |  **0.9946 ms** |   **7.290 ms** |  **13.602 ms** |   **8.616 ms** |
| **DeleteAsync** | **OptimizedDapper** |  **1000** |  **14.462 ms** |  **0.8522 ms** |  **1.7214 ms** |  **12.758 ms** |  **21.042 ms** |  **14.165 ms** |
| **DeleteAsync** | **OptimizedDapper** | **10000** |  **79.499 ms** |  **3.0648 ms** |  **6.1910 ms** |  **68.458 ms** | **106.387 ms** |  **79.739 ms** |
| **DeleteAsync** |          **EfCore** |     **1** |   **7.118 ms** |  **0.2465 ms** |  **0.4980 ms** |   **6.281 ms** |   **8.657 ms** |   **7.018 ms** |
| **DeleteAsync** |          **EfCore** |    **10** |   **7.578 ms** |  **0.3586 ms** |  **0.7244 ms** |   **6.381 ms** |   **9.508 ms** |   **7.508 ms** |
| **DeleteAsync** |          **EfCore** |    **25** |   **7.879 ms** |  **0.2381 ms** |  **0.4809 ms** |   **7.067 ms** |   **9.087 ms** |   **7.837 ms** |
| **DeleteAsync** |          **EfCore** |    **50** |   **8.915 ms** |  **0.3978 ms** |  **0.8035 ms** |   **7.584 ms** |  **10.453 ms** |   **8.850 ms** |
| **DeleteAsync** |          **EfCore** |   **100** |  **11.258 ms** |  **1.1242 ms** |  **2.2710 ms** |   **7.930 ms** |  **20.556 ms** |  **10.897 ms** |
| **DeleteAsync** |          **EfCore** |  **1000** |  **43.492 ms** |  **5.2158 ms** | **10.5362 ms** |  **29.509 ms** |  **70.154 ms** |  **41.198 ms** |
| **DeleteAsync** |          **EfCore** | **10000** | **347.740 ms** | **12.1466 ms** | **24.5368 ms** | **307.292 ms** | **418.616 ms** | **341.028 ms** |
