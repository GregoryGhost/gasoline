namespace unitTestsOnFuchu

module TestChecker = 
    open Fuchu.Tests
    open Fuchu
    open ModelService
    open Var
    open Requirements


    let testCheck checker record =
        let expected = record.Requirement 
                
        let actual =
            record.Data
            |> checker

        Assert.Equal("", expected, actual)

    let tests = 
        let testCheckName = testCheck checker.CheckName
        let testCheckEnginePower = testCheck checker.CheckEnginePower
        let testCheckWeight = testCheck checker.CheckWeight
        let testCheckTankCapacity = testCheck checker.CheckTankCapacity

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