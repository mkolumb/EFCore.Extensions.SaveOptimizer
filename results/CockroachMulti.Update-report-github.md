``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-JQTPSJ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |      Mean |      Error |    StdDev |   Median |      Min |        Max |
|------------ |---------------- |----- |----------:|-----------:|----------:|---------:|---------:|-----------:|
| **UpdateAsync** |       **Optimized** |    **1** | **10.223 ms** |  **4.1761 ms** |  **8.436 ms** | **6.606 ms** | **3.973 ms** |  **41.738 ms** |
| **UpdateAsync** |       **Optimized** |   **10** |  **8.179 ms** |  **1.4365 ms** |  **2.902 ms** | **7.607 ms** | **4.243 ms** |  **17.913 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **16.241 ms** | **11.0877 ms** | **22.398 ms** | **9.790 ms** | **3.852 ms** | **137.714 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **10** |  **4.966 ms** |  **0.6639 ms** |  **1.341 ms** | **4.621 ms** | **3.350 ms** |  **10.014 ms** |
| **UpdateAsync** |          **EfCore** |    **1** |  **4.560 ms** |  **0.5503 ms** |  **1.112 ms** | **4.250 ms** | **3.180 ms** |   **9.261 ms** |
| **UpdateAsync** |          **EfCore** |   **10** |  **8.825 ms** |  **1.0108 ms** |  **2.042 ms** | **8.265 ms** | **6.271 ms** |  **17.152 ms** |
