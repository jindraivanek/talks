module ListBenchmarks

open System.Collections.Generic
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

type MyRecord = { Id: int; Name: string }

type DU =
    | A of int
    | B of string

let inline test xs f =
    for i in xs do
        f i xs |> ignore
        
[<MemoryDiagnoser>]
type ListListTests() =
    let size = 10000
    let listOfInts = [ 1..size ]
    let listOfRecords =
        listOfInts |> List.map (fun i -> { Id = i; Name = i.ToString() })
    
    [<Benchmark(Description = "F# List workload")>]
    member _.ListWorkload() =
        listOfRecords
        |> List.map (fun x -> { x with Id = x.Id + 1})
        |> List.filter (fun x -> x.Id % 2 = 0)
        |> List.map (fun x -> x.Id)
        |> List.sum

    [<Benchmark(Description = "C# List Workload")>]
    member _.CsListWorkload() =
        let csList = List listOfRecords
        for i=0 to csList.Count - 1 do
            csList.[i] <- { csList.[i] with Id = csList.[i].Id + 1 }
        //csList.RemoveAll(fun x -> x.Id % 2 <> 0)
        let csList2 = List()
        for i=0 to csList.Count - 1 do
            if csList.[i].Id % 2 = 0 then
                csList2.Add(csList.[i])
        let mutable x = 0
        for i=0 to csList2.Count - 1 do
            x <- x + csList2.[i].Id
        //csList.Sum(fun x -> x.Id)

BenchmarkRunner.Run<ListListTests>()

