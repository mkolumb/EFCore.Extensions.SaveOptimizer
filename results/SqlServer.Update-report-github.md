``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-LRLBOJ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |      Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **UpdateAsync** |       **Optimized** |    **1** | **4.193 ms** | **0.3332 ms** | **0.6731 ms** | **3.432 ms** | **5.978 ms** | **3.976 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **3.947 ms** | **0.2977 ms** | **0.6014 ms** | **3.133 ms** | **6.756 ms** | **3.850 ms** |
| **UpdateAsync** |          **EfCore** |    **1** | **3.958 ms** | **0.2233 ms** | **0.4512 ms** | **3.114 ms** | **5.442 ms** | **3.858 ms** |
