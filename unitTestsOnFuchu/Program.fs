// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open unitTestsOnFuchu.Tests

[<EntryPoint>]
let main argv =
    suite
    |> Fuchu.Tests.runParallel