``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-ZPTBWN : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |       Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|----------:|---------:|
| **InsertAsync** |       **Optimized** |    **1** | **7.520 ms** | **1.0336 ms** | **2.0878 ms** | **5.884 ms** | **20.208 ms** | **6.958 ms** |
| **InsertAsync** |       **Optimized** |   **10** | **9.047 ms** | **1.0055 ms** | **2.0311 ms** | **6.220 ms** | **14.477 ms** | **8.606 ms** |
| **InsertAsync** | **OptimizedDapper** |    **1** | **6.866 ms** | **0.4412 ms** | **0.8912 ms** | **5.400 ms** |  **9.250 ms** | **6.837 ms** |
| **InsertAsync** | **OptimizedDapper** |   **10** | **7.170 ms** | **0.4923 ms** | **0.9944 ms** | **6.009 ms** | **11.294 ms** | **7.013 ms** |
| **InsertAsync** |          **EfCore** |    **1** | **7.158 ms** | **0.4714 ms** | **0.9522 ms** | **5.647 ms** | **10.485 ms** | **6.998 ms** |
| **InsertAsync** |          **EfCore** |   **10** | **7.289 ms** | **0.2803 ms** | **0.5663 ms** | **6.045 ms** |  **8.457 ms** | **7.326 ms** |
