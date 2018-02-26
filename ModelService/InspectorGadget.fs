namespace ModelService


module Demands =
    type Parameter =
        | Empty
        | InvalidCharacter
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

    let minSymbol = 5
    let maxSymbol = 20

open Demands

module internal InspectorGadget =
    open System.Text.RegularExpressions

    let patternForName = 
       sprintf @"^([\w\-_]){%d,%d}$" minSymbol maxSymbol

    let checkEmpty     vehicle = 
        vehicle.name 
        |> String.length = 0

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

                if List.isEmpty names then
                    [InvalidCharacter]
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

open InspectorGadget

type Gibdd() =
    let vehicle = { name = "Ferrari 458 Special";
                    enginePower = 605;
                    weight = 1480.0; 
                    resistanceWithMedian = Environment.Asphalt;
                    tankCapacity = 4497}

    let toString y =
        let y' acc x =
            let y'' =
                match x with
                | Demands.Parameter.Empty -> "Заполните поле"
                | Demands.Parameter.AboveTheMaximum z -> "Превышена максимально-допустимая длина, равная " + z.ToString()
                | Demands.Parameter.BelowTheMinimum z -> "Должно быть минимум символов - " + z.ToString()
                | Demands.Parameter.InvalidCharacter -> @"Допустимые символы - буквы английского алфавита, цифры, знаки ""-"", ""_"""
            acc + y''
        List.fold y' "" y

    let concat x =
        match x with 
        | name -> x |> toString |> Some

    let choiceRequire x =
        match x with
        | Name y -> y |> concat
        | Weight y -> Some "Weight Test"
        | EnginePower y -> Some "Engine Power Test"
        | TankCapacity y -> Some "Tank Capacity Test"
        | _ -> Some "Unknown Error 1111"

    let unpackValue value = 
        match value with
        | Some x -> x
        | None -> System.String.Empty

    let check value =                         
        value
        |> checkName 
        |> Option.bind choiceRequire
        |> unpackValue
    
    /// <summary>
    /// Проверяет название транспортного средства на соответствие заданным требованиям
    /// </summary>
    /// <param name="name">Название транспортного средства</param>
    /// <returns>Возвращает текст, содержащий в себе каким 
    ///  требованиям должно удовлетворять название транспортного средства</returns>
    member this.CheckName(name : string) : string =
        let test = {vehicle with name = name}
        check test

    member this.CheckEnginePower(enginePower : int) =
        {vehicle with enginePower = enginePower }
        |> InspectorGadget.checkEnginePower
    
    member this.CheckWeight(weight : double) =
        {vehicle with weight = weight }
        |> InspectorGadget.checkWeight
    
    member this.CheckTankCapacity(tankCapacity : int) =
        {vehicle with tankCapacity = tankCapacity }
        |> InspectorGadget.checkTankCapacity