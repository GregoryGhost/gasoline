module JsonHelper
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

 type Environment = 
    | Air = 1
    | Ground = 2
    | Asphalt = 3
    | Dirt = 4 //грязь
    | Space = 5
    | RuggedTerrain = 6//пересеченная местность

 type Vehicle = {
                name : string;     //ограничение pattern от 1 до 20 символов - цифр или букв, включая - _
         enginePower : int;        //ограничения pattern  от 1 до 1 000 000 000 л.с.
              weight : double;     //ограничение pattern от 1 до 10 000 кг - double
resistanceWithMedian : Environment;//ограничения pattern - воздух, вода, космос, земля, асфальт, грязь, пересеченная_местность
        tankCapacity : int        }//ограничения pattern - от 0 до 10 000 л

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
//Десерилизация данных из JSON файла
 let readJson =
     let testP = JRecords.Load(s)
     let print =
         for v in testP do
             printfn "%d" v.EnginePower
     print