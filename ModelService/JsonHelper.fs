namespace ModelService

module private Converter = 
 open System.IO
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
 

 //NOTE: пример вывода json записей типа JRecords
 //TODO: перенести функцию test к тестам на Fuchu
 let test =
    let json = """[{
       "name" : "mazda rx-7",
       "enginePower" : 10,
       "weight" : 10.2,
       "resistanceWithMedian" : 1,
       "tankCapacity": 10
       },
       {
       "name" : "mazda rx-7",
       "enginePower" : 10,
       "weight" : 1234.222,
       "resistanceWithMedian" : 3,
       "tankCapacity": 102
       },
       {
       "name" : "mazda rx-8",
       "enginePower" : 102,
       "weight" : 10.21234,
       "resistanceWithMedian" : 6,
       "tankCapacity": 1234
       }]"""
    let jtest = JRecords.Parse(json)
    for v in jtest do 
        v.JsonValue.ToString()
        |> printfn "ep=%s" 
 
 let toResistance = function
        | 1 -> Environment.Air
        | 2 -> Environment.Ground
        | 3 -> Environment.Asphalt
        | 4 -> Environment.Dirt
        | 5 -> Environment.Space
        | 6 -> Environment.RuggedTerrain
        | _ -> Environment.NANI

 let toVehicle (r : JRecords.Root) = 
        let v = {
                name = r.Name;
                enginePower = r.EnginePower;
                weight = double r.Weight;
                resistanceWithMedian = r.ResistanceWithMedian 
                      |> toResistance;
                tankCapacity = r.TankCapacity
        }
        v
 
 let toVehicles (jrecords : JRecords.Root[]) =
        let mutable vehicles : Vehicle list = []
        for r in jrecords do
            let v = 
                r 
                |> toVehicle
            vehicles <- v :: vehicles 
        vehicles

 let toJson (v : Vehicle) =
        JParameters.Root(v.name,
                         v.enginePower, 
                         decimal v.weight, 
                         int v.resistanceWithMedian, 
                         v.tankCapacity)

 let toJsons (vehicles : Vehicle list) =
        vehicles
        |> List.map (fun vehicle ->
                        vehicle |> toJson)

//TODO: сделать обработку исключительных ситуаций для StreamWriter
 let writeJson (jparams : JParameters.Root list) (path : string) =
        let sw = new StreamWriter(path)
        sw.WriteLine "["
        jparams
        |> List.iter (fun r -> 
                        r.JsonValue.ToString() + ","
                        |> sw.WriteLine)
        sw.WriteLine "]"
        sw.Close()

module JsonHelper =
 open Converter

 /// <summary>
 /// Десериализует JSON данные Vehicle из файла
 /// </summary>
 /// <param name="path">Путь к JSON файлу с сериализованными данными Vehicle</param>
 /// <returns>Возвращает десериализованный список с записями Vehicle</returns>
 let readFromJson (path : string) : Vehicle list =
        if !(File.Exists path) then []
        else
            let readFromFile = JRecords.Load(path)
            readFromFile
            |> toVehicles

 /// <summary>
 /// Сериализует записи Vehicle в JSON файл
 /// </summary>
 /// <param name="vehicles">Список записей Vehicle для сериализации в JSON</param>
 /// <param name="path">Путь к JSON файлу для сериализации</param>
 /// <returns></returns>
 let writeToJson (vehicles : Vehicle list) ( path : string) : unit =
        let jparams = 
            vehicles
            |> toJsons
        writeJson jparams path