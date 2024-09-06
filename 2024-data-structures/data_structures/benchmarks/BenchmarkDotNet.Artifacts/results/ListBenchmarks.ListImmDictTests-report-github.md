```

BenchmarkDotNet v0.13.12, Ubuntu 22.04.4 LTS (Jammy Jellyfish)
12th Gen Intel Core i7-12800H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2 DEBUG
  DefaultJob : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2


```
| Method                                  | Mean      | Error    | StdDev   | Gen0    | Gen1   | Allocated |
|---------------------------------------- |----------:|---------:|---------:|--------:|-------:|----------:|
| &#39;int - Map ofList&#39;                      | 118.02 μs | 2.235 μs | 2.906 μs | 46.2646 | 7.8125 |  581648 B |
| &#39;int - ImmutableDictionary CreateRange&#39; |  81.04 μs | 1.612 μs | 1.429 μs |  6.9580 | 1.2207 |   88288 B |
| &#39;int - Map cons&#39;                        | 103.36 μs | 2.055 μs | 2.019 μs | 42.4805 | 3.1738 |  533504 B |
| &#39;int - ImmutableDictionary cons&#39;        | 192.49 μs | 2.841 μs | 2.790 μs | 47.6074 | 5.1270 |  598712 B |
| &#39;int - Map find&#39;                        |  19.17 μs | 0.239 μs | 0.224 μs |       - |      - |         - |
| &#39;int - ImmutableDictionary find&#39;        |  21.77 μs | 0.416 μs | 0.409 μs |       - |      - |         - |
