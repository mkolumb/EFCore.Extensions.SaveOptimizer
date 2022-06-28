``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-NXTZTT : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |   Median |      Min |       Max |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|----------:|
| **DeleteAsync** |       **Optimized** |    **1** | **5.958 ms** | **0.4289 ms** | **0.8664 ms** | **5.779 ms** | **4.826 ms** |  **9.150 ms** |
| **DeleteAsync** |       **Optimized** |   **10** | **5.587 ms** | **0.3202 ms** | **0.6469 ms** | **5.428 ms** | **4.621 ms** |  **8.603 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **5.814 ms** | **0.3518 ms** | **0.7106 ms** | **5.539 ms** | **4.893 ms** |  **8.340 ms** |
| **DeleteAsync** | **OptimizedDapper** |   **10** | **6.135 ms** | **0.3908 ms** | **0.7894 ms** | **5.988 ms** | **5.088 ms** |  **9.101 ms** |
| **DeleteAsync** |          **EfCore** |    **1** | **7.047 ms** | **0.5869 ms** | **1.1855 ms** | **6.937 ms** | **4.799 ms** | **10.581 ms** |
| **DeleteAsync** |          **EfCore** |   **10** | **7.745 ms** | **0.8149 ms** | **1.6462 ms** | **7.114 ms** | **5.566 ms** | **12.266 ms** |
