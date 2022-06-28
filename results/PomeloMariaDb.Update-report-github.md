``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-YGUQKJ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |       Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|----------:|---------:|
| **UpdateAsync** |       **Optimized** |    **1** | **6.182 ms** | **0.3301 ms** | **0.6669 ms** | **5.341 ms** |  **7.875 ms** | **5.984 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **6.519 ms** | **0.4616 ms** | **0.9325 ms** | **5.155 ms** |  **9.421 ms** | **6.267 ms** |
| **UpdateAsync** |          **EfCore** |    **1** | **7.632 ms** | **0.8875 ms** | **1.7927 ms** | **5.138 ms** | **15.084 ms** | **7.227 ms** |
