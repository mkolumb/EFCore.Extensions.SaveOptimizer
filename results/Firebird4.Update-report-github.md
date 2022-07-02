``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]    : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Firebird4 : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Job=Firebird4  EvaluateOverhead=True  OutlierMode=RemoveUpper  
InvocationCount=1  IterationCount=20  LaunchCount=3  
RunStrategy=ColdStart  UnrollFactor=1  WarmupCount=0  

```
|      Method |         Variant | Rows |     MeanNoOut |        Mean |          Min |          Q1 |      Median |          Q3 |         Max |
|------------ |---------------- |----- |--------------:|------------:|-------------:|------------:|------------:|------------:|------------:|
| **Update** |       **Optimized** |    **1** |    **14.1132 ms** |    **14.16 ms** |    **11.110 ms** |    **13.47 ms** |    **14.02 ms** |    **14.81 ms** |    **17.88 ms** |
| **Update** |       **Optimized** |   **10** |    **14.9620 ms** |    **15.04 ms** |    **12.502 ms** |    **14.10 ms** |    **15.02 ms** |    **15.79 ms** |    **18.42 ms** |
| **Update** |       **Optimized** |   **25** |    **21.8714 ms** |    **22.13 ms** |    **17.053 ms** |    **20.91 ms** |    **21.67 ms** |    **22.93 ms** |    **34.03 ms** |
| **Update** |       **Optimized** |   **50** |    **25.1756 ms** |    **25.43 ms** |    **22.415 ms** |    **23.94 ms** |    **24.94 ms** |    **26.78 ms** |    **31.45 ms** |
| **Update** |       **Optimized** |  **100** |    **33.1507 ms** |    **33.18 ms** |    **30.088 ms** |    **32.00 ms** |    **33.12 ms** |    **34.11 ms** |    **39.38 ms** |
| **Update** |       **Optimized** | **1000** |   **328.2370 ms** |   **329.79 ms** |   **311.061 ms** |   **321.31 ms** |   **326.37 ms** |   **339.30 ms** |   **356.26 ms** |
| **Update** | **Optimized Dapper** |    **1** |    **12.1222 ms** |    **12.15 ms** |     **9.611 ms** |    **11.42 ms** |    **12.09 ms** |    **13.11 ms** |    **14.50 ms** |
| **Update** | **Optimized Dapper** |   **10** |    **12.4785 ms** |    **12.60 ms** |    **10.793 ms** |    **11.65 ms** |    **12.44 ms** |    **13.24 ms** |    **16.42 ms** |
| **Update** | **Optimized Dapper** |   **25** |    **19.3524 ms** |    **19.21 ms** |    **15.673 ms** |    **18.26 ms** |    **19.52 ms** |    **19.98 ms** |    **22.19 ms** |
| **Update** | **Optimized Dapper** |   **50** |    **22.9799 ms** |    **22.73 ms** |    **18.960 ms** |    **21.52 ms** |    **23.02 ms** |    **24.17 ms** |    **27.36 ms** |
| **Update** | **Optimized Dapper** |  **100** |    **29.9499 ms** |    **29.94 ms** |    **26.430 ms** |    **29.14 ms** |    **29.82 ms** |    **30.79 ms** |    **33.53 ms** |
| **Update** | **Optimized Dapper** | **1000** |   **333.8798 ms** |   **338.10 ms** |   **309.220 ms** |   **325.46 ms** |   **332.83 ms** |   **346.85 ms** |   **395.96 ms** |
| **Update** |          **EF Core** |    **1** |    **16.1172 ms** |    **16.04 ms** |    **13.573 ms** |    **15.11 ms** |    **16.09 ms** |    **17.00 ms** |    **19.22 ms** |
| **Update** |          **EF Core** |   **10** |    **60.9816 ms** |    **60.96 ms** |    **47.118 ms** |    **56.24 ms** |    **60.83 ms** |    **65.74 ms** |    **81.19 ms** |
| **Update** |          **EF Core** |   **25** |   **141.4150 ms** |   **142.54 ms** |   **124.697 ms** |   **136.63 ms** |   **140.66 ms** |   **147.68 ms** |   **167.65 ms** |
| **Update** |          **EF Core** |   **50** |   **268.9219 ms** |   **272.12 ms** |   **246.520 ms** |   **258.78 ms** |   **267.92 ms** |   **280.78 ms** |   **336.06 ms** |
| **Update** |          **EF Core** |  **100** |   **511.3168 ms** |   **512.49 ms** |   **475.062 ms** |   **504.13 ms** |   **509.24 ms** |   **521.37 ms** |   **561.21 ms** |
| **Update** |          **EF Core** | **1000** | **4,120.0673 ms** | **4,274.81 ms** | **3,799.500 ms** | **3,904.63 ms** | **3,964.80 ms** | **4,885.79 ms** | **5,153.64 ms** |
