``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-EBGRBP : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |        Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|-----------:|---------:|
| **DeleteAsync** |       **Optimized** |    **1** | **671.4 μs** | **115.93 μs** | **234.19 μs** | **420.0 μs** | **1,517.3 μs** | **613.3 μs** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **494.3 μs** |  **37.46 μs** |  **75.67 μs** | **363.7 μs** |   **760.5 μs** | **472.5 μs** |
| **DeleteAsync** |          **EfCore** |    **1** | **412.6 μs** |  **83.14 μs** | **167.95 μs** | **329.1 μs** | **1,565.1 μs** | **387.4 μs** |
