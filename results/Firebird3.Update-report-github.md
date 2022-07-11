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
| **Update** |       **Optimized** |    **1** |    **13.6558 ms** |    **13.78 ms** |    **11.21 ms** |    **12.67 ms** |    **13.69 ms** |    **14.64 ms** |    **17.20 ms** |
| **Update** |       **Optimized** |   **10** |    **14.8123 ms** |    **15.00 ms** |    **12.80 ms** |    **14.04 ms** |    **14.86 ms** |    **15.38 ms** |    **18.80 ms** |
| **Update** |       **Optimized** |   **25** |    **21.7676 ms** |    **21.79 ms** |    **17.32 ms** |    **19.99 ms** |    **21.78 ms** |    **23.03 ms** |    **26.67 ms** |
| **Update** |       **Optimized** |   **50** |    **25.4794 ms** |    **25.52 ms** |    **21.50 ms** |    **24.02 ms** |    **25.34 ms** |    **26.59 ms** |    **31.04 ms** |
| **Update** |       **Optimized** |  **100** |    **36.1889 ms** |    **36.00 ms** |    **31.16 ms** |    **34.89 ms** |    **36.19 ms** |    **37.21 ms** |    **38.72 ms** |
| **Update** |       **Optimized** | **1000** |   **349.2128 ms** |   **348.58 ms** |   **322.98 ms** |   **341.49 ms** |   **349.04 ms** |   **355.87 ms** |   **375.87 ms** |
| **Update** | **Optimized Dapper** |    **1** |    **14.0814 ms** |    **14.19 ms** |    **11.56 ms** |    **13.47 ms** |    **14.04 ms** |    **14.90 ms** |    **17.85 ms** |
| **Update** | **Optimized Dapper** |   **10** |    **16.2361 ms** |    **16.41 ms** |    **12.72 ms** |    **14.09 ms** |    **16.29 ms** |    **18.26 ms** |    **22.26 ms** |
| **Update** | **Optimized Dapper** |   **25** |    **21.8211 ms** |    **21.78 ms** |    **17.97 ms** |    **20.27 ms** |    **21.72 ms** |    **23.36 ms** |    **26.91 ms** |
| **Update** | **Optimized Dapper** |   **50** |    **26.5550 ms** |    **26.54 ms** |    **22.54 ms** |    **25.45 ms** |    **26.39 ms** |    **27.82 ms** |    **31.08 ms** |
| **Update** | **Optimized Dapper** |  **100** |    **37.7449 ms** |    **39.22 ms** |    **31.62 ms** |    **36.08 ms** |    **37.75 ms** |    **40.49 ms** |    **54.67 ms** |
| **Update** | **Optimized Dapper** | **1000** |   **339.7457 ms** |   **341.48 ms** |   **319.50 ms** |   **333.68 ms** |   **339.27 ms** |   **347.72 ms** |   **374.09 ms** |
| **Update** |          **EF Core** |    **1** |    **19.4881 ms** |    **19.77 ms** |    **17.41 ms** |    **18.67 ms** |    **19.44 ms** |    **20.71 ms** |    **24.90 ms** |
| **Update** |          **EF Core** |   **10** |    **68.3861 ms** |    **69.34 ms** |    **54.32 ms** |    **64.04 ms** |    **67.22 ms** |    **74.76 ms** |    **85.55 ms** |
| **Update** |          **EF Core** |   **25** |   **151.0274 ms** |   **152.46 ms** |   **134.60 ms** |   **145.60 ms** |   **149.49 ms** |   **158.43 ms** |   **182.68 ms** |
| **Update** |          **EF Core** |   **50** |   **285.3053 ms** |   **286.82 ms** |   **257.28 ms** |   **274.98 ms** |   **283.87 ms** |   **294.61 ms** |   **344.17 ms** |
| **Update** |          **EF Core** |  **100** |   **522.1755 ms** |   **525.11 ms** |   **496.45 ms** |   **513.59 ms** |   **521.12 ms** |   **541.23 ms** |   **570.72 ms** |
| **Update** |          **EF Core** | **1000** | **4,806.8017 ms** | **4,720.83 ms** | **3,881.25 ms** | **4,097.73 ms** | **5,150.02 ms** | **5,212.87 ms** | **5,365.09 ms** |
