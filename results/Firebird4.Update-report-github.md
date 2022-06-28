``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-EXRPTT : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |      Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **UpdateAsync** |       **Optimized** |    **1** | **5.583 ms** | **0.3788 ms** | **0.7652 ms** | **4.478 ms** | **7.472 ms** | **5.363 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **5.774 ms** | **0.4570 ms** | **0.9232 ms** | **4.723 ms** | **9.339 ms** | **5.523 ms** |
| **UpdateAsync** |          **EfCore** |    **1** | **6.335 ms** | **0.3743 ms** | **0.7561 ms** | **5.401 ms** | **8.670 ms** | **6.100 ms** |
