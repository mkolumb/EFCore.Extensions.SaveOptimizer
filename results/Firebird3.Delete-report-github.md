``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-DQDBFV : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |       Mean |      Error |      StdDev |     Median |        Min |        Max |
|------------ |---------------- |----- |-----------:|-----------:|------------:|-----------:|-----------:|-----------:|
| **Delete** |       **Optimized** |    **1** |   **5.874 ms** |  **0.3356 ms** |   **0.6780 ms** |   **5.766 ms** |   **5.048 ms** |   **9.329 ms** |
| **Delete** |       **Optimized** |   **10** |   **5.905 ms** |  **0.3179 ms** |   **0.6422 ms** |   **5.795 ms** |   **5.089 ms** |   **9.108 ms** |
| **Delete** |       **Optimized** |   **25** |   **6.418 ms** |  **0.2836 ms** |   **0.5729 ms** |   **6.338 ms** |   **5.555 ms** |   **8.760 ms** |
| **Delete** |       **Optimized** |   **50** |   **7.135 ms** |  **0.4036 ms** |   **0.8152 ms** |   **6.960 ms** |   **5.970 ms** |   **9.521 ms** |
| **Delete** |       **Optimized** |  **100** |   **9.054 ms** |  **0.6189 ms** |   **1.2502 ms** |   **9.064 ms** |   **6.561 ms** |  **11.297 ms** |
| **Delete** |       **Optimized** | **1000** | **166.819 ms** | **48.5092 ms** |  **97.9910 ms** | **172.466 ms** |  **36.463 ms** | **338.381 ms** |
| **Delete** | **Optimized Dapper** |    **1** |   **4.722 ms** |  **0.2702 ms** |   **0.5458 ms** |   **4.506 ms** |   **4.062 ms** |   **6.378 ms** |
| **Delete** | **Optimized Dapper** |   **10** |   **4.950 ms** |  **0.2763 ms** |   **0.5581 ms** |   **4.773 ms** |   **4.379 ms** |   **7.319 ms** |
| **Delete** | **Optimized Dapper** |   **25** |   **6.448 ms** |  **0.7055 ms** |   **1.4251 ms** |   **6.119 ms** |   **4.838 ms** |  **12.510 ms** |
| **Delete** | **Optimized Dapper** |   **50** |   **7.495 ms** |  **0.4788 ms** |   **0.9672 ms** |   **7.178 ms** |   **5.884 ms** |   **9.771 ms** |
| **Delete** | **Optimized Dapper** |  **100** |   **9.388 ms** |  **0.7418 ms** |   **1.4984 ms** |   **8.958 ms** |   **7.307 ms** |  **13.677 ms** |
| **Delete** | **Optimized Dapper** | **1000** | **166.571 ms** | **50.9624 ms** | **102.9466 ms** | **175.212 ms** |  **34.549 ms** | **334.356 ms** |
| **Delete** |          **EF Core** |    **1** |   **6.824 ms** |  **0.4989 ms** |   **1.0077 ms** |   **6.475 ms** |   **5.877 ms** |  **10.758 ms** |
| **Delete** |          **EF Core** |   **10** |   **9.620 ms** |  **0.5757 ms** |   **1.1630 ms** |   **9.270 ms** |   **8.351 ms** |  **14.164 ms** |
| **Delete** |          **EF Core** |   **25** |  **15.816 ms** |  **0.5957 ms** |   **1.2033 ms** |  **15.553 ms** |  **14.027 ms** |  **20.616 ms** |
| **Delete** |          **EF Core** |   **50** |  **25.938 ms** |  **0.8329 ms** |   **1.6825 ms** |  **25.655 ms** |  **22.800 ms** |  **30.354 ms** |
| **Delete** |          **EF Core** |  **100** |  **44.561 ms** |  **0.9653 ms** |   **1.9499 ms** |  **44.191 ms** |  **41.182 ms** |  **50.938 ms** |
| **Delete** |          **EF Core** | **1000** | **482.079 ms** | **26.4901 ms** |  **53.5113 ms** | **513.977 ms** | **399.514 ms** | **544.707 ms** |
