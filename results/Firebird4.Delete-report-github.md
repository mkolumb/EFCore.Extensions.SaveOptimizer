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
| **DeleteAsync** |       **Optimized** |    **1** | **5.496 ms** | **0.4520 ms** | **0.9131 ms** | **4.444 ms** |  **9.614 ms** | **5.245 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **5.772 ms** | **0.7253 ms** | **1.4651 ms** | **4.462 ms** | **13.547 ms** | **5.345 ms** |
| **DeleteAsync** |          **EfCore** |    **1** | **5.999 ms** | **0.6128 ms** | **1.2379 ms** | **4.792 ms** | **12.752 ms** | **5.769 ms** |
