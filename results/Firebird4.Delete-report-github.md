``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-JLACHD : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |   Median |      Min |       Max |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|----------:|
| **Delete** |       **Optimized** |    **1** | **5.234 ms** | **0.2418 ms** | **0.4885 ms** | **5.182 ms** | **4.182 ms** |  **6.950 ms** |
| **Delete** |       **Optimized** |   **10** | **5.200 ms** | **0.2542 ms** | **0.5134 ms** | **5.037 ms** | **4.374 ms** |  **6.506 ms** |
| **Delete** | **Optimized Dapper** |    **1** | **5.474 ms** | **0.2556 ms** | **0.5162 ms** | **5.347 ms** | **4.803 ms** |  **6.998 ms** |
| **Delete** | **Optimized Dapper** |   **10** | **5.214 ms** | **0.2760 ms** | **0.5575 ms** | **5.067 ms** | **4.362 ms** |  **7.824 ms** |
| **Delete** |          **EF Core** |    **1** | **5.818 ms** | **0.2736 ms** | **0.5528 ms** | **5.740 ms** | **4.984 ms** |  **7.325 ms** |
| **Delete** |          **EF Core** |   **10** | **9.661 ms** | **0.4899 ms** | **0.9896 ms** | **9.275 ms** | **8.589 ms** | **12.878 ms** |
