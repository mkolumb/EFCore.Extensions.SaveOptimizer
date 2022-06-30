``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]   : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Postgres : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Job=Postgres  EvaluateOverhead=True  OutlierMode=RemoveUpper  
InvocationCount=1  IterationCount=15  LaunchCount=8  
RunStrategy=Throughput  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |   MeanNoOut |       Mean |        Min |         Q1 |     Median |         Q3 |          Max |
|------------ |---------------- |------ |------------:|-----------:|-----------:|-----------:|-----------:|-----------:|-------------:|
| **DeleteAsync** |       **Optimized** |     **1** |   **4.2674 ms** |   **4.278 ms** |   **3.502 ms** |   **4.055 ms** |   **4.271 ms** |   **4.482 ms** |     **5.088 ms** |
| **DeleteAsync** |       **Optimized** |    **10** |   **4.2882 ms** |   **4.394 ms** |   **3.644 ms** |   **4.124 ms** |   **4.289 ms** |   **4.524 ms** |     **7.877 ms** |
| **DeleteAsync** |       **Optimized** |    **25** |   **4.8474 ms** |   **4.903 ms** |   **4.362 ms** |   **4.717 ms** |   **4.837 ms** |   **5.056 ms** |     **5.717 ms** |
| **DeleteAsync** |       **Optimized** |    **50** |   **5.7344 ms** |   **5.755 ms** |   **5.165 ms** |   **5.525 ms** |   **5.748 ms** |   **5.895 ms** |     **6.973 ms** |
| **DeleteAsync** |       **Optimized** |   **100** |   **7.6851 ms** |   **7.687 ms** |   **6.423 ms** |   **7.447 ms** |   **7.647 ms** |   **7.963 ms** |     **8.688 ms** |
| **DeleteAsync** |       **Optimized** |  **1000** |  **30.5460 ms** |  **30.564 ms** |  **27.219 ms** |  **29.728 ms** |  **30.529 ms** |  **31.369 ms** |    **37.996 ms** |
| **DeleteAsync** |       **Optimized** | **10000** | **534.2895 ms** | **566.328 ms** | **427.453 ms** | **488.909 ms** | **530.383 ms** | **615.012 ms** |   **985.562 ms** |
| **DeleteAsync** | **OptimizedDapper** |     **1** |   **3.8456 ms** |   **3.852 ms** |   **3.261 ms** |   **3.687 ms** |   **3.835 ms** |   **4.029 ms** |     **4.716 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **10** |   **4.2513 ms** |   **4.317 ms** |   **3.799 ms** |   **4.121 ms** |   **4.221 ms** |   **4.525 ms** |     **5.292 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **25** |   **4.7932 ms** |   **4.839 ms** |   **4.358 ms** |   **4.664 ms** |   **4.788 ms** |   **4.990 ms** |     **5.872 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **50** |   **6.5768 ms** |   **6.755 ms** |   **5.271 ms** |   **5.905 ms** |   **6.602 ms** |   **7.193 ms** |    **13.315 ms** |
| **DeleteAsync** | **OptimizedDapper** |   **100** |  **10.0055 ms** |  **10.082 ms** |   **7.351 ms** |   **9.240 ms** |   **9.902 ms** |  **10.933 ms** |    **14.454 ms** |
| **DeleteAsync** | **OptimizedDapper** |  **1000** |  **40.0674 ms** |  **40.458 ms** |  **33.305 ms** |  **38.012 ms** |  **39.906 ms** |  **42.014 ms** |    **55.703 ms** |
| **DeleteAsync** | **OptimizedDapper** | **10000** | **680.2921 ms** | **699.102 ms** | **482.482 ms** | **573.707 ms** | **666.724 ms** | **791.462 ms** | **1,012.734 ms** |
| **DeleteAsync** |          **EfCore** |     **1** |   **5.4082 ms** |   **5.388 ms** |   **4.364 ms** |   **5.045 ms** |   **5.414 ms** |   **5.700 ms** |     **6.616 ms** |
| **DeleteAsync** |          **EfCore** |    **10** |   **6.2882 ms** |   **6.309 ms** |   **5.539 ms** |   **6.074 ms** |   **6.294 ms** |   **6.510 ms** |     **7.340 ms** |
| **DeleteAsync** |          **EfCore** |    **25** |  **22.3584 ms** |  **26.949 ms** |   **6.240 ms** |   **6.981 ms** |   **7.831 ms** |  **51.554 ms** |    **64.567 ms** |
| **DeleteAsync** |          **EfCore** |    **50** |  **30.1818 ms** |  **31.904 ms** |   **8.282 ms** |   **9.308 ms** |  **15.430 ms** |  **54.596 ms** |    **66.536 ms** |
| **DeleteAsync** |          **EfCore** |   **100** |  **39.1260 ms** |  **38.318 ms** |  **11.728 ms** |  **12.981 ms** |  **54.935 ms** |  **59.711 ms** |    **72.902 ms** |
| **DeleteAsync** |          **EfCore** |  **1000** |  **89.0439 ms** |  **90.230 ms** |  **77.431 ms** |  **85.336 ms** |  **88.596 ms** |  **95.535 ms** |   **110.509 ms** |
| **DeleteAsync** |          **EfCore** | **10000** | **863.6658 ms** | **865.396 ms** | **821.277 ms** | **846.254 ms** | **861.939 ms** | **882.774 ms** |   **924.160 ms** |
