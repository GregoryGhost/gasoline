﻿namespace ModelService

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
                 name : string;     //ограничение pattern от 1 до 20 символов - цифр или букв, включая - _
          enginePower : int;        //ограничения pattern  от 1 до 1 000 000 000 л.с.
               weight : double;     //ограничение pattern от 1 до 10 000 кг - double
 resistanceWithMedian : Environment;//ограничения pattern - воздух, вода, космос, земля, асфальт, грязь, пересеченная_местность
         tankCapacity : int        }//ограничения pattern - от 0 до 10 000 л
with
    member internal this.CalcFuelConsumption =
           let rs = this.resistanceWithMedian
           match rs with
           | Environment.Air ->  //rs * (weight + tankCapacity) / enginePower
               let calc = (this.weight + float this.tankCapacity) / float this.enginePower
               calc * (float rs)
           | Environment.Ground -> //rs - (weight + tankCapacity) / enginePower
               let calc = (this.weight + float this.tankCapacity) / float this.enginePower
               (float rs) - calc
           | Environment.Asphalt -> //rs + (weight - tankCapacity) * enginePower
               let calc = (this.weight + float this.tankCapacity) * float this.enginePower
               (float rs) + calc
           | Environment.Dirt ->
                //resistanceWithMedian * weight + tankCapacity) / enginePower
               let calc = (float rs) * this.weight 
               calc + (float <| this.tankCapacity / this.enginePower)
           | Environment.Space ->
                //resistanceWithMedian * (weight + tankCapacity + enginePower)
               let calc = this.weight 
                          + float this.tankCapacity 
                          + float this.enginePower
               (float rs) * calc
           | Environment.RuggedTerrain ->
                //resistanceWithMedian * (weight / tankCapacity) - enginePower
               let calc = (rs |> float)
                          * (this.weight / (this.tankCapacity |> float))
                          - (this.enginePower |> float)
               calc
           | _ -> 0.0