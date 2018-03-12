namespace unitTestsOnFuchu

module Var =
    open ModelService

    //validVehicle и modelVehicle содержат одни и те же данные
    let validVehicle = {Name = "mazda_rx-8"; 
            EnginePower = 192;
            Weight = 1429.0;
            ResistanceWithMedian = Environment.Asphalt;
            TankCapacity = 65;}
    
    let modelVehicle = new VehicleModel()

    let autoShow = AutoShow.Instance 

    let checker = new Checker()

    let gibdd = new Gibdd()