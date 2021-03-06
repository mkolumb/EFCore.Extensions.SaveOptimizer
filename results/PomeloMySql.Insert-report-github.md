``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]      : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  PomeloMySql : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Job=PomeloMySql  EvaluateOverhead=True  OutlierMode=RemoveUpper  
InvocationCount=1  IterationCount=15  LaunchCount=8  
RunStrategy=Throughput  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |     MeanNoOut |        Mean |       Min |          Q1 |      Median |          Q3 |         Max |
|------------ |---------------- |------ |--------------:|------------:|----------:|------------:|------------:|------------:|------------:|
| **Insert** |       **Optimized** |     **1** |    **16.7316 ms** |    **16.93 ms** |  **14.27 ms** |    **15.87 ms** |    **16.62 ms** |    **17.71 ms** |    **22.00 ms** |
| **Insert** |       **Optimized** |    **10** |    **18.5890 ms** |    **19.40 ms** |  **15.59 ms** |    **17.68 ms** |    **18.63 ms** |    **19.82 ms** |    **38.31 ms** |
| **Insert** |       **Optimized** |    **25** |    **20.3354 ms** |    **20.73 ms** |  **16.02 ms** |    **18.70 ms** |    **20.10 ms** |    **22.31 ms** |    **31.19 ms** |
| **Insert** |       **Optimized** |    **50** |    **24.0289 ms** |    **24.10 ms** |  **18.75 ms** |    **22.32 ms** |    **24.01 ms** |    **25.64 ms** |    **31.27 ms** |
| **Insert** |       **Optimized** |   **100** |    **31.0552 ms** |    **31.71 ms** |  **23.21 ms** |    **28.91 ms** |    **30.78 ms** |    **33.64 ms** |    **45.58 ms** |
| **Insert** |       **Optimized** |  **1000** |   **103.7340 ms** |   **104.89 ms** |  **88.41 ms** |    **97.48 ms** |   **104.10 ms** |   **109.77 ms** |   **145.10 ms** |
| **Insert** |       **Optimized** | **10000** |   **957.9155 ms** |   **961.07 ms** | **897.77 ms** |   **940.55 ms** |   **956.48 ms** |   **982.44 ms** | **1,031.89 ms** |
| **Insert** | **Optimized Dapper** |     **1** |    **15.0162 ms** |    **15.38 ms** |  **11.05 ms** |    **13.98 ms** |    **14.86 ms** |    **16.84 ms** |    **22.26 ms** |
| **Insert** | **Optimized Dapper** |    **10** |    **15.6325 ms** |    **15.97 ms** |  **13.25 ms** |    **14.75 ms** |    **15.48 ms** |    **16.91 ms** |    **20.40 ms** |
| **Insert** | **Optimized Dapper** |    **25** |    **17.3873 ms** |    **17.89 ms** |  **13.87 ms** |    **16.24 ms** |    **17.38 ms** |    **19.28 ms** |    **28.13 ms** |
| **Insert** | **Optimized Dapper** |    **50** |    **20.5238 ms** |    **20.73 ms** |  **15.91 ms** |    **18.37 ms** |    **20.76 ms** |    **22.26 ms** |    **28.91 ms** |
| **Insert** | **Optimized Dapper** |   **100** |    **27.1398 ms** |    **27.72 ms** |  **21.90 ms** |    **24.96 ms** |    **26.83 ms** |    **29.77 ms** |    **40.94 ms** |
| **Insert** | **Optimized Dapper** |  **1000** |    **88.1091 ms** |    **88.60 ms** |  **77.07 ms** |    **84.53 ms** |    **87.95 ms** |    **92.53 ms** |   **108.46 ms** |
| **Insert** | **Optimized Dapper** | **10000** |   **943.8516 ms** |   **930.10 ms** | **797.03 ms** |   **903.99 ms** |   **944.15 ms** |   **974.32 ms** | **1,039.16 ms** |
| **Insert** |          **EF Core** |     **1** |    **18.4305 ms** |    **18.83 ms** |  **16.19 ms** |    **17.55 ms** |    **18.37 ms** |    **19.52 ms** |    **30.41 ms** |
| **Insert** |          **EF Core** |    **10** |    **20.4121 ms** |    **21.45 ms** |  **16.72 ms** |    **19.23 ms** |    **20.39 ms** |    **21.77 ms** |    **40.71 ms** |
| **Insert** |          **EF Core** |    **25** |    **23.1702 ms** |    **23.60 ms** |  **18.75 ms** |    **21.72 ms** |    **23.03 ms** |    **25.21 ms** |    **30.98 ms** |
| **Insert** |          **EF Core** |    **50** |    **27.5579 ms** |    **28.04 ms** |  **22.03 ms** |    **25.53 ms** |    **27.49 ms** |    **29.30 ms** |    **40.23 ms** |
| **Insert** |          **EF Core** |   **100** |    **35.7864 ms** |    **36.41 ms** |  **30.44 ms** |    **33.52 ms** |    **35.45 ms** |    **38.78 ms** |    **49.22 ms** |
| **Insert** |          **EF Core** |  **1000** |   **133.9796 ms** |   **134.62 ms** | **116.71 ms** |   **128.30 ms** |   **133.91 ms** |   **140.68 ms** |   **154.85 ms** |
| **Insert** |          **EF Core** | **10000** | **1,069.3738 ms** | **1,085.96 ms** | **936.64 ms** | **1,014.31 ms** | **1,057.24 ms** | **1,161.99 ms** | **1,322.74 ms** |
