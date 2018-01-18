namespace ModelService
//TODO: описать тип возвращаемых некорректных ситуаций AddVehicle, UpdateVehicle
type AutoShow private () =
    let mutable vehicles : Vehicle list = []
    //let mutable _dataBase = 
    //let _validation = new InspectorGadget()

    static member val Instance = 
        AutoShow() 

    ///<summary>
    ///Берет координаты А и Б -> получаем путь от А до Б
    ///</summary>
    //abstract member BuildTrafficRoute : ((int * int) * (int * int)) -> (int * int) list

    member this.GetAllVehicles =
        vehicles

    member this.AddVehicle (vehicle : Vehicle) =
        //vehicle.id <- List.length vehicles
        //checking data for pattern
        vehicles <- vehicle :: vehicles
        vehicle

    member this.UpdateVehicle (vehicle) =
        let vehiclesClone = vehicle :: vehicles
        ()

    member this.RemoveVehicle (vehicle) =
        let vehiclesClone = vehicle :: vehicles
        ()

    member this.ClearAllVehicle () =
        ()
    
    member this.Save (path : string) = JsonHelper.writeToJson vehicles path

    member this.Load (path : string) =
        vehicles <- JsonHelper.readFromJson(path)
        ()