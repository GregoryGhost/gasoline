namespace ModelService

module InspectorGadget =
    open System.Text.RegularExpressions
    open System.Globalization

    let minSymbol = 1
    let maxSymbol = 20

    let patternForName = 
        sprintf @"^(\w-){%d, %d}$" minSymbol maxSymbol
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
        | AboveTheMaximum of int
        | BellowTheMinumum of int
    type RequirementsForVehicle =
        | Name of Parameter list 
        | EnginePower of Parameter
        | TankCapacity of Parameter
        | Weight of Parameter
    ///<summary>
    ///Получает запись об транспортном средстве и проверяет ее поля на удовлетворение требованиям.
    ///Возвращает список ошибочных полей
    ///TODO: проверить работоспособность функции с помощью тестов
    ///TODO: вынести вспомогательные функции из замыкания функции check
    ///</summary>
    let test = Parameter.AboveTheMaximum(maxSymbol)
    
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
                let checkAboveMax = 
                    vehicle.name
                    |> String.length > maxSymbol
                let checkBellowMin =
                    vehicle.name
                    |> String.length < minSymbol
                
                if checkAboveMax then
                    names <- AboveTheMaximum(maxSymbol) :: names

                if checkBellowMin then
                    names <- BellowTheMinumum(minSymbol) :: names

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
                    [Unknown]
                    |> RequirementsForVehicle.Name
                    |> Some
                else
                    names
                    |> RequirementsForVehicle.Name
                    |> Some
        let checkEnginePower = 
                if vehicle.enginePower > maxEnginePower then
                    maxEnginePower
                    |> AboveTheMaximum
                    |> RequirementsForVehicle.EnginePower
                    |> Some
                elif vehicle.enginePower < minEnginePower then
                    minEnginePower
                    |> BellowTheMinumum
                    |> RequirementsForVehicle.EnginePower
                    |> Some
                else
                    None
        let checkTankCapacity = 
                if vehicle.tankCapacity > maxTankCapacity then
                    maxTankCapacity
                    |> AboveTheMaximum
                    |> RequirementsForVehicle.TankCapacity
                    |> Some
                elif vehicle.tankCapacity < minTankCapacity then
                    minTankCapacity
                    |> BellowTheMinumum
                    |> RequirementsForVehicle.TankCapacity
                    |> Some
                else 
                    None
        let checkWeight = 
                if vehicle.weight < minWeight then
                    int minWeight
                    |> BellowTheMinumum
                    |> RequirementsForVehicle.Weight
                    |> Some
                elif vehicle.weight > maxWeight then
                    int maxWeight
                    |> AboveTheMaximum
                    |> RequirementsForVehicle.Weight
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

    let validPorche = {
        name = "porche_911_GT-x";
        enginePower = 512; 
        weight = 500.; 
        resistanceWithMedian = Environment.Asphalt; 
        tankCapacity = 200 }

    check validPorche |> ignore