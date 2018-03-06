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
                testCase "below min value" <| (fun _ ->
                    belowMinEnginePower
                    |> testCheckEnginePower
                );

                testCase "above max value" <| (fun _ ->
                    aboveMaxEnginePower
                    |> testCheckEnginePower
                );
            ];

            testList "Check Weight" [
                testCase "below min value" <| (fun _ ->
                    belowMinWeight
                    |> testCheckWeight
                );

                testCase "above max value" <| (fun _ ->
                    aboveMaxWeight
                    |> testCheckWeight
                );
            ];

            testList "Check Tank Capacity" [
                testCase "below min value" <| (fun _ ->
                    belowMinTankCapacity
                    |> testCheckTankCapacity
                );

                testCase "above max value" <| (fun _ ->
                    aboveMaxTankCapacity
                    |> testCheckTankCapacity
                );
            ]
        ];