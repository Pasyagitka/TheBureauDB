using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Services;
using TheBureau.ViewModels;
using TheBureau.Views.AdminWindow.Brigade;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class BrigadeViewModel : ViewModelBase
    {
        private const string BrigadePassword = "brigade";
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;

        private ObservableCollection<Brigade> _brigades = new();
        private ICommand _openSetBrigadierCommand;
        private ICommand _addBrigade;
        private ICommand _deleteBrigade;

        private Brigade _selectedItem;

        #region Properties 
        public Brigade SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged("SelectedItem"); }
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
        #endregion

        public ICommand OpenSetBrigadierCommand => _openSetBrigadierCommand ??= new RelayCommand(OpenSetBrigadier);
        public ICommand AddBrigade
        {
            get
            {
                return _addBrigade ??= new RelayCommand(obj =>
                {
                    try
                    {
                        using SqlConnection conn = new SqlConnection(_connectionString);
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("AddBrigade", conn) {CommandType = CommandType.StoredProcedure})
                        {
                            cmd.Parameters.AddWithValue("@password", PasswordHash.CreateHash(BrigadePassword));
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Brigades.Add(new Brigade
                                    {
                                        id = (int)reader["id"],
                                        userId = reader["userId"] == DBNull.Value ? (Int32?) null : Convert.ToInt32(reader["userId"]),
                                        brigadierId = reader["brigadierId"] == DBNull.Value ? (Int32?) null : Convert.ToInt32(reader["brigadierId"]),
                                        creationDate = (DateTime)reader["creationDate"]
                                    });
                                };
                            }
                        }
                        conn.Close();
                    }
                    catch (Exception)
                    {
                        InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при создании бригады");
                        infoWindow.ShowDialog();
                    }
                });
            }
        }
        public ICommand DeleteBrigade
        {
            get
            {
                return _deleteBrigade ??= new RelayCommand(obj =>
                {
                    try
                    {
                        if (SelectedItem != null)
                        {
                            using (SqlConnection conn = new SqlConnection(_connectionString))
                            {
                                conn.Open();
                                using (SqlCommand cmd = new SqlCommand("DeleteBrigade", conn) {CommandType = CommandType.StoredProcedure})
                                {
                                    cmd.Parameters.AddWithValue("@id", SelectedItem.id);
                                    using (SqlDataReader reader = cmd.ExecuteReader())
                                    {
                                        while (reader.Read()) { };
                                    }
                                }
                                conn.Close();
                            }
                            Brigades.Remove(SelectedItem);
                            if (Brigades.Count!=0) SelectedItem = Brigades.First();
                        }
                    }
                    catch (Exception)
                    {
                        InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при удалении бригады");
                        infoWindow.ShowDialog();
                    }
                });
            }
        }

        public BrigadeViewModel()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetAllBrigades", conn)  { CommandType = CommandType.StoredProcedure }) {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())  
                            {
                                Brigades.Add(new Brigade
                                {
                                    id = (int)reader["id"],
                                    brigadierId = reader["brigadierId"] == DBNull.Value ? (Int32?) null : Convert.ToInt32(reader["brigadierId"])
                                });
                            };
                        }
                    }
                    conn.Close();
                }
                SetBrigadeEmployees();
                if (Brigades.Count != 0) SelectedItem = Brigades.First();
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при открытии страницы бригады");
                infoWindow.ShowDialog();
            }
        }
        void SetBrigadeEmployees()
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();
                foreach (Brigade brigade in Brigades)
                {
                    using SqlCommand cmd = new SqlCommand("GetEmployeesForBrigade", conn)  {CommandType = CommandType.StoredProcedure};
                    cmd.Parameters.AddWithValue("@brigadeId", brigade.id);
                    using SqlDataReader reader = cmd.ExecuteReader();
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
                conn.Close();
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при загрузке работников бригад");
                infoWindow.ShowDialog();
            }
        }


        private void OpenSetBrigadier(object commandParameter)
        {
            try
            {
                if (SelectedItem != null)
                {
                    EditBrigadeView window = new(SelectedItem);
                    window.ShowDialog();
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при редактировании заявки");
                infoWindow.ShowDialog();
            }
        }
        
        
    }
}