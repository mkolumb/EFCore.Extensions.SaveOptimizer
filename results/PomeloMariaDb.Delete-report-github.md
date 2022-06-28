``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-YGUQKJ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |   Median |      Min |      Max |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **DeleteAsync** |       **Optimized** |    **1** | **6.217 ms** | **0.4484 ms** | **0.9058 ms** | **6.009 ms** | **5.239 ms** | **8.884 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **6.254 ms** | **0.5974 ms** | **1.2068 ms** | **5.775 ms** | **4.932 ms** | **9.786 ms** |
| **DeleteAsync** |          **EfCore** |    **1** | **6.358 ms** | **0.4553 ms** | **0.9198 ms** | **6.081 ms** | **5.209 ms** | **9.288 ms** |
