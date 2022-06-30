``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-HMWWNW : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |       Mean |     Error |    StdDev |        Min |        Max |     Median |
|------------ |---------------- |----- |-----------:|----------:|----------:|-----------:|-----------:|-----------:|
| **Delete** |       **Optimized** |    **1** |   **5.144 ms** | **0.2541 ms** | **0.5132 ms** |   **4.489 ms** |   **7.290 ms** |   **5.060 ms** |
| **Delete** |       **Optimized** |   **10** |   **5.348 ms** | **0.2445 ms** | **0.4940 ms** |   **4.215 ms** |   **6.493 ms** |   **5.313 ms** |
| **Delete** |       **Optimized** |   **25** |   **5.390 ms** | **0.1679 ms** | **0.3392 ms** |   **4.616 ms** |   **6.277 ms** |   **5.441 ms** |
| **Delete** |       **Optimized** |   **50** |   **5.810 ms** | **0.3678 ms** | **0.7430 ms** |   **4.877 ms** |   **8.913 ms** |   **5.684 ms** |
| **Delete** |       **Optimized** |  **100** |   **5.941 ms** | **0.2657 ms** | **0.5367 ms** |   **5.400 ms** |   **8.173 ms** |   **5.824 ms** |
| **Delete** |       **Optimized** | **1000** |  **12.604 ms** | **1.5272 ms** | **3.0850 ms** |   **9.172 ms** |  **30.626 ms** |  **12.522 ms** |
| **Delete** | **Optimized Dapper** |    **1** |   **7.257 ms** | **0.3051 ms** | **0.6163 ms** |   **6.178 ms** |   **8.692 ms** |   **7.222 ms** |
| **Delete** | **Optimized Dapper** |   **10** |   **7.323 ms** | **0.3039 ms** | **0.6139 ms** |   **5.782 ms** |   **9.061 ms** |   **7.259 ms** |
| **Delete** | **Optimized Dapper** |   **25** |   **7.603 ms** | **0.4070 ms** | **0.8223 ms** |   **6.005 ms** |   **9.838 ms** |   **7.462 ms** |
| **Delete** | **Optimized Dapper** |   **50** |   **7.730 ms** | **0.3137 ms** | **0.6338 ms** |   **6.646 ms** |  **10.316 ms** |   **7.710 ms** |
| **Delete** | **Optimized Dapper** |  **100** |   **8.342 ms** | **0.3212 ms** | **0.6487 ms** |   **7.440 ms** |  **10.250 ms** |   **8.279 ms** |
| **Delete** | **Optimized Dapper** | **1000** |  **24.706 ms** | **1.5438 ms** | **3.1186 ms** |  **20.277 ms** |  **30.925 ms** |  **24.262 ms** |
| **Delete** |          **EF Core** |    **1** |   **5.205 ms** | **0.2573 ms** | **0.5197 ms** |   **4.554 ms** |   **6.752 ms** |   **5.110 ms** |
| **Delete** |          **EF Core** |   **10** |   **6.441 ms** | **0.3286 ms** | **0.6638 ms** |   **5.682 ms** |  **10.012 ms** |   **6.251 ms** |
| **Delete** |          **EF Core** |   **25** |   **7.906 ms** | **0.4224 ms** | **0.8533 ms** |   **7.006 ms** |  **12.880 ms** |   **7.700 ms** |
| **Delete** |          **EF Core** |   **50** |  **11.159 ms** | **0.5005 ms** | **1.0110 ms** |   **9.362 ms** |  **14.919 ms** |  **10.973 ms** |
| **Delete** |          **EF Core** |  **100** |  **17.217 ms** | **0.5996 ms** | **1.2113 ms** |  **15.231 ms** |  **20.882 ms** |  **17.110 ms** |
| **Delete** |          **EF Core** | **1000** | **125.911 ms** | **1.5767 ms** | **3.1851 ms** | **120.701 ms** | **133.612 ms** | **126.060 ms** |
