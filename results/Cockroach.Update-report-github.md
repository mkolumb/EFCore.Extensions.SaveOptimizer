``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-OBPWED : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |      Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **UpdateAsync** |       **Optimized** |    **1** | **2.782 ms** | **0.1693 ms** | **0.3420 ms** | **2.377 ms** | **4.322 ms** | **2.725 ms** |
| **UpdateAsync** |       **Optimized** |   **10** | **2.948 ms** | **0.1691 ms** | **0.3416 ms** | **2.491 ms** | **4.212 ms** | **2.890 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **2.705 ms** | **0.1409 ms** | **0.2846 ms** | **2.387 ms** | **3.736 ms** | **2.653 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **10** | **2.920 ms** | **0.1416 ms** | **0.2861 ms** | **2.531 ms** | **3.609 ms** | **2.832 ms** |
| **UpdateAsync** |          **EfCore** |    **1** | **2.814 ms** | **0.1811 ms** | **0.3657 ms** | **2.231 ms** | **4.342 ms** | **2.686 ms** |
| **UpdateAsync** |          **EfCore** |   **10** | **3.589 ms** | **0.2788 ms** | **0.5632 ms** | **3.085 ms** | **5.986 ms** | **3.416 ms** |
