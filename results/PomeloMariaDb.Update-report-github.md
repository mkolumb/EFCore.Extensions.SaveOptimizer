``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-NXTZTT : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |       Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|----------:|---------:|
| **UpdateAsync** |       **Optimized** |    **1** | **6.066 ms** | **0.3231 ms** | **0.6527 ms** | **5.130 ms** |  **8.345 ms** | **5.908 ms** |
| **UpdateAsync** |       **Optimized** |   **10** | **6.108 ms** | **0.3530 ms** | **0.7131 ms** | **4.981 ms** |  **8.967 ms** | **5.991 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **6.178 ms** | **0.3038 ms** | **0.6137 ms** | **5.297 ms** |  **7.677 ms** | **6.042 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **10** | **6.368 ms** | **0.3992 ms** | **0.8063 ms** | **5.250 ms** | **10.285 ms** | **6.173 ms** |
| **UpdateAsync** |          **EfCore** |    **1** | **6.764 ms** | **0.5818 ms** | **1.1752 ms** | **4.815 ms** | **10.771 ms** | **6.401 ms** |
| **UpdateAsync** |          **EfCore** |   **10** | **6.849 ms** | **0.3609 ms** | **0.7290 ms** | **5.506 ms** |  **9.275 ms** | **6.669 ms** |
