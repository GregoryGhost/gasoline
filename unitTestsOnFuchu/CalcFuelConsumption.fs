namespace unitTestsOnFuchu

module CalcFuelConsumption = 
    open ModelService
    open Fuchu
    open Var
    open Demands

    let testCalcFuelConsumption =
         testList "Calculate fuel consumption of vehicle" [
                testCase "valid vehicle" <| (fun _ ->
                    let v = {validVehicle with resistanceWithMedian = Environment.NANI}

                    let mes = Manager.CalcFuelConsumption(v)
                    let expected = "0"

                    Assert.Equal("", expected, mes)
                );

                testCase "invalid vehicle" <| (fun _ ->
                    let invalidVehicle = {validVehicle with enginePower = Demands.maxEnginePower + 10}

                    let mes = Manager.CalcFuelConsumption(invalidVehicle)
                    let expected = Environment.NANI.ToString()

                    Assert.Equal("", expected, mes)
                );
            ];