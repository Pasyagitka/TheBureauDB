using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Services;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class AddEquipmentViewModel : ViewModelBase
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;
        private readonly ErrorsViewModel _errorsViewModel = new();

        private string _id;
        private string _type;
        private Equipment _equipment;
        private int _selectedMountingId;
     

        private ICommand _addEquipmentCommand;
        private ObservableCollection<Mounting> _mountings;
        
        public string Type
        {
            get => _type;
            set
            { 
                _type = value; 
                _errorsViewModel.ClearErrors("Type");
                
                if (string.IsNullOrWhiteSpace(_type))
                {
                    _errorsViewModel.AddError("Type", ValidationConst.FieldCannotBeEmpty);
                }
                if (_type?.Length is > 30 or < 2)
                {
                    _errorsViewModel.AddError("Type", ValidationConst.MaxLength30);
                }
                OnPropertyChanged("Type");
            }
        }
        public string Id
        {
            get => _id;
            set { 
                _id = value; 
                _errorsViewModel.ClearErrors("Id");
                if (_id.Length is > 3 or < 1)
                {
                    _errorsViewModel.AddError("Id", ValidationConst.MaxLength3);
                }
                OnPropertyChanged("Id");
            }
        }
        public ObservableCollection<Mounting> Mountings
        {
            get => _mountings;
            set
            {
                _mountings = value;
                OnPropertyChanged("Mountings");
            }
        }

        public int SelectedMountingId
        {
            get => _selectedMountingId;
            set
            {
                _selectedMountingId = value;
                OnPropertyChanged("SelectedMountingId");
            }
        }
        public Equipment Equipment
        {
            get => _equipment;
            set
            { 
                _equipment = value;
                Id = Equipment.id;
                Type = Equipment.type;
                SelectedMountingId = Equipment.mounting;
                OnPropertyChanged("Tool");
            }
        }

        
        public ICommand AddEquipmentCommand => _addEquipmentCommand ??= new RelayCommand(AddEquipment, CanAddEquipment);
        private bool CanAddEquipment(object sender) => !HasErrors;
        private void AddEquipment(object sender)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("AddEquipment", conn) {CommandType = CommandType.StoredProcedure})
                {
                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd.Parameters.AddWithValue("@type", Type);
                    cmd.Parameters.AddWithValue("@mountingId", SelectedMountingId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) { };
                    }
                }
                conn.Close();
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при добавлении оборудования");
                infoWindow.ShowDialog();
            }
        }
        
        
        public AddEquipmentViewModel()
        {
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            _errorsViewModel.AddError("Id", ValidationConst.FieldCannotBeEmpty);
            _errorsViewModel.AddError("Type", ValidationConst.FieldCannotBeEmpty);
            
            Mountings = new ObservableCollection<Mounting>();
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("GetAllMountings", conn)  { CommandType = CommandType.StoredProcedure }) {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())  
                    {
                        Mounting a = new Mounting
                        {
                            id = (int)reader["id"],
                            mounting = (string)reader["mounting"]
                        };
                        Mountings.Add(a);
                    };
                }
            }
            conn.Close();
            if (Mountings != null) SelectedMountingId = Mountings.First().id;
        }
        
        #region Validation
        public IEnumerable GetErrors(string propertyName) => _errorsViewModel.GetErrors(propertyName);
        public bool HasErrors => _errorsViewModel.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged("CanAddEquipment");
        }
        #endregion
    }
}