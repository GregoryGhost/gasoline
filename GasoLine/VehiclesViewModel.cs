using ModelService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasoLine
{
    public class Vehicles : ObservableCollection<VehicleViewModel>
    {
        public Vehicles()
        {
            Add(new VehicleViewModel("mazda", 1000, 200.5, ModelService.Environment.Asphalt, 900));
            Add(new VehicleViewModel("mazda2", 124, 2288.5, ModelService.Environment.RuggedTerrain, 90));
            Add(new VehicleViewModel("mazda3", 124, 23335.5, ModelService.Environment.Asphalt, 90));
        }
    }

    /// <summary>
    /// Временный костыль для замены record "Vehicle" из сборки на F#
    /// </summary>
    public class Vehicle
    {
        public Vehicle()
            : this("", 0, 0, ModelService.Environment.Asphalt, 0)
        {
        }

        public Vehicle(string name, int enginePower,
             double weight, ModelService.Environment resistance, int tankCapacity)
        {
            this.name = name;
            this.enginePower = enginePower;
            this.weight = weight;
            this.resistanceWithMedian = resistance;
            this.tankCapacity = tankCapacity;
        }

        public string name { get; set; }
        public int enginePower { get; set; }
        public double weight { get; set; }
        public ModelService.Environment resistanceWithMedian { get; set; }
        public int tankCapacity { get; set; }
    }

    public class VehicleViewModel : INotifyPropertyChanged, IEditableObject
    {
        private Vehicle _copyData;
        private Vehicle _currentData;

        public VehicleViewModel()
            : this("mazda_rx-8", 192, 1429.0, ModelService.Environment.Asphalt, 65)
        {
        }

        public VehicleViewModel(string name, int enginePower,
            double weight, ModelService.Environment resistance, int tankCapacity)
        {
            _currentData = new Vehicle(name, enginePower, weight, resistance, tankCapacity);
        }

        public string Name
        {
            get { return _currentData.name; }
            set
            {
                if (_currentData.name != value)
                {
                    _currentData.name = value;
                    NotifyPropertyChanged(nameof(this.Name));
                }
            }
        }

        public int EnginePower
        {
            get { return _currentData.enginePower; }
            set
            {
                _currentData.enginePower = value;
                NotifyPropertyChanged(nameof(this.EnginePower));
            }
        }

        public double Weight
        {
            get
            {
                return _currentData.weight;
            }
            set
            {
                _currentData.weight = value;
                NotifyPropertyChanged(nameof(this.Weight));
            }
        }

        public string Resistance
        {
            get
            {
                return _currentData.resistanceWithMedian.ToString();
            }
            set
            {
                _currentData.resistanceWithMedian = value.ReadFromString();
                NotifyPropertyChanged(nameof(this.Resistance));
            }
        }

        public List<string> Resistances => 
            Enum.GetNames(typeof(ModelService.Environment)).ToList();

        public int TankCapacity
        {
            get
            {
                return _currentData.tankCapacity;
            }
            set
            {
                _currentData.tankCapacity = value;
                NotifyPropertyChanged(nameof(this.TankCapacity));
            }
        }

        public override string ToString() => $"{Name}, {EnginePower:f}, {Weight:f}, {Resistance:G}, {TankCapacity:D}";

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        #endregion

        #region IEditableObject Members

        public void BeginEdit()
        {
            _copyData = _currentData;
        }

        public void CancelEdit()
        {
            _currentData = _copyData;
            NotifyPropertyChanged("");
        }

        public void EndEdit()
        {
            _copyData = new Vehicle();
        }

        #endregion
    }
}
