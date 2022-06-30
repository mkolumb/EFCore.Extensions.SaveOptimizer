``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host] : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Sqlite : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Job=Sqlite  EvaluateOverhead=True  OutlierMode=RemoveUpper  
InvocationCount=1  IterationCount=15  LaunchCount=8  
RunStrategy=Throughput  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |   MeanNoOut |       Mean |        Min |         Q1 |     Median |         Q3 |        Max |
|------------ |---------------- |------ |------------:|-----------:|-----------:|-----------:|-----------:|-----------:|-----------:|
| **UpdateAsync** |       **Optimized** |     **1** |   **1.7149 ms** |   **1.720 ms** |   **1.552 ms** |   **1.642 ms** |   **1.728 ms** |   **1.775 ms** |   **1.987 ms** |
| **UpdateAsync** |       **Optimized** |    **10** |   **2.1011 ms** |   **2.106 ms** |   **1.924 ms** |   **2.035 ms** |   **2.103 ms** |   **2.168 ms** |   **2.410 ms** |
| **UpdateAsync** |       **Optimized** |    **25** |   **2.6443 ms** |   **2.646 ms** |   **2.363 ms** |   **2.571 ms** |   **2.651 ms** |   **2.704 ms** |   **2.941 ms** |
| **UpdateAsync** |       **Optimized** |    **50** |   **3.4707 ms** |   **3.586 ms** |   **3.127 ms** |   **3.389 ms** |   **3.460 ms** |   **3.579 ms** |   **6.544 ms** |
| **UpdateAsync** |       **Optimized** |   **100** |   **5.2381 ms** |   **5.223 ms** |   **4.396 ms** |   **5.007 ms** |   **5.268 ms** |   **5.439 ms** |   **6.096 ms** |
| **UpdateAsync** |       **Optimized** |  **1000** |  **30.2903 ms** |  **30.300 ms** |  **25.711 ms** |  **28.303 ms** |  **30.346 ms** |  **32.004 ms** |  **39.447 ms** |
| **UpdateAsync** |       **Optimized** | **10000** | **329.4178 ms** | **336.416 ms** | **292.259 ms** | **313.131 ms** | **327.969 ms** | **356.056 ms** | **419.232 ms** |
| **UpdateAsync** | **OptimizedDapper** |     **1** |   **1.5385 ms** |   **1.551 ms** |   **1.307 ms** |   **1.469 ms** |   **1.536 ms** |   **1.614 ms** |   **1.987 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **10** |   **1.8852 ms** |   **1.902 ms** |   **1.633 ms** |   **1.777 ms** |   **1.886 ms** |   **1.989 ms** |   **2.371 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **25** |   **2.3617 ms** |   **2.522 ms** |   **2.033 ms** |   **2.262 ms** |   **2.349 ms** |   **2.514 ms** |   **6.870 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **50** |   **3.1075 ms** |   **3.227 ms** |   **2.837 ms** |   **3.036 ms** |   **3.113 ms** |   **3.186 ms** |   **6.417 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **100** |   **4.6319 ms** |   **4.624 ms** |   **4.260 ms** |   **4.558 ms** |   **4.629 ms** |   **4.710 ms** |   **4.955 ms** |
| **UpdateAsync** | **OptimizedDapper** |  **1000** |  **26.0247 ms** |  **25.877 ms** |  **22.102 ms** |  **24.599 ms** |  **26.303 ms** |  **27.022 ms** |  **29.178 ms** |
| **UpdateAsync** | **OptimizedDapper** | **10000** | **324.1509 ms** | **328.478 ms** | **267.065 ms** | **307.485 ms** | **322.676 ms** | **344.264 ms** | **402.258 ms** |
| **UpdateAsync** |          **EfCore** |     **1** |   **1.6504 ms** |   **1.656 ms** |   **1.459 ms** |   **1.591 ms** |   **1.648 ms** |   **1.709 ms** |   **1.924 ms** |
| **UpdateAsync** |          **EfCore** |    **10** |   **2.5182 ms** |   **2.528 ms** |   **2.312 ms** |   **2.457 ms** |   **2.514 ms** |   **2.589 ms** |   **2.889 ms** |
| **UpdateAsync** |          **EfCore** |    **25** |   **3.9817 ms** |   **4.003 ms** |   **3.701 ms** |   **3.861 ms** |   **3.984 ms** |   **4.123 ms** |   **4.761 ms** |
| **UpdateAsync** |          **EfCore** |    **50** |   **6.4706 ms** |   **6.592 ms** |   **5.776 ms** |   **6.265 ms** |   **6.461 ms** |   **6.735 ms** |   **9.374 ms** |
| **UpdateAsync** |          **EfCore** |   **100** |  **11.2721 ms** |  **11.455 ms** |   **9.564 ms** |  **10.685 ms** |  **11.246 ms** |  **11.881 ms** |  **15.053 ms** |
| **UpdateAsync** |          **EfCore** |  **1000** |  **75.0469 ms** |  **75.113 ms** |  **56.110 ms** |  **69.815 ms** |  **73.580 ms** |  **82.085 ms** |  **94.607 ms** |
| **UpdateAsync** |          **EfCore** | **10000** | **742.3001 ms** | **742.857 ms** | **666.838 ms** | **705.123 ms** | **735.630 ms** | **784.005 ms** | **838.956 ms** |