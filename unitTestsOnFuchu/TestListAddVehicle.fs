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
                let invalidVehicle = {Name = "&&&&**";
                        EnginePower = Demands.maxEnginePower + num;
                        Weight = Demands.maxWeight + float num;
                        ResistanceWithMedian = Environment.NANI;
                        TankCapacity = Demands.maxEnginePower + num}

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
                let v1 = {validVehicle with Name = "test"}

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
                let genMaxName = maxSymbol |> Requirements.genName
                let v1 = {validVehicle with Name = genMaxName}

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
                let v1 = {validVehicle with Name = genName}

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
                let v1 = {validVehicle with EnginePower = -192}

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
                let v1 = {validVehicle with EnginePower = Demands.maxEnginePower + 1}

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
                let v1 = {validVehicle with Weight = Demands.minWeight - 10.0}

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
                let v1 = {validVehicle with Weight = Demands.maxWeight + 10.0}

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
                let v1 = {validVehicle with TankCapacity = Demands.minTankCapacity - 10}

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
                let v1 = {validVehicle with TankCapacity = Demands.maxTankCapacity + 10}

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