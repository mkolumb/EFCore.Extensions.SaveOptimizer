``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-TJPFQW : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |   Median |      Min |      Max |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **DeleteAsync** |       **Optimized** |    **1** | **1.686 ms** | **0.1535 ms** | **0.3100 ms** | **1.580 ms** | **1.282 ms** | **2.689 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **1.664 ms** | **0.1998 ms** | **0.4036 ms** | **1.552 ms** | **1.344 ms** | **3.915 ms** |
| **DeleteAsync** |          **EfCore** |    **1** | **1.761 ms** | **0.1713 ms** | **0.3461 ms** | **1.628 ms** | **1.277 ms** | **3.013 ms** |
