``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-QNAEUH : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |       Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|----------:|---------:|
| **InsertAsync** |       **Optimized** |    **1** | **7.015 ms** | **0.4115 ms** | **0.8313 ms** | **5.823 ms** | **10.207 ms** | **6.832 ms** |
| **InsertAsync** | **OptimizedDapper** |    **1** | **7.153 ms** | **0.6002 ms** | **1.2125 ms** | **5.525 ms** | **10.294 ms** | **6.761 ms** |
| **InsertAsync** |          **EfCore** |    **1** | **6.940 ms** | **0.4286 ms** | **0.8659 ms** | **5.986 ms** |  **9.573 ms** | **6.696 ms** |
