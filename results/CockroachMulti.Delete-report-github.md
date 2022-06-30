``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]    : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Cockroach : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

Job=Cockroach  EvaluateOverhead=True  OutlierMode=RemoveUpper  
InvocationCount=1  IterationCount=15  LaunchCount=8  
RunStrategy=Throughput  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant |  Rows |     MeanNoOut |        Mean |         Min |          Q1 |      Median |          Q3 |          Max |
|------------ |---------------- |------ |--------------:|------------:|------------:|------------:|------------:|------------:|-------------:|
| **DeleteAsync** |       **Optimized** |     **1** |    **15.8915 ms** |    **16.43 ms** |    **11.43 ms** |    **13.69 ms** |    **15.88 ms** |    **17.95 ms** |     **28.00 ms** |
| **DeleteAsync** |       **Optimized** |    **10** |    **16.8066 ms** |    **18.12 ms** |    **12.44 ms** |    **14.56 ms** |    **16.76 ms** |    **19.79 ms** |     **33.88 ms** |
| **DeleteAsync** |       **Optimized** |    **25** |    **20.8573 ms** |    **22.06 ms** |    **14.58 ms** |    **17.78 ms** |    **20.68 ms** |    **25.18 ms** |     **46.97 ms** |
| **DeleteAsync** |       **Optimized** |    **50** |    **25.4329 ms** |    **27.59 ms** |    **18.20 ms** |    **22.08 ms** |    **24.63 ms** |    **31.44 ms** |     **54.35 ms** |
| **DeleteAsync** |       **Optimized** |   **100** |    **34.7506 ms** |    **36.47 ms** |    **24.79 ms** |    **30.52 ms** |    **34.65 ms** |    **38.79 ms** |     **83.51 ms** |
| **DeleteAsync** |       **Optimized** |  **1000** |   **189.4472 ms** |   **192.12 ms** |   **118.61 ms** |   **171.25 ms** |   **187.99 ms** |   **210.05 ms** |    **305.98 ms** |
| **DeleteAsync** |       **Optimized** | **10000** | **2,871.8516 ms** | **3,104.17 ms** | **1,608.76 ms** | **2,519.96 ms** | **2,850.27 ms** | **3,496.92 ms** |  **6,025.73 ms** |
| **DeleteAsync** | **OptimizedDapper** |     **1** |    **24.9271 ms** |    **27.03 ms** |    **14.93 ms** |    **20.22 ms** |    **24.38 ms** |    **31.51 ms** |     **59.55 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **10** |    **80.2944 ms** |   **107.42 ms** |    **22.99 ms** |    **49.86 ms** |    **78.16 ms** |   **115.54 ms** |    **599.31 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **25** |   **165.8862 ms** |   **223.10 ms** |    **39.44 ms** |   **103.60 ms** |   **155.19 ms** |   **272.68 ms** |    **952.98 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **50** |   **189.5698 ms** |   **283.70 ms** |    **55.30 ms** |   **105.80 ms** |   **178.88 ms** |   **335.26 ms** |  **1,748.03 ms** |
| **DeleteAsync** | **OptimizedDapper** |   **100** |   **318.3333 ms** |   **376.11 ms** |    **75.69 ms** |   **189.19 ms** |   **279.98 ms** |   **602.88 ms** |  **1,077.39 ms** |
| **DeleteAsync** | **OptimizedDapper** |  **1000** | **1,019.3192 ms** | **1,029.63 ms** |   **581.65 ms** |   **785.37 ms** |   **937.41 ms** | **1,273.58 ms** |  **1,580.72 ms** |
| **DeleteAsync** | **OptimizedDapper** | **10000** |            **NA** |          **NA** |          **NA** |          **NA** |          **NA** |          **NA** |           **NA** |
| **DeleteAsync** |          **EfCore** |     **1** |   **103.6606 ms** |   **110.27 ms** |    **33.89 ms** |    **62.79 ms** |   **105.37 ms** |   **136.35 ms** |    **274.63 ms** |
| **DeleteAsync** |          **EfCore** |    **10** |   **332.5770 ms** |   **354.54 ms** |    **82.43 ms** |   **245.69 ms** |   **318.55 ms** |   **464.49 ms** |    **746.19 ms** |
| **DeleteAsync** |          **EfCore** |    **25** | **1,071.3189 ms** | **1,155.25 ms** |   **434.59 ms** |   **848.29 ms** |   **950.24 ms** | **1,507.60 ms** |  **1,891.56 ms** |
| **DeleteAsync** |          **EfCore** |    **50** |   **884.5606 ms** |   **911.83 ms** |   **711.00 ms** |   **764.29 ms** |   **886.51 ms** |   **996.47 ms** |  **1,183.50 ms** |
| **DeleteAsync** |          **EfCore** |   **100** | **2,201.6781 ms** | **2,199.58 ms** |   **466.94 ms** | **1,266.24 ms** | **2,321.49 ms** | **3,071.01 ms** |  **4,365.85 ms** |
| **DeleteAsync** |          **EfCore** |  **1000** | **7,955.1699 ms** | **7,671.68 ms** | **4,162.76 ms** | **6,041.00 ms** | **8,228.49 ms** | **9,108.14 ms** | **10,535.30 ms** |
| **DeleteAsync** |          **EfCore** | **10000** |            **NA** |          **NA** |          **NA** |          **NA** |          **NA** |          **NA** |           **NA** |

Benchmarks with issues:
  DeleteBenchmark.DeleteAsync: Cockroach(EvaluateOverhead=True, OutlierMode=RemoveUpper, InvocationCount=1, IterationCount=15, LaunchCount=8, RunStrategy=Throughput, UnrollFactor=1, WarmupCount=2) [Variant=OptimizedDapper, Rows=10000]
  DeleteBenchmark.DeleteAsync: Cockroach(EvaluateOverhead=True, OutlierMode=RemoveUpper, InvocationCount=1, IterationCount=15, LaunchCount=8, RunStrategy=Throughput, UnrollFactor=1, WarmupCount=2) [Variant=EfCore, Rows=10000]
