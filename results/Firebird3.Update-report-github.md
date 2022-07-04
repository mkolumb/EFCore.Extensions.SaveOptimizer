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
| **Update** |       **Optimized** |    **1** |    **14.1680 ms** |    **14.50 ms** |    **12.073 ms** |    **13.38 ms** |    **14.06 ms** |    **15.11 ms** |    **22.82 ms** |
| **Update** |       **Optimized** |   **10** |    **15.0826 ms** |    **15.41 ms** |    **12.749 ms** |    **14.01 ms** |    **15.01 ms** |    **16.06 ms** |    **20.69 ms** |
| **Update** |       **Optimized** |   **25** |    **20.9807 ms** |    **21.38 ms** |    **18.027 ms** |    **19.87 ms** |    **20.80 ms** |    **22.48 ms** |    **28.73 ms** |
| **Update** |       **Optimized** |   **50** |    **24.9983 ms** |    **25.13 ms** |    **21.783 ms** |    **24.07 ms** |    **25.06 ms** |    **25.77 ms** |    **28.27 ms** |
| **Update** |       **Optimized** |  **100** |    **34.1674 ms** |    **34.27 ms** |    **30.601 ms** |    **33.21 ms** |    **34.21 ms** |    **35.10 ms** |    **38.43 ms** |
| **Update** |       **Optimized** | **1000** |   **346.8637 ms** |   **347.19 ms** |   **323.279 ms** |   **338.37 ms** |   **347.48 ms** |   **354.82 ms** |   **368.94 ms** |
| **Update** | **Optimized Dapper** |    **1** |    **11.7504 ms** |    **11.89 ms** |     **9.909 ms** |    **11.04 ms** |    **11.78 ms** |    **12.61 ms** |    **15.30 ms** |
| **Update** | **Optimized Dapper** |   **10** |    **13.0170 ms** |    **13.20 ms** |    **10.644 ms** |    **12.17 ms** |    **12.98 ms** |    **13.91 ms** |    **17.66 ms** |
| **Update** | **Optimized Dapper** |   **25** |    **22.3209 ms** |    **22.57 ms** |    **18.602 ms** |    **20.71 ms** |    **22.34 ms** |    **23.86 ms** |    **29.66 ms** |
| **Update** | **Optimized Dapper** |   **50** |    **23.9999 ms** |    **23.94 ms** |    **19.505 ms** |    **22.89 ms** |    **23.89 ms** |    **25.09 ms** |    **27.21 ms** |
| **Update** | **Optimized Dapper** |  **100** |    **34.0888 ms** |    **34.21 ms** |    **29.231 ms** |    **33.45 ms** |    **34.10 ms** |    **34.89 ms** |    **37.18 ms** |
| **Update** | **Optimized Dapper** | **1000** |   **350.6905 ms** |   **352.92 ms** |   **328.633 ms** |   **343.18 ms** |   **350.42 ms** |   **359.69 ms** |   **400.28 ms** |
| **Update** |          **EF Core** |    **1** |    **16.1827 ms** |    **16.63 ms** |    **14.053 ms** |    **14.95 ms** |    **16.20 ms** |    **17.47 ms** |    **24.11 ms** |
| **Update** |          **EF Core** |   **10** |    **50.5079 ms** |    **52.00 ms** |    **44.285 ms** |    **47.96 ms** |    **50.27 ms** |    **54.75 ms** |    **72.52 ms** |
| **Update** |          **EF Core** |   **25** |   **121.3417 ms** |   **123.74 ms** |   **103.125 ms** |   **114.89 ms** |   **120.42 ms** |   **130.72 ms** |   **162.39 ms** |
| **Update** |          **EF Core** |   **50** |   **222.2677 ms** |   **225.45 ms** |   **197.083 ms** |   **213.23 ms** |   **219.69 ms** |   **237.58 ms** |   **283.45 ms** |
| **Update** |          **EF Core** |  **100** |   **428.6455 ms** |   **438.88 ms** |   **401.536 ms** |   **415.19 ms** |   **427.37 ms** |   **455.90 ms** |   **554.63 ms** |
| **Update** |          **EF Core** | **1000** | **5,109.3468 ms** | **4,937.23 ms** | **3,981.598 ms** | **4,140.16 ms** | **5,267.03 ms** | **5,330.95 ms** | **5,622.31 ms** |
