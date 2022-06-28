``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-EBZUNQ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |      Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **DeleteAsync** |       **Optimized** |    **1** | **1.786 ms** | **0.1682 ms** | **0.3397 ms** | **1.290 ms** | **2.823 ms** | **1.711 ms** |
| **DeleteAsync** |       **Optimized** |   **10** | **1.677 ms** | **0.1097 ms** | **0.2215 ms** | **1.366 ms** | **2.560 ms** | **1.654 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **1.686 ms** | **0.1351 ms** | **0.2730 ms** | **1.372 ms** | **2.749 ms** | **1.616 ms** |
| **DeleteAsync** | **OptimizedDapper** |   **10** | **2.343 ms** | **0.3246 ms** | **0.6558 ms** | **1.526 ms** | **4.565 ms** | **2.146 ms** |
| **DeleteAsync** |          **EfCore** |    **1** | **1.911 ms** | **0.1648 ms** | **0.3329 ms** | **1.510 ms** | **2.783 ms** | **1.791 ms** |
| **DeleteAsync** |          **EfCore** |   **10** | **1.964 ms** | **0.2370 ms** | **0.4788 ms** | **1.481 ms** | **3.506 ms** | **1.808 ms** |
