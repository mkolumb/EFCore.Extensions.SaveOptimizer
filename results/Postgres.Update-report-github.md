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
| **UpdateAsync** |       **Optimized** |     **1** |   **1.787 ms** | **0.1398 ms** |  **0.2824 ms** |   **1.686 ms** |   **1.482 ms** |   **2.602 ms** |
| **UpdateAsync** |       **Optimized** |    **10** |   **1.904 ms** | **0.1593 ms** |  **0.3218 ms** |   **1.881 ms** |   **1.324 ms** |   **2.971 ms** |
| **UpdateAsync** |       **Optimized** |    **25** |   **2.064 ms** | **0.1790 ms** |  **0.3616 ms** |   **1.941 ms** |   **1.703 ms** |   **3.241 ms** |
| **UpdateAsync** |       **Optimized** |    **50** |   **2.174 ms** | **0.2046 ms** |  **0.4132 ms** |   **2.055 ms** |   **1.770 ms** |   **4.036 ms** |
| **UpdateAsync** |       **Optimized** |   **100** |   **2.389 ms** | **0.1697 ms** |  **0.3428 ms** |   **2.277 ms** |   **1.815 ms** |   **3.693 ms** |
| **UpdateAsync** |       **Optimized** |  **1000** |   **8.328 ms** | **1.1766 ms** |  **2.3768 ms** |   **7.350 ms** |   **5.499 ms** |  **14.803 ms** |
| **UpdateAsync** |       **Optimized** | **10000** |  **87.409 ms** | **9.8007 ms** | **19.7980 ms** |  **92.842 ms** |  **55.048 ms** | **120.735 ms** |
| **UpdateAsync** | **OptimizedDapper** |     **1** |   **1.665 ms** | **0.1458 ms** |  **0.2945 ms** |   **1.574 ms** |   **1.293 ms** |   **2.627 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **10** |   **1.800 ms** | **0.2012 ms** |  **0.4065 ms** |   **1.658 ms** |   **1.395 ms** |   **3.296 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **25** |   **1.819 ms** | **0.2182 ms** |  **0.4408 ms** |   **1.718 ms** |   **1.492 ms** |   **4.347 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **50** |   **1.883 ms** | **0.0701 ms** |  **0.1415 ms** |   **1.882 ms** |   **1.663 ms** |   **2.420 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **100** |   **2.290 ms** | **0.2570 ms** |  **0.5191 ms** |   **2.196 ms** |   **1.887 ms** |   **5.369 ms** |
| **UpdateAsync** | **OptimizedDapper** |  **1000** |   **7.173 ms** | **1.1960 ms** |  **2.4159 ms** |   **5.905 ms** |   **5.497 ms** |  **12.652 ms** |
| **UpdateAsync** | **OptimizedDapper** | **10000** |  **98.439 ms** | **9.8112 ms** | **19.8191 ms** | **103.869 ms** |  **59.066 ms** | **136.825 ms** |
| **UpdateAsync** |          **EfCore** |     **1** |   **1.798 ms** | **0.1428 ms** |  **0.2884 ms** |   **1.724 ms** |   **1.469 ms** |   **2.806 ms** |
| **UpdateAsync** |          **EfCore** |    **10** |   **1.942 ms** | **0.2040 ms** |  **0.4122 ms** |   **1.807 ms** |   **1.520 ms** |   **3.473 ms** |
| **UpdateAsync** |          **EfCore** |    **25** |   **4.872 ms** | **1.0088 ms** |  **2.0379 ms** |   **6.011 ms** |   **1.791 ms** |   **7.352 ms** |
| **UpdateAsync** |          **EfCore** |    **50** |   **5.375 ms** | **1.0657 ms** |  **2.1527 ms** |   **6.478 ms** |   **1.950 ms** |   **7.873 ms** |
| **UpdateAsync** |          **EfCore** |   **100** |   **5.962 ms** | **1.0445 ms** |  **2.1100 ms** |   **6.982 ms** |   **2.362 ms** |   **8.732 ms** |
| **UpdateAsync** |          **EfCore** |  **1000** |  **13.986 ms** | **0.5994 ms** |  **1.2109 ms** |  **13.696 ms** |  **11.541 ms** |  **17.285 ms** |
| **UpdateAsync** |          **EfCore** | **10000** | **126.669 ms** | **2.4954 ms** |  **5.0409 ms** | **126.200 ms** | **118.398 ms** | **142.011 ms** |
