namespace unitTestsOnFuchu

module TestListAddVehicle = 
    open ModelService
    open Fuchu
    open Var
    open Demands

    let testAddVehicle = 
        testList "Add vehicle to auto show" [
            testCase "correct vehicle properties" <| (fun _ ->
                autoShow.ClearAllVehicle()

                let errors = autoShow.AddVehicle validVehicle
                let expected = Seq.empty

                Assert.Equal("", expected, errors)
            );

            testCase "exists the vehicle" <| (fun _ ->
                autoShow.ClearAllVehicle()
                autoShow.AddVehicle validVehicle |> ignore

                let errors = autoShow.AddVehicle validVehicle
                let expected = [ExistsElem] |> List.toSeq

                Assert.Equal("", expected, errors)
            );

            testCase "invalid vehicle properties, except resistance with median" <| (fun _ ->
                let num = 10
                let invalidVehicle = {name = "&&&&**";
                        enginePower = Demands.maxEnginePower + num;
                        weight = Demands.maxWeight + float num;
                        resistanceWithMedian = Environment.NANI;
                        tankCapacity = Demands.maxEnginePower + num}

                autoShow.ClearAllVehicle()
                let errors = autoShow.AddVehicle invalidVehicle
                let expected = 
                        [Name <| [InvalidCharacter];
                            EnginePower  << AboveTheMaximum <|     Demands.maxEnginePower;
                            TankCapacity << AboveTheMaximum <|     Demands.maxTankCapacity;
                            Weight       << AboveTheMaximum <| int Demands.maxWeight;]
                        |> List.toSeq

                Assert.Equal("", expected, errors)
            );

            testCase "invalid vehicle name - below min symbol" <| (fun _ ->
                let v1 = {validVehicle with name = "test"}

                autoShow.ClearAllVehicle()
                let errors = autoShow.AddVehicle v1
                let expected = 
                    let er = [
                        [Demands.minSymbol
                        |> BelowTheMinimum]
                        |> Name
                    ] 
                    er |> List.toSeq

                Assert.Equal("", expected, errors)
            );

            testCase "invalid vehicle name - above max symbol" <| (fun _ ->
                let genMaxName = String.replicate 20 "test"
                let v1 = {validVehicle with name = genMaxName}

                autoShow.ClearAllVehicle()
                let errors = autoShow.AddVehicle v1
                let expected = 
                    let er = [
                        [Demands.maxSymbol |> AboveTheMaximum]
                        |> Name
                    ] 
                    er |> List.toSeq

                Assert.Equal("", expected, errors)
            );

            testCase "invalid vehicle name - invalid character" <| (fun _ ->
                let genName = String.replicate 10 "*"
                let v1 = {validVehicle with name = genName}

                autoShow.ClearAllVehicle()
                let errors = autoShow.AddVehicle v1
                let expected = 
                    let er = [
                        [InvalidCharacter]
                        |> Name
                    ] 
                    er |> List.toSeq

                Assert.Equal("", expected, errors)
            );

            testCase "invalid vehicle enginePower - below minimum" <| (fun _ ->
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

            testCase "invalid vehicle enginePower - above maximum" <| (fun _ ->
                let v1 = {validVehicle with enginePower = Demands.maxEnginePower + 1}

                autoShow.ClearAllVehicle()
                let errors = autoShow.AddVehicle v1
                let expected = 
                    let er = [
                        Demands.maxEnginePower
                        |> AboveTheMaximum
                        |> RequirementsForVehicle.EnginePower
                    ] 
                    er |> List.toSeq

                Assert.Equal("", expected, errors)
            );

            testCase "invalid vehicle weight - below minimum" <| (fun _ ->
                let v1 = {validVehicle with weight = Demands.minWeight - 10.0}

                autoShow.ClearAllVehicle()
                let errors = autoShow.AddVehicle v1
                let expected = 
                    let er = [
                        Demands.minWeight 
                        |> int
                        |> BelowTheMinimum
                        |> RequirementsForVehicle.Weight
                    ] 
                    er |> List.toSeq

                Assert.Equal("", expected, errors)
            );

            testCase "invalid vehicle weight - above maximum" <| (fun _ ->
                let v1 = {validVehicle with weight = Demands.maxWeight + 10.0}

                autoShow.ClearAllVehicle()
                let errors = autoShow.AddVehicle v1
                let expected = 
                    let er = [
                        Demands.maxWeight
                        |> int
                        |> AboveTheMaximum
                        |> RequirementsForVehicle.Weight
                    ]
                    er |> List.toSeq

                Assert.Equal("", expected, errors)
            );

            testCase "invalid vehicle tank capacity - below minimum" <| (fun _ ->
                let v1 = {validVehicle with tankCapacity = Demands.minTankCapacity - 10}

                autoShow.ClearAllVehicle()
                let errors = autoShow.AddVehicle v1
                let expected = 
                    let er = [
                        Demands.minTankCapacity
                        |> BelowTheMinimum
                        |> RequirementsForVehicle.TankCapacity
                    ]
                    er |> List.toSeq

                Assert.Equal("", expected, errors)
            );

            testCase "invalid vehicle tank capacity - above maximum" <| (fun _ ->
                let v1 = {validVehicle with tankCapacity = Demands.maxTankCapacity + 10}

                autoShow.ClearAllVehicle()
                let errors = autoShow.AddVehicle v1
                let expected = 
                    let er = [
                        Demands.maxTankCapacity
                        |> AboveTheMaximum
                        |> RequirementsForVehicle.TankCapacity
                    ]
                    er |> List.toSeq

                Assert.Equal("", expected, errors)
            );
        ]