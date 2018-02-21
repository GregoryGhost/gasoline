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
                    //_currentData.name = value;
                    //NotifyPropertyChanged("Description");
                }
            }
        }

        public double EnginePower => _currentData.enginePower;

        public double Weight => _currentData.weight;

        public ModelService.Environment Resistance => _currentData.resistanceWithMedian;

        public int TankCapacity => _currentData.tankCapacity;

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
            _copyData = new Vehicle("mazda", 1000, 200.5, ModelService.Environment.Asphalt, 900);
        }

        #endregion
    }
}
