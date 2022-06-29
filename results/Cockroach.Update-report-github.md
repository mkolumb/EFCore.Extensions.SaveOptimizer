``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-LFETAU : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |       Mean |       Error |      StdDev |     Median |        Min |          Max |
|------------ |---------------- |------ |-----------:|------------:|------------:|-----------:|-----------:|-------------:|
| **UpdateAsync** |       **Optimized** |     **1** |   **2.057 ms** |   **0.1003 ms** |   **0.2027 ms** |   **2.005 ms** |   **1.721 ms** |     **2.751 ms** |
| **UpdateAsync** |       **Optimized** |    **10** |   **2.382 ms** |   **0.1772 ms** |   **0.3579 ms** |   **2.307 ms** |   **1.979 ms** |     **3.942 ms** |
| **UpdateAsync** |       **Optimized** |    **25** |   **2.359 ms** |   **0.1368 ms** |   **0.2763 ms** |   **2.274 ms** |   **2.073 ms** |     **3.444 ms** |
| **UpdateAsync** |       **Optimized** |    **50** |   **2.574 ms** |   **0.2221 ms** |   **0.4487 ms** |   **2.449 ms** |   **2.072 ms** |     **4.580 ms** |
| **UpdateAsync** |       **Optimized** |   **100** |   **3.159 ms** |   **0.2152 ms** |   **0.4348 ms** |   **3.023 ms** |   **2.623 ms** |     **4.878 ms** |
| **UpdateAsync** |       **Optimized** |  **1000** |  **18.572 ms** |   **1.1339 ms** |   **2.2906 ms** |  **18.094 ms** |  **14.755 ms** |    **25.921 ms** |
| **UpdateAsync** |       **Optimized** | **10000** | **292.799 ms** | **208.5106 ms** | **421.2017 ms** | **154.529 ms** | **115.818 ms** | **1,619.538 ms** |
| **UpdateAsync** | **OptimizedDapper** |     **1** |   **2.207 ms** |   **0.3141 ms** |   **0.6344 ms** |   **2.013 ms** |   **1.805 ms** |     **6.125 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **10** |   **2.619 ms** |   **0.4220 ms** |   **0.8525 ms** |   **2.355 ms** |   **2.079 ms** |     **6.537 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **25** |   **2.596 ms** |   **0.2129 ms** |   **0.4301 ms** |   **2.444 ms** |   **2.165 ms** |     **4.696 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **50** |   **2.748 ms** |   **0.2134 ms** |   **0.4310 ms** |   **2.600 ms** |   **2.282 ms** |     **4.561 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **100** |   **3.743 ms** |   **0.6078 ms** |   **1.2278 ms** |   **3.289 ms** |   **2.720 ms** |     **8.352 ms** |
| **UpdateAsync** | **OptimizedDapper** |  **1000** |  **18.558 ms** |   **1.6300 ms** |   **3.2927 ms** |  **18.588 ms** |  **13.890 ms** |    **29.051 ms** |
| **UpdateAsync** | **OptimizedDapper** | **10000** | **152.568 ms** |   **8.4873 ms** |  **17.1448 ms** | **145.555 ms** | **122.161 ms** |   **198.668 ms** |
| **UpdateAsync** |          **EfCore** |     **1** |   **2.354 ms** |   **0.3676 ms** |   **0.7426 ms** |   **2.148 ms** |   **1.853 ms** |     **7.107 ms** |
| **UpdateAsync** |          **EfCore** |    **10** |   **3.351 ms** |   **0.6561 ms** |   **1.3253 ms** |   **2.941 ms** |   **2.489 ms** |     **9.555 ms** |
| **UpdateAsync** |          **EfCore** |    **25** |   **6.234 ms** |   **0.8981 ms** |   **1.8141 ms** |   **7.073 ms** |   **3.412 ms** |     **9.923 ms** |
| **UpdateAsync** |          **EfCore** |    **50** |   **8.415 ms** |   **1.0237 ms** |   **2.0679 ms** |   **8.624 ms** |   **4.849 ms** |    **13.568 ms** |
| **UpdateAsync** |          **EfCore** |   **100** |  **11.306 ms** |   **1.1346 ms** |   **2.2920 ms** |  **11.598 ms** |   **7.829 ms** |    **17.382 ms** |
| **UpdateAsync** |          **EfCore** |  **1000** |  **75.527 ms** |   **1.8745 ms** |   **3.7866 ms** |  **74.758 ms** |  **67.984 ms** |    **83.293 ms** |
| **UpdateAsync** |          **EfCore** | **10000** | **843.419 ms** |  **44.8323 ms** |  **90.5634 ms** | **801.100 ms** | **743.192 ms** | **1,041.356 ms** |
