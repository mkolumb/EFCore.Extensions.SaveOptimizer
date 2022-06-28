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
| **UpdateAsync** |       **Optimized** |    **1** | **5.674 ms** | **0.3630 ms** | **0.7333 ms** | **4.770 ms** |  **9.040 ms** | **5.467 ms** |
| **UpdateAsync** |       **Optimized** |   **10** | **5.980 ms** | **0.4260 ms** | **0.8605 ms** | **4.714 ms** |  **8.228 ms** | **5.830 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **6.331 ms** | **0.4267 ms** | **0.8619 ms** | **4.929 ms** |  **9.058 ms** | **6.153 ms** |
| **UpdateAsync** | **OptimizedDapper** |   **10** | **6.748 ms** | **0.4729 ms** | **0.9554 ms** | **5.228 ms** |  **9.251 ms** | **6.746 ms** |
| **UpdateAsync** |          **EfCore** |    **1** | **7.757 ms** | **0.7338 ms** | **1.4823 ms** | **5.393 ms** | **11.737 ms** | **7.763 ms** |
| **UpdateAsync** |          **EfCore** |   **10** | **9.832 ms** | **0.3732 ms** | **0.7539 ms** | **8.690 ms** | **12.559 ms** | **9.729 ms** |
