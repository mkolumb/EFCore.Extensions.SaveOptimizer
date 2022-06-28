``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-YGUQKJ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |      Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **InsertAsync** |       **Optimized** |    **1** | **6.048 ms** | **0.2831 ms** | **0.5719 ms** | **5.242 ms** | **7.944 ms** | **5.918 ms** |
| **InsertAsync** | **OptimizedDapper** |    **1** | **5.833 ms** | **0.2038 ms** | **0.4117 ms** | **5.007 ms** | **7.270 ms** | **5.738 ms** |
| **InsertAsync** |          **EfCore** |    **1** | **6.403 ms** | **0.3709 ms** | **0.7492 ms** | **4.836 ms** | **8.251 ms** | **6.365 ms** |
