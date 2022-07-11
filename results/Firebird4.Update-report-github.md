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
| **Update** |       **Optimized** |    **1** |    **11.6763 ms** |    **11.80 ms** |     **9.897 ms** |    **10.87 ms** |    **11.54 ms** |    **12.68 ms** |    **14.41 ms** |
| **Update** |       **Optimized** |   **10** |    **12.4917 ms** |    **12.80 ms** |    **10.871 ms** |    **11.75 ms** |    **12.54 ms** |    **13.46 ms** |    **19.30 ms** |
| **Update** |       **Optimized** |   **25** |    **19.2344 ms** |    **19.16 ms** |    **15.650 ms** |    **18.20 ms** |    **19.24 ms** |    **20.26 ms** |    **24.73 ms** |
| **Update** |       **Optimized** |   **50** |    **22.7272 ms** |    **23.15 ms** |    **18.573 ms** |    **21.27 ms** |    **22.72 ms** |    **23.98 ms** |    **34.29 ms** |
| **Update** |       **Optimized** |  **100** |    **30.9665 ms** |    **31.07 ms** |    **27.535 ms** |    **30.33 ms** |    **30.92 ms** |    **32.06 ms** |    **33.99 ms** |
| **Update** |       **Optimized** | **1000** |   **324.6102 ms** |   **325.80 ms** |   **310.666 ms** |   **319.61 ms** |   **324.11 ms** |   **330.47 ms** |   **354.80 ms** |
| **Update** | **Optimized Dapper** |    **1** |    **13.8892 ms** |    **13.96 ms** |    **12.397 ms** |    **13.39 ms** |    **13.88 ms** |    **14.52 ms** |    **16.03 ms** |
| **Update** | **Optimized Dapper** |   **10** |    **14.6297 ms** |    **14.96 ms** |    **13.192 ms** |    **14.15 ms** |    **14.57 ms** |    **15.33 ms** |    **19.42 ms** |
| **Update** | **Optimized Dapper** |   **25** |    **21.1392 ms** |    **21.00 ms** |    **16.369 ms** |    **20.05 ms** |    **21.24 ms** |    **22.18 ms** |    **25.79 ms** |
| **Update** | **Optimized Dapper** |   **50** |    **23.2992 ms** |    **23.44 ms** |    **20.163 ms** |    **22.43 ms** |    **23.25 ms** |    **24.43 ms** |    **27.76 ms** |
| **Update** | **Optimized Dapper** |  **100** |    **30.8641 ms** |    **31.25 ms** |    **27.956 ms** |    **30.13 ms** |    **30.90 ms** |    **31.44 ms** |    **39.52 ms** |
| **Update** | **Optimized Dapper** | **1000** |   **328.5083 ms** |   **332.55 ms** |   **308.151 ms** |   **322.84 ms** |   **327.72 ms** |   **338.50 ms** |   **376.72 ms** |
| **Update** |          **EF Core** |    **1** |    **20.1407 ms** |    **20.32 ms** |    **17.086 ms** |    **19.01 ms** |    **20.19 ms** |    **22.00 ms** |    **23.45 ms** |
| **Update** |          **EF Core** |   **10** |    **70.0452 ms** |    **70.48 ms** |    **60.718 ms** |    **64.96 ms** |    **71.19 ms** |    **74.54 ms** |    **87.84 ms** |
| **Update** |          **EF Core** |   **25** |   **148.3772 ms** |   **149.10 ms** |   **132.818 ms** |   **142.34 ms** |   **147.97 ms** |   **155.48 ms** |   **171.18 ms** |
| **Update** |          **EF Core** |   **50** |   **276.2477 ms** |   **278.68 ms** |   **260.316 ms** |   **268.29 ms** |   **276.26 ms** |   **286.48 ms** |   **312.19 ms** |
| **Update** |          **EF Core** |  **100** |   **520.9160 ms** |   **518.09 ms** |   **399.055 ms** |   **511.78 ms** |   **518.69 ms** |   **534.13 ms** |   **564.24 ms** |
| **Update** |          **EF Core** | **1000** | **5,088.4936 ms** | **4,910.22 ms** | **3,967.910 ms** | **4,170.07 ms** | **5,187.97 ms** | **5,335.71 ms** | **5,475.55 ms** |
