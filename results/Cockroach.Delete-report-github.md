``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-LFETAU : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |         Mean |       Error |      StdDev |     Median |        Min |          Max |
|------------ |---------------- |------ |-------------:|------------:|------------:|-----------:|-----------:|-------------:|
| **DeleteAsync** |       **Optimized** |     **1** |     **3.032 ms** |   **0.3472 ms** |   **0.7013 ms** |   **2.882 ms** |   **2.214 ms** |     **6.015 ms** |
| **DeleteAsync** |       **Optimized** |    **10** |     **2.846 ms** |   **0.1536 ms** |   **0.3102 ms** |   **2.735 ms** |   **2.474 ms** |     **3.664 ms** |
| **DeleteAsync** |       **Optimized** |    **25** |     **2.993 ms** |   **0.2399 ms** |   **0.4846 ms** |   **2.859 ms** |   **2.355 ms** |     **5.313 ms** |
| **DeleteAsync** |       **Optimized** |    **50** |     **3.166 ms** |   **0.1755 ms** |   **0.3546 ms** |   **3.119 ms** |   **2.669 ms** |     **4.523 ms** |
| **DeleteAsync** |       **Optimized** |   **100** |     **3.813 ms** |   **0.3193 ms** |   **0.6449 ms** |   **3.588 ms** |   **3.246 ms** |     **6.470 ms** |
| **DeleteAsync** |       **Optimized** |  **1000** |    **17.898 ms** |   **1.3527 ms** |   **2.7325 ms** |  **17.862 ms** |  **11.849 ms** |    **27.266 ms** |
| **DeleteAsync** |       **Optimized** | **10000** |   **397.115 ms** | **237.4825 ms** | **479.7264 ms** | **155.178 ms** | **107.546 ms** | **1,914.439 ms** |
| **DeleteAsync** | **OptimizedDapper** |     **1** |     **3.289 ms** |   **0.5109 ms** |   **1.0320 ms** |   **2.917 ms** |   **2.627 ms** |     **7.435 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **10** |     **2.674 ms** |   **0.1966 ms** |   **0.3971 ms** |   **2.610 ms** |   **2.069 ms** |     **3.808 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **25** |     **2.542 ms** |   **0.1916 ms** |   **0.3870 ms** |   **2.445 ms** |   **2.181 ms** |     **4.345 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **50** |     **2.870 ms** |   **0.2900 ms** |   **0.5859 ms** |   **2.625 ms** |   **2.339 ms** |     **5.071 ms** |
| **DeleteAsync** | **OptimizedDapper** |   **100** |     **3.696 ms** |   **0.7442 ms** |   **1.5032 ms** |   **3.230 ms** |   **2.849 ms** |    **12.580 ms** |
| **DeleteAsync** | **OptimizedDapper** |  **1000** |    **17.657 ms** |   **1.3489 ms** |   **2.7248 ms** |  **18.046 ms** |  **11.173 ms** |    **26.040 ms** |
| **DeleteAsync** | **OptimizedDapper** | **10000** |   **131.082 ms** |   **7.9585 ms** |  **16.0766 ms** | **126.722 ms** | **107.106 ms** |   **170.848 ms** |
| **DeleteAsync** |          **EfCore** |     **1** |     **2.501 ms** |   **0.3636 ms** |   **0.7344 ms** |   **2.209 ms** |   **1.980 ms** |     **5.402 ms** |
| **DeleteAsync** |          **EfCore** |    **10** |     **2.997 ms** |   **0.1497 ms** |   **0.3024 ms** |   **2.912 ms** |   **2.637 ms** |     **4.297 ms** |
| **DeleteAsync** |          **EfCore** |    **25** |     **6.179 ms** |   **0.7580 ms** |   **1.5312 ms** |   **6.768 ms** |   **3.568 ms** |     **8.350 ms** |
| **DeleteAsync** |          **EfCore** |    **50** |     **8.162 ms** |   **0.7580 ms** |   **1.5313 ms** |   **8.501 ms** |   **4.953 ms** |    **11.821 ms** |
| **DeleteAsync** |          **EfCore** |   **100** |    **11.342 ms** |   **1.0181 ms** |   **2.0565 ms** |  **11.438 ms** |   **8.165 ms** |    **17.966 ms** |
| **DeleteAsync** |          **EfCore** |  **1000** |    **98.166 ms** |   **7.8546 ms** |  **15.8666 ms** |  **98.078 ms** |  **73.181 ms** |   **165.292 ms** |
| **DeleteAsync** |          **EfCore** | **10000** | **1,008.965 ms** |  **72.8571 ms** | **147.1749 ms** | **996.800 ms** | **808.878 ms** | **1,637.306 ms** |
