module ListBenchmarks

open System
open System.Linq
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
    [<Params(100, 1000, 10000, 100000)>]
    member val size = 100000 with get, set
    member this.listOfRecords =
        [ 1..this.size ] |> List.map (fun i -> { Id = i; Name = i.ToString() })
    member this.csList = Enumerable.Range(0, this.size).Select(fun i -> { Id = i; Name = i.ToString() }).ToList()
    
    [<Benchmark(Description = "F# List workload")>]
    member this.ListWorkload() =
        this.listOfRecords
        |> List.map (fun x -> { x with Id = x.Id + 1})
        |> List.filter (fun x -> x.Id % 2 = 0)
        |> List.map (fun x -> int64 x.Id)
        |> List.sum

    [<Benchmark(Baseline = true, Description = "C# List Workload")>]
    member this.CsListWorkload() =
        let csList = this.csList
        for i=0 to csList.Count - 1 do
            csList.[i] <-
              { csList.[i] with Id = csList.[i].Id + 1 }
        csList.RemoveAll(fun x -> x.Id % 2 <> 0)
        let x = csList.Sum(fun x -> int64 x.Id)
        x

let run() =
  let X = ListListTests()
  //for _ = 1 to 100 do X.CsListWorkload()
  assert (X.ListWorkload() = X.CsListWorkload())

BenchmarkRunner.Run<ListListTests>()


