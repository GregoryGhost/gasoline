﻿namespace ModelService


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
    let maxEnginePower = int (10.**8.)
    
    let minTankCapacity = 0
    let maxTankCapacity = 10000
    
    let minWeight = 1.
    let maxWeight = 10000.

    let minSymbol = 5
    let maxSymbol = 20


module internal InspectorGadget =
    open Demands
    open System.Text.RegularExpressions

    let patternForName = 
       sprintf @"^([\w\-_]){%d,%d}$" minSymbol maxSymbol

    let checkEmpty     vehicle = 
        vehicle.Name 
        |> String.length = 0

    let checkAboveMax  vehicle = 
        vehicle.Name 
        |> String.length > maxSymbol

    let checkBellowMin vehicle = 
        vehicle.Name 
        |> String.length < minSymbol

    let checkName vehicle = 
        let isOk = Regex.IsMatch(vehicle.Name, patternForName)

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
        if vehicle.EnginePower > maxEnginePower then
            maxEnginePower
            |> AboveTheMaximum
            |> RequirementsForVehicle.EnginePower
            |> Some
        elif vehicle.EnginePower < minEnginePower then
            minEnginePower
            |> BelowTheMinimum
            |> RequirementsForVehicle.EnginePower
            |> Some
        else
            None
    
    let checkTankCapacity vehicle = 
        if vehicle.TankCapacity > maxTankCapacity then
            maxTankCapacity
            |> AboveTheMaximum
            |> RequirementsForVehicle.TankCapacity
            |> Some
        elif vehicle.TankCapacity < minTankCapacity then
            minTankCapacity
            |> BelowTheMinimum
            |> RequirementsForVehicle.TankCapacity
            |> Some
        else 
            None

    let checkWeight vehicle = 
        if vehicle.Weight < minWeight then
            int minWeight
            |> BelowTheMinimum
            |> RequirementsForVehicle.Weight
            |> Some
        elif vehicle.Weight > maxWeight then
            int maxWeight
            |> AboveTheMaximum
            |> RequirementsForVehicle.Weight
            |> Some
        else    
            None

    ///<summary>
    ///Получает запись об транспортном средстве и 
    ///     проверяет ее поля на удовлетворение требованиям.
    ///</summary>
    ///<returns>Возвращает последовательность ошибочных полей</returns>
    let validate (vehicle : Vehicle) : seq<RequirementsForVehicle> =
        [checkName         vehicle; 
         checkEnginePower  vehicle; 
         checkTankCapacity vehicle; 
         checkWeight       vehicle]
        |> List.filter (Option.isSome) 
        |> List.map (Option.get)
        |> List.toSeq


module internal ConverterParameters =
    open Demands

    let nameToString x = 
        match x with
        | Demands.Parameter.Empty -> "Заполните поле"
        | Demands.Parameter.AboveTheMaximum z -> 
            "Превышена максимально-допустимая длина, равная " + z.ToString()
        | Demands.Parameter.BelowTheMinimum z -> 
            "Должно быть минимум символов - " + z.ToString()
        | Demands.Parameter.InvalidCharacter -> 
            @"Допустимые символы - буквы английского алфавита,\
                цифры, знаки ""-"", ""_"""

    let concat x = 
         x 
         |> List.fold (fun acc x -> acc + (nameToString x)) ""
         |> Some

    let toString x = 
        match x with
        | Demands.Parameter.Empty -> "Заполните поле"
        | Demands.Parameter.AboveTheMaximum z -> 
            "Превышено максимально-допустимое значение, равное " + z.ToString()
        | Demands.Parameter.BelowTheMinimum z -> 
            "Значение меньше минимально-допустимого значения, равного " + z.ToString()
        | Demands.Parameter.InvalidCharacter -> @"Допустимые символы - числа"

    let choiceRequire = function
        | Name y -> y |> concat
        | Weight y -> y |> toString |> Some
        | EnginePower y -> y |> toString |> Some
        | TankCapacity y -> y |> toString |> Some
        | _ -> Some "Unknown Error 1111"
    
    let unpackValue = function
        | Some x -> x
        | None -> System.String.Empty


open InspectorGadget
open Demands
open ConverterParameters

/// <summary>
/// Проверяет запись VehicleModel на соответствие требованиям
/// </summary>
type Gibdd() =

    /// <summary>
    /// Проверяет название транспортного средства 
    ///     на соответствие заданным требованиям
    /// </summary>
    /// <param name="name">Название транспортного средства</param>
    /// <returns>Возвращает текст, содержащий в себе каким 
    ///     требованиям должно удовлетворять 
    ///     название транспортного средства</returns>
    member this.CheckName(model : VehicleModel) : string =
        model.ToVehicle()
        |> checkName 
        |> Option.bind choiceRequire
        |> unpackValue
    
    /// <summary>
    /// Проверяет мощность двигателя транспортного средства 
    ///     на соответствие заданным требованиям
    /// </summary>
    /// <param name="name">Мощность двигателя транспортного средства</param>
    /// <returns>Возвращает текст, содержащий в себе каким 
    ///     требованиям должно удовлетворять 
    ///     мощность двигателя транспортного средства</returns>
    member this.CheckEnginePower(model: VehicleModel) =
        model.ToVehicle()
        |> checkEnginePower
        |> Option.bind choiceRequire
        |> unpackValue
    
    /// <summary>
    /// Проверяет вес транспортного средства 
    ///     на соответствие заданным требованиям
    /// </summary>
    /// <param name="name">Вес транспортного средства</param>
    /// <returns>Возвращает текст, содержащий в себе каким 
    ///     требованиям должно удовлетворять 
    ///     вес транспортного средства</returns>
    member this.CheckWeight(model: VehicleModel) =
        model.ToVehicle()
        |> checkWeight
        |> Option.bind choiceRequire
        |> unpackValue
    
    /// <summary>
    /// Проверяет объем бака транспортного средства
    ///     на соответствие заданным требованиям
    /// </summary>
    /// <param name="name">Объем бака транспортного средства</param>
    /// <returns>Возвращает текст, содержащий в себе каким 
    ///     требованиям должно удовлетворять 
    ///     объем бака транспортного средства</returns>
    member this.CheckTankCapacity(model: VehicleModel) =
        model.ToVehicle()
        |> checkTankCapacity
        |> Option.bind choiceRequire
        |> unpackValue


open System.Runtime.CompilerServices

[<Extension>]
module Converters =
    
    /// <summary>
    /// Конвертирует тип ошибки в текст с описанием ошибки
    /// </summary>
    /// <param name="x">Тип ошибки в требованиях 
    ///     к характеристике транспортного средства</param>
    [<Extension>]
    let ToText(x : RequirementsForVehicle) =
        x
        |> choiceRequire
        |> unpackValue


/// <summary>
/// Проверяет запись Vehicle на соответствие требованиям
/// </summary>
type Checker() =
   
    /// <summary>
    /// Проверяет название транспортного средства 
    ///     на соответствие заданным требованиям
    /// </summary>
    /// <param name="name">Название транспортного средства</param>
    /// <returns>Возвращает ошибки, которые показывают 
    ///     каким требованиям должно удовлетворять
    ///     название транспортного средства</returns>
    member this.CheckName(model : Vehicle) = 
        model
        |> checkName

    /// <summary>
    /// Проверяет мощность двигателя транспортного средства 
    ///     на соответствие заданным требованиям
    /// </summary>
    /// <param name="name">Мощность двигателя транспортного средства</param>
    /// <returns>Возвращает ошибки, которые показывают 
    ///     каким требованиям должна удовлетворять
    ///     мощность двигателя транспортного средства</returns>
    member this.CheckEnginePower(model : Vehicle) =
        model
        |> checkEnginePower

    /// <summary>
    /// Проверяет вес транспортного средства 
    ///     на соответствие заданным требованиям
    /// </summary>
    /// <param name="name">Вес транспортного средства</param>
    /// <returns>Возвращает ошибки, которые показывают 
    ///     каким требованиям должен удовлетворять
    ///     вес транспортного средства</returns>
    member this.CheckWeight(model : Vehicle) = 
        model
        |> checkWeight

    /// <summary>
    /// Проверяет объём бака транспортного средства 
    ///     на соответствие заданным требованиям
    /// </summary>
    /// <param name="name">Объём бака транспортного средства</param>
    /// <returns>Возвращает ошибки, которые показывают 
    ///     каким требованиям должен удовлетворять
    ///     объём бака транспортного средства</returns>
    member this.CheckTankCapacity(model : Vehicle) = 
        model
        |> checkTankCapacity