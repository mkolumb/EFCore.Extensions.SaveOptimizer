``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-EVMAOE : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |       Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|----------:|---------:|
| **InsertAsync** |       **Optimized** |    **1** | **3.641 ms** | **0.2145 ms** | **0.4334 ms** | **3.011 ms** |  **4.851 ms** | **3.544 ms** |
| **InsertAsync** | **OptimizedDapper** |    **1** | **4.746 ms** | **0.9383 ms** | **1.8954 ms** | **3.013 ms** | **12.284 ms** | **4.095 ms** |
| **InsertAsync** |          **EfCore** |    **1** | **3.841 ms** | **0.3702 ms** | **0.7478 ms** | **2.866 ms** |  **6.385 ms** | **3.706 ms** |
