namespace ModelService

open Demands
open System
open System.Runtime.CompilerServices


[<Extension>]
module ExtensionEnvironment =
    [<Extension>]
    let ReadFromString (x : System.String) =
            System.Enum.Parse(typeof<ModelService.Environment>, x)
            :?> ModelService.Environment

type Manager =
    /// <summary>
    /// Считает расход топлива для транспортного средства
    /// </summary>
    /// <returns>Возвращает строку с расходом топлива</returns>
    static member CalcFuelConsumption (vehicle : Vehicle) : string = 
            let isOK =  vehicle |> InspectorGadget.validate |> Seq.isEmpty
            if isOK then 
                vehicle.CalcFuelConsumption.ToString()
            else
                Environment.NANI.ToString()

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
    /// <exception cref="System.NullReferenceException">Параметр vehicle имеет значение null</exception>
    member this.AddVehicle (vehicle : Vehicle) : seq<RequirementsForVehicle> =
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
    /// <exception cref="System.NullReferenceException">Параметр vehicle или replaceVehicle имеет значение null</exception>
    member this.UpdateVehicle (vehicle : Vehicle, replaceVehicle : Vehicle) : seq<RequirementsForVehicle> =
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
    /// <exception cref="System.NullReferenceException">Параметр vehicle имеет значение null</exception>
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
    /// <exception cref="System.NullReferenceException">Параметр path имеет значение null</exception>
    /// <exception cref="System.ArgumentException">Параметр path является пустой строкой ("")</exception>
    member this.Save (path : string) =  
        JsonHelper.writeToJson vehicles path

    /// <summary>
    /// Загружает записи о транспортных средствах в базу данных
    /// </summary>
    /// <param name="path">Путь до файла с JSON записями о транспортных средствах</param>
    /// <exception cref="System.NullReferenceException">Параметр path имеет значение null</exception>
    /// <exception cref="System.ArgumentException">Параметр path является пустой строкой ("")</exception>
    /// <exception cref="System.IO.FileNotFoundException">Файл по пути path не найден</exception>
    /// <exception cref="System.Exception">Ошибка в разборе JSON</exception>
    member this.Load (path : string) =
        vehicles <- JsonHelper.readFromJson(path)
        ()