module ListBenchmarks

open System
open System.Collections.Generic
open System.Linq
open System.Collections.Immutable
open System.Collections.Frozen
open System.Collections
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

type MyRecord = { Id: int; Name: string }

type DU =
    | A of int
    | B of string

let inline test xs f =
    for i in xs do
        f i xs |> ignore

let size = 1000
let listOfInts size = [ 1..size ]
let input<'T>() : 'T list =
    let listOfStrings = listOfInts size |> List.map (fun i -> i.ToString())
    let listOfRecords =
        listOfInts size |> List.map (fun i -> { Id = i; Name = i.ToString() })

    match typeof<'T> with
    | t when t = typeof<int> -> listOfInts size |> Seq.cast<'T> |> List.ofSeq
    | t when t = typeof<string> -> listOfStrings |> Seq.cast<'T> |> List.ofSeq
    | t when t = typeof<MyRecord> -> listOfRecords |> Seq.cast<'T> |> List.ofSeq

[<GenericTypeArguments(typeof<int>)>]
[<GenericTypeArguments(typeof<string>)>]
[<GenericTypeArguments(typeof<MyRecord>)>]
type ListFunctions<'T>() =
    let xs = input<'T>()

    // [<Benchmark>]
    // member _.ListRev() =
    //     List.rev xs

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

    [<Benchmark>]
    member _.ListRevIndexed() =
        let rec go i acc =
            if i = 0 then acc
            else go (i - 1) (xs.[i] :: acc)
        go (size-1) []

type ListVsSet() =
    [<Params(64, 128, 256, 512, 1024, 8192)>]
    member val Size = 0 with get, set

    member x.ListOfInts = listOfInts x.Size
    member x.SetOfInts = x.ListOfInts |> Set.ofList
    
    [<Benchmark>]
    member x.ListContains() =
        test x.ListOfInts List.contains

    [<Benchmark>]
    member x.SetContains() =
        test x.SetOfInts Set.contains

type Set() =
    let setOfInts = listOfInts size |> set
    
    [<Benchmark>]
    member _.SetItemUnionByAdd() =
        let rec go i acc =
            if i = 0 then acc
            else go (i - 1) (Set.add (size+i) acc)
        go size setOfInts

    [<Benchmark>]
    member _.SetUnion() =
        Set.union setOfInts (listOfInts size |> List.map (fun i -> size + i) |> set)
        
[<MemoryDiagnoser>]
type ListImmListTests() =
    let size = 1000
    let listOfInts = [ 1..size ]
    let listOfIntsSmaller = [ 1..100 ]
    let listOfStrings = listOfInts |> List.map (fun i -> i.ToString())
    let listOfRecords =
        listOfInts |> List.map (fun i -> { Id = i; Name = i.ToString() })

    let immListOfInts = listOfInts |> ImmutableList.CreateRange
    let immListOfIntsSmaller = listOfIntsSmaller |> ImmutableList.CreateRange

    [<Benchmark(Description = "int - List cons")>]
    member _.IntListCons() =
        let rec go i acc =
            if i = 0 then acc
            else go (i - 1) (i :: acc)
        go size []

    [<Benchmark(Description = "int - ImmutableList cons")>]
    member _.IntImmListCons() =
        let rec go i (acc: ImmutableList<_>) =
            if i = 0 then acc
            else go (i - 1) (acc.Add i)
        go size ImmutableList<int>.Empty

    [<Benchmark(Description = "int - List.reverse")>]
    member _.IntListReverse() =
        listOfInts |> List.rev

    [<Benchmark(Description = "int - ImmutableList.reverse")>]
    member _.IntImmListReverse() =
        immListOfInts.Reverse()

    [<Benchmark(Description = "int - List.map")>]
    member _.IntListMap() =
        listOfInts |> List.map ((+) 1)

    [<Benchmark(Description = "int - ImmutableList map by LINQ Select")>]
    member _.IntImmListMap() =
        immListOfInts.Select ((+) 1) |> ImmutableList.CreateRange

    [<Benchmark(Description = "int - ImmutableList map by SetItem")>]
    member _.IntImmListMapSetItem() =
        let mutable xs = immListOfInts
        for i=0 to size-1 do
            xs <- immListOfInts.SetItem(i, i + 1)
        xs

    [<Benchmark(Description = "int - ImmutableList map by Builder")>]
    member _.IntImmListMapBuilder() =
        //let mutable xs = immListOfInts
        let b = immListOfInts.ToBuilder()
        for i=0 to size-1 do
            b[i] <- i + 1
        b.ToImmutable()

    [<Benchmark(Description = "int - List.filter")>]
    member _.IntListFilter() =
        listOfInts |> List.filter (fun i -> i % 2 = 0)

    [<Benchmark(Description = "int - ImmutableList filter by LINQ Where")>]
    member _.IntImmListFilter() =
        immListOfInts.Where (fun i -> i % 2 = 0) |> ImmutableList.CreateRange

    [<Benchmark(Description = "int - ImmutableList filter by RemoveAll")>]
    member _.IntImmListFilterRemoveAll() =
        immListOfInts.RemoveAll (fun i -> i % 2 <> 0)

    [<Benchmark(Description = "int - List.reduce")>]
    member _.IntListReduce() =
        listOfInts |> (List.reduce (+))

    [<Benchmark(Description = "int - ImmutableList.reduce")>]
    member _.IntImmListReduce() =
        let mutable x = 0
        immListOfInts.ForEach (fun y -> x <- x + y)

    [<Benchmark(Description = "int - List.contains")>]
    member _.IntListContains() =
        test listOfIntsSmaller List.contains

    [<Benchmark(Description = "int - ImmutableList.contains")>]
    member _.IntImmListContains() =
        test immListOfIntsSmaller (fun i xs -> xs.Contains i)

[<MemoryDiagnoser>]
type ListImmStackTests() =
    
    let stackRev (immStack: ImmutableStack<'t>) =
        let mutable s = immStack
        let mutable r = ImmutableStack<'t>.Empty
        while not (s.IsEmpty) do
            r <- r.Push (s.Peek())
            s <- s.Pop()
        r        

    let stackRevMap f (immStack: ImmutableStack<'t>) =
        let mutable s = immStack
        let mutable r = ImmutableStack<'t>.Empty
        while not (s.IsEmpty) do
            r <- r.Push (f <| s.Peek())
            s <- s.Pop()
        r        

    let stackRevFilter f (immStack: ImmutableStack<'t>) =
        let mutable s = immStack
        let mutable r = ImmutableStack<'t>.Empty
        while not (s.IsEmpty) do
            if f (s.Peek()) then
                r <- r.Push (s.Peek())
            s <- s.Pop()
        r        
        
    let size = 1000
    let listOfInts = [ 1..size ]
    let listOfIntsSmaller = [ 1..100 ]
    let listOfStrings = listOfInts |> List.map (fun i -> i.ToString())
    let listOfRecords =
        listOfInts |> List.map (fun i -> { Id = i; Name = i.ToString() })

    let ImmStackOfInts = listOfInts |> ImmutableStack.CreateRange
    let ImmStackOfIntsSmaller = listOfIntsSmaller |> ImmutableStack.CreateRange

    [<Benchmark(Description = "int - List cons")>]
    member _.IntListCons() =
        let rec go i acc =
            if i = 0 then acc
            else go (i - 1) (i :: acc)
        go size []

    [<Benchmark(Description = "int - ImmutableStack cons")>]
    member _.IntImmStackCons() =
        let rec go i (acc: ImmutableStack<_>) =
            if i = 0 then acc
            else go (i - 1) (acc.Push i)
        go size ImmutableStack<int>.Empty

    [<Benchmark(Description = "int - List.reverse")>]
    member _.IntListReverse() =
        listOfInts |> List.rev

    [<Benchmark(Description = "int - ImmutableStack reverse LINQ")>]
    member _.IntImmStackReverseLinq() =
        ImmutableStack.CreateRange(ImmStackOfInts.Reverse())

    [<Benchmark(Description = "int - ImmutableStack reverse Pop Push")>]
    member _.IntImmStackReverse() =
        stackRev ImmStackOfInts

    [<Benchmark(Description = "int - List.map")>]
    member _.IntListMap() =
        listOfInts |> List.map ((+) 1)

    [<Benchmark(Description = "int - ImmutableStack map by LINQ Select")>]
    member _.IntImmStackMapLinq() =
        ImmStackOfInts.Select ((+) 1) |> ImmutableStack.CreateRange

    [<Benchmark(Description = "int - ImmutableStack map double reverse")>]
    member _.IntImmStackMap() =
        ImmStackOfInts |> stackRevMap ((+) 1) |> stackRev
    
    [<Benchmark(Description = "int - List.filter")>]
    member _.IntListFilter() =
        listOfInts |> List.filter (fun i -> i % 2 = 0)

    [<Benchmark(Description = "int - ImmutableStack filter by LINQ Where")>]
    member _.IntImmStackFilterLinq() =
        ImmStackOfInts.Where (fun i -> i % 2 = 0) |> ImmutableStack.CreateRange

    [<Benchmark(Description = "int - ImmutableStack filter double reverse")>]
    member _.IntImmStackFilter() =
        ImmStackOfInts |> stackRevFilter (fun i -> i % 2 = 0) |> stackRev

    [<Benchmark(Description = "int - List.reduce")>]
    member _.IntListReduce() =
        listOfInts |> (List.reduce (+))

    [<Benchmark(Description = "int - ImmutableStack.reduce by LINQ Aggregate")>]
    member _.IntImmStackReduceLinq() =
        ImmStackOfInts.Aggregate (fun x y -> x + y)
        
    [<Benchmark(Description = "int - ImmutableStack.reduce by Pop")>]
    member _.IntImmStackReduce() =
        let mutable s = ImmStackOfInts
        let mutable x = 0
        while not s.IsEmpty do
            x <- x + s.Peek()
            s <- s.Pop()
        x

    [<Benchmark(Description = "int - List.contains")>]
    member _.IntListContains() =
        test listOfIntsSmaller List.contains

    [<Benchmark(Description = "int - ImmutableStack.contains")>]
    member _.IntImmStackContains() =
        test ImmStackOfIntsSmaller (fun i xs -> xs.Contains i)

[<MemoryDiagnoser>]
type MapImmDictTests() =
    let size = 1000000
    let mapOfInts = [ 1..size ] |> List.map (fun i -> i, i) |> Map.ofList

    let ImmDictOfInts = mapOfInts |> ImmutableDictionary.CreateRange
    let FrozenDictOfInts = mapOfInts |> Dictionary |> fun x -> x.ToFrozenDictionary()
    let ReadOnlyDictOfInts = mapOfInts |> Dictionary |> fun x -> x.AsReadOnly()

    [<Benchmark(Description = "int - Map ofSeq")>]
    member _.IntMapCreate() =
         [ 1..size ] |> Seq.map (fun i -> i, i) |> Map.ofSeq
         
    [<Benchmark(Description = "int - ImmutableDictionary CreateRange")>]
    member _.IntImmDictCreate() =
         [ 1..size ] |> Seq.map (fun i -> KeyValuePair (i, i)) |> ImmutableDictionary.CreateRange

    [<Benchmark(Description = "int - ToFrozenDictionary")>]
    member _.IntFrozenDictCreate() =
         [ 1..size ] |> Seq.map (fun i -> KeyValuePair (i, i)) |> Dictionary |> fun x -> x.ToFrozenDictionary()

    [<Benchmark(Description = "int - Dict AsReadOnly")>]
    member _.IntReadOnlyDictCreate() =
         [ 1..size ] |> Seq.map (fun i -> KeyValuePair (i, i)) |> Dictionary |> fun x -> x.AsReadOnly()
    
    [<Benchmark(Description = "int - Map add")>]
    member _.IntMapCons() =
        let rec go i acc =
            if i = 0 then acc
            else go (i - 1) (Map.add i i acc)
        go size Map.empty

    [<Benchmark(Description = "int - ImmutableDictionary add")>]
    member _.IntImmDictCons() =
        let rec go i (acc: ImmutableDictionary<_,_>) =
            if i = 0 then acc
            else go (i - 1) (acc.Add(i, i))
        go size ImmutableDictionary.Empty
        
    [<Benchmark(Description = "int - Map find")>]
    member _.IntMapFind() =
        for i in 1..size do
            if mapOfInts.[i] <> i then
                failwith "fail"
            
    [<Benchmark(Description = "int - ImmutableDictionary find")>]
    member _.IntImmDictFind() =
        for i in 1..size do
            if ImmDictOfInts.[i] <> i then
                failwith "fail"

    [<Benchmark(Description = "int - FrozenDictionary find")>]
    member _.IntFrozenDictFind() =
        for i in 1..size do
            if FrozenDictOfInts.[i] <> i then
                failwith "fail"

    [<Benchmark(Description = "int - ReadOnlyDictionary find")>]
    member _.IntReadOnlyDictFind() =
        for i in 1..size do
            if ReadOnlyDictOfInts.[i] <> i then
                failwith "fail"

ListImmStackTests().IntListReduce() |> printfn "%A"
ListImmStackTests().IntImmStackReduce() |> printfn "%A"

//BenchmarkRunner.Run<ListImmListTests>()
//BenchmarkRunner.Run<ListImmStackTests>()
BenchmarkRunner.Run<MapImmDictTests>()
// BenchmarkRunner.Run<ListVsSet>()
// BenchmarkRunner.Run<MapVsConcestorDict>()

