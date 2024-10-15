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

[<MemoryDiagnoser>]
type SetSetTests() =
    [<Params(100, 1000, 10000, 100000)>]
    member val size = 100000 with get, set
    member this.Collection = seq { 1..this.size } |> Seq.map (fun i -> { Id = i; Name = i.ToString() }) 
    
    [<Benchmark(Description = "F# Set workload")>]
    member this.SetWorkload() =
        let s =
          this.Collection |> set
          //|> Set.filter (fun x -> x.Id % 2 = 0)
        this.Collection |> Seq.iter (fun x -> Set.contains x s |> ignore)

    [<Benchmark(Baseline = true, Description = "C# Set Workload")>]
    member this.HashSetWorkload() =
        let s = this.Collection.ToHashSet()
        //s.RemoveWhere(fun x -> x.Id % 2 <> 0)
        for i in this.Collection do s.Contains(i)
        

let run() =
  let X = ListListTests()
  //for _ = 1 to 100 do X.CsListWorkload()
  assert (X.ListWorkload() = X.CsListWorkload())

//BenchmarkRunner.Run<ListListTests>()
BenchmarkRunner.Run<SetSetTests>()


