namespace ModelService

//Изменил тип DU на enum, чтобы было проще сериализовать в JSON и обратно
type Environment = 
    | Air = 1
    | Ground = 2
    | Asphalt = 3
    | Dirt = 4 //грязь
    | Space = 5
    | RuggedTerrain = 6//пересеченная местность
    | NANI = 7
    (*
    TODO: Покрыть только модуль на F# модульными тестами Fuchu и если нужно автотестами из FsCheck
    TODO: Добавить функцию на проверку работоспособности транспортного средства по заявленным характеристикам
    *)
type Vehicle = {
                name : string;     //ограничение pattern от 1 до 20 символов - цифр или букв, включая - _
         enginePower : int;        //ограничения pattern  от 1 до 1 000 000 000 л.с.
              weight : double;     //ограничение pattern от 1 до 10 000 кг - double
resistanceWithMedian : Environment;//ограничения pattern - воздух, вода, космос, земля, асфальт, грязь, пересеченная_местность
        tankCapacity : int        }//ограничения pattern - от 0 до 10 000 л