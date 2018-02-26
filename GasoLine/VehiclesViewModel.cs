﻿using ModelService;
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

    public class VehicleViewModel : INotifyPropertyChanged, IEditableObject, IDataErrorInfo
    {
        private VehicleModel _copyData;
        private VehicleModel _currentData;
        private Dictionary<string, Func<VehicleModel, string>> _methodsCheck;
        private Gibdd _gibdd;

        public VehicleViewModel()
            : this("mazda_rx-8", 192, 1429.0, ModelService.Environment.Asphalt, 65)
        {
        }

        public VehicleViewModel(string name, int enginePower,
            double weight, ModelService.Environment resistance, int tankCapacity)
        {
            _currentData = new VehicleModel(name, enginePower, weight, resistance, tankCapacity);
            _methodsCheck = new Dictionary<string, Func<VehicleModel, string>>();
            _gibdd = new Gibdd();
            _methodsCheck.Add(nameof(this.Name), new Func<VehicleModel, string>((VehicleModel value) =>_gibdd.CheckName(value)));
            _methodsCheck.Add(nameof(this.EnginePower), new Func<VehicleModel, string>((VehicleModel value) => _gibdd.CheckEnginePower(value)));
            _methodsCheck.Add(nameof(this.Weight), new Func<VehicleModel, string>((VehicleModel value) => _gibdd.CheckWeight(value)));
            _methodsCheck.Add(nameof(this.TankCapacity), new Func<VehicleModel, string>((VehicleModel value) => _gibdd.CheckTankCapacity(value)));
        }

        public string Name
        {
            get { return _currentData.Name; }
            set
            {
                if (_currentData.Name != value)
                {
                    _currentData.Name = value;
                    OnPropertyChanged(nameof(this.Name));
                    OnPropertyChanged(nameof(this.FuelConsumption));
                }
            }
        }

        public int EnginePower
        {
            get { return _currentData.EnginePower; }
            set
            {
                _currentData.EnginePower = value;
                OnPropertyChanged(nameof(this.EnginePower));
                OnPropertyChanged(nameof(this.FuelConsumption));
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
                OnPropertyChanged(nameof(this.Weight));
                OnPropertyChanged(nameof(this.FuelConsumption));
            }
        }

        public string Resistance
        {
            get => _currentData.Resistance.ToString();
            set
            {
                _currentData.Resistance = value.ReadFromString();
                OnPropertyChanged(nameof(this.Resistance));
                OnPropertyChanged(nameof(this.FuelConsumption));
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
                OnPropertyChanged(nameof(this.TankCapacity));
                OnPropertyChanged(nameof(this.FuelConsumption));
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

        string IDataErrorInfo.Error => null;

        public bool IsValid { get; private set; }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                var result = String.Empty;

                if (_methodsCheck.ContainsKey(columnName))
                {
                    result = _methodsCheck[columnName](_currentData);
                    IsValid = false;
                }
                else
                {
                    IsValid = true;
                }

                return result;
            }
        }



        public override string ToString() => $"{Name}, {EnginePower:f}, {Weight:f}, {Resistance:G}, {TankCapacity:D}, {FuelConsumption}";

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        #region IEditableObject Members

        public void BeginEdit()
        {
            _copyData = _currentData.Clone();
        }

        public void CancelEdit()
        {
            _currentData = _copyData;
            OnPropertyChanged("");
        }

        public void EndEdit()
        {
            _copyData = new VehicleModel();
        }

        #endregion
    }
}
