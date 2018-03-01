namespace unitTestsOnFuchu

module TestGibdd =
    open Fuchu.Tests
    open Fuchu
    open Var
    open ModelService.Converters

    let testMethods = 
        testList "Validate methods of Gibdd" [
            testCase "Check Name" <| (fun _ ->
                let expected = 
                    errorInvalidCharacter
                    |> Option.get
                    |> ToText

                let actual = 
                    invalidName
                    |> toVehicleModel
                    |> gibdd.CheckName

                Assert.Equal("", expected, actual)
            );

            testCase "Check Engine Power" <| (fun _ ->
                let expected = 
                    errorMinEnginePower
                    |> Option.get
                    |> ToText

                let actual = 
                    invalidEnginePower
                    |> toVehicleModel
                    |> gibdd.CheckEnginePower

                Assert.Equal("", expected, actual)
            );

            testCase "Check Weight" <| (fun _ ->
                let expected = 
                    errorMinWeight
                    |> Option.get
                    |> ToText

                let actual = 
                    invalidWeight
                    |> toVehicleModel
                    |> gibdd.CheckWeight

                Assert.Equal("", expected, actual)
            );

            testCase "Check Tank Capacity" <| (fun _ ->
                let expected = 
                    errorMinTankCapacity
                    |> Option.get
                    |> ToText

                let actual = 
                    invalidTankCapacity
                    |> toVehicleModel
                    |> gibdd.CheckTankCapacity

                Assert.Equal("", expected, actual)
            );
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