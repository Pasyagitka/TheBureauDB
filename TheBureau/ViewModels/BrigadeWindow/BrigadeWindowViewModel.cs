using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Views.AuthWindow;
using TheBureau.Views.BrigadeWindow;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class BrigadeWindowViewModel : ViewModelBase
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["BrigadeConnection"].ConnectionString;

        
        private ObservableCollection<Request> _brigadeRequests = new();
        private ObservableCollection<RequestEquipment> _requestEquipments = new();
        private Brigade _currentBrigade;
        private Request _selectedItem;
        private string _findRequestText;
        private WindowState _windowState;

        #region  Command
        private ICommand _logOutCommand;
        private ICommand _updateRequest;
        private ICommand _closeWindowCommand;
        private ICommand _minimizeWindowCommand;
        public ICommand UpdateRequestCommand => _updateRequest ??= new RelayCommand(OpenEditRequest);
        public ICommand CloseWindowCommand => _closeWindowCommand ??= new RelayCommand(obj => { Application.Current.Shutdown(); });
        public ICommand MinimizeWindowCommand => _minimizeWindowCommand ??= new RelayCommand(obj => { WindowState = WindowState.Minimized; });
        public ICommand LogOutCommand
        {
            get
            {
                return _logOutCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        Application.Current.Properties["User"] = null;
                        var helloWindow = new HelloWindowView();
                        helloWindow.Show();
                        Application.Current.Windows[0]?.Close();
                    }
                    catch (Exception)
                    {
                        InfoWindow infoWindow = new InfoWindow("Ошибка", "Не удалось выйти из аккаунта");
                        infoWindow.ShowDialog();
                    }
                });
            }
        }
        #endregion
        
        #region Properties
        public WindowState  WindowState
        {
            get => _windowState;
            set { _windowState = value; OnPropertyChanged("WindowState"); }
        }
        public string FindRequestText
        {
            get => _findRequestText;
            set
            {
                _findRequestText = value;
                //Search();
                OnPropertyChanged("FindRequestText");
            }
        }
        
        public Brigade CurrentBrigade
        {
            get => _currentBrigade;
            set { _currentBrigade = value; OnPropertyChanged("CurrentBrigade"); }
        }
        public ObservableCollection<RequestEquipment> RequestEquipments
        {
            get => _requestEquipments;
            set { _requestEquipments = value; OnPropertyChanged("RequestEquipments");}
        }
        public ObservableCollection<Request> BrigadeRequests
        {
            get => _brigadeRequests;
            set { _brigadeRequests = value; OnPropertyChanged("BrigadeRequests"); }
        }
        public Request SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                SetEquipment();
                OnPropertyChanged("SelectedItem");
            }
        }
        #endregion
        
        public BrigadeWindowViewModel()
        {
            try
            {
                WindowState = WindowState.Normal;
                var user = Application.Current.Properties["User"] as User;
                if (user != null)
                {
                    GetBrigade(user);
                    SetBrigadeRequests();
                }
            }
            catch (Exception e)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при открытии окна бригады");
                infoWindow.ShowDialog();
            }
        }
        void GetBrigade(User user)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("GetBrigadeByUserId", conn) {CommandType = CommandType.StoredProcedure})
            {
                cmd.Parameters.AddWithValue("@userId", user.id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Brigade a = new()
                        {
                            id = (int) reader["id"],
                            userId = reader["userId"] == DBNull.Value   ? (Int32?) null  : Convert.ToInt32(reader["userId"]),
                            brigadierId = reader["brigadierId"] == DBNull.Value   ? (Int32?) null  : Convert.ToInt32(reader["brigadierId"])
                        };
                        SetBrigadeEmployees(a);
                        CurrentBrigade = a;
                    }
                    ;
                }
            }
            conn.Close();
        }
        private void SetEquipment()
        {
            if (SelectedItem != null)
            {
                RequestEquipments.Clear();
                using SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("GetRequestEquipmentByRequestId", conn) {CommandType = CommandType.StoredProcedure})
                {
                    cmd.Parameters.AddWithValue("@requestId", SelectedItem.id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RequestEquipments.Add(new RequestEquipment()
                            {
                                quantity = (int) reader["quantity"],
                                equipmentId = (string) reader["equipmentId"],
                                Equipment = new()
                                {
                                    type = (string) reader["type"],
                                    mountingName = (string) reader["mounting"]
                                }
                            });
                        }
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
                using (SqlCommand cmd = new SqlCommand("GetEmployeesForBrigade", conn)  {CommandType = CommandType.StoredProcedure})
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

        void SetBrigadeRequests()
        {
            if (CurrentBrigade != null)  {
                BrigadeRequests.Clear();
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetAllRequestsForBrigade", conn)  {CommandType = CommandType.StoredProcedure}) {
                        cmd.Parameters.AddWithValue("@brigadeId", CurrentBrigade.id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Request a = new Request
                                {
                                    id = (int) reader["id"],
                                    clientId = (int) reader["clientId"],
                                    addressId = (int) reader["addressId"],
                                    stage = (int) reader["stageId"],
                                    status = (int) reader["statusId"],
                                    registerDate = (DateTime) reader["registerDate"],
                                    mountingDate = (DateTime) reader["mountingDate"],
                                    comment = (string) reader["comment"],
                                    brigadeId = reader["brigadeId"] == DBNull.Value  ? (Int32?) null   : Convert.ToInt32(reader["brigadeId"]),
                                    proceeds = reader["proceeds"] == DBNull.Value   ? (Int32?) null   : Convert.ToDecimal(reader["proceeds"]),
                                    Client = new Client
                                    {
                                        id = (int) reader["clientId"],
                                        firstname = (string) reader["firstname"],
                                        patronymic = (string) reader["patronymic"],
                                        surname = (string) reader["surname"],
                                        email = (string) reader["email"],
                                        contactNumber = (string)reader["contactNumber"]
                                    },
                                    Address = new Address
                                    {
                                        id = (int) reader["addressId"],
                                        country = (string) reader["country"],
                                        city = (string) reader["city"],
                                        street = (string) reader["street"],
                                        house = (int) reader["house"],
                                        corpus = reader["corpus"] == DBNull.Value ? null : Convert.ToString(reader["corpus"]),
                                        flat = reader["flat"] == DBNull.Value ? (Int32?) null : Convert.ToInt32(reader["flat"])
                                    }
                                };
                                BrigadeRequests.Add(a);
                            }
                        }
                    }
                    conn.Close();
                }
                if (BrigadeRequests.Count != 0) SelectedItem = BrigadeRequests.First();
            }
        }
        private void OpenEditRequest(object o)
        {
            try
            {
                var requestToEdit = SelectedItem;
                EditRequestFromBrigadeView window = new(requestToEdit);
                if (window.ShowDialog() == true)
                {
                    SetBrigadeRequests();
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при открытии окна изменения заявки");
                infoWindow.ShowDialog();
            }
        }
    }
}