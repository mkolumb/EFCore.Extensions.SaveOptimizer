``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]    : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  SqlServer : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Job=SqlServer  EvaluateOverhead=True  OutlierMode=RemoveUpper  
InvocationCount=1  IterationCount=15  LaunchCount=8  
RunStrategy=Throughput  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |     MeanNoOut |         Mean |          Min |           Q1 |       Median |           Q3 |         Max |
|------------ |---------------- |------ |--------------:|-------------:|-------------:|-------------:|-------------:|-------------:|------------:|
| **InsertAsync** |       **Optimized** |     **1** |     **9.7443 ms** |     **9.790 ms** |     **8.494 ms** |     **9.249 ms** |     **9.729 ms** |    **10.238 ms** |    **12.82 ms** |
| **InsertAsync** |       **Optimized** |    **10** |    **10.6115 ms** |    **10.607 ms** |     **9.134 ms** |    **10.177 ms** |    **10.621 ms** |    **10.995 ms** |    **12.01 ms** |
| **InsertAsync** |       **Optimized** |    **25** |    **11.8269 ms** |    **12.144 ms** |    **10.194 ms** |    **11.376 ms** |    **11.794 ms** |    **12.407 ms** |    **17.38 ms** |
| **InsertAsync** |       **Optimized** |    **50** |    **13.2477 ms** |    **13.675 ms** |    **11.250 ms** |    **12.784 ms** |    **13.202 ms** |    **13.941 ms** |    **23.40 ms** |
| **InsertAsync** |       **Optimized** |   **100** |    **18.8649 ms** |    **19.031 ms** |    **17.155 ms** |    **18.290 ms** |    **18.874 ms** |    **19.635 ms** |    **24.89 ms** |
| **InsertAsync** |       **Optimized** |  **1000** |    **98.1149 ms** |    **99.390 ms** |    **82.315 ms** |    **93.306 ms** |    **98.041 ms** |   **104.288 ms** |   **143.57 ms** |
| **InsertAsync** |       **Optimized** | **10000** |   **898.9038 ms** |   **906.372 ms** |   **769.672 ms** |   **843.891 ms** |   **885.407 ms** |   **969.856 ms** | **1,061.38 ms** |
| **InsertAsync** | **OptimizedDapper** |     **1** |     **9.5148 ms** |     **9.612 ms** |     **8.057 ms** |     **9.129 ms** |     **9.546 ms** |     **9.878 ms** |    **13.64 ms** |
| **InsertAsync** | **OptimizedDapper** |    **10** |    **10.2195 ms** |    **10.257 ms** |     **8.388 ms** |     **9.719 ms** |    **10.301 ms** |    **10.608 ms** |    **13.71 ms** |
| **InsertAsync** | **OptimizedDapper** |    **25** |    **11.2972 ms** |    **11.431 ms** |     **9.940 ms** |    **10.866 ms** |    **11.305 ms** |    **11.867 ms** |    **15.53 ms** |
| **InsertAsync** | **OptimizedDapper** |    **50** |    **12.9310 ms** |    **13.045 ms** |    **11.660 ms** |    **12.584 ms** |    **12.934 ms** |    **13.244 ms** |    **16.58 ms** |
| **InsertAsync** | **OptimizedDapper** |   **100** |    **18.7715 ms** |    **19.077 ms** |    **16.500 ms** |    **17.977 ms** |    **18.784 ms** |    **19.642 ms** |    **28.52 ms** |
| **InsertAsync** | **OptimizedDapper** |  **1000** |    **95.1410 ms** |    **95.169 ms** |    **82.565 ms** |    **90.956 ms** |    **95.356 ms** |   **100.107 ms** |   **114.87 ms** |
| **InsertAsync** | **OptimizedDapper** | **10000** |   **967.8442 ms** |   **960.569 ms** |   **819.346 ms** |   **942.487 ms** |   **967.260 ms** |   **990.331 ms** | **1,060.85 ms** |
| **InsertAsync** |          **EfCore** |     **1** |     **9.3658 ms** |     **9.438 ms** |     **8.226 ms** |     **9.000 ms** |     **9.383 ms** |     **9.784 ms** |    **11.14 ms** |
| **InsertAsync** |          **EfCore** |    **10** |    **10.4730 ms** |    **10.522 ms** |     **9.136 ms** |    **10.095 ms** |    **10.437 ms** |    **10.991 ms** |    **12.26 ms** |
| **InsertAsync** |          **EfCore** |    **25** |    **12.7798 ms** |    **12.867 ms** |    **11.182 ms** |    **12.304 ms** |    **12.746 ms** |    **13.299 ms** |    **16.40 ms** |
| **InsertAsync** |          **EfCore** |    **50** |    **18.3702 ms** |    **18.677 ms** |    **16.517 ms** |    **17.776 ms** |    **18.404 ms** |    **19.067 ms** |    **24.16 ms** |
| **InsertAsync** |          **EfCore** |   **100** |    **27.9435 ms** |    **28.661 ms** |    **24.270 ms** |    **26.629 ms** |    **27.772 ms** |    **30.109 ms** |    **40.28 ms** |
| **InsertAsync** |          **EfCore** |  **1000** |   **198.2151 ms** |   **199.939 ms** |   **168.547 ms** |   **188.210 ms** |   **198.874 ms** |   **208.652 ms** |   **253.66 ms** |
| **InsertAsync** |          **EfCore** | **10000** | **2,219.3137 ms** | **2,186.821 ms** | **1,853.629 ms** | **1,973.174 ms** | **2,269.693 ms** | **2,334.449 ms** | **2,471.78 ms** |
