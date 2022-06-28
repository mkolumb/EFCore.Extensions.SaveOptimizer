``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-EBZUNQ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |   Median |      Min |      Max |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **UpdateAsync** |       **Optimized** |    **1** | **1.600 ms** | **0.1454 ms** | **0.2937 ms** | **1.480 ms** | **1.283 ms** | **2.744 ms** |
| **UpdateAsync** |       **Optimized** |   **10** | **1.612 ms** | **0.1190 ms** | **0.2404 ms** | **1.562 ms** | **1.281 ms** | **2.640 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **2.051 ms** | **0.3131 ms** | **0.6324 ms** | **1.915 ms** | **1.309 ms** | **4.654 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **10** | **2.215 ms** | **0.4576 ms** | **0.9244 ms** | **1.840 ms** | **1.450 ms** | **5.013 ms** |
| **UpdateAsync** |          **EfCore** |    **1** | **1.732 ms** | **0.1067 ms** | **0.2156 ms** | **1.700 ms** | **1.394 ms** | **2.413 ms** |
| **UpdateAsync** |          **EfCore** |   **10** | **2.021 ms** | **0.1733 ms** | **0.3501 ms** | **1.912 ms** | **1.600 ms** | **3.212 ms** |
