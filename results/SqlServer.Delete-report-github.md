``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-DRNIHV : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |       Mean |     Error |     StdDev |        Min |        Max |     Median |
|------------ |---------------- |------ |-----------:|----------:|-----------:|-----------:|-----------:|-----------:|
| **DeleteAsync** |       **Optimized** |     **1** |   **5.179 ms** | **0.4003 ms** |  **0.8085 ms** |   **4.403 ms** |   **8.623 ms** |   **4.991 ms** |
| **DeleteAsync** |       **Optimized** |    **10** |   **5.411 ms** | **0.4599 ms** |  **0.9291 ms** |   **4.024 ms** |   **8.429 ms** |   **5.211 ms** |
| **DeleteAsync** |       **Optimized** |    **25** |   **5.275 ms** | **0.2940 ms** |  **0.5939 ms** |   **4.201 ms** |   **7.083 ms** |   **5.152 ms** |
| **DeleteAsync** |       **Optimized** |    **50** |   **5.841 ms** | **0.4030 ms** |  **0.8141 ms** |   **4.751 ms** |   **9.689 ms** |   **5.700 ms** |
| **DeleteAsync** |       **Optimized** |   **100** |   **6.299 ms** | **0.4321 ms** |  **0.8729 ms** |   **5.397 ms** |   **9.961 ms** |   **6.030 ms** |
| **DeleteAsync** |       **Optimized** |  **1000** |  **13.368 ms** | **0.9757 ms** |  **1.9709 ms** |  **11.166 ms** |  **18.630 ms** |  **12.801 ms** |
| **DeleteAsync** |       **Optimized** | **10000** |  **85.484 ms** | **5.5648 ms** | **11.2411 ms** |  **68.835 ms** | **111.962 ms** |  **85.909 ms** |
| **DeleteAsync** | **OptimizedDapper** |     **1** |   **3.451 ms** | **0.1538 ms** |  **0.3107 ms** |   **2.979 ms** |   **4.395 ms** |   **3.384 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **10** |   **3.433 ms** | **0.1595 ms** |  **0.3222 ms** |   **3.031 ms** |   **4.651 ms** |   **3.367 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **25** |   **3.712 ms** | **0.1649 ms** |  **0.3331 ms** |   **3.203 ms** |   **4.641 ms** |   **3.663 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **50** |   **3.863 ms** | **0.1784 ms** |  **0.3604 ms** |   **3.408 ms** |   **5.193 ms** |   **3.759 ms** |
| **DeleteAsync** | **OptimizedDapper** |   **100** |   **4.486 ms** | **0.5111 ms** |  **1.0325 ms** |   **3.541 ms** |  **10.297 ms** |   **4.169 ms** |
| **DeleteAsync** | **OptimizedDapper** |  **1000** |  **10.771 ms** | **0.8730 ms** |  **1.7634 ms** |   **8.953 ms** |  **16.683 ms** |  **10.132 ms** |
| **DeleteAsync** | **OptimizedDapper** | **10000** |  **81.236 ms** | **4.3239 ms** |  **8.7345 ms** |  **68.357 ms** | **111.408 ms** |  **81.299 ms** |
| **DeleteAsync** |          **EfCore** |     **1** |   **3.484 ms** | **0.1922 ms** |  **0.3883 ms** |   **3.042 ms** |   **4.809 ms** |   **3.363 ms** |
| **DeleteAsync** |          **EfCore** |    **10** |   **3.483 ms** | **0.1634 ms** |  **0.3300 ms** |   **3.027 ms** |   **4.363 ms** |   **3.458 ms** |
| **DeleteAsync** |          **EfCore** |    **25** |   **3.698 ms** | **0.2267 ms** |  **0.4579 ms** |   **3.225 ms** |   **5.137 ms** |   **3.550 ms** |
| **DeleteAsync** |          **EfCore** |    **50** |   **3.914 ms** | **0.1695 ms** |  **0.3424 ms** |   **3.473 ms** |   **5.190 ms** |   **3.857 ms** |
| **DeleteAsync** |          **EfCore** |   **100** |   **4.642 ms** | **0.2635 ms** |  **0.5324 ms** |   **3.953 ms** |   **6.044 ms** |   **4.508 ms** |
| **DeleteAsync** |          **EfCore** |  **1000** |  **14.033 ms** | **0.5164 ms** |  **1.0432 ms** |  **12.129 ms** |  **16.565 ms** |  **13.898 ms** |
| **DeleteAsync** |          **EfCore** | **10000** | **121.517 ms** | **6.0032 ms** | **12.1268 ms** | **103.881 ms** | **162.807 ms** | **125.256 ms** |
