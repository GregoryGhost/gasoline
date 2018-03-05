namespace unitTestsOnFuchu

module Requirements =
    open ModelService.Demands
    open ModelService

    open Var


    type Data = 
        { Data : Vehicle
          Requirement : RequirementsForVehicle option }
    

    let errorInvalidCharacter = 
        [InvalidCharacter]
        |> Name
        |> Some

    let invalidChar = 
                { Data = {validVehicle 
                            with name = "mazda#22"}
                  Requirement = errorInvalidCharacter }

    let invalidEnginePower =
        {validVehicle
            with enginePower = minEnginePower - 10}

    let invalidWeight =
        {validVehicle 
            with weight = minWeight - 10.}
        
    let invalidTankCapacity =
        {validVehicle 
            with tankCapacity = minTankCapacity - 10}

    let errorMinEnginePower =
        minEnginePower
        |> BelowTheMinimum
        |> EnginePower
        |> Some

    let errorMinWeight =
        Demands.minWeight |> int
        |> BelowTheMinimum
        |> Weight
        |> Some

    let errorMinTankCapacity =
        Demands.minTankCapacity
        |> BelowTheMinimum
        |> TankCapacity
        |> Some

    
module Converters = 
    open ModelService

    let toVehicleModel x =
        new VehicleModel(
            x.name
            ,x.enginePower
            ,x.weight
            ,x.resistanceWithMedian
            ,x.tankCapacity)