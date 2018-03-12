namespace ModelService

//Изменил тип DU на enum, чтобы было проще сериализовать в JSON и обратно
type Environment = 
    | Air = 1
    | Ground = 2
    | Asphalt = 3
    | Dirt = 4 //грязь
    | Space = 5
    | RuggedTerrain = 6//пересеченная местность
    | NANI = 0

type Vehicle = {
                 Name : string;     //ограничения от 1 до 20 символов
                                        //- цифры или буквы, включая "-", "_"
          EnginePower : int;        //ограничения от 1 до 1*10^9 л.с.
               Weight : double;     //ограничения от 1 до 10^4 кг - double
 ResistanceWithMedian : Environment;//ограничения - воздух,
                                        //вода, 
                                        //космос, 
                                        //земля, 
                                        //асфальт, 
                                        //грязь, 
                                        //пересеченная_местность
         TankCapacity : int        }//ограничения - от 0 до 10^4 л
with
    member internal this.CalcFuelConsumption =
           let rs = this.ResistanceWithMedian

           match rs with
           | Environment.Air ->  //rs * (weight + tankCapacity) / enginePower
               let calc = (this.Weight + float this.TankCapacity) 
                           / float this.EnginePower
               calc * (float rs)
           | Environment.Ground -> //rs - (weight + tankCapacity) / enginePower
               let calc = (this.Weight + float this.TankCapacity) 
                            / float this.EnginePower
               (float rs) - calc
           | Environment.Asphalt -> //rs + (weight - tankCapacity) * enginePower
               let calc = (this.Weight + float this.TankCapacity) 
                           * float this.EnginePower
               (float rs) + calc
           | Environment.Dirt ->
                //resistanceWithMedian * weight + tankCapacity) / enginePower
               let calc = (float rs) * this.Weight 
               calc + (float <| this.TankCapacity / this.EnginePower)
           | Environment.Space ->
                //resistanceWithMedian * (weight + tankCapacity + enginePower)
               let calc = this.Weight 
                          + float this.TankCapacity 
                          + float this.EnginePower
               (float rs) * calc
           | Environment.RuggedTerrain ->
                //resistanceWithMedian * (weight / tankCapacity) - enginePower
               let calc = (rs |> float)
                          * (this.Weight / (this.TankCapacity |> float))
                          - (this.EnginePower |> float)
               calc
           | _ -> 0.0

/// <summary>
/// Модель транспортного средства
/// </summary>
type VehicleModel(name, enginePower, weight, resistance, tankCapacity) =
    let mutable name = name
    let mutable enginePower = enginePower
    let mutable weight = weight
    let mutable resistance = resistance
    let mutable tankCapacity = tankCapacity

    new() = VehicleModel(
                "mazda_rx-8", 
                192, 
                1429.0, 
                Environment.Asphalt, 
                65)

    /// <summary>
    /// Название транспортного средства
    /// </summary>
    member this.Name 
        with get() = name 
        and set(value) = name <- value

    /// <summary>
    /// Мощность двигателя
    /// </summary>
    member this.EnginePower 
        with get() = enginePower
        and set(value) = enginePower <- value
    
    /// <summary>
    /// Вес транспортного средства
    /// </summary>
    member this.Weight
        with get() = weight
        and set(value) = weight <- value

    /// <summary>
    /// Среда, в которой предназначено использование транспортного средства
    /// </summary>
    member this.Resistance 
        with get() = resistance
        and set(value) = resistance <- value

    /// <summary>
    /// Объем бака
    /// </summary>
    member this.TankCapacity
        with get() = tankCapacity
        and set(value) = tankCapacity <- value

    /// <summary>
    /// Возвращает дубликат записи
    /// </summary>
    member this.Clone() =
        new VehicleModel(this.Name
                         ,this.EnginePower
                         ,this.Weight
                         ,this.Resistance
                         ,this.TankCapacity)
      
    /// <summary>
    /// Конвертирует в запись о транспортном средстве
    /// </summary>
    member this.ToVehicle() =
        {Name = this.Name
        ;EnginePower = this.EnginePower
        ;Weight = this.Weight
        ;ResistanceWithMedian = this.Resistance
        ;TankCapacity = this.TankCapacity}