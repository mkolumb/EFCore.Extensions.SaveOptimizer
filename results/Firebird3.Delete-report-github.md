``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]    : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Firebird3 : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Job=Firebird3  EvaluateOverhead=True  OutlierMode=RemoveUpper  
InvocationCount=1  IterationCount=20  LaunchCount=3  
RunStrategy=ColdStart  UnrollFactor=1  WarmupCount=0  

```
|      Method |         Variant | Rows |     MeanNoOut |        Mean |          Min |          Q1 |      Median |          Q3 |         Max |
|------------ |---------------- |----- |--------------:|------------:|-------------:|------------:|------------:|------------:|------------:|
| **Delete** |       **Optimized** |    **1** |    **13.5933 ms** |    **13.87 ms** |    **11.582 ms** |    **12.98 ms** |    **13.55 ms** |    **14.39 ms** |    **19.50 ms** |
| **Delete** |       **Optimized** |   **10** |    **14.0894 ms** |    **13.96 ms** |    **11.222 ms** |    **13.14 ms** |    **14.13 ms** |    **14.66 ms** |    **16.53 ms** |
| **Delete** |       **Optimized** |   **25** |    **21.8765 ms** |    **21.92 ms** |    **16.654 ms** |    **19.43 ms** |    **21.81 ms** |    **24.00 ms** |    **29.51 ms** |
| **Delete** |       **Optimized** |   **50** |    **33.5627 ms** |    **33.74 ms** |    **21.007 ms** |    **26.34 ms** |    **33.54 ms** |    **40.82 ms** |    **50.50 ms** |
| **Delete** |       **Optimized** |  **100** |    **61.3628 ms** |    **63.29 ms** |    **29.712 ms** |    **44.58 ms** |    **61.01 ms** |    **80.65 ms** |   **130.92 ms** |
| **Delete** |       **Optimized** | **1000** |   **499.4978 ms** |   **496.51 ms** |   **275.297 ms** |   **361.89 ms** |   **502.82 ms** |   **618.50 ms** |   **768.00 ms** |
| **Delete** | **Optimized Dapper** |    **1** |    **11.7876 ms** |    **11.74 ms** |     **9.330 ms** |    **11.09 ms** |    **11.88 ms** |    **12.52 ms** |    **13.77 ms** |
| **Delete** | **Optimized Dapper** |   **10** |    **13.0220 ms** |    **13.05 ms** |    **10.253 ms** |    **12.13 ms** |    **13.10 ms** |    **13.78 ms** |    **17.45 ms** |
| **Delete** | **Optimized Dapper** |   **25** |    **20.6388 ms** |    **20.76 ms** |    **15.786 ms** |    **18.73 ms** |    **20.80 ms** |    **22.72 ms** |    **27.16 ms** |
| **Delete** | **Optimized Dapper** |   **50** |    **30.6296 ms** |    **30.47 ms** |    **18.968 ms** |    **24.33 ms** |    **31.32 ms** |    **35.78 ms** |    **43.48 ms** |
| **Delete** | **Optimized Dapper** |  **100** |    **64.5764 ms** |    **63.72 ms** |    **27.897 ms** |    **44.86 ms** |    **64.42 ms** |    **79.57 ms** |   **107.15 ms** |
| **Delete** | **Optimized Dapper** | **1000** |   **490.9668 ms** |   **479.04 ms** |   **242.018 ms** |   **367.71 ms** |   **498.21 ms** |   **595.46 ms** |   **736.36 ms** |
| **Delete** |          **EF Core** |    **1** |    **15.7701 ms** |    **16.05 ms** |    **13.567 ms** |    **14.99 ms** |    **15.68 ms** |    **16.83 ms** |    **19.42 ms** |
| **Delete** |          **EF Core** |   **10** |    **48.8438 ms** |    **49.63 ms** |    **42.148 ms** |    **46.19 ms** |    **48.55 ms** |    **52.22 ms** |    **62.48 ms** |
| **Delete** |          **EF Core** |   **25** |   **110.5237 ms** |   **113.15 ms** |    **95.608 ms** |   **105.15 ms** |   **108.84 ms** |   **119.81 ms** |   **147.82 ms** |
| **Delete** |          **EF Core** |   **50** |   **208.5536 ms** |   **215.19 ms** |   **176.151 ms** |   **196.88 ms** |   **205.40 ms** |   **232.50 ms** |   **286.66 ms** |
| **Delete** |          **EF Core** |  **100** |   **399.2936 ms** |   **401.64 ms** |   **368.475 ms** |   **386.55 ms** |   **399.52 ms** |   **411.22 ms** |   **445.10 ms** |
| **Delete** |          **EF Core** | **1000** | **4,718.5511 ms** | **4,551.37 ms** | **3,680.036 ms** | **3,855.63 ms** | **4,876.22 ms** | **4,971.74 ms** | **5,089.14 ms** |
