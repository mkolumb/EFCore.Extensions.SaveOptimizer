``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-TJPFQW : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |      Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **InsertAsync** |       **Optimized** |    **1** | **1.640 ms** | **0.0950 ms** | **0.1919 ms** | **1.305 ms** | **2.351 ms** | **1.602 ms** |
| **InsertAsync** | **OptimizedDapper** |    **1** | **1.845 ms** | **0.2074 ms** | **0.4189 ms** | **1.368 ms** | **3.431 ms** | **1.720 ms** |
| **InsertAsync** |          **EfCore** |    **1** | **1.765 ms** | **0.1723 ms** | **0.3480 ms** | **1.333 ms** | **2.909 ms** | **1.672 ms** |
