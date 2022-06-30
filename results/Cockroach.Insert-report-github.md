``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]    : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Cockroach : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Job=Cockroach  EvaluateOverhead=True  OutlierMode=RemoveUpper  
InvocationCount=1  IterationCount=15  LaunchCount=8  
RunStrategy=Throughput  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |     MeanNoOut |         Mean |          Min |           Q1 |       Median |           Q3 |           Max |
|------------ |---------------- |------ |--------------:|-------------:|-------------:|-------------:|-------------:|-------------:|--------------:|
| **InsertAsync** |       **Optimized** |     **1** |    **14.4152 ms** |    **13.686 ms** |     **6.772 ms** |    **11.584 ms** |    **14.869 ms** |    **15.844 ms** |     **17.362 ms** |
| **InsertAsync** |       **Optimized** |    **10** |    **10.4980 ms** |    **11.153 ms** |     **8.102 ms** |     **9.287 ms** |    **10.136 ms** |    **13.315 ms** |     **18.827 ms** |
| **InsertAsync** |       **Optimized** |    **25** |    **11.1710 ms** |    **11.524 ms** |     **8.788 ms** |    **10.309 ms** |    **11.007 ms** |    **12.308 ms** |     **18.340 ms** |
| **InsertAsync** |       **Optimized** |    **50** |    **14.2070 ms** |    **14.609 ms** |    **11.492 ms** |    **13.227 ms** |    **14.083 ms** |    **15.451 ms** |     **21.689 ms** |
| **InsertAsync** |       **Optimized** |   **100** |    **21.8634 ms** |    **22.324 ms** |    **19.509 ms** |    **20.953 ms** |    **21.812 ms** |    **22.951 ms** |     **33.675 ms** |
| **InsertAsync** |       **Optimized** |  **1000** |   **132.2865 ms** |   **135.671 ms** |   **109.864 ms** |   **121.603 ms** |   **132.714 ms** |   **145.455 ms** |    **189.335 ms** |
| **InsertAsync** |       **Optimized** | **10000** | **1,494.4136 ms** | **1,496.491 ms** | **1,231.573 ms** | **1,372.293 ms** | **1,501.670 ms** | **1,589.964 ms** |  **1,966.019 ms** |
| **InsertAsync** | **OptimizedDapper** |     **1** |     **7.2832 ms** |     **7.194 ms** |     **5.017 ms** |     **6.492 ms** |     **7.307 ms** |     **7.920 ms** |      **9.687 ms** |
| **InsertAsync** | **OptimizedDapper** |    **10** |     **9.4889 ms** |    **11.114 ms** |     **7.516 ms** |     **8.704 ms** |     **9.416 ms** |    **10.707 ms** |     **25.634 ms** |
| **InsertAsync** | **OptimizedDapper** |    **25** |    **10.2127 ms** |    **10.358 ms** |     **8.251 ms** |     **9.385 ms** |    **10.236 ms** |    **10.991 ms** |     **14.739 ms** |
| **InsertAsync** | **OptimizedDapper** |    **50** |    **12.7156 ms** |    **13.845 ms** |    **10.756 ms** |    **11.795 ms** |    **12.654 ms** |    **14.188 ms** |     **35.536 ms** |
| **InsertAsync** | **OptimizedDapper** |   **100** |    **20.2922 ms** |    **20.729 ms** |    **17.455 ms** |    **19.568 ms** |    **20.320 ms** |    **21.179 ms** |     **28.831 ms** |
| **InsertAsync** | **OptimizedDapper** |  **1000** |   **117.2103 ms** |   **122.066 ms** |    **99.911 ms** |   **106.414 ms** |   **115.272 ms** |   **133.563 ms** |    **185.579 ms** |
| **InsertAsync** | **OptimizedDapper** | **10000** | **1,567.4699 ms** | **1,595.173 ms** | **1,267.572 ms** | **1,481.130 ms** | **1,556.881 ms** | **1,688.969 ms** |  **2,046.517 ms** |
| **InsertAsync** |          **EfCore** |     **1** |     **8.3667 ms** |     **8.430 ms** |     **6.090 ms** |     **7.899 ms** |     **8.322 ms** |     **8.869 ms** |     **12.144 ms** |
| **InsertAsync** |          **EfCore** |    **10** |    **34.3809 ms** |    **35.783 ms** |    **10.493 ms** |    **13.496 ms** |    **28.990 ms** |    **56.878 ms** |     **86.375 ms** |
| **InsertAsync** |          **EfCore** |    **25** |    **44.4174 ms** |    **47.368 ms** |    **17.666 ms** |    **21.640 ms** |    **56.736 ms** |    **65.730 ms** |    **152.295 ms** |
| **InsertAsync** |          **EfCore** |    **50** |    **58.0332 ms** |    **57.669 ms** |    **30.850 ms** |    **35.556 ms** |    **68.719 ms** |    **75.715 ms** |    **111.402 ms** |
| **InsertAsync** |          **EfCore** |   **100** |    **82.6644 ms** |    **85.695 ms** |    **52.578 ms** |    **64.261 ms** |    **87.141 ms** |   **103.559 ms** |    **161.586 ms** |
| **InsertAsync** |          **EfCore** |  **1000** |   **599.0041 ms** |   **608.098 ms** |   **501.997 ms** |   **559.731 ms** |   **597.042 ms** |   **651.807 ms** |    **801.886 ms** |
| **InsertAsync** |          **EfCore** | **10000** | **7,880.3053 ms** | **7,879.086 ms** | **5,934.367 ms** | **7,288.550 ms** | **7,794.031 ms** | **8,566.818 ms** | **10,641.411 ms** |
