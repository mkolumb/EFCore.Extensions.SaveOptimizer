``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-JQTPSJ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |      Mean |     Error |    StdDev |   Median |      Min |       Max |
|------------ |---------------- |----- |----------:|----------:|----------:|---------:|---------:|----------:|
| **Insert** |       **Optimized** |    **1** |  **3.561 ms** | **0.3708 ms** | **0.7490 ms** | **3.310 ms** | **2.661 ms** |  **5.698 ms** |
| **Insert** |       **Optimized** |   **10** |  **4.072 ms** | **0.3999 ms** | **0.8078 ms** | **3.852 ms** | **2.909 ms** |  **6.529 ms** |
| **Insert** | **Optimized Dapper** |    **1** |  **3.758 ms** | **0.8196 ms** | **1.6557 ms** | **3.393 ms** | **2.648 ms** | **14.255 ms** |
| **Insert** | **Optimized Dapper** |   **10** |  **3.845 ms** | **0.4224 ms** | **0.8533 ms** | **3.580 ms** | **2.789 ms** |  **7.118 ms** |
| **Insert** |          **EF Core** |    **1** |  **3.810 ms** | **0.5884 ms** | **1.1886 ms** | **3.314 ms** | **2.804 ms** |  **7.404 ms** |
| **Insert** |          **EF Core** |   **10** | **11.148 ms** | **3.0492 ms** | **6.1596 ms** | **9.527 ms** | **4.338 ms** | **34.378 ms** |
