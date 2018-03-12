using ModelService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GasoLine
{
    public class Vehicles :
        ObservableCollection<VehicleViewModel>
    {
        public Vehicles()
        {
            Add(new VehicleViewModel(
                "mazda",
                1000,
                200.5,
                ModelService.Environment.Asphalt,
                900));
            Add(new VehicleViewModel(
                "mazda2",
                124,
                2288.5,
                ModelService.Environment.RuggedTerrain,
                90));
            Add(new VehicleViewModel(
                "mazda3",
                124,
                23335.5,
                ModelService.Environment.Asphalt,
                90));
        }

        ///NOTE: временный костыль
        bool _result = true;
        private bool Result
        {
            get
            {
                return _result;
            }
            set
            {
                if (_result)
                {
                    _result = value;
                }
            }
        }

        private void ResetResult() => _result = true;

        public bool Save(string path)
        {
            ResetResult();
            var bd = AutoShow.Instance;
            //NOTE: удаление записей из БД 
            //      перед повторным добавлением 
            //      записей в БД - КОСТЫЛЬ!!!
            bd.ClearAllVehicle();
            Items
                .ToList()
                .ForEach((VehicleViewModel v) =>
                            Result = (bd.AddVehicle(v.GetVehicleModel)
                                        .Any() == false));
            if (Result)
            {
                bd.Save(path);
            }
            return Result;
        }

        public bool Open(string path)
        {
            var bd = AutoShow.Instance;
            //NOTE: удаление записей из БД перед повторным добавлением записей в БД - КОСТЫЛЬ!!!
            bd.ClearAllVehicle();
            bd.Load(path);

            var vehicles = bd.GetAllVehicles;
            var result = vehicles.Any();
            this.ClearItems();

            vehicles
                .ToList()
                .ForEach((Vehicle v) =>
                            this.Items.Add(new VehicleViewModel(v)));

            return result;
        }
    }

    public class VehicleViewModel :
        INotifyPropertyChanged,
        IEditableObject,
        IDataErrorInfo
    {
        private delegate string CheckParam(VehicleModel model);

        private VehicleModel _copyData;
        private VehicleModel _currentData;
        private Dictionary<string, CheckParam> _methodsCheck;
        private Gibdd _gibdd;

        public VehicleViewModel()
            : this("mazda_rx-8",
                  192,
                  1429.0,
                  ModelService.Environment.Asphalt,
                  65)
        {
        }

        public VehicleViewModel(
            string name,
            int enginePower,
            double weight,
            ModelService.Environment resistance,
            int tankCapacity)
        {
            _currentData = new VehicleModel(name,
                                            enginePower,
                                            weight,
                                            resistance,
                                            tankCapacity);
            _gibdd = new Gibdd();
            _methodsCheck = new Dictionary<string, CheckParam>
            {
                { nameof(this.Name),
                    ((VehicleModel value) => _gibdd.CheckName(value)) },
                { nameof(this.EnginePower),
                    ((VehicleModel value) => _gibdd.CheckEnginePower(value))},
                { nameof(this.Weight),
                    ((VehicleModel value) => _gibdd.CheckWeight(value)) },
                { nameof(this.TankCapacity),
                    ((VehicleModel value) => _gibdd.CheckTankCapacity(value))}
            };
        }

        public VehicleViewModel(Vehicle v)
            : this(v.Name,
                  v.EnginePower,
                  v.Weight,
                  v.ResistanceWithMedian,
                  v.TankCapacity)
        { }

        public Vehicle GetVehicleModel => _currentData.ToVehicle();

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
            Enum.GetNames(typeof(ModelService.Environment))
            .ToList();

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
                var vehicle = _currentData.ToVehicle();
                var result = Manager.CalcFuelConsumption(vehicle);

                return result;
            }
        }

        string IDataErrorInfo.Error => null;

        public bool IsValid
        {
            get
            {
                var valid = true;
                foreach (var p in _isValidParams)
                {
                    valid = p.Value;
                    if (valid == false) break;
                }
                return valid;
            }
        }
        private Dictionary<string, bool> _isValidParams = 
            new Dictionary<string, bool>();

        string IDataErrorInfo.this[string column]
        {
            get
            {
                var result = String.Empty;

                if (_methodsCheck.ContainsKey(column))
                {
                    result = _methodsCheck[column](_currentData);
                    if (_isValidParams.ContainsKey(column) == false)
                    {
                        _isValidParams.Add(column, true);
                    }
                    _isValidParams[column] = (result == "") ? true : false;
                }

                return result;
            }
        }



        public override string ToString() =>
            $"{Name}, " +
            $"{EnginePower:d}, " +
            $"{Weight:f}, " +
            $"{Resistance:G}, " +
            $"{TankCapacity:D}, " +
            $"{FuelConsumption}";

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
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
            _copyData = null;
        }

        #endregion
    }
}
