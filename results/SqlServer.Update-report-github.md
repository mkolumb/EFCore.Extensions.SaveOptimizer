``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-DRNIHV : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |       Mean |     Error |    StdDev |        Min |        Max |     Median |
|------------ |---------------- |------ |-----------:|----------:|----------:|-----------:|-----------:|-----------:|
| **UpdateAsync** |       **Optimized** |     **1** |   **4.324 ms** | **0.1524 ms** | **0.3079 ms** |   **3.777 ms** |   **5.194 ms** |   **4.217 ms** |
| **UpdateAsync** |       **Optimized** |    **10** |   **4.572 ms** | **0.2906 ms** | **0.5871 ms** |   **3.550 ms** |   **6.699 ms** |   **4.432 ms** |
| **UpdateAsync** |       **Optimized** |    **25** |   **4.613 ms** | **0.2234 ms** | **0.4513 ms** |   **3.985 ms** |   **5.954 ms** |   **4.507 ms** |
| **UpdateAsync** |       **Optimized** |    **50** |   **4.902 ms** | **0.2826 ms** | **0.5709 ms** |   **4.178 ms** |   **6.629 ms** |   **4.812 ms** |
| **UpdateAsync** |       **Optimized** |   **100** |   **4.936 ms** | **0.2224 ms** | **0.4492 ms** |   **4.328 ms** |   **6.210 ms** |   **4.848 ms** |
| **UpdateAsync** |       **Optimized** |  **1000** |  **10.173 ms** | **0.3510 ms** | **0.7090 ms** |   **8.736 ms** |  **12.541 ms** |  **10.061 ms** |
| **UpdateAsync** |       **Optimized** | **10000** |  **63.971 ms** | **1.6833 ms** | **3.4003 ms** |  **58.593 ms** |  **73.940 ms** |  **63.857 ms** |
| **UpdateAsync** | **OptimizedDapper** |     **1** |   **4.226 ms** | **0.1731 ms** | **0.3496 ms** |   **3.691 ms** |   **5.165 ms** |   **4.171 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **10** |   **4.379 ms** | **0.2297 ms** | **0.4641 ms** |   **3.586 ms** |   **6.205 ms** |   **4.329 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **25** |   **4.518 ms** | **0.1923 ms** | **0.3885 ms** |   **3.968 ms** |   **5.722 ms** |   **4.422 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **50** |   **4.757 ms** | **0.2527 ms** | **0.5104 ms** |   **4.198 ms** |   **6.851 ms** |   **4.594 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **100** |   **4.968 ms** | **0.2585 ms** | **0.5222 ms** |   **4.169 ms** |   **6.493 ms** |   **4.905 ms** |
| **UpdateAsync** | **OptimizedDapper** |  **1000** |  **10.110 ms** | **0.4721 ms** | **0.9536 ms** |   **8.790 ms** |  **13.178 ms** |  **10.065 ms** |
| **UpdateAsync** | **OptimizedDapper** | **10000** |  **54.300 ms** | **1.0894 ms** | **2.2007 ms** |  **50.634 ms** |  **60.663 ms** |  **53.992 ms** |
| **UpdateAsync** |          **EfCore** |     **1** |   **3.538 ms** | **0.2154 ms** | **0.4350 ms** |   **3.077 ms** |   **5.539 ms** |   **3.399 ms** |
| **UpdateAsync** |          **EfCore** |    **10** |   **3.518 ms** | **0.1384 ms** | **0.2796 ms** |   **3.099 ms** |   **4.200 ms** |   **3.471 ms** |
| **UpdateAsync** |          **EfCore** |    **25** |   **3.631 ms** | **0.1888 ms** | **0.3814 ms** |   **3.142 ms** |   **5.190 ms** |   **3.544 ms** |
| **UpdateAsync** |          **EfCore** |    **50** |   **4.048 ms** | **0.1992 ms** | **0.4025 ms** |   **3.633 ms** |   **5.763 ms** |   **3.913 ms** |
| **UpdateAsync** |          **EfCore** |   **100** |   **4.741 ms** | **0.3192 ms** | **0.6447 ms** |   **3.981 ms** |   **6.755 ms** |   **4.553 ms** |
| **UpdateAsync** |          **EfCore** |  **1000** |  **14.666 ms** | **0.6687 ms** | **1.3508 ms** |  **12.551 ms** |  **20.172 ms** |  **14.497 ms** |
| **UpdateAsync** |          **EfCore** | **10000** | **111.073 ms** | **1.6923 ms** | **3.4184 ms** | **104.839 ms** | **122.614 ms** | **110.886 ms** |
