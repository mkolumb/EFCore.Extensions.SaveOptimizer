``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-EVMAOE : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |   Median |      Min |      Max |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **DeleteAsync** |       **Optimized** |    **1** | **4.198 ms** | **0.4763 ms** | **0.9621 ms** | **4.035 ms** | **3.149 ms** | **8.568 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **3.907 ms** | **0.3631 ms** | **0.7334 ms** | **3.740 ms** | **2.830 ms** | **6.186 ms** |
| **DeleteAsync** |          **EfCore** |    **1** | **4.425 ms** | **0.6190 ms** | **1.2504 ms** | **3.910 ms** | **3.059 ms** | **7.928 ms** |
