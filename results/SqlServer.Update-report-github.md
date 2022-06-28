``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-MMFLEI : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |   Median |      Min |      Max |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **UpdateAsync** |       **Optimized** |    **1** | **3.990 ms** | **0.2307 ms** | **0.4659 ms** | **3.839 ms** | **3.231 ms** | **5.402 ms** |
| **UpdateAsync** |       **Optimized** |   **10** | **3.961 ms** | **0.2087 ms** | **0.4217 ms** | **3.853 ms** | **3.417 ms** | **5.494 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **3.929 ms** | **0.2204 ms** | **0.4451 ms** | **3.773 ms** | **3.279 ms** | **5.244 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **10** | **4.102 ms** | **0.2627 ms** | **0.5307 ms** | **3.948 ms** | **3.352 ms** | **5.430 ms** |
| **UpdateAsync** |          **EfCore** |    **1** | **4.134 ms** | **0.3600 ms** | **0.7273 ms** | **3.842 ms** | **3.123 ms** | **5.993 ms** |
| **UpdateAsync** |          **EfCore** |   **10** | **4.141 ms** | **0.3221 ms** | **0.6507 ms** | **4.010 ms** | **3.441 ms** | **7.562 ms** |
