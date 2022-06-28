``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-EBZUNQ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |   Median |      Min |      Max |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **InsertAsync** |       **Optimized** |    **1** | **1.877 ms** | **0.2609 ms** | **0.5270 ms** | **1.774 ms** | **1.378 ms** | **4.855 ms** |
| **InsertAsync** |       **Optimized** |   **10** | **2.014 ms** | **0.2598 ms** | **0.5249 ms** | **1.867 ms** | **1.627 ms** | **4.495 ms** |
| **InsertAsync** | **OptimizedDapper** |    **1** | **2.192 ms** | **0.3881 ms** | **0.7840 ms** | **1.865 ms** | **1.481 ms** | **5.420 ms** |
| **InsertAsync** | **OptimizedDapper** |   **10** | **1.827 ms** | **0.1382 ms** | **0.2792 ms** | **1.750 ms** | **1.538 ms** | **2.997 ms** |
| **InsertAsync** |          **EfCore** |    **1** | **1.893 ms** | **0.1906 ms** | **0.3850 ms** | **1.763 ms** | **1.474 ms** | **3.230 ms** |
| **InsertAsync** |          **EfCore** |   **10** | **4.418 ms** | **1.1271 ms** | **2.2767 ms** | **5.930 ms** | **1.385 ms** | **7.145 ms** |
