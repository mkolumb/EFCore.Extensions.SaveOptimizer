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
| **InsertAsync** |       **Optimized** |    **1** | **6.092 ms** | **0.3812 ms** | **0.7700 ms** | **4.539 ms** |  **9.085 ms** | **5.962 ms** |
| **InsertAsync** |       **Optimized** |   **10** | **6.381 ms** | **0.4929 ms** | **0.9956 ms** | **4.844 ms** | **11.455 ms** | **6.246 ms** |
| **InsertAsync** | **OptimizedDapper** |    **1** | **6.579 ms** | **0.4431 ms** | **0.8951 ms** | **5.148 ms** |  **8.681 ms** | **6.373 ms** |
| **InsertAsync** | **OptimizedDapper** |   **10** | **5.917 ms** | **0.7034 ms** | **1.4209 ms** | **4.182 ms** | **12.660 ms** | **5.889 ms** |
| **InsertAsync** |          **EfCore** |    **1** | **4.366 ms** | **0.1872 ms** | **0.3781 ms** | **3.686 ms** |  **5.484 ms** | **4.278 ms** |
| **InsertAsync** |          **EfCore** |   **10** | **4.576 ms** | **0.8876 ms** | **1.7931 ms** | **3.595 ms** | **16.001 ms** | **4.149 ms** |
