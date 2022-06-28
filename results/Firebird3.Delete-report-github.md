``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-RNAYNH : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |      Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **DeleteAsync** |       **Optimized** |    **1** | **5.382 ms** | **0.3312 ms** | **0.6691 ms** | **4.568 ms** | **7.502 ms** | **5.190 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **5.248 ms** | **0.2981 ms** | **0.6022 ms** | **4.427 ms** | **7.206 ms** | **5.135 ms** |
| **DeleteAsync** |          **EfCore** |    **1** | **5.905 ms** | **0.2839 ms** | **0.5735 ms** | **5.124 ms** | **8.070 ms** | **5.813 ms** |
