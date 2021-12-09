using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using TheBureau.Models;

namespace TheBureau.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;

        private ObservableCollection<Client> _clients = new ObservableCollection<Client>();
        private ObservableCollection<Request> _requests = new ObservableCollection<Request>();
        private ObservableCollection<Brigade> _brigades = new ObservableCollection<Brigade>();
        private int _countRed;
        private int _countYellow;
        private int _countGreen;

        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set { _clients = value; OnPropertyChanged("Clients"); }
        }
        public ObservableCollection<Request> Requests
        {
            get => _requests;
            set { _requests = value; OnPropertyChanged("Requests"); }
        }
        public ObservableCollection<Brigade> Brigades
        {
            get => _brigades;
            set { _brigades = value; OnPropertyChanged("Brigades"); }
        }
        public string CountRedRequests
        {
            get => $" Новые заявки ( {_countRed} )";
            set { _countRed = Int32.Parse(value); OnPropertyChanged("CountRedRequests"); }
        }
        public int CountRed
        {
            get => _countRed;
            set { _countRed = value; OnPropertyChanged("CountRed"); }
        }
        public int CountYellow
        {
            get => _countYellow;
            set { _countYellow = value; OnPropertyChanged("CountYellow"); }
        }
        public int CountGreen
        {
            get => _countGreen;
            set { _countGreen = value; OnPropertyChanged("CountGreen"); }
        }

        public StatisticsViewModel()
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("GetAllClients", conn)  { CommandType = CommandType.StoredProcedure }) {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())  
                    {
                        Client a = new Client
                        {
                            id = (int)reader["id"],
                            firstname = (string)reader["firstname"],
                            patronymic = (string)reader["patronymic"],
                            surname = (string)reader["surname"],
                            email = (string)reader["email"],
                            contactNumber = (string)reader["contactNumber"]
                        };
                        Clients.Add(a);
                    };
                }
            }
            using (SqlCommand cmd = new SqlCommand("GetAllRequests", conn)  { CommandType = CommandType.StoredProcedure }) {
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
                            proceeds = reader["proceeds"] == DBNull.Value ? (Int32?) null : Convert.ToDecimal(reader["proceeds"])
                        };
                        Requests.Add(a);
                    };
                }
            }
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
            using (SqlCommand cmd = new SqlCommand("GetRequestsCountByStatusId", conn)  { CommandType = CommandType.StoredProcedure }) {
                cmd.Parameters.AddWithValue("@statusId", 1); //Red - In processing
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())  
                    {
                        CountRed = (int)reader["rqcount"];
                    };
                }
            }
            using (SqlCommand cmd = new SqlCommand("GetRequestsCountByStatusId", conn)  { CommandType = CommandType.StoredProcedure }) {
                cmd.Parameters.AddWithValue("@statusId", 2); //Yellow - In progress
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())  
                    {
                        CountYellow = (int)reader["rqcount"];
                    };
                }
            }
            using (SqlCommand cmd = new SqlCommand("GetRequestsCountByStatusId", conn)  { CommandType = CommandType.StoredProcedure }) {
                cmd.Parameters.AddWithValue("@statusId", 3); //Green - Done
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())  
                    {
                        CountGreen = (int)reader["rqcount"];
                    };
                }
            }
            conn.Close();
        }
    }
}