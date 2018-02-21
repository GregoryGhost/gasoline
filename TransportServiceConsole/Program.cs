using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelService;

namespace TransportServiceConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = AutoShow.Instance;
            var vehicle = new Vehicle("mazda", 1000, 200.5, ModelService.Environment.Asphalt, 900);
            var v2 = new Vehicle("mazda2", 124, 2225.5, ModelService.Environment.RuggedTerrain, 90);
            var v3 = new Vehicle("mazda3", 124, 2225.5, ModelService.Environment.Asphalt, 90);

            c.AddVehicle(vehicle);
            c.AddVehicle(v2);
            c.AddVehicle(v3);
            c.AddVehicle(vehicle);
            var path = System.IO.Directory.GetCurrentDirectory() + "\\" + "test.json";
            c.Save(path);
            c.ClearAllVehicle();
            try
            {
                c.Load(path);
            }
            catch(Exception e)
            {
                Console.WriteLine("{0}", e.Message);
            }
            
            var t = c.GetAllVehicles;
            WriteAllVehicle(t);
        }

        static void WriteAllVehicle(IEnumerable<Vehicle> vehicles)
        {
            foreach (var e in vehicles)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Расход топлива: {0}", Manager.CalcFuelConsumption(e));
                Console.WriteLine();
            }
        }
    }
}
