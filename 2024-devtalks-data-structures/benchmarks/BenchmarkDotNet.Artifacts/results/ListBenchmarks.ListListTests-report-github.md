```

BenchmarkDotNet v0.13.12, Ubuntu 22.04.5 LTS (Jammy Jellyfish)
12th Gen Intel Core i7-12800H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.204
  [Host]     : .NET 8.0.4 (8.0.424.16909), X64 RyuJIT AVX2 DEBUG
  DefaultJob : .NET 8.0.4 (8.0.424.16909), X64 RyuJIT AVX2


```
| Method             | Mean     | Error   | StdDev  | Gen0    | Gen1    | Gen2    | Allocated |
|------------------- |---------:|--------:|--------:|--------:|--------:|--------:|----------:|
| &#39;F# List workload&#39; | 208.0 μs | 4.13 μs | 6.78 μs | 76.4160 | 31.4941 |       - |  937.5 KB |
| &#39;C# List Workload&#39; | 496.8 μs | 9.67 μs | 9.50 μs | 82.0313 | 41.0156 | 41.0156 | 697.15 KB |
