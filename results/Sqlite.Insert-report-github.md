``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1826 (21H2)
12th Gen Intel Core i9-12900HK, 1 CPU, 20 logical and 14 physical cores
.NET SDK=6.0.302
  [Host] : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT
  Sqlite : .NET 6.0.7 (6.0.722.32202), X64 RyuJIT

Job=Sqlite  EvaluateOverhead=True  OutlierMode=RemoveUpper  
InvocationCount=1  IterationCount=20  LaunchCount=3  
RunStrategy=ColdStart  UnrollFactor=1  WarmupCount=0  

```
|      Method |         Variant |  Rows |       MeanNoOut |         Mean |          Min |           Q1 |       Median |           Q3 |          Max |
|------------ |---------------- |------ |----------------:|-------------:|-------------:|-------------:|-------------:|-------------:|-------------:|
| **Insert** |       **Optimized** |     **1** |     **856.7259 μs** |     **860.8 μs** |     **581.6 μs** |     **722.5 μs** |     **858.8 μs** |     **995.1 μs** |   **1,405.8 μs** |
| **Insert** |       **Optimized** |    **10** |   **1,046.8250 μs** |   **1,078.2 μs** |     **820.4 μs** |     **919.0 μs** |   **1,075.2 μs** |   **1,177.7 μs** |   **2,109.5 μs** |
| **Insert** |       **Optimized** |    **25** |   **3,287.1385 μs** |   **3,464.5 μs** |   **2,422.4 μs** |   **2,980.7 μs** |   **3,264.3 μs** |   **3,936.7 μs** |   **4,940.3 μs** |
| **Insert** |       **Optimized** |    **50** |   **3,913.3654 μs** |   **4,020.9 μs** |   **3,323.6 μs** |   **3,682.9 μs** |   **3,860.6 μs** |   **4,365.1 μs** |   **5,012.3 μs** |
| **Insert** |       **Optimized** |   **100** |   **5,943.6720 μs** |   **6,038.9 μs** |   **4,617.4 μs** |   **5,473.4 μs** |   **5,987.9 μs** |   **6,572.2 μs** |   **8,149.3 μs** |
| **Insert** |       **Optimized** |  **1000** |  **44,210.6300 μs** |  **46,352.8 μs** |  **28,298.7 μs** |  **36,380.4 μs** |  **41,917.1 μs** |  **55,603.1 μs** |  **75,794.0 μs** |
| **Insert** |       **Optimized** | **10000** | **640,886.3429 μs** | **642,877.4 μs** | **543,798.1 μs** | **612,558.7 μs** | **643,092.1 μs** | **669,206.3 μs** | **770,428.4 μs** |
| **Insert** | **Optimized Dapper** |     **1** |     **930.2778 μs** |     **986.1 μs** |     **590.7 μs** |     **775.0 μs** |     **868.8 μs** |   **1,163.8 μs** |   **1,909.2 μs** |
| **Insert** | **Optimized Dapper** |    **10** |   **1,183.5500 μs** |   **1,229.1 μs** |     **948.9 μs** |   **1,058.7 μs** |   **1,180.2 μs** |   **1,312.8 μs** |   **2,044.4 μs** |
| **Insert** | **Optimized Dapper** |    **25** |   **3,933.2207 μs** |   **3,976.4 μs** |   **2,841.7 μs** |   **3,389.6 μs** |   **3,851.7 μs** |   **4,561.6 μs** |   **5,455.1 μs** |
| **Insert** | **Optimized Dapper** |    **50** |   **4,787.4821 μs** |   **4,894.4 μs** |   **3,637.4 μs** |   **4,296.9 μs** |   **4,803.3 μs** |   **5,303.5 μs** |   **7,245.4 μs** |
| **Insert** | **Optimized Dapper** |   **100** |   **6,805.6926 μs** |   **7,128.6 μs** |   **5,350.2 μs** |   **6,104.9 μs** |   **6,759.5 μs** |   **7,651.1 μs** |  **10,752.9 μs** |
| **Insert** | **Optimized Dapper** |  **1000** |  **44,483.0433 μs** |  **46,423.4 μs** |  **26,671.4 μs** |  **34,531.1 μs** |  **43,428.7 μs** |  **56,598.9 μs** |  **72,648.6 μs** |
| **Insert** | **Optimized Dapper** | **10000** | **638,473.8393 μs** | **638,639.0 μs** | **544,668.2 μs** | **608,018.4 μs** | **638,553.4 μs** | **670,793.0 μs** | **738,106.0 μs** |
| **Insert** |          **EF Core** |     **1** |     **762.3111 μs** |     **794.5 μs** |     **613.3 μs** |     **659.8 μs** |     **753.2 μs** |     **876.6 μs** |   **1,266.4 μs** |
| **Insert** |          **EF Core** |    **10** |   **1,234.6769 μs** |   **1,312.1 μs** |     **986.1 μs** |   **1,077.5 μs** |   **1,268.5 μs** |   **1,420.3 μs** |   **2,433.9 μs** |
| **Insert** |          **EF Core** |    **25** |   **3,713.6379 μs** |   **3,547.1 μs** |   **1,611.1 μs** |   **3,245.3 μs** |   **3,765.4 μs** |   **4,148.0 μs** |   **4,820.0 μs** |
| **Insert** |          **EF Core** |    **50** |   **3,273.1857 μs** |   **3,310.5 μs** |   **2,685.9 μs** |   **3,048.4 μs** |   **3,251.2 μs** |   **3,560.6 μs** |   **4,038.7 μs** |
| **Insert** |          **EF Core** |   **100** |   **6,099.0400 μs** |   **6,195.1 μs** |   **4,660.5 μs** |   **5,602.2 μs** |   **6,220.3 μs** |   **6,642.8 μs** |   **9,822.8 μs** |
| **Insert** |          **EF Core** |  **1000** |  **51,974.5067 μs** |  **53,809.7 μs** |  **34,233.4 μs** |  **40,802.4 μs** |  **51,190.4 μs** |  **62,657.3 μs** |  **88,358.6 μs** |
| **Insert** |          **EF Core** | **10000** | **546,620.3833 μs** | **548,836.8 μs** | **463,145.2 μs** | **521,092.7 μs** | **543,286.4 μs** | **575,530.8 μs** | **626,740.4 μs** |
