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
|      Method |         Variant | Rows |     MeanNoOut |        Mean |         Min |          Q1 |      Median |          Q3 |         Max |
|------------ |---------------- |----- |--------------:|------------:|------------:|------------:|------------:|------------:|------------:|
| **Update** |       **Optimized** |    **1** |    **14.1411 ms** |    **14.20 ms** |    **11.82 ms** |    **13.44 ms** |    **14.13 ms** |    **14.73 ms** |    **17.70 ms** |
| **Update** |       **Optimized** |   **10** |    **16.1727 ms** |    **16.60 ms** |    **12.63 ms** |    **14.82 ms** |    **16.07 ms** |    **17.88 ms** |    **25.64 ms** |
| **Update** |       **Optimized** |   **25** |    **22.1581 ms** |    **22.09 ms** |    **17.59 ms** |    **21.11 ms** |    **22.12 ms** |    **23.26 ms** |    **26.69 ms** |
| **Update** |       **Optimized** |   **50** |    **26.3588 ms** |    **26.71 ms** |    **22.70 ms** |    **25.19 ms** |    **26.33 ms** |    **27.65 ms** |    **33.49 ms** |
| **Update** |       **Optimized** |  **100** |    **37.6053 ms** |    **39.77 ms** |    **32.94 ms** |    **35.15 ms** |    **37.69 ms** |    **41.92 ms** |    **61.92 ms** |
| **Update** |       **Optimized** | **1000** |   **325.6777 ms** |   **327.09 ms** |   **302.84 ms** |   **317.12 ms** |   **324.86 ms** |   **334.47 ms** |   **364.63 ms** |
| **Update** | **Optimized Dapper** |    **1** |    **11.6734 ms** |    **11.82 ms** |    **10.01 ms** |    **11.07 ms** |    **11.65 ms** |    **12.30 ms** |    **15.24 ms** |
| **Update** | **Optimized Dapper** |   **10** |    **12.5161 ms** |    **12.73 ms** |    **10.49 ms** |    **11.92 ms** |    **12.48 ms** |    **13.19 ms** |    **18.48 ms** |
| **Update** | **Optimized Dapper** |   **25** |    **19.7051 ms** |    **19.87 ms** |    **17.77 ms** |    **18.90 ms** |    **19.76 ms** |    **20.52 ms** |    **22.48 ms** |
| **Update** | **Optimized Dapper** |   **50** |    **28.2598 ms** |    **28.28 ms** |    **23.45 ms** |    **26.42 ms** |    **28.07 ms** |    **30.38 ms** |    **34.40 ms** |
| **Update** | **Optimized Dapper** |  **100** |    **35.1717 ms** |    **35.44 ms** |    **31.13 ms** |    **34.07 ms** |    **35.21 ms** |    **36.16 ms** |    **44.69 ms** |
| **Update** | **Optimized Dapper** | **1000** |   **326.8432 ms** |   **328.68 ms** |   **290.91 ms** |   **317.55 ms** |   **325.79 ms** |   **338.14 ms** |   **378.10 ms** |
| **Update** |          **EF Core** |    **1** |    **16.0891 ms** |    **16.32 ms** |    **13.08 ms** |    **15.21 ms** |    **16.14 ms** |    **16.88 ms** |    **20.84 ms** |
| **Update** |          **EF Core** |   **10** |    **53.3835 ms** |    **55.08 ms** |    **45.40 ms** |    **49.94 ms** |    **53.25 ms** |    **58.62 ms** |    **78.46 ms** |
| **Update** |          **EF Core** |   **25** |   **121.9428 ms** |   **123.45 ms** |   **106.73 ms** |   **117.17 ms** |   **122.17 ms** |   **128.00 ms** |   **149.29 ms** |
| **Update** |          **EF Core** |   **50** |   **231.3820 ms** |   **238.81 ms** |   **193.73 ms** |   **215.62 ms** |   **231.89 ms** |   **249.87 ms** |   **342.68 ms** |
| **Update** |          **EF Core** |  **100** |   **450.9618 ms** |   **466.74 ms** |   **394.74 ms** |   **423.83 ms** |   **448.15 ms** |   **488.47 ms** |   **610.68 ms** |
| **Update** |          **EF Core** | **1000** | **4,349.2804 ms** | **4,582.01 ms** | **3,979.52 ms** | **4,149.19 ms** | **4,231.81 ms** | **5,346.45 ms** | **5,707.39 ms** |
