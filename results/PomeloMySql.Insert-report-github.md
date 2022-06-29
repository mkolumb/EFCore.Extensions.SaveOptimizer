``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-DFVVOF : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |       Mean |     Error |    StdDev |        Min |        Max |     Median |
|------------ |---------------- |------ |-----------:|----------:|----------:|-----------:|-----------:|-----------:|
| **InsertAsync** |       **Optimized** |     **1** |   **8.354 ms** | **0.3426 ms** | **0.6920 ms** |   **7.341 ms** |  **10.821 ms** |   **8.338 ms** |
| **InsertAsync** |       **Optimized** |    **10** |   **8.270 ms** | **0.3246 ms** | **0.6556 ms** |   **7.084 ms** |  **10.560 ms** |   **8.180 ms** |
| **InsertAsync** |       **Optimized** |    **25** |   **8.715 ms** | **0.3956 ms** | **0.7990 ms** |   **7.241 ms** |  **10.865 ms** |   **8.672 ms** |
| **InsertAsync** |       **Optimized** |    **50** |   **9.060 ms** | **0.2782 ms** | **0.5621 ms** |   **8.003 ms** |  **11.092 ms** |   **8.975 ms** |
| **InsertAsync** |       **Optimized** |   **100** |   **9.807 ms** | **0.3767 ms** | **0.7610 ms** |   **8.493 ms** |  **11.919 ms** |   **9.618 ms** |
| **InsertAsync** |       **Optimized** |  **1000** |  **16.826 ms** | **0.6771 ms** | **1.3678 ms** |  **14.492 ms** |  **20.571 ms** |  **16.687 ms** |
| **InsertAsync** |       **Optimized** | **10000** | **113.334 ms** | **4.1489 ms** | **8.3811 ms** |  **96.380 ms** | **130.172 ms** | **114.208 ms** |
| **InsertAsync** | **OptimizedDapper** |     **1** |   **9.627 ms** | **0.4665 ms** | **0.9424 ms** |   **8.209 ms** |  **12.037 ms** |   **9.457 ms** |
| **InsertAsync** | **OptimizedDapper** |    **10** |   **9.694 ms** | **0.4630 ms** | **0.9353 ms** |   **7.976 ms** |  **12.504 ms** |   **9.584 ms** |
| **InsertAsync** | **OptimizedDapper** |    **25** |   **9.949 ms** | **0.4959 ms** | **1.0017 ms** |   **8.255 ms** |  **13.094 ms** |   **9.849 ms** |
| **InsertAsync** | **OptimizedDapper** |    **50** |   **9.903 ms** | **1.3980 ms** | **2.8241 ms** |   **7.183 ms** |  **21.274 ms** |   **9.475 ms** |
| **InsertAsync** | **OptimizedDapper** |   **100** |   **8.535 ms** | **0.4632 ms** | **0.9358 ms** |   **7.457 ms** |  **12.067 ms** |   **8.261 ms** |
| **InsertAsync** | **OptimizedDapper** |  **1000** |  **14.072 ms** | **0.5693 ms** | **1.1501 ms** |  **12.268 ms** |  **17.643 ms** |  **14.003 ms** |
| **InsertAsync** | **OptimizedDapper** | **10000** |  **90.998 ms** | **1.3568 ms** | **2.7407 ms** |  **85.685 ms** |  **96.050 ms** |  **90.603 ms** |
| **InsertAsync** |          **EfCore** |     **1** |   **7.177 ms** | **0.3340 ms** | **0.6746 ms** |   **6.151 ms** |   **9.805 ms** |   **7.078 ms** |
| **InsertAsync** |          **EfCore** |    **10** |   **7.334 ms** | **0.3742 ms** | **0.7559 ms** |   **5.938 ms** |   **9.337 ms** |   **7.097 ms** |
| **InsertAsync** |          **EfCore** |    **25** |   **7.435 ms** | **0.2922 ms** | **0.5903 ms** |   **6.680 ms** |   **9.692 ms** |   **7.309 ms** |
| **InsertAsync** |          **EfCore** |    **50** |   **8.212 ms** | **0.5643 ms** | **1.1398 ms** |   **6.643 ms** |  **13.436 ms** |   **7.864 ms** |
| **InsertAsync** |          **EfCore** |   **100** |  **10.074 ms** | **0.5911 ms** | **1.1940 ms** |   **8.054 ms** |  **13.546 ms** |   **9.814 ms** |
| **InsertAsync** |          **EfCore** |  **1000** |  **21.556 ms** | **0.8113 ms** | **1.6388 ms** |  **18.977 ms** |  **25.464 ms** |  **21.307 ms** |
| **InsertAsync** |          **EfCore** | **10000** | **108.686 ms** | **1.8449 ms** | **3.7269 ms** | **102.207 ms** | **116.124 ms** | **108.254 ms** |
