``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-DFVVOF : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |       Mean |      Error |     StdDev |     Median |        Min |        Max |
|------------ |---------------- |------ |-----------:|-----------:|-----------:|-----------:|-----------:|-----------:|
| **UpdateAsync** |       **Optimized** |     **1** |   **7.090 ms** |  **0.2954 ms** |  **0.5966 ms** |   **7.045 ms** |   **6.019 ms** |   **8.935 ms** |
| **UpdateAsync** |       **Optimized** |    **10** |   **7.101 ms** |  **0.3380 ms** |  **0.6828 ms** |   **7.011 ms** |   **5.847 ms** |  **10.159 ms** |
| **UpdateAsync** |       **Optimized** |    **25** |   **7.150 ms** |  **0.2939 ms** |  **0.5937 ms** |   **7.011 ms** |   **6.118 ms** |   **9.287 ms** |
| **UpdateAsync** |       **Optimized** |    **50** |   **8.585 ms** |  **0.4044 ms** |  **0.8168 ms** |   **8.607 ms** |   **6.937 ms** |  **10.712 ms** |
| **UpdateAsync** |       **Optimized** |   **100** |   **9.341 ms** |  **0.3488 ms** |  **0.7047 ms** |   **9.223 ms** |   **7.824 ms** |  **11.118 ms** |
| **UpdateAsync** |       **Optimized** |  **1000** |  **14.371 ms** |  **1.5001 ms** |  **3.0303 ms** |  **13.516 ms** |  **11.376 ms** |  **28.396 ms** |
| **UpdateAsync** |       **Optimized** | **10000** |  **55.094 ms** |  **1.2154 ms** |  **2.4552 ms** |  **54.854 ms** |  **51.450 ms** |  **60.794 ms** |
| **UpdateAsync** | **OptimizedDapper** |     **1** |   **7.279 ms** |  **0.5304 ms** |  **1.0714 ms** |   **7.210 ms** |   **5.871 ms** |  **12.638 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **10** |   **7.068 ms** |  **0.2711 ms** |  **0.5476 ms** |   **7.006 ms** |   **6.115 ms** |   **9.148 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **25** |   **7.164 ms** |  **0.2775 ms** |  **0.5606 ms** |   **6.972 ms** |   **6.298 ms** |   **8.828 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **50** |   **7.303 ms** |  **0.3165 ms** |  **0.6394 ms** |   **7.126 ms** |   **6.395 ms** |   **9.306 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **100** |   **7.640 ms** |  **0.2992 ms** |  **0.6044 ms** |   **7.504 ms** |   **6.767 ms** |   **9.369 ms** |
| **UpdateAsync** | **OptimizedDapper** |  **1000** |  **11.782 ms** |  **0.6262 ms** |  **1.2649 ms** |  **11.501 ms** |   **9.755 ms** |  **16.537 ms** |
| **UpdateAsync** | **OptimizedDapper** | **10000** |  **56.166 ms** |  **1.3371 ms** |  **2.7011 ms** |  **56.189 ms** |  **52.418 ms** |  **63.578 ms** |
| **UpdateAsync** |          **EfCore** |     **1** |   **7.026 ms** |  **0.3870 ms** |  **0.7817 ms** |   **6.793 ms** |   **6.166 ms** |  **10.624 ms** |
| **UpdateAsync** |          **EfCore** |    **10** |   **7.232 ms** |  **0.3058 ms** |  **0.6178 ms** |   **7.163 ms** |   **5.778 ms** |   **9.118 ms** |
| **UpdateAsync** |          **EfCore** |    **25** |   **8.270 ms** |  **0.3873 ms** |  **0.7823 ms** |   **8.214 ms** |   **7.030 ms** |  **10.621 ms** |
| **UpdateAsync** |          **EfCore** |    **50** |   **9.299 ms** |  **0.4160 ms** |  **0.8403 ms** |   **9.175 ms** |   **7.853 ms** |  **12.826 ms** |
| **UpdateAsync** |          **EfCore** |   **100** |  **11.505 ms** |  **0.6849 ms** |  **1.3836 ms** |  **11.627 ms** |   **8.699 ms** |  **16.601 ms** |
| **UpdateAsync** |          **EfCore** |  **1000** |  **50.600 ms** |  **2.2678 ms** |  **4.5810 ms** |  **49.868 ms** |  **44.404 ms** |  **62.549 ms** |
| **UpdateAsync** |          **EfCore** | **10000** | **496.996 ms** | **29.4413 ms** | **59.4729 ms** | **461.261 ms** | **423.987 ms** | **596.001 ms** |
