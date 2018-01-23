namespace unitTestsOnFuchu

module Var =
    open ModelService

    let validVehicle = {name = "mazda_rx-8"; 
            enginePower = 192;
            weight = 1429.0;
            resistanceWithMedian = Environment.Asphalt;
            tankCapacity = 65;}

    let autoShow = AutoShow.Instance 