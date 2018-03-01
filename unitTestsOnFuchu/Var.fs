namespace unitTestsOnFuchu

module Var =
    open ModelService
    open Demands

    //validVehicle и modelVehicle содержат одни и те же данные
    let validVehicle = {name = "mazda_rx-8"; 
            enginePower = 192;
            weight = 1429.0;
            resistanceWithMedian = Environment.Asphalt;
            tankCapacity = 65;}
    
    let modelVehicle = new VehicleModel()

    let toVehicleModel x =
        new VehicleModel(
            x.name
            ,x.enginePower
            ,x.weight
            ,x.resistanceWithMedian
            ,x.tankCapacity)

    let invalidName =
        {validVehicle with name = "mazda#22"}

    let invalidEnginePower =
        {validVehicle
            with enginePower = minEnginePower - 10}

    let invalidWeight =
        {validVehicle 
            with weight = minWeight - 10.}
        
    let invalidTankCapacity =
        {validVehicle 
            with tankCapacity = minTankCapacity - 10}

    let errorInvalidCharacter = 
        [InvalidCharacter]
        |> Name
        |> Some

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

    let autoShow = AutoShow.Instance 

    let checker = new Checker()

    let gibdd = new Gibdd()