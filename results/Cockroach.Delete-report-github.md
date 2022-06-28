``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-OBPWED : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |   Median |      Min |       Max |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|----------:|
| **DeleteAsync** |       **Optimized** |    **1** | **2.803 ms** | **0.1489 ms** | **0.3008 ms** | **2.712 ms** | **2.433 ms** |  **3.636 ms** |
| **DeleteAsync** |       **Optimized** |   **10** | **2.882 ms** | **0.1522 ms** | **0.3074 ms** | **2.806 ms** | **2.433 ms** |  **3.908 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **3.226 ms** | **0.5111 ms** | **1.0325 ms** | **2.812 ms** | **2.307 ms** |  **6.049 ms** |
| **DeleteAsync** | **OptimizedDapper** |   **10** | **3.718 ms** | **0.7986 ms** | **1.6132 ms** | **3.263 ms** | **2.672 ms** | **11.167 ms** |
| **DeleteAsync** |          **EfCore** |    **1** | **3.107 ms** | **0.2666 ms** | **0.5385 ms** | **2.963 ms** | **2.398 ms** |  **5.323 ms** |
| **DeleteAsync** |          **EfCore** |   **10** | **3.738 ms** | **0.2915 ms** | **0.5889 ms** | **3.575 ms** | **3.051 ms** |  **6.149 ms** |
