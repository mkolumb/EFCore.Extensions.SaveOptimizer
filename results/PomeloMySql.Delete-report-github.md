``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]      : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  PomeloMySql : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Job=PomeloMySql  EvaluateOverhead=True  OutlierMode=RemoveUpper  
InvocationCount=1  IterationCount=15  LaunchCount=8  
RunStrategy=Throughput  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |     MeanNoOut |        Mean |         Min |          Q1 |      Median |          Q3 |         Max |
|------------ |---------------- |------ |--------------:|------------:|------------:|------------:|------------:|------------:|------------:|
| **DeleteAsync** |       **Optimized** |     **1** |    **15.4375 ms** |    **15.64 ms** |    **11.32 ms** |    **14.57 ms** |    **15.35 ms** |    **16.60 ms** |    **22.04 ms** |
| **DeleteAsync** |       **Optimized** |    **10** |    **16.5683 ms** |    **16.63 ms** |    **13.63 ms** |    **15.64 ms** |    **16.56 ms** |    **17.55 ms** |    **20.95 ms** |
| **DeleteAsync** |       **Optimized** |    **25** |    **18.0679 ms** |    **18.52 ms** |    **14.23 ms** |    **16.93 ms** |    **17.87 ms** |    **19.54 ms** |    **29.36 ms** |
| **DeleteAsync** |       **Optimized** |    **50** |    **19.9762 ms** |    **20.16 ms** |    **15.47 ms** |    **19.11 ms** |    **19.77 ms** |    **21.36 ms** |    **27.49 ms** |
| **DeleteAsync** |       **Optimized** |   **100** |    **22.5805 ms** |    **23.05 ms** |    **18.73 ms** |    **21.15 ms** |    **22.51 ms** |    **24.02 ms** |    **33.78 ms** |
| **DeleteAsync** |       **Optimized** |  **1000** |    **64.7192 ms** |    **65.55 ms** |    **55.88 ms** |    **61.48 ms** |    **64.59 ms** |    **68.39 ms** |    **93.46 ms** |
| **DeleteAsync** |       **Optimized** | **10000** |   **588.9130 ms** |   **590.33 ms** |   **548.19 ms** |   **576.25 ms** |   **587.93 ms** |   **601.69 ms** |   **645.20 ms** |
| **DeleteAsync** | **OptimizedDapper** |     **1** |    **14.0348 ms** |    **14.04 ms** |    **10.22 ms** |    **13.15 ms** |    **14.01 ms** |    **14.75 ms** |    **17.77 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **10** |    **14.5892 ms** |    **14.72 ms** |    **11.38 ms** |    **13.74 ms** |    **14.50 ms** |    **15.53 ms** |    **19.92 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **25** |    **16.1315 ms** |    **16.41 ms** |    **12.96 ms** |    **15.04 ms** |    **16.12 ms** |    **17.40 ms** |    **21.57 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **50** |    **17.5786 ms** |    **17.94 ms** |    **14.67 ms** |    **16.91 ms** |    **17.47 ms** |    **18.58 ms** |    **24.48 ms** |
| **DeleteAsync** | **OptimizedDapper** |   **100** |    **20.7731 ms** |    **21.14 ms** |    **17.70 ms** |    **19.69 ms** |    **20.75 ms** |    **21.98 ms** |    **27.69 ms** |
| **DeleteAsync** | **OptimizedDapper** |  **1000** |    **64.2450 ms** |    **65.67 ms** |    **55.33 ms** |    **61.02 ms** |    **63.78 ms** |    **69.36 ms** |    **89.78 ms** |
| **DeleteAsync** | **OptimizedDapper** | **10000** |   **673.4550 ms** |   **676.95 ms** |   **633.39 ms** |   **661.86 ms** |   **672.94 ms** |   **687.85 ms** |   **783.02 ms** |
| **DeleteAsync** |          **EfCore** |     **1** |    **18.8121 ms** |    **19.05 ms** |    **15.78 ms** |    **17.89 ms** |    **18.71 ms** |    **19.92 ms** |    **23.93 ms** |
| **DeleteAsync** |          **EfCore** |    **10** |    **23.0635 ms** |    **23.51 ms** |    **18.73 ms** |    **21.68 ms** |    **23.06 ms** |    **24.81 ms** |    **33.36 ms** |
| **DeleteAsync** |          **EfCore** |    **25** |    **29.9827 ms** |    **30.53 ms** |    **23.74 ms** |    **28.02 ms** |    **29.95 ms** |    **31.97 ms** |    **45.92 ms** |
| **DeleteAsync** |          **EfCore** |    **50** |    **35.5747 ms** |    **35.75 ms** |    **27.41 ms** |    **33.41 ms** |    **35.56 ms** |    **38.16 ms** |    **47.94 ms** |
| **DeleteAsync** |          **EfCore** |   **100** |    **55.0794 ms** |    **55.95 ms** |    **36.56 ms** |    **50.31 ms** |    **54.06 ms** |    **61.02 ms** |    **79.41 ms** |
| **DeleteAsync** |          **EfCore** |  **1000** |   **407.4844 ms** |   **411.46 ms** |   **307.31 ms** |   **382.49 ms** |   **405.48 ms** |   **437.04 ms** |   **577.81 ms** |
| **DeleteAsync** |          **EfCore** | **10000** | **4,930.2131 ms** | **4,824.89 ms** | **3,957.44 ms** | **4,474.03 ms** | **4,967.10 ms** | **5,123.21 ms** | **5,523.38 ms** |
