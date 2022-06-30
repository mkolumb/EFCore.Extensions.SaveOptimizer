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
| **Insert** |       **Optimized** |    **1** |  **5.205 ms** | **0.3146 ms** | **0.6355 ms** |  **5.092 ms** |  **4.103 ms** |  **7.976 ms** |
| **Insert** |       **Optimized** |   **10** |  **6.962 ms** | **0.6710 ms** | **1.3554 ms** |  **6.447 ms** |  **5.141 ms** | **11.910 ms** |
| **Insert** |       **Optimized** |   **25** |  **8.533 ms** | **0.2835 ms** | **0.5727 ms** |  **8.492 ms** |  **7.534 ms** | **10.719 ms** |
| **Insert** |       **Optimized** |   **50** | **11.722 ms** | **1.1425 ms** | **1.7100 ms** | **11.317 ms** |  **9.444 ms** | **19.231 ms** |
| **Insert** |       **Optimized** |  **100** | **23.629 ms** | **4.3970 ms** | **5.0636 ms** | **22.068 ms** | **20.836 ms** | **43.535 ms** |
| **Insert** |       **Optimized** | **1000** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |
| **Insert** | **Optimized Dapper** |    **1** |  **6.638 ms** | **0.2583 ms** | **0.5217 ms** |  **6.675 ms** |  **5.575 ms** |  **7.835 ms** |
| **Insert** | **Optimized Dapper** |   **10** |  **8.852 ms** | **0.7182 ms** | **1.2766 ms** |  **8.532 ms** |  **7.376 ms** | **13.515 ms** |
| **Insert** | **Optimized Dapper** |   **25** | **10.852 ms** | **0.4370 ms** | **0.8828 ms** | **10.642 ms** |  **9.375 ms** | **14.391 ms** |
| **Insert** | **Optimized Dapper** |   **50** | **15.181 ms** | **0.6535 ms** | **1.3200 ms** | **14.829 ms** | **13.003 ms** | **19.386 ms** |
| **Insert** | **Optimized Dapper** |  **100** | **24.512 ms** | **1.6624 ms** | **2.9550 ms** | **23.813 ms** | **21.155 ms** | **34.811 ms** |
| **Insert** | **Optimized Dapper** | **1000** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |
| **Insert** |          **EF Core** |    **1** |  **6.444 ms** | **0.3111 ms** | **0.6284 ms** |  **6.237 ms** |  **5.503 ms** |  **8.593 ms** |
| **Insert** |          **EF Core** |   **10** |  **6.994 ms** | **0.3145 ms** | **0.5590 ms** |  **6.904 ms** |  **6.001 ms** |  **8.667 ms** |
| **Insert** |          **EF Core** |   **25** |  **8.142 ms** | **0.3852 ms** | **0.6847 ms** |  **7.923 ms** |  **7.095 ms** |  **9.804 ms** |
| **Insert** |          **EF Core** |   **50** | **11.260 ms** | **0.8142 ms** | **1.4472 ms** | **11.006 ms** |  **9.683 ms** | **17.185 ms** |
| **Insert** |          **EF Core** |  **100** | **16.440 ms** | **1.1606 ms** | **1.3365 ms** | **16.088 ms** | **13.595 ms** | **18.548 ms** |
| **Insert** |          **EF Core** | **1000** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |        **NA** |

Benchmarks with issues:
  InsertBenchmark.Insert: Job-HMWWNW(InvocationCount=10, IterationCount=10, LaunchCount=5, RunStrategy=Monitoring, UnrollFactor=1, WarmupCount=2) [Variant=Optimized, Rows=1000]
  InsertBenchmark.Insert: Job-HMWWNW(InvocationCount=10, IterationCount=10, LaunchCount=5, RunStrategy=Monitoring, UnrollFactor=1, WarmupCount=2) [Variant=Optimized Dapper, Rows=1000]
  InsertBenchmark.Insert: Job-HMWWNW(InvocationCount=10, IterationCount=10, LaunchCount=5, RunStrategy=Monitoring, UnrollFactor=1, WarmupCount=2) [Variant=EF Core, Rows=1000]
