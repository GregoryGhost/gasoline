namespace unitTestsOnFuchu

module TestVehicleModel =
    open Fuchu.Tests
    open Fuchu
    open Var

    let testProperties = 
        testList "Check properties of Vehicle Model" [
            testCase "Name" <| (fun _ ->
                let expected = modelVehicle.Name
                
                let mes = 
                    modelVehicle.Name <- modelVehicle.Name
                    modelVehicle.Name

                Assert.Equal("", expected, mes); 
            );

            testCase "Engine Power" <| (fun _ ->
                let expected = modelVehicle.EnginePower
                
                let mes = 
                    modelVehicle.EnginePower <- modelVehicle.EnginePower
                    modelVehicle.EnginePower

                Assert.Equal("", expected, mes); 
            );

            testCase "Weight" <| (fun _ ->
                let expected = modelVehicle.Weight
                
                let mes = 
                    modelVehicle.Weight <- modelVehicle.Weight
                    modelVehicle.Weight

                Assert.Equal("", expected, mes); 
            );

            testCase "Resistance" <| (fun _ ->
                let expected = modelVehicle.Resistance
                
                let mes = 
                    modelVehicle.Resistance <- modelVehicle.Resistance
                    modelVehicle.Resistance

                Assert.Equal("", expected, mes); 
            );

            testCase "TankCapacity" <| (fun _ ->
                let expected = modelVehicle.TankCapacity
                
                let mes = 
                    modelVehicle.TankCapacity <- modelVehicle.TankCapacity
                    modelVehicle.TankCapacity

                Assert.Equal("", expected, mes); 
            );
        ];