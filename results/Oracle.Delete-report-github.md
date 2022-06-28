``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-IBCNHC : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |   StdDev |      Min |      Max |   Median |
|------------ |---------------- |----- |---------:|----------:|---------:|---------:|---------:|---------:|
| **DeleteAsync** |       **Optimized** |    **1** | **7.346 ms** | **1.5176 ms** | **3.066 ms** | **4.279 ms** | **20.19 ms** | **6.495 ms** |
| **DeleteAsync** |       **Optimized** |   **10** | **6.322 ms** | **0.8860 ms** | **1.790 ms** | **4.429 ms** | **12.41 ms** | **5.783 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **6.580 ms** | **1.1200 ms** | **2.262 ms** | **4.560 ms** | **17.99 ms** | **6.039 ms** |
| **DeleteAsync** | **OptimizedDapper** |   **10** | **6.691 ms** | **1.1506 ms** | **2.324 ms** | **4.667 ms** | **14.52 ms** | **5.816 ms** |
| **DeleteAsync** |          **EfCore** |    **1** | **6.414 ms** | **1.1606 ms** | **2.344 ms** | **4.365 ms** | **17.88 ms** | **5.789 ms** |
| **DeleteAsync** |          **EfCore** |   **10** | **7.602 ms** | **0.9822 ms** | **1.984 ms** | **5.150 ms** | **13.85 ms** | **7.040 ms** |
