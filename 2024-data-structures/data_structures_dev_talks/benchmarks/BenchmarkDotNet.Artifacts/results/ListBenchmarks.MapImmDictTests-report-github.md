```

BenchmarkDotNet v0.13.12, Ubuntu 22.04.4 LTS (Jammy Jellyfish)
12th Gen Intel Core i7-12800H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.204
  [Host]     : .NET 8.0.4 (8.0.424.16909), X64 RyuJIT AVX2 DEBUG
  DefaultJob : .NET 8.0.4 (8.0.424.16909), X64 RyuJIT AVX2


```
| Method                                  | Mean       | Error      | StdDev     | Gen0       | Gen1      | Gen2     | Allocated    |
|---------------------------------------- |-----------:|-----------:|-----------:|-----------:|----------:|---------:|-------------:|
| &#39;int - Map ofSeq&#39;                       | 459.273 ms |  9.0652 ms | 10.4395 ms | 82000.0000 | 8000.0000 |        - | 1029666928 B |
| &#39;int - ImmutableDictionary CreateRange&#39; | 272.887 ms |  5.3916 ms | 13.0212 ms |  7500.0000 | 7000.0000 | 500.0000 |   88001348 B |
| &#39;int - ToFrozenDictionary&#39;              |  86.633 ms |  1.6625 ms |  1.4738 ms |  3142.8571 | 3000.0000 | 571.4286 |  117963226 B |
| &#39;int - Dict AsReadOnly&#39;                 |  68.672 ms |  1.3543 ms |  2.5103 ms |  3000.0000 | 2857.1429 | 428.5714 |   85890579 B |
| &#39;int - Map add&#39;                         | 402.154 ms |  7.9555 ms | 11.6611 ms | 80000.0000 | 6000.0000 |        - | 1013666640 B |
| &#39;int - ImmutableDictionary add&#39;         | 621.857 ms | 12.2887 ms | 10.8936 ms | 92000.0000 | 9000.0000 |        - | 1157280872 B |
| &#39;int - Map find&#39;                        |  48.168 ms |  0.4956 ms |  0.4139 ms |          - |         - |        - |         97 B |
| &#39;int - ImmutableDictionary find&#39;        |  51.157 ms |  0.8907 ms |  0.7896 ms |          - |         - |        - |         97 B |
| &#39;int - FrozenDictionary find&#39;           |   1.524 ms |  0.0296 ms |  0.0317 ms |          - |         - |        - |          2 B |
| &#39;int - ReadOnlyDictionary find&#39;         |   2.333 ms |  0.0442 ms |  0.0454 ms |          - |         - |        - |          4 B |
