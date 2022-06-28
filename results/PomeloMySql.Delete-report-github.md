``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-QNAEUH : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |   StdDev |   Median |      Min |      Max |
|------------ |---------------- |----- |---------:|----------:|---------:|---------:|---------:|---------:|
| **DeleteAsync** |       **Optimized** |    **1** | **7.087 ms** | **0.6224 ms** | **1.257 ms** | **6.720 ms** | **5.870 ms** | **13.72 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **7.361 ms** | **0.6493 ms** | **1.312 ms** | **6.842 ms** | **5.767 ms** | **10.98 ms** |
| **DeleteAsync** |          **EfCore** |    **1** | **7.679 ms** | **0.6354 ms** | **1.283 ms** | **7.413 ms** | **5.934 ms** | **12.27 ms** |
