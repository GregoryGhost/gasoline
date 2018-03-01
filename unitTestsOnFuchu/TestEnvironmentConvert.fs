namespace unitTestsOnFuchu

module TestEnvironmentConvert =
    open Fuchu.Tests
    open Fuchu
    open ModelService.ExtensionEnvironment
    open ModelService

    let testReadEnvironmentStateFromStr env =
        let expected = env

        let actual = 
            env.ToString()
            |> ReadFromString

        Assert.Equal("", expected, actual)

    let testReadFromString =
        testList "Validate read from string environment states" [
            //Для каждого состояния перечисления Environment
            //  генерируется тестовый случай с именем этого состояния,
            //  также это состояние передается на тестирование
            for e in System.Enum.GetValues(typeof<Environment>) do
                let env = e :?> Environment
                let name = env.ToString()

                yield testCase name <| (fun _ ->
                        testReadEnvironmentStateFromStr env );
        ];