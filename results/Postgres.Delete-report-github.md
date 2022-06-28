``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-BUPMKE : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |       Mean |     Error |     StdDev |     Median |       Min |        Max |
|------------ |---------------- |------ |-----------:|----------:|-----------:|-----------:|----------:|-----------:|
| **DeleteAsync** |       **Optimized** |     **1** |   **1.776 ms** | **0.1684 ms** |  **0.3403 ms** |   **1.681 ms** |  **1.396 ms** |   **3.405 ms** |
| **DeleteAsync** |       **Optimized** |    **10** |   **1.746 ms** | **0.1253 ms** |  **0.2531 ms** |   **1.682 ms** |  **1.392 ms** |   **2.764 ms** |
| **DeleteAsync** |       **Optimized** |    **25** |   **1.844 ms** | **0.1346 ms** |  **0.2719 ms** |   **1.790 ms** |  **1.462 ms** |   **2.874 ms** |
| **DeleteAsync** |       **Optimized** |    **50** |   **1.909 ms** | **0.1230 ms** |  **0.2485 ms** |   **1.891 ms** |  **1.479 ms** |   **2.665 ms** |
| **DeleteAsync** |       **Optimized** |   **100** |   **2.074 ms** | **0.1173 ms** |  **0.2370 ms** |   **2.024 ms** |  **1.725 ms** |   **2.829 ms** |
| **DeleteAsync** |       **Optimized** |  **1000** |   **5.115 ms** | **0.2295 ms** |  **0.4635 ms** |   **5.056 ms** |  **4.184 ms** |   **6.616 ms** |
| **DeleteAsync** |       **Optimized** | **10000** |  **65.239 ms** | **6.5017 ms** | **13.1337 ms** |  **60.759 ms** | **46.576 ms** | **104.899 ms** |
| **DeleteAsync** | **OptimizedDapper** |     **1** |   **1.667 ms** | **0.1180 ms** |  **0.2384 ms** |   **1.643 ms** |  **1.329 ms** |   **2.434 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **10** |   **1.711 ms** | **0.1335 ms** |  **0.2696 ms** |   **1.642 ms** |  **1.278 ms** |   **2.822 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **25** |   **1.819 ms** | **0.1734 ms** |  **0.3503 ms** |   **1.737 ms** |  **1.495 ms** |   **3.768 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **50** |   **1.858 ms** | **0.1166 ms** |  **0.2355 ms** |   **1.797 ms** |  **1.588 ms** |   **2.785 ms** |
| **DeleteAsync** | **OptimizedDapper** |   **100** |   **2.112 ms** | **0.2242 ms** |  **0.4528 ms** |   **1.999 ms** |  **1.760 ms** |   **4.313 ms** |
| **DeleteAsync** | **OptimizedDapper** |  **1000** |   **4.765 ms** | **0.4104 ms** |  **0.8289 ms** |   **4.650 ms** |  **3.735 ms** |   **8.607 ms** |
| **DeleteAsync** | **OptimizedDapper** | **10000** |  **63.456 ms** | **7.8006 ms** | **15.7576 ms** |  **58.103 ms** | **46.778 ms** | **102.937 ms** |
| **DeleteAsync** |          **EfCore** |     **1** |   **2.026 ms** | **0.1774 ms** |  **0.3584 ms** |   **1.940 ms** |  **1.503 ms** |   **3.154 ms** |
| **DeleteAsync** |          **EfCore** |    **10** |   **2.135 ms** | **0.1436 ms** |  **0.2902 ms** |   **2.071 ms** |  **1.686 ms** |   **3.215 ms** |
| **DeleteAsync** |          **EfCore** |    **25** |   **5.521 ms** | **1.0271 ms** |  **2.0749 ms** |   **6.467 ms** |  **1.820 ms** |   **7.584 ms** |
| **DeleteAsync** |          **EfCore** |    **50** |   **6.237 ms** | **0.7844 ms** |  **1.5845 ms** |   **6.780 ms** |  **2.154 ms** |   **7.849 ms** |
| **DeleteAsync** |          **EfCore** |   **100** |   **6.008 ms** | **1.1128 ms** |  **2.2480 ms** |   **7.162 ms** |  **2.620 ms** |   **9.764 ms** |
| **DeleteAsync** |          **EfCore** |  **1000** |  **13.055 ms** | **0.7189 ms** |  **1.4523 ms** |  **12.899 ms** | **10.231 ms** |  **16.096 ms** |
| **DeleteAsync** |          **EfCore** | **10000** | **111.360 ms** | **4.4945 ms** |  **9.0791 ms** | **109.393 ms** | **96.551 ms** | **131.457 ms** |
