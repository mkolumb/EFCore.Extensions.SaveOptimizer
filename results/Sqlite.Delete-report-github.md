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
|      Method |         Variant |  Rows |   MeanNoOut |       Mean |        Min |         Q1 |     Median |           Q3 |          Max |
|------------ |---------------- |------ |------------:|-----------:|-----------:|-----------:|-----------:|-------------:|-------------:|
| **DeleteAsync** |       **Optimized** |     **1** |   **1.8142 ms** |   **1.977 ms** |   **1.597 ms** |   **1.723 ms** |   **1.817 ms** |     **1.905 ms** |     **7.210 ms** |
| **DeleteAsync** |       **Optimized** |    **10** |   **2.0863 ms** |   **2.100 ms** |   **1.813 ms** |   **1.994 ms** |   **2.086 ms** |     **2.207 ms** |     **2.497 ms** |
| **DeleteAsync** |       **Optimized** |    **25** |   **2.8001 ms** |   **2.810 ms** |   **2.355 ms** |   **2.660 ms** |   **2.793 ms** |     **2.954 ms** |     **3.382 ms** |
| **DeleteAsync** |       **Optimized** |    **50** |   **4.0403 ms** |   **4.036 ms** |   **3.393 ms** |   **3.834 ms** |   **4.046 ms** |     **4.230 ms** |     **4.666 ms** |
| **DeleteAsync** |       **Optimized** |   **100** |   **6.0780 ms** |   **6.013 ms** |   **4.493 ms** |   **5.466 ms** |   **6.213 ms** |     **6.453 ms** |     **8.588 ms** |
| **DeleteAsync** |       **Optimized** |  **1000** |  **35.8469 ms** |  **38.926 ms** |  **27.436 ms** |  **31.799 ms** |  **35.208 ms** |    **47.837 ms** |    **69.220 ms** |
| **DeleteAsync** |       **Optimized** | **10000** | **808.3019 ms** | **808.704 ms** | **586.937 ms** | **725.582 ms** | **806.496 ms** |   **900.237 ms** | **1,046.874 ms** |
| **DeleteAsync** | **OptimizedDapper** |     **1** |   **1.9187 ms** |   **1.944 ms** |   **1.622 ms** |   **1.834 ms** |   **1.915 ms** |     **2.018 ms** |     **2.525 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **10** |   **2.6244 ms** |   **2.749 ms** |   **2.158 ms** |   **2.461 ms** |   **2.614 ms** |     **2.813 ms** |     **4.896 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **25** |   **3.1883 ms** |   **3.243 ms** |   **2.728 ms** |   **3.021 ms** |   **3.194 ms** |     **3.344 ms** |     **4.383 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **50** |   **4.4519 ms** |   **4.468 ms** |   **3.196 ms** |   **4.224 ms** |   **4.482 ms** |     **4.719 ms** |     **5.682 ms** |
| **DeleteAsync** | **OptimizedDapper** |   **100** |   **7.0340 ms** |   **7.010 ms** |   **5.079 ms** |   **6.472 ms** |   **7.088 ms** |     **7.503 ms** |     **9.115 ms** |
| **DeleteAsync** | **OptimizedDapper** |  **1000** |  **42.7947 ms** |  **46.100 ms** |  **32.879 ms** |  **39.429 ms** |  **41.961 ms** |    **52.772 ms** |    **70.570 ms** |
| **DeleteAsync** | **OptimizedDapper** | **10000** | **723.8145 ms** | **732.290 ms** | **589.076 ms** | **669.098 ms** | **728.591 ms** |   **780.255 ms** |   **925.068 ms** |
| **DeleteAsync** |          **EfCore** |     **1** |   **1.7540 ms** |   **1.758 ms** |   **1.492 ms** |   **1.691 ms** |   **1.758 ms** |     **1.811 ms** |     **2.057 ms** |
| **DeleteAsync** |          **EfCore** |    **10** |   **2.5945 ms** |   **2.702 ms** |   **2.361 ms** |   **2.508 ms** |   **2.604 ms** |     **2.687 ms** |     **5.590 ms** |
| **DeleteAsync** |          **EfCore** |    **25** |   **5.0379 ms** |   **4.927 ms** |   **3.551 ms** |   **4.248 ms** |   **5.098 ms** |     **5.356 ms** |     **6.983 ms** |
| **DeleteAsync** |          **EfCore** |    **50** |   **8.0259 ms** |   **8.057 ms** |   **5.686 ms** |   **7.594 ms** |   **8.012 ms** |     **8.423 ms** |    **11.188 ms** |
| **DeleteAsync** |          **EfCore** |   **100** |  **12.8085 ms** |  **12.887 ms** |  **10.301 ms** |  **12.162 ms** |  **12.868 ms** |    **13.668 ms** |    **17.703 ms** |
| **DeleteAsync** |          **EfCore** |  **1000** |  **81.6407 ms** |  **83.816 ms** |  **56.102 ms** |  **71.754 ms** |  **79.786 ms** |    **93.833 ms** |   **126.423 ms** |
| **DeleteAsync** |          **EfCore** | **10000** | **967.7627 ms** | **958.670 ms** | **736.827 ms** | **896.509 ms** | **974.921 ms** | **1,026.014 ms** | **1,132.039 ms** |
