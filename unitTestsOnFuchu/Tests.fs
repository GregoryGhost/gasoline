namespace unitTestsOnFuchu

module Tests =
    open Fuchu.Tests
    open Fuchu
    open ModelService
    open ModelService.Demands

    let validVehicle = {name = "mazda_rx-8"; 
            enginePower = 192;
            weight = 1429.0;
            resistanceWithMedian = Environment.Asphalt;
            tankCapacity = 65;}

    let autoShow = AutoShow.Instance

    let suite =
        TestList [
            testList "Add vehicle to auto show" [
                testCase "correct vehicle properties" <| (fun _ ->
                    autoShow.ClearAllVehicle()
                    let errors = autoShow.AddVehicle validVehicle
                    let expected = Seq.empty

                    Assert.Equal("", expected, errors)
                );

                testCase "invalid vehicle enginePower - below zero" <| (fun _ ->
                    let v1 = {validVehicle with enginePower = -192}

                    autoShow.ClearAllVehicle()
                    let errors = autoShow.AddVehicle v1
                    let expected = 
                        let er = [
                            Demands.minEnginePower
                            |> BelowTheMinimum
                            |> RequirementsForVehicle.EnginePower
                        ] 
                        er |> List.toSeq

                    Assert.Equal("", expected, errors)
                );
            ];
            testList "Update vehicle to auto show" [
                testCase "new record" <| (fun _ ->
                    let newV = {validVehicle with enginePower = 225}

                    autoShow.ClearAllVehicle()
                    autoShow.AddVehicle(validVehicle) |> ignore
                    let errors = autoShow.UpdateVehicle(validVehicle, newV)
                    let expected = Seq.empty

                    Assert.Equal("", expected, errors)
                );

                testCase "but already exists record" <| (fun _ ->
                    let newV = validVehicle

                    autoShow.AddVehicle(validVehicle) |> ignore
                    let errors = autoShow.UpdateVehicle(validVehicle, newV)
                    let expected = 
                        [ExistsElem] |> List.toSeq

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