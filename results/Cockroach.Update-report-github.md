``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-CSECNH : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |      Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **UpdateAsync** |       **Optimized** |    **1** | **2.880 ms** | **0.1736 ms** | **0.3508 ms** | **2.357 ms** | **3.989 ms** | **2.810 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **2.954 ms** | **0.2280 ms** | **0.4606 ms** | **2.465 ms** | **4.469 ms** | **2.837 ms** |
| **UpdateAsync** |          **EfCore** |    **1** | **3.044 ms** | **0.2387 ms** | **0.4822 ms** | **2.410 ms** | **4.312 ms** | **2.868 ms** |
