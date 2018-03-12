namespace unitTestsOnFuchu

module TestGibdd =
    open Fuchu.Tests
    open Fuchu
    open Var
    open Requirements
    open ModelService.Converters
    open Converters

    let testCheck checker record =
        let expected = 
            record.Requirement 
            |> Option.get
            |> ToText
                
        let actual =
            record.Data |> toVehicleModel
            |> checker

        Assert.Equal("", expected, actual)

    let tests = 
        let testCheckName = testCheck gibdd.CheckName
        let testCheckEnginePower = testCheck gibdd.CheckEnginePower
        let testCheckWeight = testCheck gibdd.CheckWeight
        let testCheckTankCapacity = testCheck gibdd.CheckTankCapacity

        let checkBelowParam checker value =
            testCase "below min value" <| (fun _ -> value |> checker);
            
        let checkAboveParam checker value =
             testCase "above max value" <| (fun _ -> value |> checker);

        testList "Validate methods of check errors of Checker" [
            testList "Check Name" [
                testCase "invalid character" <| (fun _ ->
                    invalidChar
                    |> testCheckName
                );

                testCase "above max symbol" <| (fun _ ->
                    aboveMaxSymbol
                    |> testCheckName
                );

                testCase "below min symbol" <| (fun _ ->
                    belowMinSymbol
                    |> testCheckName
                );

                testCase "empty string" <| (fun _ ->
                    emptyName
                    |> testCheckName
                );
            ];

            testList "Check Engine Power" [
                checkBelowParam testCheckEnginePower belowMinEnginePower

                checkAboveParam testCheckEnginePower aboveMaxEnginePower
            ];

            testList "Check Weight" [
                checkBelowParam testCheckWeight belowMinWeight

                checkAboveParam testCheckWeight aboveMaxWeight
            ];

            testList "Check Tank Capacity" [
                checkBelowParam testCheckTankCapacity belowMinTankCapacity

                checkAboveParam testCheckTankCapacity aboveMaxTankCapacity
            ]
        ];
    
    let testToText =
        testList @"Validate a method ""ToText"" 
            - error of parameters vehicle to text " [
            testCase "Check not empty text" <| (fun _ ->
                let expected = true

                let actual = 
                    errorMinWeight
                    |> Option.get
                    |> ToText
                    |> String.length > 0
                
                Assert.Equal("", expected, actual)
            );
        ];