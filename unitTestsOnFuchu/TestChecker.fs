namespace unitTestsOnFuchu

module TestChecker = 
    open Fuchu.Tests
    open Fuchu
    open Var
    open ModelService.Demands
    open ModelService

    let tests = 
        testList "Validate methods of check errors of Checker" [
            testCase "Check Name" <| (fun _ ->
                let invalidVehicle =
                    {validVehicle with name = "mazda#22"}

                let expected = 
                    [InvalidCharacter]
                    |> Name
                    |> Some
                    
                let actual =
                    invalidVehicle
                    |> checker.CheckName

                Assert.Equal("", expected, actual);
            );

            testCase "Check Engine Power" <| (fun _ ->
                let invalidVehicle =
                    {validVehicle
                        with enginePower = minEnginePower - 10}

                let expected = 
                    Demands.minEnginePower
                    |> BelowTheMinimum
                    |> EnginePower
                    |> Some
                    
                let actual = 
                    invalidVehicle
                    |> checker.CheckEnginePower

                Assert.Equal("", expected, actual);
            );

            testCase "Check Weight" <| (fun _ ->
                let invalidVehicle =
                    {validVehicle 
                        with weight = minWeight - 10.}

                let expected = 
                    Demands.minWeight |> int
                    |> BelowTheMinimum
                    |> Weight
                    |> Some
                    
                let actual = 
                    invalidVehicle
                    |> checker.CheckWeight

                Assert.Equal("", expected, actual);
            );

            testCase "Check Tank Capacity" <| (fun _ ->
                let invalidVehicle =
                    {validVehicle 
                        with tankCapacity = minTankCapacity - 10}

                let expected = 
                    Demands.minTankCapacity
                    |> BelowTheMinimum
                    |> TankCapacity
                    |> Some
                    
                let actual = 
                    invalidVehicle
                    |> checker.CheckTankCapacity

                Assert.Equal("", expected, actual);
            );
        ];