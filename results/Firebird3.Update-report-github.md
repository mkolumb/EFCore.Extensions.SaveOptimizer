``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-RNAYNH : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |   Median |      Min |      Max |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **UpdateAsync** |       **Optimized** |    **1** | **5.315 ms** | **0.3629 ms** | **0.7331 ms** | **4.999 ms** | **4.595 ms** | **8.224 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **5.531 ms** | **0.3147 ms** | **0.6358 ms** | **5.307 ms** | **4.566 ms** | **7.559 ms** |
| **UpdateAsync** |          **EfCore** |    **1** | **5.744 ms** | **0.3345 ms** | **0.6757 ms** | **5.493 ms** | **4.858 ms** | **7.806 ms** |
