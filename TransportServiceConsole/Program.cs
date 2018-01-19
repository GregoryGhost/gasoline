using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelService;
using Library1;

namespace TransportServiceConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = AutoShow.Instance;
            var vehicle = new Vehicle("mazda", 1000, 200.5, ModelService.Environment.Asphalt, 900);
            c.AddVehicle(vehicle);
            c.AddVehicle(vehicle);
            c.AddVehicle(vehicle);
            var path = System.IO.Directory.GetCurrentDirectory() + "\\" + "test.json";
            c.Save(path);
            c.Load(path);
            var t = c.GetAllVehicles;
            WriteAllVehicle(t);
        }

        static void WriteAllVehicle(Vehicle[] vehicles)
        {
            foreach (var e in vehicles)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
