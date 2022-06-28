``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-XOEIAY : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |      Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **UpdateAsync** |       **Optimized** |    **1** | **5.906 ms** | **0.4334 ms** | **0.8755 ms** | **4.739 ms** | **8.818 ms** | **5.612 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **5.563 ms** | **0.3378 ms** | **0.6823 ms** | **4.554 ms** | **7.461 ms** | **5.358 ms** |
| **UpdateAsync** |          **EfCore** |    **1** | **5.579 ms** | **0.3568 ms** | **0.7207 ms** | **4.521 ms** | **8.489 ms** | **5.452 ms** |
