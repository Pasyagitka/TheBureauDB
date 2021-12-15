using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Views;
using TheBureau.Views.AdminWindow;
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
        private ICommand _openFolder;
        private ICommand _updateRequest;
        public ICommand UpdateRequestCommand => _updateRequest ??= new RelayCommand(OpenEditRequest);
        

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
                    EditRequestView window = new(SelectedItem);
                    if (window.ShowDialog() == true)
                    {
                        Request request = new Request();
                        request.id = SelectedItem.id;
                        request.clientId = SelectedItem.clientId;
                        request.addressId = SelectedItem.addressId;
                        request.stage = SelectedItem.stage;
                        request.status = SelectedItem.status;
                        request.registerDate = SelectedItem.registerDate;
                        request.mountingDate = SelectedItem.mountingDate;
                        request.comment = SelectedItem.comment;
                        request.brigadeId = SelectedItem.brigadeId;
                        request.proceeds = SelectedItem.proceeds;
                        request.Client = SelectedItem.Client;
                        request.Address = SelectedItem.Address;
                        //request = SelectedItem;
                        
                        using (SqlConnection conn = new SqlConnection(_connectionString))
                        {
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand("GetUpdatedRequest", conn) { CommandType = CommandType.StoredProcedure })
                            {
                                cmd.Parameters.AddWithValue("@id", SelectedItem.id);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        request.status = (int)reader["statusId"];
                                        request.brigadeId = reader["brigadeId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(reader["brigadeId"]);
                                        Requests[Requests.IndexOf(SelectedItem)] = request;
                                    }
                                    ;
                                }
                            }
                            conn.Close();
                        }
                        SelectedItem = request;
                    }
                }
            }
            catch (Exception e)
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