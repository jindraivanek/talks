```

BenchmarkDotNet v0.13.12, Ubuntu 22.04.4 LTS (Jammy Jellyfish)
12th Gen Intel Core i7-12800H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2 DEBUG
  DefaultJob : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2


```
| Method                                          | Mean      | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|------------------------------------------------ |----------:|----------:|----------:|-------:|-------:|----------:|
| &#39;int - List cons&#39;                               |  3.315 μs | 0.0653 μs | 0.1193 μs | 2.5482 | 0.1450 |   32000 B |
| &#39;int - ImmutableStack cons&#39;                     |  3.410 μs | 0.0679 μs | 0.0996 μs | 2.5482 | 0.1450 |   32000 B |
| &#39;int - List.reverse&#39;                            |  3.457 μs | 0.0682 μs | 0.1176 μs | 2.5482 | 0.1450 |   32000 B |
| &#39;int - ImmutableStack reverse LINQ&#39;             |  7.030 μs | 0.1395 μs | 0.2973 μs | 3.2196 | 0.0458 |   40480 B |
| &#39;int - ImmutableStack reverse Pop Push&#39;         |  3.653 μs | 0.0699 μs | 0.0777 μs | 2.5482 | 0.1450 |   32000 B |
| &#39;int - List.map&#39;                                |  3.628 μs | 0.0717 μs | 0.0636 μs | 2.5482 | 0.2747 |   32000 B |
| &#39;int - ImmutableStack map by LINQ Select&#39;       |  8.332 μs | 0.1205 μs | 0.1127 μs | 2.5482 | 0.1373 |   32160 B |
| &#39;int - ImmutableStack map manual&#39;               |  7.150 μs | 0.1206 μs | 0.1128 μs | 5.0964 | 0.4272 |   64000 B |
| &#39;int - List.filter&#39;                             |  1.961 μs | 0.0388 μs | 0.0363 μs | 1.2741 | 0.0725 |   16000 B |
| &#39;int - ImmutableStack filter by LINQ Where&#39;     |  5.726 μs | 0.0502 μs | 0.0419 μs | 1.2817 | 0.0381 |   16160 B |
| &#39;int - ImmutableStack filter manual&#39;            |  3.996 μs | 0.0785 μs | 0.1126 μs | 2.5482 | 0.1068 |   32000 B |
| &#39;int - List.reduce&#39;                             |  1.075 μs | 0.0057 μs | 0.0050 μs |      - |      - |         - |
| &#39;int - ImmutableStack.reduce by LINQ Aggregate&#39; |  3.056 μs | 0.0242 μs | 0.0202 μs | 0.0076 |      - |     104 B |
| &#39;int - ImmutableStack.reduce by Pop&#39;            |  1.075 μs | 0.0040 μs | 0.0036 μs |      - |      - |         - |
| &#39;int - List.contains&#39;                           |  5.162 μs | 0.0671 μs | 0.0595 μs |      - |      - |      40 B |
| &#39;int - ImmutableStack.contains&#39;                 | 12.626 μs | 0.0787 μs | 0.0657 μs | 0.3204 |      - |    4040 B |
