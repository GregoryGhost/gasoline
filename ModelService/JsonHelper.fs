namespace ModelService
open System.IO


module private Converter = 
 open FSharp.Data

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
 
 let toResistance = function
        | 1 -> Environment.Air
        | 2 -> Environment.Ground
        | 3 -> Environment.Asphalt
        | 4 -> Environment.Dirt
        | 5 -> Environment.Space
        | 6 -> Environment.RuggedTerrain
        | _ -> Environment.NANI

 let toVehicle (r : JRecords.Root) = 
        { Name = r.Name
          ;EnginePower = r.EnginePower
          ;Weight = r.Weight |> double
          ;ResistanceWithMedian = 
            r.ResistanceWithMedian 
            |> toResistance
          ;TankCapacity = r.TankCapacity }
 
 let toVehicles (jrecords : JRecords.Root[]) =
        let mutable vehicles : Vehicle list = []

        for r in jrecords do
            let v = r |> toVehicle
            vehicles <- v :: vehicles 

        vehicles

 let toJson (v : Vehicle) =
        JParameters.Root(v.Name
                         ,v.EnginePower
                         ,v.Weight |> decimal 
                         ,v.ResistanceWithMedian |> int 
                         ,v.TankCapacity)

 let toJsons (vehicles : Vehicle list) =
        vehicles
        |> List.map (fun vehicle ->
                        vehicle |> toJson)

 let writeJson (jparams : JParameters.Root list) (path : string) =
        use sw = new StreamWriter(path)

        sw.WriteLine "["

        for i = 0 to jparams.Length-1 do
            let mutable r = jparams.[i].ToString()

            if (i + 1 <= jparams.Length-1) then
                r <- r + ","
            r |> sw.WriteLine

        sw.WriteLine "]"
        sw.Close()


module internal JsonHelper =
 open Converter

 /// <summary>
 /// Десериализует JSON данные Vehicle из файла
 /// </summary>
 /// <param name="path">Путь к JSON файлу с 
 ///                    сериализованными данными Vehicle</param>
 /// <returns>Возвращает десериализованный список с записями Vehicle</returns>
 let readFromJson (path : string) : Vehicle list =
        let readFromFile = JRecords.Load(path)

        readFromFile
        |> toVehicles

 /// <summary>
 /// Сериализует записи Vehicle в JSON файл
 /// </summary>
 /// <param name="vehicles">Список записей Vehicle 
 ///                        для сериализации в JSON</param>
 /// <param name="path">Путь к JSON файлу для сериализации</param>
 let writeToJson (vehicles : Vehicle list) (path : string) : unit =
        if List.isEmpty vehicles then 
            ()
        else
            let jparams = vehicles |> toJsons

            writeJson jparams path