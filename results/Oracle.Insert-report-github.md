``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-IBCNHC : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |       Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|----------:|---------:|
| **InsertAsync** |       **Optimized** |    **1** | **6.427 ms** | **0.9999 ms** | **2.0199 ms** | **4.566 ms** | **14.348 ms** | **5.821 ms** |
| **InsertAsync** |       **Optimized** |   **10** | **8.753 ms** | **1.6183 ms** | **2.8765 ms** | **5.812 ms** | **18.360 ms** | **7.811 ms** |
| **InsertAsync** | **OptimizedDapper** |    **1** | **6.109 ms** | **0.5086 ms** | **1.0274 ms** | **4.805 ms** |  **9.577 ms** | **5.922 ms** |
| **InsertAsync** | **OptimizedDapper** |   **10** | **8.044 ms** | **0.9570 ms** | **1.4325 ms** | **6.299 ms** | **13.050 ms** | **7.609 ms** |
| **InsertAsync** |          **EfCore** |    **1** | **6.145 ms** | **0.2211 ms** | **0.4466 ms** | **5.101 ms** |  **6.996 ms** | **6.125 ms** |
| **InsertAsync** |          **EfCore** |   **10** | **6.993 ms** | **2.2686 ms** | **1.5006 ms** | **5.672 ms** | **10.937 ms** | **6.515 ms** |
