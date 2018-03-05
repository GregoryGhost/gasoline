﻿namespace unitTestsOnFuchu

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
        testList "Validate methods of check errors of Checker" [
            testList "Check Name" [
                testCase "invalid character" <| (fun _ ->
                    invalidChar
                    |> testCheck checker.CheckName
                );

                testCase "above max symbol" <| (fun _ ->
                    aboveMaxSymbol
                    |> testCheck checker.CheckName
                );

                testCase "below min symbol" <| (fun _ ->
                    belowMinSymbol
                    |> testCheck checker.CheckName
                );
            ];

            testCase "Check Engine Power" <| (fun _ ->
                let expected = 
                    errorMinEnginePower
                    
                let actual = 
                    invalidEnginePower
                    |> checker.CheckEnginePower

                Assert.Equal("", expected, actual);
            );

            testCase "Check Weight" <| (fun _ ->
                let expected = 
                    errorMinWeight

                let actual = 
                    invalidWeight
                    |> checker.CheckWeight

                Assert.Equal("", expected, actual);
            );

            testCase "Check Tank Capacity" <| (fun _ ->
                let expected = 
                    errorMinTankCapacity
                    
                let actual = 
                    invalidTankCapacity
                    |> checker.CheckTankCapacity

                Assert.Equal("", expected, actual);
            );
        ];