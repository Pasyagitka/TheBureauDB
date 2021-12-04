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
    public class RequestViewModel : ViewModelBase
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;

        private ObservableCollection<Request> _requests = new();
        private ObservableCollection<Brigade> _brigades = new();
        private ObservableCollection<RequestEquipment> _requestEquipments = new();

        private Request _selectedItem;

        private ICommand _updateRequest;
        //private ICommand _hideGreenRequests;
        //private ICommand _showAllRequests;
        public ICommand UpdateRequestCommand => _updateRequest ??= new RelayCommand(OpenEditRequest);
        
        // public ICommand HideGreenRequestsCommand =>
        //     _hideGreenRequests ??= new RelayCommand(o =>
        //     {
        //         try
        //         {
        //             using (SqlConnection conn = new SqlConnection(_connectionString))
        //             {
        //                 conn.Open();
        //                 using (SqlCommand cmd = new SqlCommand("GetToDoRequests", conn)  { CommandType = CommandType.StoredProcedure }) {
        //                     using (SqlDataReader reader = cmd.ExecuteReader())
        //                     {
        //                         while (reader.Read())  
        //                         {
        //                             Request a = new Request();
        //                             a.id = (int)reader["id"];
        //                             a.clientId = (int)reader["clientId"];
        //                             a.addressId = (int)reader["addressId"];
        //                             a.stage = (int)reader["stageId"];
        //                             a.status = (int)reader["statusId"];
        //                             a.registerDate = (DateTime)reader["registerDate"];
        //                             a.mountingDate = (DateTime)reader["mountingDate"];
        //                             a.comment = (string)reader["comment"];
        //                             a.brigadeId = reader["brigadeId"] == DBNull.Value ? (Int32?) null : Convert.ToInt32(reader["brigadeId"]);
        //                             a.proceeds = reader["proceeds"] == DBNull.Value ? (Int32?) null : Convert.ToDecimal(reader["proceeds"]);
        //                             Requests.Add(a);
        //                         };
        //                     }
        //                 }
        //                 conn.Close();
        //             }
        //             SelectedItem = Requests.First();
        //         }
        //         catch (Exception)
        //         {
        //             InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при выполнении команды");
        //             infoWindow.ShowDialog();
        //         } 
        //     });

        // public ICommand ShowAllRequestsCommand =>
        //     _showAllRequests ??= new RelayCommand(o =>
        //     {
        //         try
        //         {
        //             Requests.Clear();
        //             using (SqlConnection conn = new SqlConnection(_connectionString))
        //             {
        //                 conn.Open();
        //                 using (SqlCommand cmd = new SqlCommand("GetAllRequestsForRequestView", conn)  { CommandType = CommandType.StoredProcedure }) {
        //                     using (SqlDataReader reader = cmd.ExecuteReader())
        //                     {
        //                         while (reader.Read())  
        //                         {
        //                             Request a = new Request();
        //                             a.id = (int)reader["id"];
        //                             a.clientId = (int)reader["clientId"];
        //                             a.addressId = (int)reader["addressId"];
        //                             a.stage = (int)reader["stageId"];
        //                             a.status = (int)reader["statusId"];
        //                             a.registerDate = (DateTime)reader["registerDate"];
        //                             a.mountingDate = (DateTime)reader["mountingDate"];
        //                             a.comment = (string)reader["comment"];
        //                             a.brigadeId = reader["brigadeId"] == DBNull.Value ? (Int32?) null : Convert.ToInt32(reader["brigadeId"]);
        //                             a.proceeds = reader["proceeds"] == DBNull.Value ? (Int32?) null : Convert.ToDecimal(reader["proceeds"]);
        //
        //                             Client client = new Client();
        //                             client.id = (int) reader["clientId"];
        //                             client.firstname = (string) reader["firstname"];
        //                             client.patronymic = (string) reader["patronymic"];
        //                             client.surname = (string) reader["surname"];
        //                             client.email = (string) reader["email"];
        //                             client.contactNumber = (string)reader["contactNumber"];
        //                             a.Client = client;
        //
        //                             Address address = new Address();
        //                             address.id = (int) reader["addressId"];
        //                             address.country = (string) reader["country"];
        //                             address.city = (string) reader["city"];
        //                             address.street = (string) reader["street"];
        //                             address.house = (int) reader["house"];
        //                             address.corpus = reader["corpus"] == DBNull.Value ? null : Convert.ToString(reader["corpus"]);
        //                             address.flat = reader["flat"] == DBNull.Value ? (Int32?) null : Convert.ToInt32(reader["flat"]);
        //                             a.Address = address;
        //                             
        //                             Requests.Add(a);
        //                         };
        //                     }
        //                 }
        //                 if (Requests!= null )SelectedItem = Requests.First();
        //                 conn.Close();
        //             }
        //         }
        //         catch (Exception)
        //         {
        //             InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при выполнении команды");
        //             infoWindow.ShowDialog();
        //         }
        //     });

        public RequestViewModel()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetAllRequestsForRequestView", conn)  { CommandType = CommandType.StoredProcedure }) {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())  
                            {
                                Request a = new Request
                                {
                                    id = (int)reader["id"],
                                    clientId = (int)reader["clientId"],
                                    addressId = (int)reader["addressId"],
                                    stage = (int)reader["stageId"],
                                    status = (int)reader["statusId"],
                                    registerDate = (DateTime)reader["registerDate"],
                                    mountingDate = (DateTime)reader["mountingDate"],
                                    comment = (string)reader["comment"],
                                    brigadeId = reader["brigadeId"] == DBNull.Value ? (Int32?) null : Convert.ToInt32(reader["brigadeId"]),
                                    proceeds = reader["proceeds"] == DBNull.Value ? (Int32?) null : Convert.ToDecimal(reader["proceeds"]),
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

                                Requests.Add(a);
                            };
                        }
                    }
                    if (Requests.Count != 0) SelectedItem = Requests.First();

                    using (SqlCommand cmd = new SqlCommand("GetAllBrigades", conn)  { CommandType = CommandType.StoredProcedure }) {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())  
                            {
                                Brigade a = new Brigade
                                {
                                    id = (int)reader["id"],
                                    userId = reader["userId"] == DBNull.Value ? (Int32?) null : Convert.ToInt32(reader["userId"]),
                                    brigadierId = reader["brigadierId"] == DBNull.Value ? (Int32?) null : Convert.ToInt32(reader["brigadierId"]),
                                    creationDate = (DateTime)reader["creationDate"]
                                };
                                Brigades.Add(a);
                            };
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при открытии страницы заявок");
                infoWindow.ShowDialog();
            }
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
        public ObservableCollection<Brigade> Brigades
        {
            get => _brigades;
            set { _brigades = value; OnPropertyChanged("Brigades"); }
        }
        public ObservableCollection<Request> Requests
        {
            get => _requests;
            set { _requests = value; OnPropertyChanged("Requests"); }
        }
        public ObservableCollection<RequestEquipment> RequestEquipments
        {
            get => _requestEquipments;
            set { _requestEquipments = value; OnPropertyChanged("RequestEquipments"); }
        }

        private void OpenEditRequest(object o)
        {
            try
            {
                if (SelectedItem != null)
                {
                    var requestToEdit = SelectedItem;
                    EditRequestView window = new(requestToEdit);
                    if (window.ShowDialog() == true)
                    {
                        // if (ShowAllRequestsCommand.CanExecute(null))  ShowAllRequestsCommand.Execute(null);

                        //Brigades = new ObservableCollection<Brigade>(_brigadeRepository.GetAll());
                        //SelectedItem = _requestRepository.Get(requestToEdit.id);
                    }
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при редактировании заявки");
                infoWindow.ShowDialog();
            }
        }

        private void SetEquipment()
        {
            if (SelectedItem != null)
            {
                RequestEquipments.Clear();
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetRequestEquipmentByRequestId", conn) {CommandType = CommandType.StoredProcedure})
                    {
                        cmd.Parameters.AddWithValue("@requestId", SelectedItem.id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                RequestEquipment b = new()
                                {
                                    quantity = (int) reader["quantity"],
                                    equipmentId = (string) reader["equipmentId"]
                                };

                                Equipment e = new()
                                {
                                    type = (string) reader["type"],
                                    mountingName = (string) reader["mounting"]
                                };

                                b.Equipment = e;
                                RequestEquipments.Add(b);
                            }
                            ;
                        }
                    }
                    conn.Close();
                }
            }
        }
    }
}