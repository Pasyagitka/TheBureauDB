using System.Data.SqlClient;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Views;
using TheBureau.Views.AdminWindow;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class ClientViewModel : ViewModelBase
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;
        
        private ObservableCollection<Client> _clients = new();
        private ObservableCollection<Request> _clientsRequests = new();

        Client _selectedItem;
        private string _findClientsText;
        
        private ICommand _deleteCommand;
        private ICommand _openEditClientWindowCommand;

        #region Properties
        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set { _clients = value; OnPropertyChanged("Clients"); }
        }
        public Client SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                SetClientsRequests();
                OnPropertyChanged("SelectedItem");
            }
        }
        public ObservableCollection<Request> ClientRequests
        {
            get => _clientsRequests;
            set {_clientsRequests = value; OnPropertyChanged("ClientRequests"); }
        }
        public string FindClientText
        {
            get => _findClientsText;
            set
            {
                _findClientsText = value;
                OnPropertyChanged("FindClientText");
            }
        }
        

        #endregion

        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        if (SelectedItem != null)
                        {
                            using SqlConnection conn = new SqlConnection(_connectionString);
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand("DeleteClient", conn) {CommandType = CommandType.StoredProcedure})
                            {
                                cmd.Parameters.AddWithValue("@clientId", SelectedItem.id);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())  { }; //todo delete client - hide!
                                }
                                conn.Close();
                            }
                            Clients.Remove(SelectedItem);
                            SelectedItem = Clients.First();
                        }
                    }
                    catch (Exception)
                    {
                        InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при удалении клиента");
                        infoWindow.ShowDialog();
                    }
                });
            }
        }
        public ICommand OpenEditClientWindowCommand => _openEditClientWindowCommand ??= new RelayCommand(OpenEditClientWindow);

        private void OpenEditClientWindow(object sender)
        {
            try
            {
                if (SelectedItem != null)
                {
                    var clientToEdit = SelectedItem;
                    EditClientView window = new(clientToEdit);
                    if (window.ShowDialog() == true)
                    {
                        Client client = new Client();
                        using (SqlConnection conn = new SqlConnection(_connectionString))
                        {
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand("GetClient", conn) {CommandType = CommandType.StoredProcedure})
                            {
                                cmd.Parameters.AddWithValue("@id", clientToEdit.id);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        client.id = (int) reader["id"];
                                        client.firstname = (string) reader["firstname"];
                                        client.patronymic = (string) reader["patronymic"];
                                        client.surname = (string) reader["surname"];
                                        client.email = (string) reader["email"];
                                        client.contactNumber = (string)reader["contactNumber"];
                                        Clients[Clients.IndexOf(SelectedItem)] = client; //todo update observable collection
                                    }
                                    ;
                                }
                            }
                            conn.Close();
                        }
                        SelectedItem = client;
                    }
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при редактировании клиента");
                infoWindow.ShowDialog();
            }
        }
     
        public ClientViewModel()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetAllClients", conn) {CommandType = CommandType.StoredProcedure})
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Clients.Add( new Client
                                {
                                    id = (int) reader["id"],
                                    firstname = (string) reader["firstname"],
                                    patronymic = (string) reader["patronymic"],
                                    surname = (string) reader["surname"],
                                    email = (string) reader["email"],
                                    contactNumber = (string)reader["contactNumber"]
                                });
                            }
                            ;
                        }
                    }
                    conn.Close();
                }
                //todo selected item -> collection.count
                if (Clients.Count!=0) SelectedItem = Clients.First();
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при открытии страницы клиента");
                infoWindow.ShowDialog();
            }
        }
        void SetClientsRequests()
        {
            if (SelectedItem != null)
            {
                ClientRequests.Clear(); //todo Clear collections
                using SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("GetRequestsForClient", conn) {CommandType = CommandType.StoredProcedure})
                {
                    cmd.Parameters.AddWithValue("@clientId", SelectedItem.id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           ClientRequests.Add(new Request
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
                               Client = SelectedItem,
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
                           });
                        }
                        ;
                    }
                }
                conn.Close();
            }
        }
    }
}