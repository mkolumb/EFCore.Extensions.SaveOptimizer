``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-MMFLEI : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |       Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|----------:|---------:|
| **DeleteAsync** |       **Optimized** |    **1** | **6.497 ms** | **0.4067 ms** | **0.8215 ms** | **5.254 ms** |  **9.492 ms** | **6.352 ms** |
| **DeleteAsync** |       **Optimized** |   **10** | **6.670 ms** | **0.3945 ms** | **0.7969 ms** | **5.014 ms** |  **8.650 ms** | **6.593 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **5.938 ms** | **0.6089 ms** | **1.2299 ms** | **4.386 ms** | **12.840 ms** | **5.840 ms** |
| **DeleteAsync** | **OptimizedDapper** |   **10** | **6.245 ms** | **0.3818 ms** | **0.7713 ms** | **4.684 ms** |  **9.310 ms** | **6.077 ms** |
| **DeleteAsync** |          **EfCore** |    **1** | **6.294 ms** | **0.3851 ms** | **0.7779 ms** | **5.189 ms** |  **8.342 ms** | **6.044 ms** |
| **DeleteAsync** |          **EfCore** |   **10** | **6.274 ms** | **0.3636 ms** | **0.7345 ms** | **5.194 ms** |  **8.784 ms** | **6.226 ms** |
