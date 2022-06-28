``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-BUPMKE : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |       Mean |     Error |     StdDev |     Median |        Min |        Max |
|------------ |---------------- |------ |-----------:|----------:|-----------:|-----------:|-----------:|-----------:|
| **InsertAsync** |       **Optimized** |     **1** |   **2.081 ms** | **0.1662 ms** |  **0.3358 ms** |   **2.025 ms** |   **1.538 ms** |   **3.158 ms** |
| **InsertAsync** |       **Optimized** |    **10** |   **2.072 ms** | **0.1912 ms** |  **0.3862 ms** |   **1.961 ms** |   **1.551 ms** |   **3.009 ms** |
| **InsertAsync** |       **Optimized** |    **25** |   **2.238 ms** | **0.2144 ms** |  **0.4332 ms** |   **2.105 ms** |   **1.655 ms** |   **3.314 ms** |
| **InsertAsync** |       **Optimized** |    **50** |   **2.755 ms** | **0.3664 ms** |  **0.7401 ms** |   **2.580 ms** |   **1.844 ms** |   **5.778 ms** |
| **InsertAsync** |       **Optimized** |   **100** |   **3.265 ms** | **0.3067 ms** |  **0.6194 ms** |   **3.182 ms** |   **2.365 ms** |   **4.924 ms** |
| **InsertAsync** |       **Optimized** |  **1000** |   **9.672 ms** | **1.0099 ms** |  **2.0401 ms** |   **8.724 ms** |   **7.388 ms** |  **14.322 ms** |
| **InsertAsync** |       **Optimized** | **10000** |  **87.291 ms** | **3.8446 ms** |  **7.7663 ms** |  **86.672 ms** |  **71.993 ms** | **109.419 ms** |
| **InsertAsync** | **OptimizedDapper** |     **1** |   **1.643 ms** | **0.1261 ms** |  **0.2547 ms** |   **1.561 ms** |   **1.278 ms** |   **2.604 ms** |
| **InsertAsync** | **OptimizedDapper** |    **10** |   **1.836 ms** | **0.1391 ms** |  **0.2810 ms** |   **1.768 ms** |   **1.501 ms** |   **2.823 ms** |
| **InsertAsync** | **OptimizedDapper** |    **25** |   **1.779 ms** | **0.1104 ms** |  **0.2230 ms** |   **1.722 ms** |   **1.443 ms** |   **2.474 ms** |
| **InsertAsync** | **OptimizedDapper** |    **50** |   **1.814 ms** | **0.0785 ms** |  **0.1585 ms** |   **1.803 ms** |   **1.597 ms** |   **2.397 ms** |
| **InsertAsync** | **OptimizedDapper** |   **100** |   **2.156 ms** | **0.1049 ms** |  **0.2120 ms** |   **2.131 ms** |   **1.808 ms** |   **2.995 ms** |
| **InsertAsync** | **OptimizedDapper** |  **1000** |   **8.015 ms** | **1.0074 ms** |  **2.0350 ms** |   **6.859 ms** |   **5.805 ms** |  **12.605 ms** |
| **InsertAsync** | **OptimizedDapper** | **10000** |  **75.477 ms** | **2.9608 ms** |  **5.9810 ms** |  **74.925 ms** |  **66.324 ms** |  **91.512 ms** |
| **InsertAsync** |          **EfCore** |     **1** |   **1.569 ms** | **0.1592 ms** |  **0.3215 ms** |   **1.478 ms** |   **1.244 ms** |   **2.617 ms** |
| **InsertAsync** |          **EfCore** |    **10** |   **4.760 ms** | **1.1563 ms** |  **2.3359 ms** |   **6.167 ms** |   **1.273 ms** |   **7.288 ms** |
| **InsertAsync** |          **EfCore** |    **25** |   **5.013 ms** | **1.1665 ms** |  **2.3564 ms** |   **6.244 ms** |   **1.467 ms** |   **7.842 ms** |
| **InsertAsync** |          **EfCore** |    **50** |   **5.242 ms** | **1.0446 ms** |  **2.1102 ms** |   **6.453 ms** |   **1.740 ms** |   **7.450 ms** |
| **InsertAsync** |          **EfCore** |   **100** |   **5.938 ms** | **1.0269 ms** |  **2.0744 ms** |   **7.026 ms** |   **2.295 ms** |   **8.345 ms** |
| **InsertAsync** |          **EfCore** |  **1000** |  **12.649 ms** | **0.4673 ms** |  **0.9440 ms** |  **12.612 ms** |  **11.174 ms** |  **15.508 ms** |
| **InsertAsync** |          **EfCore** | **10000** | **138.942 ms** | **5.4133 ms** | **10.9350 ms** | **140.295 ms** | **117.522 ms** | **159.045 ms** |
