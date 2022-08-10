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
|      Method |         Variant |  Rows |     MeanNoOut |         Mean |          Min |           Q1 |       Median |           Q3 |          Max |
|------------ |---------------- |------ |--------------:|-------------:|-------------:|-------------:|-------------:|-------------:|-------------:|
| **Delete** |       **Optimized** |     **1** |     **4.5105 ms** |     **4.595 ms** |     **3.506 ms** |     **4.282 ms** |     **4.439 ms** |     **4.820 ms** |     **5.837 ms** |
| **Delete** |       **Optimized** |    **10** |     **5.0720 ms** |     **5.197 ms** |     **3.809 ms** |     **4.661 ms** |     **5.044 ms** |     **5.710 ms** |     **8.155 ms** |
| **Delete** |       **Optimized** |    **25** |     **5.1311 ms** |     **5.219 ms** |     **3.948 ms** |     **4.756 ms** |     **5.125 ms** |     **5.565 ms** |     **6.835 ms** |
| **Delete** |       **Optimized** |    **50** |     **6.1189 ms** |     **6.247 ms** |     **4.717 ms** |     **5.364 ms** |     **6.074 ms** |     **7.030 ms** |     **9.037 ms** |
| **Delete** |       **Optimized** |   **100** |     **7.8162 ms** |     **7.816 ms** |     **5.765 ms** |     **7.029 ms** |     **7.771 ms** |     **8.636 ms** |    **10.389 ms** |
| **Delete** |       **Optimized** |  **1000** |    **68.1148 ms** |    **67.694 ms** |    **45.047 ms** |    **65.397 ms** |    **68.570 ms** |    **70.802 ms** |    **78.399 ms** |
| **Delete** |       **Optimized** | **10000** | **2,025.5320 ms** | **1,830.913 ms** |   **586.962 ms** |   **715.838 ms** | **2,122.525 ms** | **2,558.255 ms** | **2,794.872 ms** |
| **Delete** | **Optimized Dapper** |     **1** |     **5.9818 ms** |     **5.929 ms** |     **4.065 ms** |     **5.196 ms** |     **5.963 ms** |     **6.705 ms** |     **8.069 ms** |
| **Delete** | **Optimized Dapper** |    **10** |     **5.8111 ms** |     **5.959 ms** |     **3.913 ms** |     **4.963 ms** |     **5.710 ms** |     **7.020 ms** |     **9.026 ms** |
| **Delete** | **Optimized Dapper** |    **25** |     **6.0850 ms** |     **6.191 ms** |     **4.723 ms** |     **5.593 ms** |     **6.064 ms** |     **6.628 ms** |     **8.588 ms** |
| **Delete** | **Optimized Dapper** |    **50** |     **6.8183 ms** |     **6.915 ms** |     **4.938 ms** |     **6.184 ms** |     **6.843 ms** |     **7.559 ms** |    **10.644 ms** |
| **Delete** | **Optimized Dapper** |   **100** |     **8.1024 ms** |     **8.158 ms** |     **6.098 ms** |     **7.441 ms** |     **8.139 ms** |     **8.786 ms** |    **11.432 ms** |
| **Delete** | **Optimized Dapper** |  **1000** |    **64.8988 ms** |    **65.946 ms** |    **52.167 ms** |    **63.799 ms** |    **64.880 ms** |    **66.645 ms** |    **88.576 ms** |
| **Delete** | **Optimized Dapper** | **10000** | **2,012.0500 ms** | **1,882.595 ms** |   **636.587 ms** |   **722.806 ms** | **2,103.390 ms** | **2,565.094 ms** | **5,291.755 ms** |
| **Delete** |          **EF Core** |     **1** |     **7.6119 ms** |     **7.923 ms** |     **5.574 ms** |     **6.786 ms** |     **7.617 ms** |     **8.525 ms** |    **13.823 ms** |
| **Delete** |          **EF Core** |    **10** |    **13.5676 ms** |    **13.572 ms** |     **9.961 ms** |    **12.724 ms** |    **13.519 ms** |    **14.442 ms** |    **18.508 ms** |
| **Delete** |          **EF Core** |    **25** |    **20.6899 ms** |    **20.906 ms** |    **15.722 ms** |    **18.340 ms** |    **20.256 ms** |    **23.243 ms** |    **29.609 ms** |
| **Delete** |          **EF Core** |    **50** |    **37.0154 ms** |    **37.536 ms** |    **28.645 ms** |    **33.922 ms** |    **36.714 ms** |    **41.088 ms** |    **50.085 ms** |
| **Delete** |          **EF Core** |   **100** |    **62.2879 ms** |    **64.212 ms** |    **51.622 ms** |    **57.997 ms** |    **60.592 ms** |    **71.192 ms** |    **86.692 ms** |
| **Delete** |          **EF Core** |  **1000** |   **542.9741 ms** |   **546.596 ms** |   **511.310 ms** |   **531.994 ms** |   **540.876 ms** |   **554.027 ms** |   **686.587 ms** |
| **Delete** |          **EF Core** | **10000** | **6,590.6151 ms** | **6,624.737 ms** | **5,525.315 ms** | **6,397.470 ms** | **6,571.264 ms** | **6,912.975 ms** | **7,908.243 ms** |