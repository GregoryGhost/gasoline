namespace ModelService

type Environment = 
    | Air
    | Ground
    | Asphalt
    | Dirt  //грязь
    | Space
    | RuggedTerrain //пересеченная местность

type Vehicle =
    (* тип транспортного средства для ООП-совместимости с dotNET *)
    (*  *)
    (*
    TODO: Определить интерфейс методов, что принимают, 
          что возвращают для вызова их из клиентского кода C#
    TODO: Написать в скрипте Script.fsx код, показывающий как использовать код модуля
          в пользовательском коде
    TODO: Подключить для выполнения операций над матрицами
          специальный тип из FSharp.Data - Matrix
    TODO: Реализовать/Десеарилизацию сериализацию данных об транспортных средствах в JSON + F#
    TODO: Покрыть только модуль на F# модульными тестами Fuchu и если нужно автотестами из FsCheck
    TODO: Добавить функцию на проверку работоспособности транспортного средства по заявленным характеристикам
    *)
    (*let mutable id = 0
    //ограничение pattern от 5 до 20 символов - цифр или букв, включая - _
    let _name : string
    //ограничение pattern от 1 до 1 000 000 000 л.с. - целочисленный
    let mutable _enginePower = 20
    //ограничение pattern от 1 до 10 000 кг - double
    let mutable _weight = 120
    //ограничения pattern - воздух, вода, космос, земля, асфальт, грязь, пересеченная_местность
    let _resistanceWithMedian : float
    //ограничения pattern - от 0 до 10 000 л
    let mutable _tankCapacity = 12*)
    val name : string
    val enginePower : int 
    val weight : double
    val resistanceWithMedian : Environment 
    val tankCapacity : int 

    new (name, enginePower, weight, resistanceWithMedian, tankCapacity) = {
        name = name;
        enginePower = enginePower;
        weight = weight;
        resistanceWithMedian = resistanceWithMedian;
        tankCapacity = tankCapacity;
    }

    new () = Vehicle("mazda", 1234, 1234.3, Environment.Air, 120)