// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open unitTestsOnFuchu.Tests
open Fuchu.Test
open Fuchu.Tests

[<EntryPoint>]
let main argv =
    suite
    //|> filter (fun s -> s.StartsWith "Add vehicle to auto show")
    |> runParallel