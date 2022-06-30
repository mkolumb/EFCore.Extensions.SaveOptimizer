``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-DQDBFV : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |       Mean |      Error |     StdDev |     Median |        Min |        Max |
|------------ |---------------- |----- |-----------:|-----------:|-----------:|-----------:|-----------:|-----------:|
| **Insert** |       **Optimized** |    **1** |   **6.804 ms** |  **0.3182 ms** |  **0.6427 ms** |   **6.693 ms** |   **5.800 ms** |   **9.135 ms** |
| **Insert** |       **Optimized** |   **10** |  **15.204 ms** |  **0.8693 ms** |  **1.7559 ms** |  **14.897 ms** |  **13.524 ms** |  **24.840 ms** |
| **Insert** |       **Optimized** |   **25** |  **29.466 ms** |  **0.7585 ms** |  **1.5322 ms** |  **29.550 ms** |  **26.705 ms** |  **32.729 ms** |
| **Insert** |       **Optimized** |   **50** |  **51.041 ms** |  **0.8997 ms** |  **1.8175 ms** |  **50.787 ms** |  **47.404 ms** |  **54.480 ms** |
| **Insert** |       **Optimized** |  **100** |  **89.280 ms** |  **5.1230 ms** | **10.3486 ms** |  **93.604 ms** |  **73.899 ms** | **111.031 ms** |
| **Insert** |       **Optimized** | **1000** | **815.141 ms** | **43.3580 ms** | **87.5853 ms** | **865.813 ms** | **689.971 ms** | **939.712 ms** |
| **Insert** | **Optimized Dapper** |    **1** |   **6.551 ms** |  **0.3017 ms** |  **0.6094 ms** |   **6.381 ms** |   **5.730 ms** |   **8.675 ms** |
| **Insert** | **Optimized Dapper** |   **10** |  **15.192 ms** |  **0.7110 ms** |  **1.4363 ms** |  **14.912 ms** |  **12.989 ms** |  **21.607 ms** |
| **Insert** | **Optimized Dapper** |   **25** |  **28.603 ms** |  **1.0364 ms** |  **2.0935 ms** |  **28.668 ms** |  **25.987 ms** |  **35.677 ms** |
| **Insert** | **Optimized Dapper** |   **50** |  **51.219 ms** |  **1.2653 ms** |  **2.5560 ms** |  **50.771 ms** |  **46.760 ms** |  **59.289 ms** |
| **Insert** | **Optimized Dapper** |  **100** |  **97.559 ms** |  **1.7180 ms** |  **3.4705 ms** |  **97.168 ms** |  **91.133 ms** | **109.335 ms** |
| **Insert** | **Optimized Dapper** | **1000** | **790.467 ms** | **41.8714 ms** | **84.5822 ms** | **734.322 ms** | **693.925 ms** | **916.274 ms** |
| **Insert** |          **EF Core** |    **1** |   **6.183 ms** |  **0.4498 ms** |  **0.9086 ms** |   **5.735 ms** |   **5.065 ms** |   **9.408 ms** |
| **Insert** |          **EF Core** |   **10** |  **13.672 ms** |  **1.1478 ms** |  **2.3186 ms** |  **12.834 ms** |  **11.394 ms** |  **23.137 ms** |
| **Insert** |          **EF Core** |   **25** |  **29.150 ms** |  **0.7880 ms** |  **1.5918 ms** |  **29.032 ms** |  **26.577 ms** |  **32.491 ms** |
| **Insert** |          **EF Core** |   **50** |  **51.906 ms** |  **1.2283 ms** |  **2.4812 ms** |  **51.464 ms** |  **48.476 ms** |  **59.710 ms** |
| **Insert** |          **EF Core** |  **100** |  **95.826 ms** |  **1.2921 ms** |  **2.6100 ms** |  **95.659 ms** |  **90.935 ms** | **103.675 ms** |
| **Insert** |          **EF Core** | **1000** | **825.726 ms** | **45.0771 ms** | **91.0580 ms** | **878.253 ms** | **699.593 ms** | **935.943 ms** |
