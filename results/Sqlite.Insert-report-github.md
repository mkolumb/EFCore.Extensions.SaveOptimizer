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
|      Method |         Variant |  Rows |     MeanNoOut |         Mean |        Min |           Q1 |       Median |           Q3 |          Max |
|------------ |---------------- |------ |--------------:|-------------:|-----------:|-------------:|-------------:|-------------:|-------------:|
| **InsertAsync** |       **Optimized** |     **1** |     **1.8049 ms** |     **1.911 ms** |   **1.646 ms** |     **1.750 ms** |     **1.800 ms** |     **1.872 ms** |     **4.828 ms** |
| **InsertAsync** |       **Optimized** |    **10** |     **2.4414 ms** |     **2.456 ms** |   **2.205 ms** |     **2.352 ms** |     **2.442 ms** |     **2.539 ms** |     **2.837 ms** |
| **InsertAsync** |       **Optimized** |    **25** |     **4.7116 ms** |     **4.677 ms** |   **3.109 ms** |     **4.509 ms** |     **4.699 ms** |     **4.941 ms** |     **6.032 ms** |
| **InsertAsync** |       **Optimized** |    **50** |     **6.9492 ms** |     **6.971 ms** |   **6.113 ms** |     **6.632 ms** |     **6.940 ms** |     **7.264 ms** |     **9.020 ms** |
| **InsertAsync** |       **Optimized** |   **100** |     **9.7985 ms** |     **9.805 ms** |   **8.024 ms** |     **9.226 ms** |     **9.744 ms** |    **10.407 ms** |    **12.284 ms** |
| **InsertAsync** |       **Optimized** |  **1000** |    **72.7598 ms** |    **74.467 ms** |  **51.964 ms** |    **61.739 ms** |    **71.065 ms** |    **87.512 ms** |   **104.218 ms** |
| **InsertAsync** |       **Optimized** | **10000** | **1,100.4639 ms** | **1,104.733 ms** | **925.588 ms** | **1,031.410 ms** | **1,081.169 ms** | **1,195.153 ms** | **1,270.518 ms** |
| **InsertAsync** | **OptimizedDapper** |     **1** |     **1.8369 ms** |     **1.967 ms** |   **1.638 ms** |     **1.772 ms** |     **1.832 ms** |     **1.903 ms** |     **5.092 ms** |
| **InsertAsync** | **OptimizedDapper** |    **10** |     **2.4463 ms** |     **2.468 ms** |   **2.201 ms** |     **2.374 ms** |     **2.447 ms** |     **2.527 ms** |     **3.877 ms** |
| **InsertAsync** | **OptimizedDapper** |    **25** |     **4.5279 ms** |     **4.481 ms** |   **3.037 ms** |     **4.285 ms** |     **4.496 ms** |     **4.833 ms** |     **6.809 ms** |
| **InsertAsync** | **OptimizedDapper** |    **50** |     **6.7350 ms** |     **6.930 ms** |   **5.854 ms** |     **6.361 ms** |     **6.675 ms** |     **7.384 ms** |     **9.654 ms** |
| **InsertAsync** | **OptimizedDapper** |   **100** |    **10.4502 ms** |    **10.768 ms** |   **7.890 ms** |    **10.011 ms** |    **10.373 ms** |    **11.305 ms** |    **18.291 ms** |
| **InsertAsync** | **OptimizedDapper** |  **1000** |    **80.2187 ms** |    **83.333 ms** |  **57.502 ms** |    **68.967 ms** |    **78.301 ms** |    **96.728 ms** |   **126.017 ms** |
| **InsertAsync** | **OptimizedDapper** | **10000** | **1,179.5229 ms** | **1,166.419 ms** | **946.028 ms** | **1,118.328 ms** | **1,186.517 ms** | **1,233.398 ms** | **1,321.819 ms** |
| **InsertAsync** |          **EfCore** |     **1** |     **1.6790 ms** |     **1.681 ms** |   **1.509 ms** |     **1.613 ms** |     **1.688 ms** |     **1.732 ms** |     **1.905 ms** |
| **InsertAsync** |          **EfCore** |    **10** |     **2.6898 ms** |     **2.704 ms** |   **2.373 ms** |     **2.594 ms** |     **2.696 ms** |     **2.776 ms** |     **3.138 ms** |
| **InsertAsync** |          **EfCore** |    **25** |     **5.6866 ms** |     **5.608 ms** |   **4.059 ms** |     **5.458 ms** |     **5.698 ms** |     **5.939 ms** |     **6.926 ms** |
| **InsertAsync** |          **EfCore** |    **50** |     **8.5770 ms** |     **8.610 ms** |   **7.281 ms** |     **8.252 ms** |     **8.599 ms** |     **8.953 ms** |    **10.075 ms** |
| **InsertAsync** |          **EfCore** |   **100** |    **14.2216 ms** |    **14.264 ms** |  **11.631 ms** |    **13.626 ms** |    **14.236 ms** |    **14.846 ms** |    **17.961 ms** |
| **InsertAsync** |          **EfCore** |  **1000** |    **97.2600 ms** |    **98.287 ms** |  **75.566 ms** |    **87.385 ms** |    **97.012 ms** |   **109.563 ms** |   **129.982 ms** |
| **InsertAsync** |          **EfCore** | **10000** | **1,252.1201 ms** | **1,239.751 ms** | **995.563 ms** | **1,196.886 ms** | **1,253.080 ms** | **1,304.583 ms** | **1,388.497 ms** |