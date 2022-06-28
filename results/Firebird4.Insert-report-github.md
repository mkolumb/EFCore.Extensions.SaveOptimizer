``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-EXRPTT : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |       Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|----------:|---------:|
| **InsertAsync** |       **Optimized** |    **1** | **6.257 ms** | **0.3203 ms** | **0.6470 ms** | **5.409 ms** |  **8.744 ms** | **6.142 ms** |
| **InsertAsync** | **OptimizedDapper** |    **1** | **6.867 ms** | **0.8028 ms** | **1.6218 ms** | **5.196 ms** | **12.349 ms** | **6.338 ms** |
| **InsertAsync** |          **EfCore** |    **1** | **7.277 ms** | **0.5784 ms** | **1.1684 ms** | **5.395 ms** | **11.161 ms** | **7.093 ms** |
