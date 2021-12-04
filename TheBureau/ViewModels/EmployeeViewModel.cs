using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Views;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class EmployeeViewModel : ViewModelBase
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;

        private ObservableCollection<Employee> _employees = new();
        private Brigade _employeeBrigades;
        
        private Employee _selectedItem;
        private string _findEmployeesText;
        
        private ICommand _deleteCommand;
        private ICommand _openEditEmployeeWindowCommand;
        private ICommand _openAddEmployeeWindowCommand;
        
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set { _employees = value; OnPropertyChanged("Employees"); }
        }
        public EmployeeViewModel()
        {
            try
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
                                Employee a = new Employee
                                {
                                    id = (int) reader["id"],
                                    firstname = (string) reader["firstname"],
                                    patronymic = (string) reader["patronymic"],
                                    surname = (string) reader["surname"],
                                    email = reader["email"] == DBNull.Value ? null : Convert.ToString(reader["email"]),
                                    contactNumber = reader["contactNumber"] == DBNull.Value ? null : Convert.ToString(reader["contactNumber"]),
                                    brigadeId = reader["brigadeId"] == DBNull.Value ? (Int32?) null : Convert.ToInt32(reader["brigadeId"])
                                };
                                Employees.Add(a);
                            }
                            ;
                        }
                    }
                }
                if (Employees.Count != 0) SelectedItem = Employees.First();
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
                                    using (SqlDataReader reader = cmd.ExecuteReader()) { while (reader.Read()) { }; }
                                }
                                conn.Close();
                            }
                            Employees.Remove(SelectedItem);
                            if (SelectedItem != null) SelectedItem = Employees.First();
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
                    //Update(); - отобразить добавленного работника
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
                    //Update();
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
                                }
                                ;
                            }
                        }
                        conn.Close();
                    }
                    if (SelectedItem != null) SelectedItem = employee;
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при изменении работника");
                infoWindow.ShowDialog();
            }
        }

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
        
        // public void Update()
        // {
        //     try
        //     {
        //         using (SqlConnection conn = new SqlConnection(_connectionString))
        //         {
        //             conn.Open();
        //             using (SqlCommand cmd = new SqlCommand("GetAllEmployees", conn) {CommandType = CommandType.StoredProcedure})
        //             {
        //                 using (SqlDataReader reader = cmd.ExecuteReader())
        //                 {
        //                     while (reader.Read())
        //                     {
        //                         Employee a = new Employee();
        //                         a.id = (int) reader["id"];
        //                         a.firstname = (string) reader["firstname"];
        //                         a.patronymic = (string) reader["patronymic"];
        //                         a.surname = (string) reader["surname"];
        // employee.email = reader["email"] == DBNull.Value ? null : Convert.ToString(reader["email"]);
        // employee.contactNumber = reader["contactNumber"] == DBNull.Value ? null : Convert.ToString(reader["contactNumber"]);
        //                         Employees.Add(a);
        //                     }
        //                     ;
        //                 }
        //             }
        //         }
        //         
        //         Employees = new ObservableCollection<Employee>(_employeeRepository.GetAll());
        //         EmployeeBrigade = new ObservableCollection<Brigade>(_brigadeRepository.GetAll());
        //         SelectedItem = Employees.First();
        //     }
        //     catch (Exception)
        //     {
        //         InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при обновлении данных");
        //         infoWindow.ShowDialog();
        //     }
        // }
        
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

        void SetEmployeeBrigade()
        {
            if (SelectedItem != null)
            {
                EmployeeBrigade = null;
                if (SelectedItem.brigadeId == null) return;
                using SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("GetBrigade", conn)
                    {CommandType = CommandType.StoredProcedure})
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
                            Employee a = new Employee
                            {
                                id = (int) reader["id"],
                                firstname = (string) reader["firstname"],
                                patronymic = (string) reader["patronymic"],
                                surname = (string) reader["surname"],
                                email = (string) reader["email"],
                                contactNumber = (string) reader["contactNumber"]
                            };
                            brigade.Employees.Add(a);
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
    }
}