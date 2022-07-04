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
| **Delete** |       **Optimized** |    **1** |    **12.7852 ms** |    **13.48 ms** |    **10.26 ms** |    **11.68 ms** |    **12.76 ms** |    **14.64 ms** |    **31.05 ms** |
| **Delete** |       **Optimized** |   **10** |    **13.7472 ms** |    **13.82 ms** |    **10.54 ms** |    **12.52 ms** |    **13.70 ms** |    **15.04 ms** |    **17.16 ms** |
| **Delete** |       **Optimized** |   **25** |    **21.0908 ms** |    **21.27 ms** |    **15.00 ms** |    **17.80 ms** |    **20.82 ms** |    **23.85 ms** |    **31.81 ms** |
| **Delete** |       **Optimized** |   **50** |    **36.3510 ms** |    **36.61 ms** |    **21.70 ms** |    **29.05 ms** |    **36.50 ms** |    **43.21 ms** |    **57.56 ms** |
| **Delete** |       **Optimized** |  **100** |    **71.5194 ms** |    **73.82 ms** |    **28.98 ms** |    **48.99 ms** |    **68.26 ms** |    **97.41 ms** |   **131.51 ms** |
| **Delete** |       **Optimized** | **1000** |   **243.8503 ms** |   **244.49 ms** |   **226.95 ms** |   **237.41 ms** |   **243.15 ms** |   **251.64 ms** |   **269.42 ms** |
| **Delete** | **Optimized Dapper** |    **1** |    **11.9265 ms** |    **12.14 ms** |    **10.26 ms** |    **10.96 ms** |    **12.06 ms** |    **12.89 ms** |    **16.31 ms** |
| **Delete** | **Optimized Dapper** |   **10** |    **12.6185 ms** |    **12.87 ms** |    **10.85 ms** |    **11.92 ms** |    **12.61 ms** |    **13.53 ms** |    **16.75 ms** |
| **Delete** | **Optimized Dapper** |   **25** |    **20.6433 ms** |    **21.15 ms** |    **15.45 ms** |    **18.46 ms** |    **20.44 ms** |    **23.15 ms** |    **33.41 ms** |
| **Delete** | **Optimized Dapper** |   **50** |    **34.8281 ms** |    **35.54 ms** |    **20.48 ms** |    **27.27 ms** |    **33.88 ms** |    **43.52 ms** |    **68.26 ms** |
| **Delete** | **Optimized Dapper** |  **100** |    **82.1107 ms** |    **81.77 ms** |    **27.80 ms** |    **53.78 ms** |    **82.64 ms** |   **109.54 ms** |   **140.87 ms** |
| **Delete** | **Optimized Dapper** | **1000** |   **253.3690 ms** |   **254.30 ms** |   **232.90 ms** |   **245.14 ms** |   **253.36 ms** |   **262.65 ms** |   **279.66 ms** |
| **Delete** |          **EF Core** |    **1** |    **17.2477 ms** |    **17.51 ms** |    **13.48 ms** |    **16.03 ms** |    **17.15 ms** |    **18.80 ms** |    **23.70 ms** |
| **Delete** |          **EF Core** |   **10** |    **51.8572 ms** |    **53.44 ms** |    **44.71 ms** |    **48.78 ms** |    **51.63 ms** |    **56.23 ms** |    **69.44 ms** |
| **Delete** |          **EF Core** |   **25** |   **118.0384 ms** |   **121.64 ms** |    **98.70 ms** |   **108.59 ms** |   **115.26 ms** |   **132.04 ms** |   **176.77 ms** |
| **Delete** |          **EF Core** |   **50** |   **216.4455 ms** |   **220.83 ms** |   **174.91 ms** |   **206.06 ms** |   **215.39 ms** |   **232.36 ms** |   **294.75 ms** |
| **Delete** |          **EF Core** |  **100** |   **413.6114 ms** |   **425.11 ms** |   **374.71 ms** |   **396.97 ms** |   **413.11 ms** |   **431.28 ms** |   **577.31 ms** |
| **Delete** |          **EF Core** | **1000** | **4,046.3651 ms** | **4,044.69 ms** | **3,846.28 ms** | **3,983.01 ms** | **4,051.83 ms** | **4,100.89 ms** | **4,208.66 ms** |
