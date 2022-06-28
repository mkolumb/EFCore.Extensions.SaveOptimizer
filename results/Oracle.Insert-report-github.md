``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-XOEIAY : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |      Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **InsertAsync** |       **Optimized** |    **1** | **6.092 ms** | **0.4819 ms** | **0.9734 ms** | **4.887 ms** | **9.507 ms** | **5.783 ms** |
| **InsertAsync** | **OptimizedDapper** |    **1** | **5.675 ms** | **0.4771 ms** | **0.9638 ms** | **4.037 ms** | **8.454 ms** | **5.366 ms** |
| **InsertAsync** |          **EfCore** |    **1** | **6.081 ms** | **0.5667 ms** | **1.1448 ms** | **4.652 ms** | **9.220 ms** | **5.784 ms** |
