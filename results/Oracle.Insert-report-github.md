``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host] : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Oracle : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Job=Oracle  EvaluateOverhead=True  OutlierMode=RemoveUpper  
InvocationCount=1  IterationCount=20  LaunchCount=3  
RunStrategy=ColdStart  UnrollFactor=1  WarmupCount=0  

```
|      Method |         Variant | Rows |     MeanNoOut |        Mean |        Min |          Q1 |      Median |          Q3 |         Max |
|------------ |---------------- |----- |--------------:|------------:|-----------:|------------:|------------:|------------:|------------:|
| **Insert** |       **Optimized** |    **1** |    **11.3909 ms** |    **12.19 ms** |   **8.312 ms** |    **10.00 ms** |    **11.62 ms** |    **13.07 ms** |    **25.02 ms** |
| **Insert** |       **Optimized** |   **10** |    **11.5027 ms** |    **11.71 ms** |  **10.139 ms** |    **11.15 ms** |    **11.51 ms** |    **12.05 ms** |    **15.82 ms** |
| **Insert** |       **Optimized** |   **25** |    **15.1088 ms** |    **15.23 ms** |  **12.852 ms** |    **13.88 ms** |    **15.14 ms** |    **16.04 ms** |    **19.10 ms** |
| **Insert** |       **Optimized** |   **50** |    **20.6463 ms** |    **21.05 ms** |  **18.258 ms** |    **19.77 ms** |    **20.39 ms** |    **22.46 ms** |    **26.35 ms** |
| **Insert** |       **Optimized** |  **100** |    **33.7647 ms** |    **34.16 ms** |  **29.939 ms** |    **31.87 ms** |    **33.77 ms** |    **35.35 ms** |    **41.32 ms** |
| **Insert** |       **Optimized** | **1000** |   **372.6665 ms** |   **449.23 ms** | **236.810 ms** |   **261.61 ms** |   **398.43 ms** |   **494.14 ms** | **1,056.78 ms** |
| **Insert** | **Optimized Dapper** |    **1** |    **11.8899 ms** |    **13.54 ms** |   **8.625 ms** |    **10.09 ms** |    **11.78 ms** |    **15.31 ms** |    **25.75 ms** |
| **Insert** | **Optimized Dapper** |   **10** |    **12.3301 ms** |    **12.53 ms** |  **10.782 ms** |    **11.82 ms** |    **12.33 ms** |    **12.97 ms** |    **18.14 ms** |
| **Insert** | **Optimized Dapper** |   **25** |    **16.1473 ms** |    **16.19 ms** |  **14.365 ms** |    **15.76 ms** |    **16.16 ms** |    **16.54 ms** |    **20.65 ms** |
| **Insert** | **Optimized Dapper** |   **50** |    **23.4861 ms** |    **23.85 ms** |  **21.062 ms** |    **22.47 ms** |    **23.28 ms** |    **25.15 ms** |    **29.44 ms** |
| **Insert** | **Optimized Dapper** |  **100** |    **39.9661 ms** |    **41.11 ms** |  **34.982 ms** |    **38.38 ms** |    **39.86 ms** |    **41.60 ms** |    **62.95 ms** |
| **Insert** | **Optimized Dapper** | **1000** |   **437.3011 ms** |   **502.38 ms** | **267.275 ms** |   **320.43 ms** |   **410.17 ms** |   **655.72 ms** | **1,139.66 ms** |
| **Insert** |          **EF Core** |    **1** |    **13.3824 ms** |    **14.51 ms** |  **10.566 ms** |    **12.52 ms** |    **13.37 ms** |    **14.38 ms** |    **27.82 ms** |
| **Insert** |          **EF Core** |   **10** |    **16.4422 ms** |    **16.62 ms** |  **14.507 ms** |    **15.90 ms** |    **16.36 ms** |    **17.29 ms** |    **19.80 ms** |
| **Insert** |          **EF Core** |   **25** |    **28.6316 ms** |    **29.14 ms** |  **24.081 ms** |    **27.08 ms** |    **28.55 ms** |    **30.95 ms** |    **40.18 ms** |
| **Insert** |          **EF Core** |   **50** |    **54.9142 ms** |    **55.69 ms** |  **46.591 ms** |    **52.42 ms** |    **54.43 ms** |    **60.14 ms** |    **66.32 ms** |
| **Insert** |          **EF Core** |  **100** |   **100.4684 ms** |   **101.73 ms** |  **84.333 ms** |    **95.43 ms** |   **100.73 ms** |   **105.43 ms** |   **130.20 ms** |
| **Insert** |          **EF Core** | **1000** | **1,337.6169 ms** | **1,473.82 ms** | **904.192 ms** | **1,050.97 ms** | **1,328.44 ms** | **1,847.72 ms** | **3,032.15 ms** |
