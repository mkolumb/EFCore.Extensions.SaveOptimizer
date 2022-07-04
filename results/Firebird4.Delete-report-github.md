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
| **Delete** |       **Optimized** |    **1** |    **11.8923 ms** |    **11.85 ms** |     **9.360 ms** |    **11.17 ms** |    **11.99 ms** |    **12.63 ms** |    **14.20 ms** |
| **Delete** |       **Optimized** |   **10** |    **13.0556 ms** |    **13.09 ms** |    **10.938 ms** |    **12.41 ms** |    **13.18 ms** |    **13.62 ms** |    **16.73 ms** |
| **Delete** |       **Optimized** |   **25** |    **21.8722 ms** |    **22.00 ms** |    **14.550 ms** |    **18.25 ms** |    **22.28 ms** |    **25.06 ms** |    **34.44 ms** |
| **Delete** |       **Optimized** |   **50** |    **38.3326 ms** |    **38.82 ms** |    **18.143 ms** |    **29.36 ms** |    **37.97 ms** |    **48.12 ms** |    **63.74 ms** |
| **Delete** |       **Optimized** |  **100** |    **85.2871 ms** |    **85.37 ms** |    **31.516 ms** |    **54.07 ms** |    **87.37 ms** |   **112.67 ms** |   **149.82 ms** |
| **Delete** |       **Optimized** | **1000** |   **285.0030 ms** |   **293.36 ms** |   **218.763 ms** |   **239.05 ms** |   **283.87 ms** |   **342.84 ms** |   **499.10 ms** |
| **Delete** | **Optimized Dapper** |    **1** |    **11.6266 ms** |    **12.70 ms** |     **9.224 ms** |    **10.59 ms** |    **11.53 ms** |    **13.00 ms** |    **29.37 ms** |
| **Delete** | **Optimized Dapper** |   **10** |    **14.0615 ms** |    **14.23 ms** |    **11.349 ms** |    **12.93 ms** |    **14.00 ms** |    **15.41 ms** |    **17.88 ms** |
| **Delete** | **Optimized Dapper** |   **25** |    **20.2683 ms** |    **20.89 ms** |    **13.728 ms** |    **17.69 ms** |    **19.83 ms** |    **23.44 ms** |    **32.45 ms** |
| **Delete** | **Optimized Dapper** |   **50** |    **38.1119 ms** |    **38.10 ms** |    **20.661 ms** |    **28.88 ms** |    **38.16 ms** |    **45.66 ms** |    **63.72 ms** |
| **Delete** | **Optimized Dapper** |  **100** |    **87.3036 ms** |    **87.21 ms** |    **28.829 ms** |    **54.98 ms** |    **85.54 ms** |   **116.07 ms** |   **140.77 ms** |
| **Delete** | **Optimized Dapper** | **1000** |   **280.0176 ms** |   **291.29 ms** |   **222.530 ms** |   **241.16 ms** |   **277.15 ms** |   **342.50 ms** |   **457.56 ms** |
| **Delete** |          **EF Core** |    **1** |    **15.8022 ms** |    **16.15 ms** |    **13.140 ms** |    **14.28 ms** |    **15.69 ms** |    **17.69 ms** |    **24.21 ms** |
| **Delete** |          **EF Core** |   **10** |    **51.4536 ms** |    **52.21 ms** |    **45.186 ms** |    **47.75 ms** |    **51.49 ms** |    **55.53 ms** |    **64.43 ms** |
| **Delete** |          **EF Core** |   **25** |   **119.1288 ms** |   **120.72 ms** |    **98.228 ms** |   **112.25 ms** |   **117.96 ms** |   **127.54 ms** |   **154.69 ms** |
| **Delete** |          **EF Core** |   **50** |   **221.2029 ms** |   **225.93 ms** |   **203.203 ms** |   **212.65 ms** |   **220.51 ms** |   **233.81 ms** |   **285.24 ms** |
| **Delete** |          **EF Core** |  **100** |   **431.1281 ms** |   **434.14 ms** |   **385.296 ms** |   **416.64 ms** |   **433.13 ms** |   **444.99 ms** |   **501.81 ms** |
| **Delete** |          **EF Core** | **1000** | **4,104.0212 ms** | **4,117.87 ms** | **3,954.998 ms** | **4,048.31 ms** | **4,103.98 ms** | **4,155.49 ms** | **4,424.70 ms** |
