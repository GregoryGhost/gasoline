namespace ModelService

module InspectorGadget =
    open System.Text.RegularExpressions
    open System.Globalization

    let patternForName = @"^(\w-){1,20}$"
    let patternLetters = @"^([a-zA-Z])+$"
    let patternNumbers = @"^([0-9])+$"
    let patternHyphen =  @"^-+$"
    let patternUnderline = @"^_+$"

    let minEnginePower = 0
    let maxEnginePower = int (10.**9.)

    let minTankCapacity = 0
    let maxTankCapacity = 10000
    
    let minWeight = 1.
    let maxWeight = 10000.

    type Parameter =
        | Letters
        | Numbers
        | Hyphen //дефис
        | Empty
        | Unknown
        | Underline
        | AboveTheMaximum
        | BellowTheMinumum
    type RequirementsForVehicle =
        | Name of Parameter list //1ый параметр - Letters, 2 - Numbers, 3 - Hyphen, 4 - Underline
        | EnginePower of Parameter
        | TankCapacity of Parameter
        | Weight of Parameter
    ///<summary>
    ///Получает запись об транспортном средстве и проверяет ее поля на удовлетворение требованиям.
    ///Возвращает список ошибочных полей
    ///TODO: проверить работоспособность функции с помощью тестов
    ///TODO: вынести вспомогательные функции из замыкания функции check
    ///</summary>
    let check (vehicle : Vehicle) =
        let checkName = 
            let isOk = 
                (vehicle.name, patternForName)
                |> Regex.IsMatch
            if isOk then None
            else
                let mutable names : Parameter list = []
                let checkEmpty =
                    vehicle.name
                    |> String.length = 0
                let checkLetters = 
                    (vehicle.name, patternLetters)
                    |> Regex.IsMatch
                let checkNumbers =
                    (vehicle.name, patternNumbers)
                    |> Regex.IsMatch
                let checkHyphen = 
                    (vehicle.name, patternHyphen)
                    |> Regex.IsMatch
                let checkUnderline =
                    (vehicle.name, patternUnderline)
                    |> Regex.IsMatch

                if checkEmpty then
                    names <- Empty :: names

                if checkLetters then 
                    names <- Letters :: names

                if checkNumbers then 
                    names <- Numbers :: names

                if checkHyphen then
                    names <- Hyphen :: names

                if checkUnderline then
                    names <- Underline :: names

                if (List.length names) = 0 then
                    RequirementsForVehicle.Name([Unknown]) |> Some
                else
                    RequirementsForVehicle.Name(names)
                    |> Some
        let checkEnginePower = 
                if vehicle.enginePower > maxEnginePower then
                    RequirementsForVehicle.EnginePower(AboveTheMaximum)
                    |> Some
                elif vehicle.enginePower < minEnginePower then
                    RequirementsForVehicle.EnginePower(BellowTheMinumum)
                    |> Some
                else
                    None
        let checkTankCapacity = 
                if vehicle.tankCapacity > maxTankCapacity then
                    RequirementsForVehicle.TankCapacity(AboveTheMaximum)
                    |> Some
                elif vehicle.tankCapacity < minTankCapacity then
                    RequirementsForVehicle.TankCapacity(BellowTheMinumum)
                    |> Some
                else 
                    None
        let checkWeight = 
                if vehicle.weight < minWeight then
                    RequirementsForVehicle.Weight(BellowTheMinumum)
                    |> Some
                elif vehicle.weight > maxWeight then
                    RequirementsForVehicle.Weight(AboveTheMaximum)
                    |> Some
                else    
                    None
         
        [checkName; checkEnginePower; checkTankCapacity; checkWeight]
        |> List.filter (fun x -> 
                            match x with
                            | Some(y) -> true
                            | None -> false)

module Test = 
    open InspectorGadget
    let v = new Vehicle()
    let validPorche = new Vehicle("porche_911_GT-x", 512, 500.0, Environment.Asphalt, 200)
    let invalidLamba = new Vehicle("lamba", -1, -1., Environment.Asphalt, -200)
    check v |> ignore
    check validPorche |> ignore
    check invalidLamba |> ignore 