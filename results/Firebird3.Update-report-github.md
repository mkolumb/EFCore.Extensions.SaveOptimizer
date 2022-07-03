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
|      Method |         Variant | Rows |     MeanNoOut |        Mean |         Min |          Q1 |      Median |          Q3 |         Max |
|------------ |---------------- |----- |--------------:|------------:|------------:|------------:|------------:|------------:|------------:|
| **Update** |       **Optimized** |    **1** |    **13.8823 ms** |    **14.09 ms** |    **11.76 ms** |    **13.11 ms** |    **13.92 ms** |    **14.61 ms** |    **19.53 ms** |
| **Update** |       **Optimized** |   **10** |    **14.9068 ms** |    **15.16 ms** |    **12.79 ms** |    **14.17 ms** |    **14.93 ms** |    **15.72 ms** |    **19.73 ms** |
| **Update** |       **Optimized** |   **25** |    **22.3640 ms** |    **22.89 ms** |    **18.78 ms** |    **21.63 ms** |    **22.14 ms** |    **23.60 ms** |    **30.45 ms** |
| **Update** |       **Optimized** |   **50** |    **26.9112 ms** |    **27.06 ms** |    **23.66 ms** |    **25.95 ms** |    **26.95 ms** |    **27.87 ms** |    **30.87 ms** |
| **Update** |       **Optimized** |  **100** |    **36.1684 ms** |    **36.51 ms** |    **32.27 ms** |    **35.29 ms** |    **36.14 ms** |    **37.45 ms** |    **42.42 ms** |
| **Update** |       **Optimized** | **1000** |   **342.2050 ms** |   **338.66 ms** |   **285.95 ms** |   **314.38 ms** |   **346.55 ms** |   **357.03 ms** |   **383.68 ms** |
| **Update** | **Optimized Dapper** |    **1** |    **14.0114 ms** |    **14.07 ms** |    **11.79 ms** |    **13.43 ms** |    **14.02 ms** |    **14.77 ms** |    **16.38 ms** |
| **Update** | **Optimized Dapper** |   **10** |    **15.1213 ms** |    **15.52 ms** |    **13.02 ms** |    **14.31 ms** |    **15.02 ms** |    **15.99 ms** |    **21.78 ms** |
| **Update** | **Optimized Dapper** |   **25** |    **22.3428 ms** |    **22.27 ms** |    **18.82 ms** |    **21.51 ms** |    **22.26 ms** |    **23.21 ms** |    **25.72 ms** |
| **Update** | **Optimized Dapper** |   **50** |    **26.7421 ms** |    **27.75 ms** |    **24.88 ms** |    **25.97 ms** |    **26.76 ms** |    **27.73 ms** |    **42.52 ms** |
| **Update** | **Optimized Dapper** |  **100** |    **35.9731 ms** |    **36.28 ms** |    **32.65 ms** |    **34.66 ms** |    **35.91 ms** |    **37.29 ms** |    **42.97 ms** |
| **Update** | **Optimized Dapper** | **1000** |   **333.1096 ms** |   **324.48 ms** |   **261.05 ms** |   **282.90 ms** |   **342.73 ms** |   **347.70 ms** |   **389.82 ms** |
| **Update** |          **EF Core** |    **1** |    **15.6126 ms** |    **15.95 ms** |    **13.22 ms** |    **14.77 ms** |    **15.44 ms** |    **17.45 ms** |    **21.29 ms** |
| **Update** |          **EF Core** |   **10** |    **50.9538 ms** |    **51.52 ms** |    **43.39 ms** |    **47.73 ms** |    **51.32 ms** |    **53.32 ms** |    **70.19 ms** |
| **Update** |          **EF Core** |   **25** |   **115.6955 ms** |   **117.56 ms** |   **101.24 ms** |   **110.76 ms** |   **114.58 ms** |   **124.66 ms** |   **141.68 ms** |
| **Update** |          **EF Core** |   **50** |   **205.0416 ms** |   **208.03 ms** |   **188.97 ms** |   **200.29 ms** |   **203.98 ms** |   **214.58 ms** |   **240.31 ms** |
| **Update** |          **EF Core** |  **100** |   **399.6706 ms** |   **401.09 ms** |   **374.90 ms** |   **390.06 ms** |   **398.54 ms** |   **409.55 ms** |   **433.44 ms** |
| **Update** |          **EF Core** | **1000** | **4,945.8121 ms** | **4,733.99 ms** | **3,758.87 ms** | **4,792.12 ms** | **4,947.58 ms** | **4,997.97 ms** | **5,170.10 ms** |
