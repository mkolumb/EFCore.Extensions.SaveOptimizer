``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-DFVVOF : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |       Mean |      Error |     StdDev |     Median |        Min |        Max |
|------------ |---------------- |------ |-----------:|-----------:|-----------:|-----------:|-----------:|-----------:|
| **DeleteAsync** |       **Optimized** |     **1** |   **6.922 ms** |  **0.3571 ms** |  **0.7213 ms** |   **6.807 ms** |   **5.912 ms** |  **10.189 ms** |
| **DeleteAsync** |       **Optimized** |    **10** |   **7.133 ms** |  **0.3271 ms** |  **0.6609 ms** |   **7.020 ms** |   **6.235 ms** |   **9.323 ms** |
| **DeleteAsync** |       **Optimized** |    **25** |   **7.129 ms** |  **0.2589 ms** |  **0.5229 ms** |   **7.062 ms** |   **6.302 ms** |   **8.345 ms** |
| **DeleteAsync** |       **Optimized** |    **50** |   **7.427 ms** |  **0.3303 ms** |  **0.6671 ms** |   **7.302 ms** |   **6.468 ms** |   **9.617 ms** |
| **DeleteAsync** |       **Optimized** |   **100** |   **7.729 ms** |  **0.2726 ms** |  **0.5508 ms** |   **7.692 ms** |   **6.623 ms** |   **9.068 ms** |
| **DeleteAsync** |       **Optimized** |  **1000** |  **11.983 ms** |  **0.5886 ms** |  **1.1889 ms** |  **11.821 ms** |  **10.409 ms** |  **17.010 ms** |
| **DeleteAsync** |       **Optimized** | **10000** |  **69.042 ms** |  **3.1934 ms** |  **6.4509 ms** |  **66.407 ms** |  **60.344 ms** |  **85.259 ms** |
| **DeleteAsync** | **OptimizedDapper** |     **1** |   **9.441 ms** |  **0.3942 ms** |  **0.7963 ms** |   **9.513 ms** |   **8.086 ms** |  **12.247 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **10** |   **9.555 ms** |  **0.4588 ms** |  **0.9268 ms** |   **9.461 ms** |   **7.672 ms** |  **13.620 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **25** |   **9.964 ms** |  **0.4724 ms** |  **0.9543 ms** |   **9.881 ms** |   **8.363 ms** |  **14.300 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **50** |  **10.110 ms** |  **0.4170 ms** |  **0.8424 ms** |  **10.121 ms** |   **8.474 ms** |  **11.878 ms** |
| **DeleteAsync** | **OptimizedDapper** |   **100** |  **10.489 ms** |  **0.4111 ms** |  **0.8305 ms** |  **10.457 ms** |   **9.191 ms** |  **12.783 ms** |
| **DeleteAsync** | **OptimizedDapper** |  **1000** |  **15.670 ms** |  **0.6974 ms** |  **1.4088 ms** |  **15.792 ms** |  **12.815 ms** |  **18.992 ms** |
| **DeleteAsync** | **OptimizedDapper** | **10000** |  **73.042 ms** |  **2.5297 ms** |  **5.1101 ms** |  **73.715 ms** |  **61.808 ms** |  **85.327 ms** |
| **DeleteAsync** |          **EfCore** |     **1** |   **6.670 ms** |  **0.3008 ms** |  **0.6076 ms** |   **6.596 ms** |   **5.734 ms** |   **8.161 ms** |
| **DeleteAsync** |          **EfCore** |    **10** |   **6.971 ms** |  **0.2791 ms** |  **0.5638 ms** |   **6.942 ms** |   **6.037 ms** |   **8.833 ms** |
| **DeleteAsync** |          **EfCore** |    **25** |   **7.626 ms** |  **0.3261 ms** |  **0.6588 ms** |   **7.489 ms** |   **6.674 ms** |   **9.346 ms** |
| **DeleteAsync** |          **EfCore** |    **50** |   **8.743 ms** |  **0.4788 ms** |  **0.9672 ms** |   **8.659 ms** |   **6.868 ms** |  **11.396 ms** |
| **DeleteAsync** |          **EfCore** |   **100** |  **11.076 ms** |  **0.6292 ms** |  **1.2711 ms** |  **11.035 ms** |   **7.851 ms** |  **14.650 ms** |
| **DeleteAsync** |          **EfCore** |  **1000** |  **47.194 ms** |  **2.5699 ms** |  **5.1914 ms** |  **47.118 ms** |  **38.784 ms** |  **62.492 ms** |
| **DeleteAsync** |          **EfCore** | **10000** | **430.162 ms** | **11.0138 ms** | **22.2484 ms** | **429.194 ms** | **388.703 ms** | **495.890 ms** |
