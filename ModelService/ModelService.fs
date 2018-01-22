namespace ModelService

open Demands

//TODO: описать тип возвращаемых некорректных ситуаций AddVehicle, UpdateVehicle
type AutoShow private () =
    let mutable vehicles : Vehicle list = []

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
    /// <returns>Возвращает последовательность записей об транспортных средствах</returns>
    member this.GetAllVehicles =
        vehicles
        |> List.toSeq

    member private this.existsElem vehicle = 
        vehicles
        |> List.tryFind (fun x -> x = vehicle)
        |> Option.isSome

    /// <summary>
    /// Добавить запись об транспортном средстве в базу
    /// </summary>
    /// <param name="vehicle">Запись об характеристиках транспортного средства</param>
    /// <returns>Возвращает последовательность ошибочных полей в записе</returns>
    member this.AddVehicle (vehicle : Vehicle) =
        let errors = vehicle |> InspectorGadget.validate
        if Seq.isEmpty errors then
            if this.existsElem vehicle then
                let errorExistsElem = [RequirementsForVehicle.ExistsElem] |> List.toSeq
                errorExistsElem
            else
                vehicles <- vehicle :: vehicles
                Seq.empty
        else
            errors
    
    /// <summary>
    /// Обновляет данные о записе транспортного средства в базе данных
    /// </summary>
    /// <param name="vehicle">Обновляемая запись транспортного средства</param>
    /// <param name="replaceVehicle">Новая запись транспортного средства</param>
    /// <returns>Возвращает последовательность ошибочных полей в новой записе.</returns>
    member this.UpdateVehicle (vehicle : Vehicle, replaceVehicle : Vehicle) =
        let isValidVehicle = 
            replaceVehicle
            |> InspectorGadget.validate
        if Seq.isEmpty isValidVehicle then
            let updateClone =
                vehicles
                |> List.map (fun x -> if x = vehicle then replaceVehicle else x)
            vehicles <- updateClone
            Seq.empty
        else
            isValidVehicle

    /// <summary>
    /// Удаляет запись об транспортном средстве из базы данных
    /// </summary>
    /// <param name="vehicle">Удаляемая запись об транспортном средстве</param>
    member this.RemoveVehicle (vehicle : Vehicle) =
        let withoutVehicle = vehicles |> List.filter (fun x -> x <> vehicle)
        vehicles <- withoutVehicle
        ()

    /// <summary>
    /// Удаляет все записи об транспортных средствах из базы данных
    /// </summary>
    member this.ClearAllVehicle () =
        vehicles <- []
        ()
    
    /// <summary>
    /// Сохраняет записи о транспортных средствах из базы данных в указанный файл
    /// </summary>
    /// <param name="path">Путь файла для сохранения</param>
    member this.Save (path : string) =  
        JsonHelper.writeToJson vehicles path

    /// <summary>
    /// Загружает записи о транспортных средствах в базу данных
    /// </summary>
    /// <param name="path">Путь до файла с JSON записями о транспортных средствах</param>
    member this.Load (path : string) =
        vehicles <- JsonHelper.readFromJson(path)
        ()