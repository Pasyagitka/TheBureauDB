using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Views;
using TheBureau.Views.AdminWindow;
using TheBureau.Views.AdminWindow.Employee;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class EmployeeViewModel : ViewModelBase
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;
        private readonly string _exportConnectionString = ConfigurationManager.ConnectionStrings["ExportConnection"].ConnectionString;

        private ObservableCollection<Employee> _employees = new();
        private Brigade _employeeBrigades;
        
        private Employee _selectedItem;
        private string _findEmployeesText;
        
        private ICommand _deleteCommand;
        private ICommand _openEditEmployeeWindowCommand;
        private ICommand _openAddEmployeeWindowCommand;
        private ICommand _openFolder;

        #region Properties

        public Employee SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                SetEmployeeBrigade();
                OnPropertyChanged("SelectedItem");
            }
        }
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set { _employees = value; OnPropertyChanged("Employees"); }
        }
        public Brigade EmployeeBrigade
        {
            get => _employeeBrigades;
            set { _employeeBrigades = value; OnPropertyChanged("EmployeeBrigade"); }
        }
        public string FindEmployeeText
        {
            get => _findEmployeesText;
            set
            {
                _findEmployeesText = value;
                //Search(_findEmployeesText); 
                OnPropertyChanged("FindEmployeeText");
            }
        }

        #endregion
        
        public EmployeeViewModel()
        {
            try
            {
                GetAllEmployees();
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при открытии страницы работников");
                infoWindow.ShowDialog();
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        var empl = SelectedItem;
                        if (empl != null)
                        {
                            int emplId = empl.id;
                            
                            using (SqlConnection conn = new SqlConnection(_connectionString))
                            {
                                conn.Open();
                                using (SqlCommand cmd = new SqlCommand("DeleteEmployee", conn) {CommandType = CommandType.StoredProcedure})
                                {
                                    cmd.Parameters.AddWithValue("@id", emplId);
                                    using (SqlDataReader reader = cmd.ExecuteReader()) { while (reader.Read()) { }; } //todo delete employee - hide!
                                }
                                conn.Close();
                            }
                            Employees.Remove(SelectedItem);
                            if (Employees.Count != 0) SelectedItem = Employees.First();
                        }
                    }
                    catch (Exception)
                    {
                        InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при удалении работника");
                        infoWindow.ShowDialog();
                    }
                });
            }
        }
        
        public ICommand OpenEditEmployeeWindowCommand => _openEditEmployeeWindowCommand ??= new RelayCommand(OpenEditEmployeeWindow);
        public ICommand OpenAddEmployeeWindowCommand => _openAddEmployeeWindowCommand ??= new RelayCommand(OpenAddEmployeeWindow);

        private void OpenAddEmployeeWindow(object sender)
        {
            try
            {
                AddEmployeeView view = new();
                if (view.ShowDialog() == true)
                {
                    GetAllEmployees();
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при добавлении работника");
                infoWindow.ShowDialog();
            }
        }
        
        private void OpenEditEmployeeWindow(object sender)
        {
            try
            {
                var employeeToEdit = SelectedItem;
                EditEmployeeView window = new(employeeToEdit);
                if (window.ShowDialog() == true)
                {
                    Employee employee = new Employee();
                    using (SqlConnection conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("GetEmployee", conn) {CommandType = CommandType.StoredProcedure})
                        {
                            cmd.Parameters.AddWithValue("@id", employeeToEdit.id);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read()) 
                                {
                                    employee.id = (int) reader["id"];
                                    employee.firstname = (string) reader["firstname"];
                                    employee.patronymic = (string) reader["patronymic"];
                                    employee.surname = (string) reader["surname"];
                                    employee.email = reader["email"] == DBNull.Value ? null : Convert.ToString(reader["email"]);
                                    employee.contactNumber = reader["contactNumber"] == DBNull.Value ? null : Convert.ToString(reader["contactNumber"]);
                                    employee.brigadeId = reader["brigadeId"] == DBNull.Value ? (Int32?) null : Convert.ToInt32(reader["brigadeId"]);
                                    Employees[Employees.IndexOf(SelectedItem)] = employee; 
                                }
                                ;
                            }
                        }
                        conn.Close();
                    }
                    if (Employees.Count != 0) SelectedItem = Employees.First();
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при изменении работника");
                infoWindow.ShowDialog();
            }
        }


        void GetAllEmployees()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("GetAllEmployees", conn) {CommandType = CommandType.StoredProcedure})
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employees.Add(new Employee
                            {
                                id = (int) reader["id"],
                                firstname = (string) reader["firstname"],
                                patronymic = (string) reader["patronymic"],
                                surname = (string) reader["surname"],
                                email = reader["email"] == DBNull.Value ? null : Convert.ToString(reader["email"]),
                                contactNumber = reader["contactNumber"] == DBNull.Value ? null : Convert.ToString(reader["contactNumber"]),
                                brigadeId = reader["brigadeId"] == DBNull.Value ? (Int32?) null : Convert.ToInt32(reader["brigadeId"])
                            });
                        }
                        ;
                    }
                }
            }
            if (Employees.Count != 0) SelectedItem = Employees.First();
        }
        
    
        void SetEmployeeBrigade()
        {
            if (SelectedItem != null)
            {
                EmployeeBrigade = null;
                if (SelectedItem.brigadeId == null) return;
                using SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("GetBrigade", conn)  {CommandType = CommandType.StoredProcedure})
                {
                    cmd.Parameters.AddWithValue("@id", SelectedItem.brigadeId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Brigade a = new Brigade
                            {
                                id = (int) reader["id"],
                                userId = reader["userId"] == DBNull.Value   ? (Int32?) null  : Convert.ToInt32(reader["userId"]),
                                brigadierId = reader["brigadierId"] == DBNull.Value   ? (Int32?) null  : Convert.ToInt32(reader["brigadierId"]),
                                creationDate = (DateTime) reader["creationDate"]
                            };
                            SetBrigadeEmployees(a);
                            EmployeeBrigade = a;
                        }
                        ;
                    }
                }
                conn.Close();
            }
        }
        
        void SetBrigadeEmployees(Brigade brigade)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("GetEmployeesForBrigade", conn) {CommandType = CommandType.StoredProcedure})
                {
                    cmd.Parameters.AddWithValue("@brigadeId", brigade.id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            brigade.Employees.Add(new Employee
                            {
                                id = (int) reader["id"],
                                firstname = (string) reader["firstname"],
                                patronymic = (string) reader["patronymic"],
                                surname = (string) reader["surname"],
                                email = (string) reader["email"],
                                contactNumber = (string) reader["contactNumber"]
                            });
                        }
                        ;
                    }
                }
                conn.Close();
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при загрузке работников бригад");
                infoWindow.ShowDialog();
            }
        }
        
                     #region Export
        
        public ICommand ExportCommand => _openFolder ??= new RelayCommand(OpenFolder);
          private void OpenFolder(object sender)
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
                {
                    RootFolder = Environment.SpecialFolder.Desktop,
                };
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;
                if (ExportSchedule(folderBrowserDialog.SelectedPath))
                {
                    InfoWindow infoWindow =
                        new InfoWindow("Экспортировано в файл", folderBrowserDialog.SelectedPath);
                    infoWindow.ShowDialog();
                }
                else
                {
                    InfoWindow infoWindow = new InfoWindow("Данные не были экспортированы в файл",
                        folderBrowserDialog.SelectedPath);
                    infoWindow.ShowDialog();
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow =
                    new InfoWindow("Ошибка", "Ошибка при отрытии диалогового окна для выбора файла");
                infoWindow.ShowDialog();
            }
        }

  
        private bool ExportSchedule(string path)
        {
            var success = 1;
            string filename;
            using var conn = new SqlConnection(_exportConnectionString);
            conn.Open();
            using (var cmd = new SqlCommand("ExportScheduleForEmployee", conn) {CommandType = CommandType.StoredProcedure})
            {
                cmd.Parameters.AddWithValue("@path", path);
                cmd.Parameters.AddWithValue("@employeeId", SelectedItem.id);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        filename = (string) reader["filename"];
                        success = (int) reader["success"];
                    }
                    ;
                }
            }
            conn.Close();
            return success == 0; //@result = 0 - success, otherwise failure
        }
        #endregion
    }
}