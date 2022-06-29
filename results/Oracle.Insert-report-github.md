``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-HMWWNW : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |      Mean |     Error |    StdDev |    Median |       Min |       Max |
|------------ |---------------- |----- |----------:|----------:|----------:|----------:|----------:|----------:|
| **InsertAsync** |       **Optimized** |    **1** |  **5.205 ms** | **0.3146 ms** | **0.6355 ms** |  **5.092 ms** |  **4.103 ms** |  **7.976 ms** |
| **InsertAsync** |       **Optimized** |   **10** |  **6.962 ms** | **0.6710 ms** | **1.3554 ms** |  **6.447 ms** |  **5.141 ms** | **11.910 ms** |
| **InsertAsync** |       **Optimized** |   **25** |  **8.533 ms** | **0.2835 ms** | **0.5727 ms** |  **8.492 ms** |  **7.534 ms** | **10.719 ms** |
| **InsertAsync** |       **Optimized** |   **50** | **11.722 ms** | **1.1425 ms** | **1.7100 ms** | **11.317 ms** |  **9.444 ms** | **19.231 ms** |
| **InsertAsync** |       **Optimized** |  **100** | **23.629 ms** | **4.3970 ms** | **5.0636 ms** | **22.068 ms** | **20.836 ms** | **43.535 ms** |
| **InsertAsync** |       **Optimized** | **1000** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |
| **InsertAsync** | **OptimizedDapper** |    **1** |  **6.638 ms** | **0.2583 ms** | **0.5217 ms** |  **6.675 ms** |  **5.575 ms** |  **7.835 ms** |
| **InsertAsync** | **OptimizedDapper** |   **10** |  **8.852 ms** | **0.7182 ms** | **1.2766 ms** |  **8.532 ms** |  **7.376 ms** | **13.515 ms** |
| **InsertAsync** | **OptimizedDapper** |   **25** | **10.852 ms** | **0.4370 ms** | **0.8828 ms** | **10.642 ms** |  **9.375 ms** | **14.391 ms** |
| **InsertAsync** | **OptimizedDapper** |   **50** | **15.181 ms** | **0.6535 ms** | **1.3200 ms** | **14.829 ms** | **13.003 ms** | **19.386 ms** |
| **InsertAsync** | **OptimizedDapper** |  **100** | **24.512 ms** | **1.6624 ms** | **2.9550 ms** | **23.813 ms** | **21.155 ms** | **34.811 ms** |
| **InsertAsync** | **OptimizedDapper** | **1000** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |
| **InsertAsync** |          **EfCore** |    **1** |  **6.444 ms** | **0.3111 ms** | **0.6284 ms** |  **6.237 ms** |  **5.503 ms** |  **8.593 ms** |
| **InsertAsync** |          **EfCore** |   **10** |  **6.994 ms** | **0.3145 ms** | **0.5590 ms** |  **6.904 ms** |  **6.001 ms** |  **8.667 ms** |
| **InsertAsync** |          **EfCore** |   **25** |  **8.142 ms** | **0.3852 ms** | **0.6847 ms** |  **7.923 ms** |  **7.095 ms** |  **9.804 ms** |
| **InsertAsync** |          **EfCore** |   **50** | **11.260 ms** | **0.8142 ms** | **1.4472 ms** | **11.006 ms** |  **9.683 ms** | **17.185 ms** |
| **InsertAsync** |          **EfCore** |  **100** | **16.440 ms** | **1.1606 ms** | **1.3365 ms** | **16.088 ms** | **13.595 ms** | **18.548 ms** |
| **InsertAsync** |          **EfCore** | **1000** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |

Benchmarks with issues:
  InsertBenchmark.InsertAsync: Job-HMWWNW(InvocationCount=10, IterationCount=10, LaunchCount=5, RunStrategy=Monitoring, UnrollFactor=1, WarmupCount=2) [Variant=Optimized, Rows=1000]
  InsertBenchmark.InsertAsync: Job-HMWWNW(InvocationCount=10, IterationCount=10, LaunchCount=5, RunStrategy=Monitoring, UnrollFactor=1, WarmupCount=2) [Variant=OptimizedDapper, Rows=1000]
  InsertBenchmark.InsertAsync: Job-HMWWNW(InvocationCount=10, IterationCount=10, LaunchCount=5, RunStrategy=Monitoring, UnrollFactor=1, WarmupCount=2) [Variant=EfCore, Rows=1000]
