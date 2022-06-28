``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-RNAYNH : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |       Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|----------:|---------:|
| **InsertAsync** |       **Optimized** |    **1** | **6.145 ms** | **0.3273 ms** | **0.6611 ms** | **5.035 ms** |  **8.161 ms** | **6.093 ms** |
| **InsertAsync** | **OptimizedDapper** |    **1** | **6.975 ms** | **0.7593 ms** | **1.5338 ms** | **5.120 ms** | **11.219 ms** | **6.528 ms** |
| **InsertAsync** |          **EfCore** |    **1** | **7.554 ms** | **0.6913 ms** | **1.3965 ms** | **5.486 ms** | **11.800 ms** | **7.268 ms** |
