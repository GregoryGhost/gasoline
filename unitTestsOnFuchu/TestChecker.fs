namespace unitTestsOnFuchu

module TestChecker = 
    open Fuchu.Tests
    open Fuchu
    open Var
    open ModelService.Demands
    open ModelService

    let tests = 
        testList "Validate methods of check errors of Checker" [
            testCase "Check Name" <| (fun _ ->
                let expected = errorInvalidCharacter  
                
                let actual =
                    invalidName
                    |> checker.CheckName

                Assert.Equal("", expected, actual);
            );

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