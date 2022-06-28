``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-JQTPSJ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |   Median |      Min |       Max |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|----------:|
| **DeleteAsync** |       **Optimized** |    **1** | **3.470 ms** | **0.3091 ms** | **0.6245 ms** | **3.229 ms** | **2.802 ms** |  **6.076 ms** |
| **DeleteAsync** |       **Optimized** |   **10** | **4.332 ms** | **0.6009 ms** | **1.2139 ms** | **4.056 ms** | **3.062 ms** | **10.679 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **3.671 ms** | **0.2720 ms** | **0.5495 ms** | **3.544 ms** | **2.908 ms** |  **5.930 ms** |
| **DeleteAsync** | **OptimizedDapper** |   **10** | **4.654 ms** | **0.5216 ms** | **1.0536 ms** | **4.527 ms** | **3.015 ms** |  **8.072 ms** |
| **DeleteAsync** |          **EfCore** |    **1** | **4.160 ms** | **0.7911 ms** | **1.5981 ms** | **3.675 ms** | **2.801 ms** | **13.392 ms** |
| **DeleteAsync** |          **EfCore** |   **10** | **7.304 ms** | **1.1956 ms** | **2.4151 ms** | **6.696 ms** | **5.370 ms** | **17.704 ms** |
