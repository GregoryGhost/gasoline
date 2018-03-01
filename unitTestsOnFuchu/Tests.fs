namespace unitTestsOnFuchu

module Tests =
    open Fuchu.Tests
    open Fuchu
    open Var
    open ModelService
    open unitTestsOnFuchu

    let suite =
        TestList [
            TestListAddVehicle.testAddVehicle
            ;CalcFuelConsumption.testValidVehicle
            ;CalcFuelConsumption.testInvalidVehicle
            ;TestAutoShow.testRemoveRecord
            ;TestAutoShow.testUpdateVehicle
            ;TestVehicleModel.testProperties
            ;TestVehicleModel.testMethods
            ;TestChecker.tests
            ;TestGibdd.testMethods
            ;TestGibdd.testToText
            ;TestEnvironmentConvert.testReadFromString
        ]