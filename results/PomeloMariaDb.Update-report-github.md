``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-WAPFFE : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |       Mean |      Error |     StdDev |        Min |        Max |     Median |
|------------ |---------------- |------ |-----------:|-----------:|-----------:|-----------:|-----------:|-----------:|
| **UpdateAsync** |       **Optimized** |     **1** |   **7.145 ms** |  **0.2522 ms** |  **0.5095 ms** |   **6.185 ms** |   **8.782 ms** |   **7.037 ms** |
| **UpdateAsync** |       **Optimized** |    **10** |   **7.102 ms** |  **0.3009 ms** |  **0.6077 ms** |   **6.069 ms** |   **8.739 ms** |   **7.003 ms** |
| **UpdateAsync** |       **Optimized** |    **25** |   **7.105 ms** |  **0.2237 ms** |  **0.4520 ms** |   **6.258 ms** |   **8.573 ms** |   **7.062 ms** |
| **UpdateAsync** |       **Optimized** |    **50** |   **7.458 ms** |  **0.2922 ms** |  **0.5903 ms** |   **6.510 ms** |   **9.258 ms** |   **7.342 ms** |
| **UpdateAsync** |       **Optimized** |   **100** |   **7.435 ms** |  **0.2244 ms** |  **0.4533 ms** |   **6.678 ms** |   **8.706 ms** |   **7.439 ms** |
| **UpdateAsync** |       **Optimized** |  **1000** |  **12.134 ms** |  **0.6933 ms** |  **1.4004 ms** |  **10.165 ms** |  **19.353 ms** |  **11.805 ms** |
| **UpdateAsync** |       **Optimized** | **10000** |  **58.697 ms** |  **1.3422 ms** |  **2.7112 ms** |  **54.362 ms** |  **66.415 ms** |  **58.232 ms** |
| **UpdateAsync** | **OptimizedDapper** |     **1** |   **7.047 ms** |  **0.2704 ms** |  **0.5462 ms** |   **6.109 ms** |   **8.755 ms** |   **6.977 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **10** |   **7.060 ms** |  **0.3221 ms** |  **0.6507 ms** |   **6.125 ms** |   **9.505 ms** |   **6.934 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **25** |   **7.071 ms** |  **0.3331 ms** |  **0.6729 ms** |   **6.021 ms** |   **9.478 ms** |   **6.961 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **50** |   **7.523 ms** |  **0.2900 ms** |  **0.5858 ms** |   **6.364 ms** |   **9.429 ms** |   **7.476 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **100** |   **7.100 ms** |  **0.3782 ms** |  **0.7639 ms** |   **5.711 ms** |   **8.385 ms** |   **7.243 ms** |
| **UpdateAsync** | **OptimizedDapper** |  **1000** |   **9.883 ms** |  **0.3339 ms** |  **0.6746 ms** |   **8.932 ms** |  **12.010 ms** |   **9.765 ms** |
| **UpdateAsync** | **OptimizedDapper** | **10000** |  **50.268 ms** |  **0.8296 ms** |  **1.6758 ms** |  **47.259 ms** |  **55.447 ms** |  **50.216 ms** |
| **UpdateAsync** |          **EfCore** |     **1** |   **5.471 ms** |  **0.2242 ms** |  **0.4528 ms** |   **4.766 ms** |   **7.291 ms** |   **5.359 ms** |
| **UpdateAsync** |          **EfCore** |    **10** |   **5.774 ms** |  **0.1941 ms** |  **0.3921 ms** |   **4.959 ms** |   **7.226 ms** |   **5.695 ms** |
| **UpdateAsync** |          **EfCore** |    **25** |   **6.313 ms** |  **0.2847 ms** |  **0.5752 ms** |   **5.180 ms** |   **7.819 ms** |   **6.223 ms** |
| **UpdateAsync** |          **EfCore** |    **50** |   **6.947 ms** |  **0.3710 ms** |  **0.7494 ms** |   **5.714 ms** |   **9.864 ms** |   **6.907 ms** |
| **UpdateAsync** |          **EfCore** |   **100** |   **8.511 ms** |  **0.4736 ms** |  **0.9568 ms** |   **6.570 ms** |  **10.443 ms** |   **8.527 ms** |
| **UpdateAsync** |          **EfCore** |  **1000** |  **36.421 ms** |  **1.6771 ms** |  **3.3879 ms** |  **30.872 ms** |  **42.359 ms** |  **36.884 ms** |
| **UpdateAsync** |          **EfCore** | **10000** | **355.509 ms** | **15.3274 ms** | **30.9620 ms** | **292.979 ms** | **470.309 ms** | **358.707 ms** |
