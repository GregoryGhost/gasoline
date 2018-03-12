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
                            with Name = "mazda#22"}
                  Requirement = errorInvalidCharacter }

    let aboveMaxSymbol = 
                { Data = {validVehicle 
                            with Name = maxSymbol |> genName}
                  Requirement = errorMaxSymbolName }
    
    let belowMinSymbol = 
                { Data = {validVehicle 
                            with Name = minName}
                  Requirement = errorMinSymbolName }
    let emptyName = 
                { Data = {validVehicle 
                            with Name = System.String.Empty}
                  Requirement = errorEmptyName }
    
    let belowMinEnginePower = 
                { Data = {validVehicle
                            with EnginePower = minEnginePower - 10}
                  Requirement = errorMinEnginePower }

    let aboveMaxEnginePower = 
                { Data = {validVehicle
                            with EnginePower = maxEnginePower + 10}
                  Requirement = errorMaxEnginePower }

    let belowMinWeight = 
                { Data = {validVehicle 
                            with Weight = minWeight - 10.}
                  Requirement = errorMinWeight }

    let aboveMaxWeight = 
                { Data = {validVehicle 
                            with Weight = maxWeight + 10.}
                  Requirement = errorMaxWeight }

    let belowMinTankCapacity = 
                { Data = {validVehicle 
                            with TankCapacity = minTankCapacity - 10}
                  Requirement = errorMinTankCapacity }

    let aboveMaxTankCapacity = 
                { Data = {validVehicle 
                            with TankCapacity = maxTankCapacity + 10}
                  Requirement = errorMaxTankCapacity }

    
module Converters = 
    open ModelService

    let toVehicleModel x =
        new VehicleModel(
            x.Name
            ,x.EnginePower
            ,x.Weight
            ,x.ResistanceWithMedian
            ,x.TankCapacity)