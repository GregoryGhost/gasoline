namespace ModelService

type Environment = 
    | Air = 1
    | Ground = 2
    | Asphalt = 3
    | Dirt = 4    //грязь
    | Space = 0
    | RuggedTerrain = 10 //пересеченная местность


type Vehicle () =
    (* тип транспортного средства для ООП-совместимости с dotNET *)
    (*  *)
    (*
    TODO: Определить интерфейс методов, что принимают, 
          что возвращают для вызова их из клиентского кода C#
    TODO: Написать в скрипте Script.fsx код, показывающий как использовать код модуля
          в пользовательском коде
    TODO: Подключить для выполнения операций над матрицами
          специальный тип из FSharp.Data - Matrix
    TODO: Реализовать/Десеарилизацию сериализацию данных об транспортных средствах в JSON + F#
    TODO: Покрыть только модуль на F# модульными тестами Fuchu и если нужно автотестами из FsCheck
    TODO: Добавить функцию на проверку работоспособности транспортного средства по заявленным характеристикам
    *)
    let mutable _id = 0
    //ограничение pattern от 5 до 20 символов - цифр или букв, включая - _
    let mutable _name = "mazda"
    //ограничение pattern от 1 до 1 000 000 000 л.с. - целочисленный
    let mutable _enginePower = 20
    //ограничение pattern от 1 до 10 000 кг - double
    let mutable _weight = 120
    //ограничения pattern - воздух, вода, космос, земля, асфальт, грязь, пересеченная_местность
    let mutable _resistanceWithMedian : Environment = Environment.Asphalt
    //ограничения pattern - от 0 до 10 000 л
    let mutable _tankCapacity = 12



    member val internal Id = _id with get, set

    member val Name = _name with get, set

    member this.EnginePower 
        with get() = _enginePower
        and  set(enginePower) = _enginePower<- enginePower

    member this.Weight 
        with get() = _weight
        and  set(weight) = _weight<- weight

    member this.ResistanceWithMedian
        with get() = _resistanceWithMedian
        and  set(resistanceWithMedian) = _resistanceWithMedian<- resistanceWithMedian 

    member this.TankCapacity 
        with get() = _tankCapacity
        and set(tankCapacity) = _tankCapacity<- tankCapacity


open FSharp.Data

type JParameters = JsonProvider<""" {
       "volumeOfTank":10.1,
       "massOfVehicle":10.1,
       "enginePower":14.4,
       "CCM":12.4 } """>

//TODO: описать тип возвращаемых некорректных ситуаций AddVehicle, UpdateVehicle
type AutoShow private () =
    let mutable vehicles : Vehicle list = []
   // let mutable _dataBase = 

    static member val Instance = AutoShow()

    ///<summary>
    ///Берет координаты А и Б -> получаем путь от А до Б
    ///</summary>
    //abstract member BuildTrafficRoute : ((int * int) * (int * int)) -> (int * int) list


    member this.GetAllVehicles =
        vehicles

    member this.AddVehicle (vehicle : Vehicle) =
        vehicle.Id <- List.length vehicles
        vehicles <- vehicle :: vehicles
        vehicle.Id

    member this.UpdateVehicle (vehicle) =
        let vehiclesClone = vehicle :: vehicles
        ()

    member this.RemoveVehicle (vehicle) =
        let vehiclesClone = vehicle :: vehicles
        ()

    member this.GetVehicleById (id) =
        vehicles
        |> List.find (fun vehicle -> vehicle.Id = id)

    member this.ClearAllVehicle () =
        ()
