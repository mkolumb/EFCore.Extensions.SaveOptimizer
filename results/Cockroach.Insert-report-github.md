``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-CSECNH : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |      Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **InsertAsync** |       **Optimized** |    **1** | **2.930 ms** | **0.1875 ms** | **0.3788 ms** | **2.421 ms** | **4.218 ms** | **2.858 ms** |
| **InsertAsync** | **OptimizedDapper** |    **1** | **2.705 ms** | **0.1475 ms** | **0.2979 ms** | **2.238 ms** | **3.433 ms** | **2.654 ms** |
| **InsertAsync** |          **EfCore** |    **1** | **2.961 ms** | **0.2145 ms** | **0.4333 ms** | **2.394 ms** | **4.725 ms** | **2.869 ms** |
