module benchmarks.Concestor

open System.Collections.Generic
open BenchmarkDotNet.Attributes

type Operation =
    | Add
    | Remove
    | Update

type Delta<'k,'v,'t> =
    | Delta of Operation * 'k * 'v * 'v * Concestor<'k,'v,'t>
    | Data of 't

and Concestor<'k,'v,'t> = {
    mutable Ancestry : Delta<'k, 'v, 't>
    LockObj: obj
}

let create d = { Ancestry = Data d; LockObj = obj() }

type Conc<'t>(emptyF, addF, removeF, updateF, getF) =
    let apply c d op k v nv =
        match op with
        | Add ->
            addF k v d
            Delta (Remove, k, v, Unchecked.defaultof<_>, c)
        | Remove ->
            removeF k d
            Delta (Add, k, v, Unchecked.defaultof<_>, c)
        | Update ->
            updateF k nv d
            Delta (Update, k, nv, v, c)

    let rec ossifyCPS concestor k =
      match concestor.Ancestry with
      | Data s -> k s
      | Delta(op, key, value, newValue, relative) ->
          ossifyCPS relative (fun (d: 't) ->
            relative.Ancestry <-
              apply concestor d op key value newValue
            // OPTIM: We only need to set this once.
            concestor.Ancestry <- Data d
            k d)

    let ossify concestor = ossifyCPS concestor id

    let locked concestor f =
        lock concestor.LockObj (fun () -> f(ossify concestor))

    member __.empty = create <| emptyF()

    member __.add key value concestor =
        locked concestor (fun d ->
            let curr = { concestor with Ancestry = Data d }
            let ancestry =
              addF key value d
              Delta(Remove, key, value, Unchecked.defaultof<_>, curr)
            concestor.Ancestry <- ancestry
            curr)

    member __.remove key concestor =
        locked concestor (fun d ->
            let curr = { concestor with Ancestry = Data d }
            let ancestry =
              let value = getF key d
              removeF key d
              Delta(Add, key, value, Unchecked.defaultof<_>, curr)
            concestor.Ancestry <- ancestry
            curr)

    member __.update key newValue concestor =
        locked concestor (fun d ->
            let curr = { concestor with Ancestry = Data d }
            let ancestry =
              let value = getF key d
              updateF key newValue d
              Delta(Update, key, newValue, value, curr)
            concestor.Ancestry <- ancestry
            curr)

    member _.get concestor =
        locked concestor id

let mapC: Conc<Dictionary<string,int>> = Conc<Dictionary<_,_>>(
    (fun () -> Dictionary<_,_>()),
    (fun k v d -> d.Add(k, v)),
    (fun k d -> d.Remove(k) |> ignore),
    (fun k v d -> d[k] <- v),
    (fun k d -> d[k]))

type MapVsConcestorDict() =
    let listOfInts size = [ 1..size ]
    
    [<Params(64, 128, 256, 512, 1024, 8192)>]
    member val Size = 0 with get, set

    member x.MapOfInts = lazy (listOfInts x.Size |> List.map (fun x -> x.ToString(), x) |> Map.ofList)
    member x.ConcestorDictOfInts = lazy (listOfInts x.Size |> List.map (fun i -> i.ToString(), i) |> Seq.fold (fun d (k, v) -> mapC.add k v d) mapC.empty)

    [<Benchmark>]
    member x.MapFind() =
        for k in Map.keys x.MapOfInts.Value do
            x.MapOfInts.Value.[k] |> ignore

    [<Benchmark>]
    member x.ConcestorDictFind() =
        for k in Map.keys x.MapOfInts.Value do
            (mapC.get x.ConcestorDictOfInts.Value).[k] |> ignore

    // [<Benchmark>]
    // member x.MapAdd() =
    //     test x.MapOfInts (fun i xs -> Map.add (size+i) (size+i) xs)

    // [<Benchmark>]
    // member x.ConcestorDictAdd() =
    //     test x.ConcestorDictOfInts (fun i xs -> mapC.add (size+i) (size+i) xs)

    // [<Benchmark>]
    // member x.MapRemove() =
    //     test x.MapOfInts (fun i xs -> Map.remove i xs)

    // [<Benchmark>]
    // member x.ConcestorDictRemove() =
    //     test x.ConcestorDictOfInts (fun i xs -> mapC.remove i xs)

    // [<Benchmark>]
    // member x.MapUpdate() =
    //     test x.MapOfInts (fun i xs -> Map.add i (size+i) xs)

    // [<Benchmark>]
    // member x.ConcestorDictUpdate() =
    //     test x.ConcestorDictOfInts (fun i xs -> mapC.update i (size+i) xs)

    // [<Benchmark>]
    // member x.MapTryFind() =
    //     test x.MapOfInts (fun i xs -> Map.tryFind i xs)

    // [<Benchmark>]
    // member x.ConcestorDictTryFind() =
    //     test x.ConcestorDictOfInts (fun i xs -> mapC.get xs |> Map.tryFind i)

    // [<Benchmark>]
    // member x.MapTryFindOpt() =
    //     test x.MapOfInts (fun i xs -> Map.tryFindOpt i xs)

    // [<Benchmark>]
    // member x.ConcestorDictTryFindOpt() =
    //     test x.ConcestorDictOfInts (fun i xs -> mapC.get xs |> Map.tryFindOpt i)
