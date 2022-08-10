``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1826 (21H2)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK=6.0.302
  [Host]   : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT
  Oracle21 : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT

Job=Oracle21  EvaluateOverhead=True  OutlierMode=RemoveUpper  
InvocationCount=1  IterationCount=20  LaunchCount=3  
RunStrategy=ColdStart  UnrollFactor=1  WarmupCount=0  

```
|      Method |         Variant | Rows |   MeanNoOut |       Mean |        Min |         Q1 |     Median |           Q3 |          Max |
|------------ |---------------- |----- |------------:|-----------:|-----------:|-----------:|-----------:|-------------:|-------------:|
| **Insert** |       **Optimized** |    **1** |   **5.2729 ms** |   **5.420 ms** |   **3.672 ms** |   **4.478 ms** |   **5.146 ms** |     **6.118 ms** |     **8.436 ms** |
| **Insert** |       **Optimized** |   **10** |   **6.3585 ms** |   **6.522 ms** |   **4.574 ms** |   **5.742 ms** |   **6.194 ms** |     **7.174 ms** |     **9.125 ms** |
| **Insert** |       **Optimized** |   **25** |   **7.9702 ms** |   **8.135 ms** |   **6.309 ms** |   **6.856 ms** |   **7.994 ms** |     **8.753 ms** |    **14.854 ms** |
| **Insert** |       **Optimized** |   **50** |  **11.5421 ms** |  **12.638 ms** |   **8.835 ms** |  **10.972 ms** |  **11.557 ms** |    **12.331 ms** |    **37.795 ms** |
| **Insert** |       **Optimized** |  **100** |  **20.4393 ms** |  **30.122 ms** |  **16.131 ms** |  **17.690 ms** |  **19.303 ms** |    **32.629 ms** |    **84.238 ms** |
| **Insert** |       **Optimized** | **1000** | **251.5867 ms** | **273.626 ms** | **123.195 ms** | **191.088 ms** | **240.943 ms** |   **329.376 ms** |   **820.892 ms** |
| **Insert** | **Optimized Dapper** |    **1** |   **5.9457 ms** |   **6.009 ms** |   **4.224 ms** |   **5.122 ms** |   **5.929 ms** |     **6.670 ms** |     **9.101 ms** |
| **Insert** | **Optimized Dapper** |   **10** |   **6.8930 ms** |   **7.008 ms** |   **4.952 ms** |   **6.221 ms** |   **6.763 ms** |     **7.624 ms** |    **10.899 ms** |
| **Insert** | **Optimized Dapper** |   **25** |   **7.7360 ms** |   **7.828 ms** |   **5.768 ms** |   **6.946 ms** |   **7.717 ms** |     **8.459 ms** |    **11.507 ms** |
| **Insert** | **Optimized Dapper** |   **50** |  **11.3405 ms** |  **13.421 ms** |   **9.848 ms** |  **10.552 ms** |  **11.234 ms** |    **12.208 ms** |    **39.633 ms** |
| **Insert** | **Optimized Dapper** |  **100** |  **18.7753 ms** |  **21.711 ms** |  **16.701 ms** |  **17.643 ms** |  **18.578 ms** |    **20.857 ms** |    **51.203 ms** |
| **Insert** | **Optimized Dapper** | **1000** | **240.3390 ms** | **287.800 ms** | **127.427 ms** | **132.278 ms** | **252.781 ms** |   **381.439 ms** | **1,355.866 ms** |
| **Insert** |          **EF Core** |    **1** |   **7.0377 ms** |   **7.281 ms** |   **4.945 ms** |   **6.122 ms** |   **6.991 ms** |     **7.755 ms** |    **12.511 ms** |
| **Insert** |          **EF Core** |   **10** |   **8.6782 ms** |   **8.766 ms** |   **6.944 ms** |   **7.922 ms** |   **8.652 ms** |     **9.340 ms** |    **11.816 ms** |
| **Insert** |          **EF Core** |   **25** |  **14.2764 ms** |  **14.443 ms** |  **12.470 ms** |  **13.435 ms** |  **14.207 ms** |    **15.203 ms** |    **18.985 ms** |
| **Insert** |          **EF Core** |   **50** |  **27.5474 ms** |  **28.122 ms** |  **23.655 ms** |  **26.147 ms** |  **27.569 ms** |    **28.646 ms** |    **53.470 ms** |
| **Insert** |          **EF Core** |  **100** |  **44.9785 ms** |  **53.069 ms** |  **41.095 ms** |  **43.657 ms** |  **44.977 ms** |    **48.309 ms** |   **146.901 ms** |
| **Insert** |          **EF Core** | **1000** | **763.3822 ms** | **825.446 ms** | **388.660 ms** | **466.159 ms** | **775.918 ms** | **1,112.050 ms** | **1,997.386 ms** |