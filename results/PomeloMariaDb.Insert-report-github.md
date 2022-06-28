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
| **InsertAsync** |       **Optimized** |    **1** | **6.430 ms** | **0.4244 ms** | **0.8573 ms** | **4.986 ms** |  **8.677 ms** | **6.213 ms** |
| **InsertAsync** |       **Optimized** |   **10** | **6.493 ms** | **0.5515 ms** | **1.1141 ms** | **5.225 ms** | **10.536 ms** | **6.138 ms** |
| **InsertAsync** | **OptimizedDapper** |    **1** | **6.449 ms** | **0.4675 ms** | **0.9443 ms** | **5.143 ms** | **11.062 ms** | **6.377 ms** |
| **InsertAsync** | **OptimizedDapper** |   **10** | **6.588 ms** | **0.3621 ms** | **0.7315 ms** | **5.317 ms** |  **9.985 ms** | **6.537 ms** |
| **InsertAsync** |          **EfCore** |    **1** | **7.208 ms** | **0.7521 ms** | **1.5194 ms** | **4.698 ms** | **12.878 ms** | **6.953 ms** |
| **InsertAsync** |          **EfCore** |   **10** | **7.056 ms** | **0.4681 ms** | **0.9456 ms** | **5.802 ms** | **10.882 ms** | **6.873 ms** |
