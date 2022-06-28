``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-LRLBOJ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |      Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **InsertAsync** |       **Optimized** |    **1** | **4.132 ms** | **0.3306 ms** | **0.6678 ms** | **3.242 ms** | **6.166 ms** | **3.949 ms** |
| **InsertAsync** | **OptimizedDapper** |    **1** | **4.302 ms** | **0.5032 ms** | **1.0165 ms** | **3.138 ms** | **8.549 ms** | **4.039 ms** |
| **InsertAsync** |          **EfCore** |    **1** | **3.942 ms** | **0.2043 ms** | **0.4127 ms** | **3.371 ms** | **5.468 ms** | **3.867 ms** |
