``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-YLGLZT : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |       Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|----------:|---------:|
| **DeleteAsync** |       **Optimized** |    **1** | **6.641 ms** | **0.9106 ms** | **1.8394 ms** | **4.571 ms** | **14.085 ms** | **6.022 ms** |
| **DeleteAsync** |       **Optimized** |   **10** | **5.773 ms** | **0.4135 ms** | **0.8353 ms** | **4.765 ms** |  **8.338 ms** | **5.493 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **5.640 ms** | **0.3936 ms** | **0.7950 ms** | **4.649 ms** |  **8.428 ms** | **5.395 ms** |
| **DeleteAsync** | **OptimizedDapper** |   **10** | **5.403 ms** | **0.3870 ms** | **0.7818 ms** | **4.619 ms** |  **9.312 ms** | **5.194 ms** |
| **DeleteAsync** |          **EfCore** |    **1** | **5.950 ms** | **0.2250 ms** | **0.4545 ms** | **4.873 ms** |  **6.994 ms** | **5.969 ms** |
| **DeleteAsync** |          **EfCore** |   **10** | **9.997 ms** | **0.7170 ms** | **1.4484 ms** | **8.677 ms** | **15.135 ms** | **9.532 ms** |
