```

BenchmarkDotNet v0.13.12, Ubuntu 22.04.4 LTS (Jammy Jellyfish)
12th Gen Intel Core i7-12800H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2 DEBUG
  DefaultJob : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2


```
| Method                                     | Mean       | Error     | StdDev    | Gen0    | Gen1   | Allocated |
|------------------------------------------- |-----------:|----------:|----------:|--------:|-------:|----------:|
| &#39;int - List cons&#39;                          |   3.126 μs | 0.0426 μs | 0.0378 μs |  2.5482 | 0.1450 |   32000 B |
| &#39;int - ImmutableList cons&#39;                 | 116.616 μs | 2.3127 μs | 2.4746 μs | 40.0391 | 3.6621 |  502896 B |
| &#39;int - List.reverse&#39;                       |   3.351 μs | 0.0656 μs | 0.0702 μs |  2.5482 | 0.1450 |   32000 B |
| &#39;int - ImmutableList.reverse&#39;              |  80.645 μs | 0.4608 μs | 0.3848 μs |  3.7842 | 0.4883 |   48024 B |
| &#39;int - List.map&#39;                           |   3.875 μs | 0.0765 μs | 0.1847 μs |  2.5482 | 0.2747 |   32000 B |
| &#39;int - ImmutableList map by LINQ Select&#39;   |  34.928 μs | 0.6706 μs | 0.7454 μs |  4.1504 | 0.3052 |   52200 B |
| &#39;int - ImmutableList map by SetItem&#39;       | 132.245 μs | 1.9784 μs | 1.6521 μs | 36.1328 |      - |  455376 B |
| &#39;int - ImmutableList map by Builder&#39;       |  39.691 μs | 0.4538 μs | 0.4023 μs |  3.7842 | 0.5493 |   48072 B |
| &#39;int - List.filter&#39;                        |   1.975 μs | 0.0375 μs | 0.0447 μs |  1.2741 | 0.0725 |   16000 B |
| &#39;int - ImmutableList filter by LINQ Where&#39; |  14.810 μs | 0.1225 μs | 0.1146 μs |  2.2736 | 0.0916 |   28672 B |
| &#39;int - ImmutableList filter by RemoveAll&#39;  |  59.245 μs | 0.3857 μs | 0.3608 μs |  2.3804 | 0.1221 |   30376 B |
| &#39;int - List.reduce&#39;                        |   1.073 μs | 0.0080 μs | 0.0071 μs |       - |      - |         - |
| &#39;int - ImmutableList.reduce&#39;               |   4.564 μs | 0.0640 μs | 0.0599 μs |  0.0076 |      - |     112 B |
| &#39;int - List.contains&#39;                      |   5.103 μs | 0.0164 μs | 0.0137 μs |       - |      - |      40 B |
| &#39;int - ImmutableList.contains&#39;             |  13.902 μs | 0.0967 μs | 0.0904 μs |       - |      - |      72 B |
