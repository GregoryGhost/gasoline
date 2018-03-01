namespace unitTestsOnFuchu

module CalcFuelConsumption = 
    open ModelService
    open Fuchu
    open Var
    open Demands

    let testingCalcFuelConsumption enviroment expected  =
        let mes = 
            {validVehicle with 
                resistanceWithMedian = enviroment}
            |> Manager.CalcFuelConsumption

        Assert.Equal("", expected, mes)

    let testValidVehicle =
        testList "Calculate fuel consumption of valid vehicle" [
            testCase "environment NANI" <| (fun _ ->
                testingCalcFuelConsumption Environment.NANI "0"
            );

            testCase "environment Air" <| (fun _ ->
                testingCalcFuelConsumption Environment.Air "7,78125"
            );

            testCase "environment Asphalt" <| (fun _ ->
                testingCalcFuelConsumption Environment.Asphalt "286851"
            );

            testCase "environment Dirt" <| (fun _ ->
                testingCalcFuelConsumption Environment.Dirt "5716"
            );

            testCase "environment Ground" <| (fun _ ->
                testingCalcFuelConsumption Environment.Ground "-5,78125"
            );

            testCase "environment RuggedTerrain" <| (fun _ ->
                testingCalcFuelConsumption Environment.RuggedTerrain "-60,0923076923077"
            );

            testCase "environment Space" <| (fun _ ->
                testingCalcFuelConsumption Environment.Space "8430"
            );
        ];

    let testInvalidVehicle = 
        testList "Calculate fuel consumption of invalid vehicle" [
            testCase "invalid enginePower" <| (fun _ ->
                let mes = 
                    {validVehicle 
                        with enginePower = maxEnginePower + 10}
                    |> Manager.CalcFuelConsumption

                let expected = Environment.NANI.ToString()

                Assert.Equal("", expected, mes)
            );
        ];