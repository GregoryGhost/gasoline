namespace unitTestsOnFuchu

module Tests =
    open Fuchu.Tests
    open Fuchu
    open Var
    open ModelService
    open unitTestsOnFuchu

    let suite =
        TestList [
            TestListAddVehicle.testAddVehicle;
            CalcFuelConsumption.testValidVehicle;
            CalcFuelConsumption.testInvalidVehicle;
            testList "Update vehicle to auto show" [
                testCase "new record" <| (fun _ ->
                    let newV = {validVehicle with enginePower = 225}

                    autoShow.ClearAllVehicle()
                    autoShow.AddVehicle(validVehicle) |> ignore
                    let errors = autoShow.UpdateVehicle(validVehicle, newV)
                    let expected = Seq.empty

                    Assert.Equal("", expected, errors)
                );

                testCase "replaced the record of the same record" <| (fun _ ->
                    autoShow.ClearAllVehicle()
                    autoShow.AddVehicle(validVehicle) |> ignore

                    let errors = autoShow.UpdateVehicle(validVehicle, validVehicle)
                    let expected = Seq.empty

                    Assert.Equal("", expected, errors)
                );
            ];
            testList "Remove record from auto show" [
                testCase "one record" <| (fun _ ->                   
                    autoShow.ClearAllVehicle()
                    autoShow.AddVehicle(validVehicle) |> ignore
                    autoShow.RemoveVehicle(validVehicle)

                    let result = autoShow.GetAllVehicles
                    let expected = [] |> List.toSeq

                    Assert.Equal("", expected, result)
                );

                testCase "some record" <| (fun _ ->
                    let v1 = {validVehicle with enginePower = 222}

                    autoShow.ClearAllVehicle()
                    autoShow.AddVehicle(validVehicle) |> ignore
                    autoShow.AddVehicle(v1) |> ignore
                    autoShow.RemoveVehicle(validVehicle)

                    let result = autoShow.GetAllVehicles
                    let expected = [v1] |> List.toSeq

                    Assert.Equal("", expected, result)
                );
            ];
        ]