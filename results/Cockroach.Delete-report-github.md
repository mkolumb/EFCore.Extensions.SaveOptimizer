``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-CSECNH : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |   Median |      Min |      Max |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **DeleteAsync** |       **Optimized** |    **1** | **2.734 ms** | **0.1796 ms** | **0.3627 ms** | **2.639 ms** | **2.262 ms** | **4.329 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **2.834 ms** | **0.2466 ms** | **0.4981 ms** | **2.697 ms** | **2.310 ms** | **4.818 ms** |
| **DeleteAsync** |          **EfCore** |    **1** | **2.858 ms** | **0.3131 ms** | **0.6325 ms** | **2.582 ms** | **2.387 ms** | **5.211 ms** |
