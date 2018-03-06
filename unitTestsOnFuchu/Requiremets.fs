namespace unitTestsOnFuchu

module Requirements =
    open ModelService.Demands
    open ModelService

    open Var


    type Data = 
        { Data : Vehicle
          Requirement : RequirementsForVehicle option }
    

    let minName = "test"
    let genName n = String.replicate n minName

    let forName param =
        param
        |> Name
        |> Some

    let errorInvalidCharacter = 
        [InvalidCharacter]
        |> forName
    
    let errorMaxSymbolName =
        [AboveTheMaximum <| maxSymbol]
        |> forName

    let errorMinSymbolName =
        [BelowTheMinimum <| minSymbol]
        |> forName

    let errorEmptyName =
        [Empty]
        |> forName

    
    let forEnginePower param = 
        param
        |> EnginePower
        |> Some

    let errorMinEnginePower =
        minEnginePower
        |> BelowTheMinimum
        |> forEnginePower

    let errorMaxEnginePower =
        maxEnginePower
        |> AboveTheMaximum
        |> forEnginePower

    let forWeight param = 
        param
        |> Weight
        |> Some

    let errorMinWeight =
        Demands.minWeight |> int
        |> BelowTheMinimum
        |> forWeight

    let errorMaxWeight =
        Demands.maxWeight |> int
        |> AboveTheMaximum
        |> forWeight

    let forTankCapacity param =
        param 
        |> TankCapacity
        |> Some

    let errorMinTankCapacity =
        Demands.minTankCapacity
        |> BelowTheMinimum
        |> forTankCapacity
    
    let errorMaxTankCapacity =
        Demands.maxTankCapacity
        |> AboveTheMaximum
        |> forTankCapacity

    let invalidChar = 
                { Data = {validVehicle 
                            with name = "mazda#22"}
                  Requirement = errorInvalidCharacter }

    let aboveMaxSymbol = 
                { Data = {validVehicle 
                            with name = maxSymbol |> genName}
                  Requirement = errorMaxSymbolName }
    
    let belowMinSymbol = 
                { Data = {validVehicle 
                            with name = minName}
                  Requirement = errorMinSymbolName }
    let emptyName = 
                { Data = {validVehicle 
                            with name = System.String.Empty}
                  Requirement = errorEmptyName }
    
    let belowMinEnginePower = 
                { Data = {validVehicle
                            with enginePower = minEnginePower - 10}
                  Requirement = errorMinEnginePower }

    let aboveMaxEnginePower = 
                { Data = {validVehicle
                            with enginePower = maxEnginePower + 10}
                  Requirement = errorMaxEnginePower }

    let belowMinWeight = 
                { Data = {validVehicle 
                            with weight = minWeight - 10.}
                  Requirement = errorMinWeight }

    let aboveMaxWeight = 
                { Data = {validVehicle 
                            with weight = maxWeight + 10.}
                  Requirement = errorMaxWeight }

    let belowMinTankCapacity = 
                { Data = {validVehicle 
                            with tankCapacity = minTankCapacity - 10}
                  Requirement = errorMinTankCapacity }

    let aboveMaxTankCapacity = 
                { Data = {validVehicle 
                            with tankCapacity = maxTankCapacity + 10}
                  Requirement = errorMaxTankCapacity }

    
module Converters = 
    open ModelService

    let toVehicleModel x =
        new VehicleModel(
            x.name
            ,x.enginePower
            ,x.weight
            ,x.resistanceWithMedian
            ,x.tankCapacity)