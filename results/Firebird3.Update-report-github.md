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
| **Update** |       **Optimized** |    **1** |   **5.073 ms** |  **0.3751 ms** |  **0.7578 ms** |   **4.839 ms** |   **4.161 ms** |   **7.968 ms** |
| **Update** |       **Optimized** |   **10** |   **5.238 ms** |  **0.4014 ms** |  **0.8109 ms** |   **4.956 ms** |   **4.488 ms** |   **8.743 ms** |
| **Update** |       **Optimized** |   **25** |   **5.983 ms** |  **0.3433 ms** |  **0.6935 ms** |   **5.792 ms** |   **5.114 ms** |   **8.109 ms** |
| **Update** |       **Optimized** |   **50** |   **6.726 ms** |  **0.2767 ms** |  **0.5589 ms** |   **6.647 ms** |   **5.767 ms** |   **8.793 ms** |
| **Update** |       **Optimized** |  **100** |   **7.846 ms** |  **0.6112 ms** |  **1.2347 ms** |   **7.500 ms** |   **6.623 ms** |  **14.458 ms** |
| **Update** |       **Optimized** | **1000** |  **40.559 ms** |  **1.2396 ms** |  **2.5041 ms** |  **40.334 ms** |  **35.929 ms** |  **46.748 ms** |
| **Update** | **Optimized Dapper** |    **1** |   **6.134 ms** |  **0.4585 ms** |  **0.9263 ms** |   **5.859 ms** |   **5.100 ms** |  **10.133 ms** |
| **Update** | **Optimized Dapper** |   **10** |   **6.704 ms** |  **1.3568 ms** |  **2.7409 ms** |   **6.033 ms** |   **5.207 ms** |  **23.227 ms** |
| **Update** | **Optimized Dapper** |   **25** |   **6.496 ms** |  **0.2901 ms** |  **0.5861 ms** |   **6.431 ms** |   **5.598 ms** |   **8.331 ms** |
| **Update** | **Optimized Dapper** |   **50** |   **7.143 ms** |  **0.6239 ms** |  **1.2603 ms** |   **6.810 ms** |   **5.994 ms** |  **13.017 ms** |
| **Update** | **Optimized Dapper** |  **100** |   **7.752 ms** |  **0.2243 ms** |  **0.4531 ms** |   **7.686 ms** |   **7.107 ms** |   **9.819 ms** |
| **Update** | **Optimized Dapper** | **1000** |  **41.196 ms** |  **1.6532 ms** |  **3.3395 ms** |  **41.010 ms** |  **35.804 ms** |  **51.266 ms** |
| **Update** |          **EF Core** |    **1** |   **5.277 ms** |  **0.2759 ms** |  **0.5573 ms** |   **5.087 ms** |   **4.663 ms** |   **7.539 ms** |
| **Update** |          **EF Core** |   **10** |   **9.070 ms** |  **0.3313 ms** |  **0.6692 ms** |   **8.951 ms** |   **8.350 ms** |  **12.079 ms** |
| **Update** |          **EF Core** |   **25** |  **16.073 ms** |  **0.5311 ms** |  **1.0729 ms** |  **15.957 ms** |  **14.274 ms** |  **19.305 ms** |
| **Update** |          **EF Core** |   **50** |  **28.234 ms** |  **1.6848 ms** |  **3.4033 ms** |  **26.788 ms** |  **23.056 ms** |  **38.503 ms** |
| **Update** |          **EF Core** |  **100** |  **58.172 ms** |  **0.9909 ms** |  **2.0016 ms** |  **57.809 ms** |  **54.494 ms** |  **64.035 ms** |
| **Update** |          **EF Core** | **1000** | **484.173 ms** | **29.6751 ms** | **59.9451 ms** | **520.561 ms** | **401.661 ms** | **584.747 ms** |
