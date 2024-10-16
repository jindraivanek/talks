---
# try also 'default' to start simple
theme: default
# random image from a curated Unsplash collection by Anthony
# like them? see https://unsplash.com/collections/94734566/slidev
#background: images/immutable-tree-3061166569.png
# apply any windi css classes to the current slide
class: 'text-center'
# https://sli.dev/custom/highlighters.html
highlighter: shiki
# show line numbers in code blocks
lineNumbers: false
# some information about the slides, markdown enabled
info: |
  ## Immutable data structures presentation
# persist drawings in exports and build
drawings:
  persist: false
# use UnoCSS (experimental)
css: unocss
---

<style>
h1 {
  color: white;
  text-shadow: 2px 2px 4px #000000;
}
.imageText {
  font-size: 1.8rem;
  background-color: rgba(0, 0, 0, 0.2);;
  text-align: center;
  text-shadow: 2px 2px 4px #000000;
}
</style>

# Immutable Data Structures
![Alt text](img/immutable-tree-3061166569.png)

---

# Immutable Data Structures talks series
1) **Immutable Data Structures introduction**
   - why, how, structural sharing
   - tuples, records, classes
   - Linked list (F# list)
   - Tree (F# Set, Map)
2) **Immutable Data Structures in C#**
   - ReadOnly vs. Immutable vs. Frozen
   - ImmutableList, LinkedList
   - ImmutableDictionary, ImmutableHashSet, ImmutableSortedSet
   - benchmarks, notes
3) **Working with Immutable Data Structures**
   - F# List
   - F# Map and Set
   - C# builder pattern
   - F# Seq, C# IEnumerable
   - Lazy
   - Structural comparison
   - Referential transparency

---

# **PART 1**

---

# Immutable Data Structures
* no part of object can be changed after it's created

## Why?
* mutation is common source of bugs
* immutable data structures are easier to reason about
  - value passed to a function, can't be changed
* immutable data structures are thread-safe
* bonus: memory efficient time traveling

---

MYTH: to create new immutable value, you need to copy the whole thing

<Transform :scale="0.8">
<img src ="img/meme.jpg"/>
</Transform>

---

# How?
* we can share parts of the structure between old and new value
* **Structural sharing**

![Structural sharing](img/structural_sharing.png)

---

## Records

```fsharp
{ Id: int; Name: string }
```

- Immutable by default
- No special immutable structure
- Update syntax create new record with not-changed fields shared with old record
  - ```fsharp
    { oldRecord with Name = "Bob" }
    ```
  - only reference is copied (except for *structs*)

---

## F# (Linked) list

```fsharp
let listA = [1; 2; 3]
let listA = 1 :: 2 :: 3 :: []
```

![Alt text](img/list1.png)

---

## F# (Linked) list sharing

```fsharp
let listA = [1; 2; 3]
let listA = 1 :: 2 :: 3 :: []
let listA2 = listA
let listB = 4 :: listA
let listB2 = [4] @ listA
```

![width:1000px](img/linked_list_sharing.png)

---



![bg](img/terminusdb-commit-graph-diagram-regtech-1536x864.png)

---

# F# Set
Unordered set of values

Internally implemented as a (balanced) tree

```fsharp
let s = [11; 20; 29; 32; 41; 50; 65; 72; 91; 99] |> set
```

![Example tree](img/set1.png)

---

Insert = search + add

```fsharp
let s2 = s |> Set.add 35
```

![tree insert](img/tree-insert.gif)

from https://visualgo.net/en/bst

---

```fsharp
let s = [1; 7; 3; 9; 5; 6; 2; 8; 4] |> set
```

![bg contain](img/tree-inserts.gif)

from https://visualgo.net/en/bst

---

# F# Map
* Dictionary like immutable data structure
* Like `Set`, but with value linked with each key (node)

TODO image

---

```fsharp
let mapA = Map.ofList [1, "A"; 2, "B"; 3, "C"]
let mapB = Map.ofList [1, "A"; 2, "B"; 3, "C"; 4, "D"]
let mapB2 = Map.add 4 "D" mapA
mapB = mapB2 // true
```

---

# Structural comparison

- definition of equality based on values, not references
- all F# data types have defined structural comparison and ordering
- only few C# (compound) types have defined structural comparison and ordering
  - Tuples, Records, Array, ImmutableArray
- Immutability and structural comparison are different features, but it is common that immutable data structures have defined structural comparison
  - same value with different references are more common when working with immutable data structures

---

# PART 2 - C# Immutable collections

---

# Terminology

- **Mutable** (a.k.a read/write): a collection or type that allows for in-place updates that anyone with a reference to that object may observe.
- **Immutable**: a collection or type that cannot be changed at all, but can be efficiently mutated by allocating a new collection that shares much of the same memory with the original, but has new memory describing the change.
- **Freezable**: a collection or type that is mutable until some point in time when it is frozen, after which it cannot be changed.
- **Read only**: a reference whose type does not permit mutation of the underlying data, which is usually mutable by another reference.

> source: https://devblogs.microsoft.com/premier-developer/read-only-frozen-and-immutable-collections/

---

# ImmutableStack
- Stack = LIFO (Last In First Out) collection
- Linked list, F# list is actually immutable stack

<img src ="img/ImmutableStack.png"/>

> source: https://learn.microsoft.com/en-us/archive/msdn-magazine/2017/march/net-framework-immutable-collections

---

# ImmutableStack
<div grid="~ cols-2 gap-4">
<div>
<img src ="img/ImmutableStack.png"/>
</div>
<div>
```csharp
var s1 = ImmutableStack<Int32>.Empty;
var s2 = s1.Push(1);
var s3 = s2.Push(2);
var s4 = s3.Push(3);
var s5 = s4.Push(4);
var s6 = s4.Pop();
var s7 = s6.Pop();
```
</div>
</div>

---

# ImmutableStack
<div grid="~ cols-2 gap-4">
<div>
```csharp
var s1 = ImmutableStack<Int32>.Empty;
var s2 = s1.Push(1);
var s3 = s2.Push(2);
var s4 = s3.Push(3);
var s5 = s4.Push(4);
var s6 = s4.Pop();
var s7 = s6.Pop();
```
</div>
<div>
```fsharp
let s1 = []
let s2 = 1 :: s1
let s3 = 2 :: s2
let s4 = 3 :: s3
let s5 = 4 :: s4
let (_ :: s6) = s5 // same as let s6 = List.tail s5
let (_ :: s7) = s6 // same as let s7 = List.tail s6
```
</div>
</div>

---

# ImmutableStack benchmarks
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

---

# ImmutableList
- Indexable, represented as (balanced binary) tree (similar to `Map<int, T>`)

<Transform :scale="0.8">
<img src ="img/ImmutableList.png"/>
</Transform>

---

# ImmutableList

<div grid="~ cols-2 gap-4">
<div>
<Transform :scale="0.8">
<img src ="img/ImmutableList.png"/>
</Transform>
</div>
<div>
```csharp
var l1 = ImmutableList.Create<Int32>();
var l2 = l1.Add(1);
var l3 = l2.Add(2);
var l4 = l3.Add(3);
var l5 = l4.Replace(2, 4);
```
</div>
</div>

---

# ImmutableHashSet
- Represented as balanced binary tree of hash buckets
- hash as key, list of values with same hash as tree node value
- similar to F# `Set<'T>`

---

# ImmutableHashSet benchmarks
TODO

---

# ImmutableDictionary
- Same internal representation as `ImmutableHashSet`, but use hash of key.
- similar to F# `Map<'K, 'V>`

---

# ImmutableDictionary benchmarks

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

---

# ImmutableSortedSet, ImmutableSortedDictionary
- `ImmutableSortedSet` uses AVL tree, same as F# `Set`
- `ImmutableSortedDictionary` uses AVL tree, same as F# `Map`

---

# ImmutableArray
- immutable collection only by name - FrozenArray would be better
- updating is slow - whole array is copied

---

# Builders
- frozen flags

---

# C# Immutable collections - structural equality
- Only Immutable Array implements `IStructuralEquatable` and `IStructuralComparable` (TODO)
- Equals in other collections work sometimes as expected, sometimes not

```csharp
var s1 = ImmutableList.Create<int>();
var s2 = ImmutableList.Create<int>();
s1.Add(1);
s2.Add(1);
s1 == s2; // True
```

but

```csharp
var s1 = ImmutableList.Create<int>(0);
var s2 = ImmutableList.Create<int>(0);
s1.Add(1);
s2.Add(1);
s1 == s2; // False
```


---

# F# / C# naming

<style>
table {
  font-size: 18px;
}
</style>

| Collection | F# | C# |
| --- | --- | --- |
| Linked list | `list<'T>` | `ImmutableStack<T>` |
| Resizable array | `ResizeArray<'T>` | `List<T>` |
| Array | `array<'T>`, `'T[]` | `T[]` |
| Map (immutable dictionary) | `Map<'K, 'V>` | `ImmutableDictionary<K, V>` |
| Set (immutable set) | `Set<'T>` | `ImmutableHashSet<T>` |
| Dictionary (mutable) | - | `Dictionary<K, V>` |
| HashSet (mutable) | - | `HashSet<T>` |
| Enumerable | `seq<'T>` | `IEnumerable<T>` |

---

# Other useful C# collections

* `Queue<T>`
* `PriorityQueue<T>`
* `ConcurrentDictionary<K, V>`

---

# ImmutableList benchmarks

<style>
table {
  font-size: 10px;
}
</style>

|                                     Method |       Mean |     Error |    StdDev |    Gen0 |   Gen1 | Allocated |
|--- |-----------:|----------:|----------:|--------:|-------:|----------:|
|                          **'int - List cons'** |   2.375 us | 0.0473 us | 0.1059 us |  2.5482 | 0.4234 |   32000 B |
|                 'int - ImmutableList cons' |  95.410 us | 1.7462 us | 1.6334 us | 40.0391 | 9.6436 |  502896 B |
|                       **'int - List.reverse'** |   2.511 us | 0.0413 us | 0.0606 us |  2.5482 | 0.4234 |   32000 B |
|              'int - ImmutableList.reverse' |  71.121 us | 0.6854 us | 0.6411 us |  3.7842 | 0.8545 |   48024 B |
|                           **'int - List.map'** |   2.781 us | 0.0543 us | 0.0687 us |  2.5482 | 0.5074 |   32000 B |
|   'int - ImmutableList map by LINQ Select' |  31.375 us | 0.5986 us | 0.7571 us |  4.1504 | 0.9766 |   52200 B |
|       'int - ImmutableList map by SetItem' | 113.180 us | 2.1415 us | 2.4661 us | 36.2549 |      - |  455376 B |
|       'int - ImmutableList map by Builder' |  36.315 us | 0.6762 us | 0.6944 us |  3.7842 | 1.0376 |   48072 B |
|                        **'int - List.filter'** |   1.756 us | 0.0350 us | 0.0623 us |  1.2741 | 0.1411 |   16000 B |
| 'int - ImmutableList filter by LINQ Where' |  13.979 us | 0.2794 us | 0.3825 us |  2.2736 | 0.2747 |   28672 B |
|  'int - ImmutableList filter by RemoveAll' |  57.953 us | 0.9039 us | 0.8455 us |  2.3804 | 0.2441 |   30376 B |
|                        **'int - List.reduce'** |   1.095 us | 0.0148 us | 0.0138 us |       - |      - |         - |
|               'int - ImmutableList.reduce' |   4.495 us | 0.0656 us | 0.0806 us |  0.0076 |      - |     112 B |
|                      **'int - List.contains'** |   5.087 us | 0.0649 us | 0.0607 us |       - |      - |      40 B |
|             'int - ImmutableList.contains' |  12.743 us | 0.1634 us | 0.1448 us |       - |      - |      72 B |

---

# Enumerable, seq - lazy sequences

* Every collection implements `seq<'T>` (alias for `IEnumerable<T>`) interface.

* Interface for reading elements one by one.

* Lazy abstraction - elements are computed on demand.

---

## `seq<'t>`

```fsharp
xs |> Seq.map (fun x -> expensiveFun x) |> Seq.take 10 |> Seq.toList
```

Only first 10 elements are computed.

```fsharp
xs |> Seq.filter (...) |> Seq.map (fun x -> expensiveFun x) |> Seq.tryFind (...)
```

Only elements that pass the filter are computed.

---

## `seq<'t>`

There are cases where using `Seq` can be faster than `List`.

Example: expensive filtering and then taking first *k* elements.

```fsharp
xs |> Seq.filter (fun x -> expensiveFun x) |> Seq.take k |> Seq.toList
```

---

## Infinite sequences

Seq can be also used for generating (possible infinite) sequences.

```fsharp
let cycle xs =
    let arr = Array.ofSeq xs
    Seq.initInfinite (fun i -> arr.[i % arr.Length])
```

Or sequence of random numbers:

```fsharp
let r = System.Random()
Seq.initInfinite (fun _ -> r.Next())
```

---

# PART 3

---

# F# list performance

## F# list type definition

```fsharp
type List<'T> = 
| ([]) : 'T list
| ( :: ) : Head: 'T * Tail: 'T list -> 'T list
```

equivalently

```fsharp
type List<'T> = 
| Nil : 'T list
| Cons : Head: 'T * Tail: 'T list -> 'T list

let listA = Cons(1, Cons(2, Cons(3, Nil)))
```

---

## F# list performance

* fast iteration, mapping, filtering, append to start
* slow indexing, append on end
* `x :: xs` super fast
* `xs @ ys` slow

---

```fsharp
    [<Benchmark>]
    member _.ListAddToEnd() =
        let rec go i acc =
            if i = 0 then acc
            else go (i - 1) (acc @ [i])
        go size []

    [<Benchmark>]
    member _.ListAddToEndAcc() =
        let rec go i acc =
            if i = 0 then acc
            else go (i - 1) (i :: acc)
        go size [] |> List.rev
```

---

|          Method |        Mean |      Error |     StdDev |
|---------------- |------------:|-----------:|-----------:|
|    ListAddToEnd | 5,178.36 us | 102.125 us | 139.790 us |
| ListAddToEndAcc |    15.99 us |   0.308 us |   0.303 us |

* List.rev is fast!

---

# F# Set performance

## F# Set type definition

```fsharp
    (* A classic functional language implementation of binary trees *)

    [<CompilationRepresentation(CompilationRepresentationFlags.UseNullAsTrueValue)>]
    [<NoEquality; NoComparison>]
    type SetTree<'T> when 'T: comparison = 
        | SetEmpty                                          // height = 0   
        | SetNode of 'T * SetTree<'T> *  SetTree<'T> * int    // height = int 
        | SetOne  of 'T                                     // height = 1   
```

```fsharp
SetNode(41, SetNode(20, SetOne(11), SetNode(29, SetEmpty, SetOne(32), 1), 2), SetNode(65, SetOne(50), SetNode(91, SetOne(72), SetOne(99), 1), 2), 3)
```

---

## search, indexing

![Searching in list](img/list_search.gif)

* `List.find`, `List.nth` goes through list one by one
* `Set` is better for searching in big lists
* if you really need indexing, use array

---

* values must be comparable
* searching for item (`Set.exists`, `Set.contains`) by binary search
* insert, remove - unchanged part of tree is shared
![after insert](img/map_after_insert.png)
* functions with predicate on value (`Set.map`, `Set.filter`, `Set.partition`), goes through whole tree! (in order)
* keys cannot be duplicate - insert (`Set.add`) replace value if key already exists

---

## When to use Set instead of List?

* generally its faster to search for item with `Set`
* but for small sizes `List.contains` is faster

---

## When to use Set instead of List?

<style scoped>
table {
  font-size: 22px;
}
</style>

|       Method | Size |          Mean |       Error |      StdDev |
|------------- |----- |--------------:|------------:|------------:|
| **ListContains** |   **64** |      **2.159 μs** |   **0.0431 μs** |   **0.0998 μs** |
|  SetContains |   64 |      4.561 μs |   0.0833 μs |   0.0780 μs |
| **ListContains** |  **128** |      **8.241 μs** |   **0.0473 μs** |   **0.0443 μs** |
|  SetContains |  128 |     10.347 μs |   0.1933 μs |   0.1985 μs |
| **ListContains** |  **256** |     **31.169 μs** |   **0.1609 μs** |   **0.1426 μs** |
|  SetContains |  256 |     23.488 μs |   0.3803 μs |   0.3557 μs |
| **ListContains** |  **512** |    **119.456 μs** |   **0.5491 μs** |   **0.5136 μs** |
|  SetContains |  512 |     52.889 μs |   0.8146 μs |   0.6802 μs |
| **ListContains** | **1024** |    **467.593 μs** |   **1.9139 μs** |   **1.7902 μs** |
|  SetContains | 1024 |    149.908 μs |   1.2287 μs |   1.1494 μs |
| **ListContains** | **8192** | **29,487.104 μs** | **114.3813 μs** | **101.3960 μs** |
|  SetContains | 8192 |  1,548.127 μs |  19.6668 μs |  18.3963 μs |

---

## Another important functions
* `Set.union`
* `Set.intersect`
* `Set.difference`

* all of them work recursively on tree structure -> faster than the same on `list`

* `Set.isSubset`
* `Set.isSuperset`

* try to find all elements of first set in second

---

# F# Map performance

## F# Map type definition

```fsharp
[<NoEquality; NoComparison>]
[<AllowNullLiteral>]
type internal MapTree<'Key, 'Value>(k: 'Key, v: 'Value, h: int) =
    member _.Height = h
    member _.Key = k
    member _.Value = v
    new(k: 'Key, v: 'Value) = MapTree(k, v, 1)

[<NoEquality; NoComparison>]
[<Sealed>]
[<AllowNullLiteral>]
type internal MapTreeNode<'Key, 'Value>
    (
        k: 'Key,
        v: 'Value,
        left: MapTree<'Key, 'Value>,
        right: MapTree<'Key, 'Value>,
        h: int
    ) =
    inherit MapTree<'Key, 'Value>(k, v, h)
    member _.Left = left
    member _.Right = right
```

---

## F# Map performance

* keys must be comparable
* searching for item (`Map.find`, `Map.containsKey`) by binary search
* insert, remove - unchanged part of tree is shared
![after insert](img/map_after_insert.png)
* functions with predicate on key (`Map.pick`, `Map.findKey`), goes through whole tree! (in keys order)
* keys cannot be duplicate - insert (`Map.add`) replace value if key already exists

---

## F# Map performance

Creation of `Map` - List.groupBy

```fsharp
[1..1000] |> List.groupBy (fun x -> x % 100) |> Map.ofList
```

---

# Structural equality

## F# data types
* unit
* primitive types - `int`, `float`, `string`, `bool`, ...
* records
* tuples
* discriminated unions

## composed types

* `list`
* `Set`
* `Map`

all F# data types have defined structural equality and ordering - can be used in `Set` and `Map`

---

# Ordering
Ordering by field/case position, then recurse or prim. type ordering

```fsharp
type R = {A: int; B: string}
{A = 1; B = "b"} < {A = 2; B = "a"}
{A = 1; B = "a"} = {A = 1; B = "a"}
{A = 1; B = "a"} < {A = 1; B = "b"}

type R2 = {B: string; A: int}
{B = "b"; A = 1} > {B = "a"; A = 2}
{B = "a"; A = 2} > {B = "a"; A = 1}

("a", 1) < ("a", 2)
```

---

```fsharp
//DU - by order of cases

Some 1 < Some 2
None < Some System.Int32.MaxValue
```

(Ab)use of ordering example

```fsharp
type PokerHand =
    | HighCard of int
    | Pair of int
    | TwoPair of int * int
    | ThreeOfAKind of int
    | Straight of int
    | Flush of int
    | FullHouse of int * int
    | FourOfAKind of int
    | StraightFlush of int
    | RoyalFlush
```

---


# Referential transparency
* replace the function call with its result doesn't change meaning of the program
  - always returns the same result for the same input ("math-y" function)

* Immutable data structures allows us to write **Referential transparent** functions.

* no mutable variables / data structures, no side effects => **referential transparency**

---

* BUT:
* **referential transparency** can be achieved even with mutable data structures or side-effects
* mutable variables and data structures are perfectly fine when not leaking outside of function

---

```fsharp
    [<CompiledName("Fold")>]
    let fold<'T, 'State> folder (state: 'State) (list: 'T list) =
        match list with
        | [] -> state
        | _ ->
            let f = OptimizedClosures.FSharpFunc<_, _, _>.Adapt (folder)
            let mutable acc = state

            for x in list do
                acc <- f.Invoke(acc, x)

            acc
```

---

### Memoize function:

```fsharp
let memoizeBy projection f =
    let cache = System.Collections.Concurrent.ConcurrentDictionary()
    fun x -> cache.GetOrAdd(projection x, lazy f x).Value
```

---

# Pure functions

* **Pure** function:
    - always returns the same result for the same input (**referential transparency**)
    - no side effects

* no mutable variables / data structures, no side effects <=> **pure function**
* every **pure** function is **referential transparent**
* **pure function** is more strict, but can be checked by compiler - one of idea behind Haskell

---

# QUESTIONS?