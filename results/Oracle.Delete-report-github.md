``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-XOEIAY : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |   StdDev |      Min |      Max |   Median |
|------------ |---------------- |----- |---------:|----------:|---------:|---------:|---------:|---------:|
| **DeleteAsync** |       **Optimized** |    **1** | **6.802 ms** | **0.8381 ms** | **1.693 ms** | **4.872 ms** | **12.97 ms** | **6.191 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **6.958 ms** | **0.6365 ms** | **1.286 ms** | **5.436 ms** | **10.65 ms** | **6.631 ms** |
| **DeleteAsync** |          **EfCore** |    **1** | **7.401 ms** | **0.6692 ms** | **1.352 ms** | **5.691 ms** | **11.20 ms** | **6.994 ms** |
