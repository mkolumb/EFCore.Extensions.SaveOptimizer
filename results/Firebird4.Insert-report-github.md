``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-JLACHD : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |      Mean |     Error |    StdDev |       Min |       Max |    Median |
|------------ |---------------- |----- |----------:|----------:|----------:|----------:|----------:|----------:|
| **Insert** |       **Optimized** |    **1** |  **6.229 ms** | **0.4174 ms** | **0.8432 ms** |  **5.308 ms** | **10.037 ms** |  **6.022 ms** |
| **Insert** |       **Optimized** |   **10** | **13.157 ms** | **0.5857 ms** | **1.1832 ms** | **11.398 ms** | **17.567 ms** | **12.763 ms** |
| **Insert** | **Optimized Dapper** |    **1** |  **5.730 ms** | **0.2837 ms** | **0.5731 ms** |  **4.953 ms** |  **7.446 ms** |  **5.551 ms** |
| **Insert** | **Optimized Dapper** |   **10** | **18.914 ms** | **3.0427 ms** | **6.1464 ms** | **11.868 ms** | **39.241 ms** | **18.093 ms** |
| **Insert** |          **EF Core** |    **1** |  **7.226 ms** | **0.4442 ms** | **0.8973 ms** |  **5.730 ms** |  **9.644 ms** |  **7.061 ms** |
| **Insert** |          **EF Core** |   **10** | **13.960 ms** | **0.5852 ms** | **1.1820 ms** | **12.125 ms** | **17.679 ms** | **13.849 ms** |
