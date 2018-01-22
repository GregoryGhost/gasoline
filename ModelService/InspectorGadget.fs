namespace ModelService

module Demands =
    type Parameter =
        | Letters
        | Numbers
        | Hyphen //дефис
        | Empty
        | Unknown
        | Underline
        | AboveTheMaximum of int
        | BelowTheMinimum of int
    type RequirementsForVehicle =
        | Name of Parameter list 
        | EnginePower of Parameter
        | TankCapacity of Parameter
        | Weight of Parameter
        | ExistsElem
        
    let minEnginePower = 0
    let maxEnginePower = int (10.**9.)

    let minTankCapacity = 0
    let maxTankCapacity = 10000
    
    let minWeight = 1.
    let maxWeight = 10000.

    let minSymbol = 1
    let maxSymbol = 20

module internal InspectorGadget =
    open System.Text.RegularExpressions
    open System.Globalization
    open Demands

    let patternForName = 
        sprintf @"^(\w-){%d, %d}$" minSymbol maxSymbol
    let patternLetters = @"^([a-zA-Z])+$"
    let patternNumbers = @"^([0-9])+$"
    let patternHyphen =  @"^-+$"
    let patternUnderline = @"^_+$"

    let checkEmpty     vehicle = 
        vehicle.name 
        |> String.length = 0

    let checkLetters   vehicle = Regex.IsMatch(vehicle.name, patternLetters)

    let checkNumbers   vehicle = Regex.IsMatch(vehicle.name, patternNumbers)

    let checkHyphen    vehicle = Regex.IsMatch(vehicle.name, patternHyphen)

    let checkUnderline vehicle = Regex.IsMatch(vehicle.name, patternUnderline)

    let checkAboveMax  vehicle = 
        vehicle.name 
        |> String.length > maxSymbol

    let checkBellowMin vehicle = 
        vehicle.name 
        |> String.length < minSymbol

    let checkName vehicle = 
        let isOk = Regex.IsMatch(vehicle.name, patternForName)

        if isOk then 
            None
        else               
            if checkEmpty vehicle then
                [Empty]
                |> RequirementsForVehicle.Name
                |> Some
            else 
                let mutable names : Parameter list = []

                if checkAboveMax vehicle then
                    names <- AboveTheMaximum(maxSymbol) :: names

                if checkBellowMin vehicle then
                    names <- BelowTheMinimum(minSymbol) :: names

                if checkLetters vehicle then 
                    names <- Letters :: names

                if checkNumbers vehicle then 
                    names <- Numbers :: names

                if checkHyphen vehicle then
                    names <- Hyphen :: names

                if checkUnderline vehicle then
                    names <- Underline :: names

                if (List.length names) = 0 then
                    [Unknown]
                    |> RequirementsForVehicle.Name
                    |> Some
                else
                    names
                    |> RequirementsForVehicle.Name
                    |> Some

    let checkEnginePower vehicle = 
        if vehicle.enginePower > maxEnginePower then
            maxEnginePower
            |> AboveTheMaximum
            |> RequirementsForVehicle.EnginePower
            |> Some
        elif vehicle.enginePower < minEnginePower then
            minEnginePower
            |> BelowTheMinimum
            |> RequirementsForVehicle.EnginePower
            |> Some
        else
            None
    
    let checkTankCapacity vehicle = 
        if vehicle.tankCapacity > maxTankCapacity then
            maxTankCapacity
            |> AboveTheMaximum
            |> RequirementsForVehicle.TankCapacity
            |> Some
        elif vehicle.tankCapacity < minTankCapacity then
            minTankCapacity
            |> BelowTheMinimum
            |> RequirementsForVehicle.TankCapacity
            |> Some
        else 
            None

    let checkWeight vehicle = 
        if vehicle.weight < minWeight then
            int minWeight
            |> BelowTheMinimum
            |> RequirementsForVehicle.Weight
            |> Some
        elif vehicle.weight > maxWeight then
            int maxWeight
            |> AboveTheMaximum
            |> RequirementsForVehicle.Weight
            |> Some
        else    
            None

    ///<summary>
    ///Получает запись об транспортном средстве и проверяет ее поля на удовлетворение требованиям.
    ///</summary>
    ///<returns>Возвращает список ошибочных полей</returns>
    let validate (vehicle : Vehicle) : seq<RequirementsForVehicle> =
        [checkName         vehicle; 
         checkEnginePower  vehicle; 
         checkTankCapacity vehicle; 
         checkWeight       vehicle]
        |> List.filter (function 
                        | Some x -> true 
                        | None   -> false) 
        |> List.map (Option.get)
        |> List.toSeq