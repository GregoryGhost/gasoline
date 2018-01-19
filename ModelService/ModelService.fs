namespace ModelService
//TODO: описать тип возвращаемых некорректных ситуаций AddVehicle, UpdateVehicle
type AutoShow private () =
    let mutable vehicles : Vehicle list = []
    //let mutable _dataBase = 
    //let _validation = new InspectorGadget()

    /// <summary>
    /// Возвращает инициализированный объект типа AutoShow
    /// </summary>
    static member val Instance = 
        AutoShow() 

    ///<summary>
    ///Берет координаты А и Б -> получаем путь от А до Б
    ///</summary>
    //abstract member BuildTrafficRoute : ((int * int) * (int * int)) -> (int * int) list

    /// <summary>
    /// Получить все записи об транспортных средствах
    /// </summary>
    /// <returns>Возвращает массив записей об транспортных средствах</returns>
    member this.GetAllVehicles =
        vehicles
        |> List.toArray

    /// <summary>
    /// Добавить запись об транспортном средстве в базу
    /// </summary>
    /// <param name="vehicle">Запись об характеристиках транспортного средства</param>
    /// <returns>Возвращает массив ошибочных полей в записе.</returns>
    member this.AddVehicle (vehicle : Vehicle) =
        //vehicle.id <- List.length vehicles
        //checking data for pattern
        vehicles <- vehicle :: vehicles
        vehicle
    
    /// <summary>
    /// Обновляет данные о записе транспортного средства в базе данных
    /// </summary>
    /// <param name="vehicle">Обновляемая запись транспортного средства</param>
    /// <returns>Возвращает массив ошибочных полей в записе.</returns>
    member this.UpdateVehicle (vehicle) =
        let vehiclesClone = vehicle :: vehicles
        ()

    /// <summary>
    /// Удаляет запись об транспортном средстве из базы данных
    /// </summary>
    /// <param name="vehicle">Удаляемая запись об транспортном средстве</param>
    /// <returns>Возвращает успешность удаления записи из базы данных</returns>
    member this.RemoveVehicle (vehicle) =
        let vehiclesClone = vehicle :: vehicles
        ()

    /// <summary>
    /// Удаляет все записи об транспортных средствах из базы данных
    /// </summary>
    /// <returns>Возвращает успешность удаления записей из базы данных</returns>
    member this.ClearAllVehicle () =
        ()
    
    /// <summary>
    /// Сохраняет записи о транспортных средствах из базы данных в указанный файл
    /// </summary>
    /// <param name="path">Путь файла для сохранения</param>
    member this.Save (path : string) = JsonHelper.writeToJson vehicles path

    /// <summary>
    /// Загружает записи о транспортных средствах в базу данных
    /// </summary>
    /// <param name="path">Путь до файла с JSON записями о транспортных средствах</param>
    member this.Load (path : string) =
        vehicles <- JsonHelper.readFromJson(path)
        ()