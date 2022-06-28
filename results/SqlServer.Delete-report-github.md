``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-LRLBOJ : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |      Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **DeleteAsync** |       **Optimized** |    **1** | **3.883 ms** | **0.2125 ms** | **0.4293 ms** | **3.121 ms** | **5.069 ms** | **3.784 ms** |
| **DeleteAsync** | **OptimizedDapper** |    **1** | **3.590 ms** | **0.1688 ms** | **0.3411 ms** | **3.066 ms** | **4.689 ms** | **3.529 ms** |
| **DeleteAsync** |          **EfCore** |    **1** | **3.840 ms** | **0.2161 ms** | **0.4364 ms** | **3.092 ms** | **4.929 ms** | **3.826 ms** |
