``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1706 (20H2/October2020Update)
Intel Core i7-8850H CPU 2.60GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.301
  [Host]     : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT
  Job-EVMAOE : .NET 6.0.6 (6.0.622.26707), X64 RyuJIT

InvocationCount=10  IterationCount=10  LaunchCount=5  
RunStrategy=Monitoring  UnrollFactor=1  WarmupCount=2  

```
|      Method |         Variant | Rows |     Mean |     Error |    StdDev |      Min |      Max |   Median |
|------------ |---------------- |----- |---------:|----------:|----------:|---------:|---------:|---------:|
| **UpdateAsync** |       **Optimized** |    **1** | **4.042 ms** | **0.5139 ms** | **1.0382 ms** | **3.155 ms** | **8.968 ms** | **3.686 ms** |
| **UpdateAsync** | **OptimizedDapper** |    **1** | **3.975 ms** | **0.4814 ms** | **0.9724 ms** | **2.963 ms** | **7.373 ms** | **3.693 ms** |
| **UpdateAsync** |          **EfCore** |    **1** | **4.105 ms** | **0.4773 ms** | **0.9642 ms** | **2.914 ms** | **7.401 ms** | **3.792 ms** |
