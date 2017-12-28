namespace ModelService
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
 
 type writeJson = JParameters list-> unit
 type readJson = unit -> JParameters list

 type toJson = Vehicle -> JParameters
 type toJsons = Vehicle list -> JRecords

 type toVehicles = JRecords -> Vehicle list
 type toVehicle = JParameters -> Vehicle
//Десерилизация данных из JSON файла
 let readJson =
     let testP = JRecords.Load(s)
     let print =
         for v in testP do
             printfn "%d" v.EnginePower
     print
