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
    public class EditAccessoryViewModel : ViewModelBase
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;
        private readonly ErrorsViewModel _errorsViewModel = new();

        private string _art;
        private string _name;
        private decimal _price;
        private string _selectedEquipmentId;
        private Accessory _accessory;
     
        private int _id;

        private ICommand _editAccessoryCommand;
        private ObservableCollection<Equipment> _equipments;
        
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged("Id"); }
        }
        public string Name
        {
            get => _name;
            set
            { 
                _name = value; 
                _errorsViewModel.ClearErrors("Name");
                if (string.IsNullOrWhiteSpace(_name))
                {
                    _errorsViewModel.AddError("Name", ValidationConst.FieldCannotBeEmpty);
                }
                if (_name?.Length is > 150 or < 2)
                {
                    _errorsViewModel.AddError("Name", ValidationConst.MaxLength150);
                }
                OnPropertyChanged("Name");
            }
        }
        public string Price
        {
            get => _price.ToString();
            set { 
                _price = decimal.Parse(value); 
                _errorsViewModel.ClearErrors("Price");
                if (string.IsNullOrWhiteSpace(_price.ToString()))
                {
                    _errorsViewModel.AddError("Price", ValidationConst.FieldCannotBeEmpty);
                }
                OnPropertyChanged("Price"); 
            }
        }
        
        public ObservableCollection<Equipment> Equipments
        {
            get => _equipments;
            set
            {
                _equipments = value;
                OnPropertyChanged("Equipments");
            }
        }

        public string SelectedEquipmentId
        {
            get => _selectedEquipmentId;
            set
            {
                _selectedEquipmentId = value;
                _errorsViewModel.ClearErrors("SelectedEquipmentId");
                if (string.IsNullOrWhiteSpace(_selectedEquipmentId))
                {
                    _errorsViewModel.AddError("SelectedEquipmentId", ValidationConst.FieldCannotBeEmpty);
                }
                if (_selectedEquipmentId?.Length is > 3)
                {
                    _errorsViewModel.AddError("SelectedEquipmentId", ValidationConst.MaxLength3);
                }
                OnPropertyChanged("SelectedEquipmentId");
            }
        }
        public Accessory Accessory
        {
            get => _accessory;
            set
            { 
                _accessory = value;
                Id = Accessory.id;
                Art = Accessory.art;
                Name = Accessory.name;
                Price = Accessory.price.ToString();
                SelectedEquipmentId = Accessory.equipmentId;
                OnPropertyChanged("Accessory");
            }
        }

        public string Art { 
            get => _art;
            set
            {
                _art = value;
                _errorsViewModel.ClearErrors("Art");
                if (_art?.Length is > 15)
                {
                    _errorsViewModel.AddError("Art", ValidationConst.MaxLength15);
                }
                OnPropertyChanged("Art");
            } 
        }


        public ICommand EditAccessoryCommand => _editAccessoryCommand ??= new RelayCommand(EditAccessory, CanEditAccessory);
        private bool CanEditAccessory(object sender) => !HasErrors;
        private void EditAccessory(object sender)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UpdateAccessory", conn) {CommandType = CommandType.StoredProcedure})
                {
                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd.Parameters.AddWithValue("@art", Art == null? DBNull.Value : Art);
                    cmd.Parameters.AddWithValue("@name", Name);
                    cmd.Parameters.AddWithValue("@equipmentId", SelectedEquipmentId);
                    cmd.Parameters.AddWithValue("@price", _price);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) { };
                    }
                }
                conn.Close();
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при редактировании комплектующего");
                infoWindow.ShowDialog();
            }
        }
        
        
        public EditAccessoryViewModel(Accessory selectedAccessory)
        {
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            Accessory = selectedAccessory;
            
            Equipments = new ObservableCollection<Equipment>();
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("GetAllEquipment", conn)  { CommandType = CommandType.StoredProcedure }) {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())  
                    {
                        var a = new Equipment()
                        {
                            id = (string)reader["id"],
                            type = (string)reader["type"],
                            mounting = (int)reader["mountingId"]
                        };
                        Equipments.Add(a);
                    };
                }
            }
            conn.Close();
            if (Equipments != null) SelectedEquipmentId = Equipments.First().id;
        }
        
        #region Validation
        public IEnumerable GetErrors(string propertyName) => _errorsViewModel.GetErrors(propertyName);
        public bool HasErrors => _errorsViewModel.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged("CanEditAccessory");
        }
        #endregion
    }
}