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

    let testMethods = 
        testList "Validate methods of Gibdd" [
            testList "Check Name" [
                testCase "invalid character" <| (fun _ ->
                    invalidChar
                    |> testCheck gibdd.CheckName
                );

                testCase "above max symbol" <| (fun _ ->
                    aboveMaxSymbol
                    |> testCheck gibdd.CheckName
                );
            ];

            //testCase "Check Engine Power" <| (fun _ ->
            //    let expected = 
            //        errorMinEnginePower
            //        |> Option.get
            //        |> ToText

            //    let actual = 
            //        invalidEnginePower
            //        |> toVehicleModel
            //        |> gibdd.CheckEnginePower

            //    Assert.Equal("", expected, actual)
            //);

            //testCase "Check Weight" <| (fun _ ->
            //    let expected = 
            //        errorMinWeight
            //        |> Option.get
            //        |> ToText

            //    let actual = 
            //        invalidWeight
            //        |> toVehicleModel
            //        |> gibdd.CheckWeight

            //    Assert.Equal("", expected, actual)
            //);

            //testCase "Check Tank Capacity" <| (fun _ ->
            //    let expected = 
            //        errorMinTankCapacity
            //        |> Option.get
            //        |> ToText

            //    let actual = 
            //        invalidTankCapacity
            //        |> toVehicleModel
            //        |> gibdd.CheckTankCapacity

            //    Assert.Equal("", expected, actual)
            //);
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