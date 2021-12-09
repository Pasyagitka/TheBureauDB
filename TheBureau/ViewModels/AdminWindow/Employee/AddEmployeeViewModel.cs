using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Services;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class AddEmployeeViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;

        private readonly ErrorsViewModel _errorsViewModel = new ErrorsViewModel();

        private ObservableCollection<Brigade> _brigades = new();
        int _selectedBrigadeId;

        private int _id;
        private string _surname;
        private string _firstname;
        private string _patronymic;
        private string _email;
        private string _contactNumber;
        private int? _brigadeid;
        private Employee _employee;

        private ICommand _addEmployeeCommand;

        #region Properties
        public int SelectedBrigadeId
        {
            get => _selectedBrigadeId;
            set
            {
                _selectedBrigadeId = value;
                OnPropertyChanged("SelectedBrigadeId");
            }
        }

        public ObservableCollection<Brigade> Brigades
        {
            get => _brigades;
            set
            {
                _brigades = value;
                OnPropertyChanged("Brigades");
            }
        }
        public Employee Employee
        {
            get => _employee;
            set
            {
                _employee = value;
                Id = Employee.id;
                Firstname = Employee.firstname;
                Surname = Employee.surname;
                Patronymic = Employee.patronymic;
                Email = Employee.email;
                ContactNumber = Employee.contactNumber;
                BrigadeId = Employee.brigadeId;
                OnPropertyChanged("Employee");
            }
        }
        #endregion

        #region EmployeePropetries

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                _errorsViewModel.ClearErrors("Surname");

                if (string.IsNullOrWhiteSpace(_surname))
                {
                    _errorsViewModel.AddError("Surname", ValidationConst.FieldCannotBeEmpty);
                }

                if (_surname?.Length is > 20 or < 2)
                {
                    _errorsViewModel.AddError("Surname", ValidationConst.NameLengthExceeded);
                }

                if (_surname != null)
                {
                    var regex = new Regex(ValidationConst.LettersHyphenRegex);
                    if (!regex.IsMatch(_surname))
                    {
                        _errorsViewModel.AddError("Surname", ValidationConst.IncorrectSurname);
                    }
                }

                OnPropertyChanged("Surname");
            }
        }

        public string Firstname
        {
            get => _firstname;
            set
            {
                _firstname = value;
                _errorsViewModel.ClearErrors("Firstname");

                if (string.IsNullOrWhiteSpace(_firstname))
                {
                    _errorsViewModel.AddError("Firstname", ValidationConst.FieldCannotBeEmpty);
                }

                if (_firstname?.Length is > 20 or < 2)
                {
                    _errorsViewModel.AddError("Firstname", ValidationConst.NameLengthExceeded);
                }

                if (_firstname != null)
                {
                    var regex = new Regex(ValidationConst.LettersHyphenRegex);
                    if (!regex.IsMatch(_firstname))
                    {
                        _errorsViewModel.AddError("Firstname", ValidationConst.IncorrectFirstname);
                    }
                }

                OnPropertyChanged("Firstname");
            }
        }

        public string Patronymic
        {
            get => _patronymic;
            set
            {
                _patronymic = value;
                _errorsViewModel.ClearErrors("Patronymic");

                if (string.IsNullOrWhiteSpace(_patronymic))
                {
                    _errorsViewModel.AddError("Patronymic", ValidationConst.FieldCannotBeEmpty);
                }

                if (_patronymic?.Length is > 20 or < 2)
                {
                    _errorsViewModel.AddError("Patronymic", ValidationConst.NameLengthExceeded);
                }

                if (_patronymic != null)
                {
                    var regex = new Regex(ValidationConst.LettersHyphenRegex);
                    if (!regex.IsMatch(_patronymic))
                    {
                        _errorsViewModel.AddError("Patronymic", ValidationConst.IncorrectPatronymic);
                    }
                }

                OnPropertyChanged("Patronymic");
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                _errorsViewModel.ClearErrors("Email");
                if (string.IsNullOrWhiteSpace(_email))
                {
                    _errorsViewModel.AddError("Email", ValidationConst.FieldCannotBeEmpty);
                }

                if (_email?.Length > 255)
                {
                    _errorsViewModel.AddError("Email", ValidationConst.EmailLengthExceeded);
                }

                if (_email != null)
                {
                    var regex = new Regex(ValidationConst.EmailRegex);
                    if (!regex.IsMatch(_email))
                    {
                        _errorsViewModel.AddError("Email", ValidationConst.IncorrectEmailStructure);
                    }
                }

                OnPropertyChanged("Email");
            }
        }

        public string ContactNumber
        {
            get => _contactNumber;
            set
            {
                _contactNumber = value;
                _errorsViewModel.ClearErrors("ContactNumber");

                if (string.IsNullOrWhiteSpace(_contactNumber.ToString()))
                {
                    _errorsViewModel.AddError("ContactNumber", ValidationConst.FieldCannotBeEmpty);
                }

                if (ContactNumber != null)
                {
                    var regex = new Regex(ValidationConst.ContactNumberRegex);
                    if (!regex.IsMatch(_contactNumber.ToString()))
                    {
                        _errorsViewModel.AddError("ContactNumber", ValidationConst.IncorrectNumberStructure);
                    }
                }

                OnPropertyChanged("ContactNumber");
            }
        }

        public int? BrigadeId
        {
            get => _brigadeid;
            set
            {
                _brigadeid = value;
                OnPropertyChanged("BrigadeId");
            }
        }

        #endregion
        
        public AddEmployeeViewModel()
        {
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            _errorsViewModel.AddError("Surname", ValidationConst.FieldCannotBeEmpty);
            _errorsViewModel.AddError("Firstname", ValidationConst.FieldCannotBeEmpty);
            _errorsViewModel.AddError("Patronymic", ValidationConst.FieldCannotBeEmpty);
            _errorsViewModel.AddError("Email", ValidationConst.FieldCannotBeEmpty);
            _errorsViewModel.AddError("ContactNumber", ValidationConst.FieldCannotBeEmpty);

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("GetAllBrigades", conn) {CommandType = CommandType.StoredProcedure})
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Brigades.Add(new Brigade
                            {
                                id = (int) reader["id"],
                                userId = reader["userId"] == DBNull.Value
                                    ? (Int32?) null
                                    : Convert.ToInt32(reader["userId"]),
                                brigadierId = reader["brigadierId"] == DBNull.Value
                                    ? (Int32?) null
                                    : Convert.ToInt32(reader["brigadierId"]),
                                creationDate = (DateTime) reader["creationDate"]
                            });
                        }
                        ;
                    }
                }
                conn.Close();
            }
            Brigades.Add(new Brigade {id = 0});
        }

        public ICommand AddEmployeeCommand => _addEmployeeCommand ??= new RelayCommand(AddEmployee, CanAddEmployee);
        private bool CanAddEmployee(object sender) => !HasErrors;

        private void AddEmployee(object sender)
        {
            try
            {
                //decimal newEmployeeId = 0;
                using SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("AddEmployee", conn)
                    {CommandType = CommandType.StoredProcedure})
                {
                    cmd.Parameters.AddWithValue("@firstname", Firstname);
                    cmd.Parameters.AddWithValue("@patronymic", Patronymic);
                    cmd.Parameters.AddWithValue("@surname", Surname);
                    cmd.Parameters.AddWithValue("@email", Email);
                    cmd.Parameters.AddWithValue("@contactNumber", ContactNumber);
                    cmd.Parameters.AddWithValue("@brigadeId",
                        SelectedBrigadeId == 0 ? DBNull.Value : SelectedBrigadeId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //newEmployeeId = (decimal)reader["id"];
                        }
                        ;
                    }
                }
                conn.Close();
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при создании работника");
                infoWindow.ShowDialog();
            }
        }

        #region Validation

        public IEnumerable GetErrors(string propertyName) => _errorsViewModel.GetErrors(propertyName);
        public bool HasErrors => _errorsViewModel.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged("CanAddEmployee");
        }

        #endregion
    }
}