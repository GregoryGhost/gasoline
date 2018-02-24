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

    public class VehicleViewModel : INotifyPropertyChanged, IEditableObject
    {
        private VehicleModel _copyData;
        private VehicleModel _currentData;

        public VehicleViewModel()
            : this("mazda_rx-8", 192, 1429.0, ModelService.Environment.Asphalt, 65)
        {
        }

        public VehicleViewModel(string name, int enginePower,
            double weight, ModelService.Environment resistance, int tankCapacity)
        {
            _currentData = new VehicleModel(name, enginePower, weight, resistance, tankCapacity);
        }

        public string Name
        {
            get { return _currentData.Name; }
            set
            {
                if (_currentData.Name != value)
                {
                    _currentData.Name = value;
                    NotifyPropertyChanged(nameof(this.Name));
                    NotifyPropertyChanged(nameof(this.FuelConsumption));
                }
            }
        }

        public int EnginePower
        {
            get { return _currentData.EnginePower; }
            set
            {
                _currentData.EnginePower = value;
                NotifyPropertyChanged(nameof(this.EnginePower));
                NotifyPropertyChanged(nameof(this.FuelConsumption));
            }
        }

        public double Weight
        {
            get
            {
                return _currentData.Weight;
            }
            set
            {
                _currentData.Weight = value;
                NotifyPropertyChanged(nameof(this.Weight));
                NotifyPropertyChanged(nameof(this.FuelConsumption));
            }
        }

        public string Resistance
        {
            get => _currentData.Resistance.ToString();
            set
            {
                _currentData.Resistance = value.ReadFromString();
                NotifyPropertyChanged(nameof(this.Resistance));
                NotifyPropertyChanged(nameof(this.FuelConsumption));
            }
        }

        public List<string> Resistances =>
            Enum.GetNames(typeof(ModelService.Environment)).ToList();

        public int TankCapacity
        {
            get
            {
                return _currentData.TankCapacity;
            }
            set
            {
                _currentData.TankCapacity = value;
                NotifyPropertyChanged(nameof(this.TankCapacity));
                NotifyPropertyChanged(nameof(this.FuelConsumption));
            }
        }

        public string FuelConsumption
        {
            get
            {
                var vehicle = new ModelService.Vehicle(this.Name,
                    this.EnginePower,
                    this.Weight,
                    this.Resistance.ReadFromString(),
                    this.TankCapacity);
                var result = Manager.CalcFuelConsumption(vehicle);
                
                return result;
            }
        }

        public override string ToString() => $"{Name}, {EnginePower:f}, {Weight:f}, {Resistance:G}, {TankCapacity:D}, {FuelConsumption}";

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
            _copyData = new VehicleModel();
        }

        #endregion
    }
}
