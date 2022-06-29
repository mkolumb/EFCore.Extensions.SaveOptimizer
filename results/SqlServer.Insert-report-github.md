``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-DRNIHV : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |       Mean |     Error |     StdDev |        Min |        Max |     Median |
|------------ |---------------- |------ |-----------:|----------:|-----------:|-----------:|-----------:|-----------:|
| **InsertAsync** |       **Optimized** |     **1** |   **4.187 ms** | **0.1630 ms** |  **0.3293 ms** |   **3.639 ms** |   **5.269 ms** |   **4.126 ms** |
| **InsertAsync** |       **Optimized** |    **10** |   **4.273 ms** | **0.2277 ms** |  **0.4600 ms** |   **3.666 ms** |   **5.929 ms** |   **4.164 ms** |
| **InsertAsync** |       **Optimized** |    **25** |   **4.357 ms** | **0.1921 ms** |  **0.3881 ms** |   **3.664 ms** |   **5.777 ms** |   **4.273 ms** |
| **InsertAsync** |       **Optimized** |    **50** |   **4.466 ms** | **0.1541 ms** |  **0.3113 ms** |   **3.938 ms** |   **5.452 ms** |   **4.450 ms** |
| **InsertAsync** |       **Optimized** |   **100** |   **5.316 ms** | **0.3640 ms** |  **0.7353 ms** |   **4.412 ms** |   **7.464 ms** |   **5.041 ms** |
| **InsertAsync** |       **Optimized** |  **1000** |  **13.368 ms** | **0.5433 ms** |  **1.0976 ms** |  **11.193 ms** |  **17.735 ms** |  **13.325 ms** |
| **InsertAsync** |       **Optimized** | **10000** |  **99.998 ms** | **1.4965 ms** |  **3.0230 ms** |  **94.210 ms** | **106.882 ms** |  **99.966 ms** |
| **InsertAsync** | **OptimizedDapper** |     **1** |   **4.261 ms** | **0.2081 ms** |  **0.4204 ms** |   **3.605 ms** |   **6.016 ms** |   **4.183 ms** |
| **InsertAsync** | **OptimizedDapper** |    **10** |   **4.248 ms** | **0.1531 ms** |  **0.3092 ms** |   **3.765 ms** |   **4.945 ms** |   **4.216 ms** |
| **InsertAsync** | **OptimizedDapper** |    **25** |   **4.363 ms** | **0.1773 ms** |  **0.3581 ms** |   **3.828 ms** |   **5.295 ms** |   **4.299 ms** |
| **InsertAsync** | **OptimizedDapper** |    **50** |   **4.467 ms** | **0.2107 ms** |  **0.4256 ms** |   **3.928 ms** |   **5.730 ms** |   **4.345 ms** |
| **InsertAsync** | **OptimizedDapper** |   **100** |   **5.471 ms** | **0.2781 ms** |  **0.5618 ms** |   **4.503 ms** |   **7.063 ms** |   **5.420 ms** |
| **InsertAsync** | **OptimizedDapper** |  **1000** |  **13.399 ms** | **0.5661 ms** |  **1.1435 ms** |  **11.727 ms** |  **17.230 ms** |  **13.072 ms** |
| **InsertAsync** | **OptimizedDapper** | **10000** |  **96.431 ms** | **3.9122 ms** |  **7.9028 ms** |  **84.186 ms** | **130.067 ms** |  **96.651 ms** |
| **InsertAsync** |          **EfCore** |     **1** |   **3.526 ms** | **0.1676 ms** |  **0.3387 ms** |   **3.020 ms** |   **4.791 ms** |   **3.441 ms** |
| **InsertAsync** |          **EfCore** |    **10** |   **3.709 ms** | **0.2120 ms** |  **0.4282 ms** |   **3.109 ms** |   **5.060 ms** |   **3.586 ms** |
| **InsertAsync** |          **EfCore** |    **25** |   **3.839 ms** | **0.1622 ms** |  **0.3277 ms** |   **3.338 ms** |   **4.817 ms** |   **3.762 ms** |
| **InsertAsync** |          **EfCore** |    **50** |   **4.628 ms** | **0.3001 ms** |  **0.6061 ms** |   **3.829 ms** |   **6.372 ms** |   **4.502 ms** |
| **InsertAsync** |          **EfCore** |   **100** |   **5.848 ms** | **0.4173 ms** |  **0.8429 ms** |   **4.741 ms** |   **9.789 ms** |   **5.676 ms** |
| **InsertAsync** |          **EfCore** |  **1000** |  **22.479 ms** | **0.7578 ms** |  **1.5307 ms** |  **20.283 ms** |  **28.129 ms** |  **22.015 ms** |
| **InsertAsync** |          **EfCore** | **10000** | **229.461 ms** | **9.2029 ms** | **18.5903 ms** | **191.449 ms** | **268.602 ms** | **232.845 ms** |
