``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-OBPWED : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |   Median |      Min |      Max |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **InsertAsync** |       **Optimized** |    **1** | **2.878 ms** | **0.2203 ms** | **0.4451 ms** | **2.769 ms** | **2.249 ms** | **4.504 ms** |
| **InsertAsync** |       **Optimized** |   **10** | **2.991 ms** | **0.3237 ms** | **0.6539 ms** | **2.877 ms** | **2.398 ms** | **7.059 ms** |
| **InsertAsync** | **OptimizedDapper** |    **1** | **2.781 ms** | **0.2069 ms** | **0.4180 ms** | **2.660 ms** | **2.202 ms** | **4.400 ms** |
| **InsertAsync** | **OptimizedDapper** |   **10** | **2.839 ms** | **0.1518 ms** | **0.3067 ms** | **2.761 ms** | **2.448 ms** | **3.830 ms** |
| **InsertAsync** |          **EfCore** |    **1** | **2.847 ms** | **0.1578 ms** | **0.3188 ms** | **2.791 ms** | **2.386 ms** | **3.875 ms** |
| **InsertAsync** |          **EfCore** |   **10** | **5.421 ms** | **1.0641 ms** | **2.1495 ms** | **6.773 ms** | **2.560 ms** | **8.520 ms** |
