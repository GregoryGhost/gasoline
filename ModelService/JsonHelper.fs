namespace ModelService

open System.Xml

module JsonHelper =
 open FSharp.Data
 open FSharp.Data.JsonExtensions

 type JParameters = JsonProvider<"""{
       "name" : "mazda rx-7",
       "enginePower" : 10,
       "weight" : 10.2,
       "resistanceWithMedian" : 1,
       "tankCapacity": 10
       }""">

 type JRecords = JsonProvider<"""[{
       "name" : "mazda rx-7",
       "enginePower" : 10,
       "weight" : 10.2,
       "resistanceWithMedian" : 1,
       "tankCapacity": 10
       }]""">
 
 let s = "test.json"
//TODO: сделать нормальную сериализацию списка Vehicle в JSON
 //let writeJSON v
 //Серилизация данных JSON в файл
 let writeJson =
     let t = JParameters.GetSample().ToString()
     let write v  =
         let sw = new System.IO.StreamWriter(s)
         let w =
             sw.WriteLine "["
             for i=1 to 10 do
                 t+"," |> sw.WriteLine
             t |> sw.WriteLine
             sw.WriteLine "]"
         w
         sw.Close()
     write
 
 type writeJson = JParameters.Root list-> unit
 type readJson = unit -> Vehicle list

 type toJson = Vehicle -> JParameters.Root
 type toJsons = Vehicle list -> JRecords.Root

 type toVehicles = JRecords.Root -> Vehicle list
 type toVehicle = JParameters.Root -> Vehicle

  (*| Air = 1
    | Ground = 2
    | Asphalt = 3
    | Dirt = 4 //грязь
    | Space = 5
    | RuggedTerrain = 6//пересеченная местность*)
  let toResistance = function
    | 1 -> Air
    | 2 -> Ground
    | 3 -> Asphalt
    | 4 -> Dirt
    | 5 -> Space
    | 6 -> RuggedTerrain

 let toVehicle : toVehicle =
    fun r ->
        let v = {
                name = r.Name;
                enginePower = r.EnginePower;
                weight = double r.Weight;
                resistanceWithMedian = r.ResistanceWithMedian 
                      |> toResistance;
                tankCapacity = r.TankCapacity
        }
        v

 let toVehicles : toVehicles =
    let mutable vehicles : Vehicle list = []
    fun jrecords -> 
        for r in jrecords do
           r |> toVehicle
           vehicles <- v :: vehicles 
        vehicles

 let writeJson : writeJson = 
    fun jparams ->
        let sw = new System.IO.StreamWriter(s)
        jparams
        |> List.iter (fun x -> 
                        x.Root.ToString() 
                        |> sw.WriteLine)
 
 let readJson : readJson = 
    let readFromFile = JRecords.Load(s)
    readFromFile
    |> toVehicles

//Десерилизация данных из JSON файла
 let readJson =
     let testP = JRecords.Load(s)
     let print =
         for v in testP do
             printfn "%d" v.EnginePower
     print
