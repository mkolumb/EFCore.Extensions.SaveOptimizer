``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-WAPFFE : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |       Mean |     Error |     StdDev |       Min |        Max |     Median |
|------------ |---------------- |------ |-----------:|----------:|-----------:|----------:|-----------:|-----------:|
| **InsertAsync** |       **Optimized** |     **1** |   **7.770 ms** | **0.4263 ms** |  **0.8612 ms** |  **6.613 ms** |  **12.021 ms** |   **7.600 ms** |
| **InsertAsync** |       **Optimized** |    **10** |   **7.716 ms** | **0.2959 ms** |  **0.5978 ms** |  **6.835 ms** |   **9.347 ms** |   **7.537 ms** |
| **InsertAsync** |       **Optimized** |    **25** |   **7.776 ms** | **0.3897 ms** |  **0.7873 ms** |  **6.636 ms** |   **9.735 ms** |   **7.578 ms** |
| **InsertAsync** |       **Optimized** |    **50** |   **8.430 ms** | **0.3318 ms** |  **0.6703 ms** |  **7.303 ms** |  **10.782 ms** |   **8.243 ms** |
| **InsertAsync** |       **Optimized** |   **100** |   **8.828 ms** | **0.2919 ms** |  **0.5897 ms** |  **7.748 ms** |  **10.109 ms** |   **8.708 ms** |
| **InsertAsync** |       **Optimized** |  **1000** |  **14.877 ms** | **0.6548 ms** |  **1.3228 ms** | **12.179 ms** |  **20.426 ms** |  **14.911 ms** |
| **InsertAsync** |       **Optimized** | **10000** |  **91.428 ms** | **1.6719 ms** |  **3.3774 ms** | **85.960 ms** | **102.396 ms** |  **90.266 ms** |
| **InsertAsync** | **OptimizedDapper** |     **1** |   **7.610 ms** | **0.4374 ms** |  **0.8836 ms** |  **6.459 ms** |  **11.579 ms** |   **7.409 ms** |
| **InsertAsync** | **OptimizedDapper** |    **10** |   **6.264 ms** | **0.4093 ms** |  **0.8268 ms** |  **4.912 ms** |   **8.149 ms** |   **6.068 ms** |
| **InsertAsync** | **OptimizedDapper** |    **25** |   **5.942 ms** | **0.2143 ms** |  **0.4329 ms** |  **5.170 ms** |   **6.951 ms** |   **5.861 ms** |
| **InsertAsync** | **OptimizedDapper** |    **50** |   **6.384 ms** | **0.2530 ms** |  **0.5111 ms** |  **5.479 ms** |   **8.339 ms** |   **6.381 ms** |
| **InsertAsync** | **OptimizedDapper** |   **100** |   **6.799 ms** | **0.3007 ms** |  **0.6074 ms** |  **5.900 ms** |   **8.763 ms** |   **6.625 ms** |
| **InsertAsync** | **OptimizedDapper** |  **1000** |  **11.983 ms** | **0.5560 ms** |  **1.1231 ms** | **10.605 ms** |  **16.020 ms** |  **11.649 ms** |
| **InsertAsync** | **OptimizedDapper** | **10000** |  **75.863 ms** | **1.1329 ms** |  **2.2885 ms** | **72.286 ms** |  **84.066 ms** |  **75.462 ms** |
| **InsertAsync** |          **EfCore** |     **1** |   **5.472 ms** | **0.3379 ms** |  **0.6826 ms** |  **4.714 ms** |   **7.959 ms** |   **5.243 ms** |
| **InsertAsync** |          **EfCore** |    **10** |   **5.768 ms** | **0.5213 ms** |  **1.0530 ms** |  **4.715 ms** |   **9.730 ms** |   **5.549 ms** |
| **InsertAsync** |          **EfCore** |    **25** |   **7.288 ms** | **0.2829 ms** |  **0.5714 ms** |  **6.325 ms** |   **8.451 ms** |   **7.164 ms** |
| **InsertAsync** |          **EfCore** |    **50** |   **7.883 ms** | **0.2933 ms** |  **0.5926 ms** |  **6.560 ms** |   **9.147 ms** |   **7.892 ms** |
| **InsertAsync** |          **EfCore** |   **100** |   **8.794 ms** | **0.3254 ms** |  **0.6574 ms** |  **6.968 ms** |  **10.473 ms** |   **8.885 ms** |
| **InsertAsync** |          **EfCore** |  **1000** |  **16.865 ms** | **0.6152 ms** |  **1.2427 ms** | **14.963 ms** |  **20.814 ms** |  **16.787 ms** |
| **InsertAsync** |          **EfCore** | **10000** | **112.428 ms** | **8.5518 ms** | **17.2751 ms** | **99.440 ms** | **202.668 ms** | **107.361 ms** |
