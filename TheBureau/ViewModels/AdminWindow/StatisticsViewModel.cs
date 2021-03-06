using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using TheBureau.Models;
using System.Windows.Input;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;
        private readonly string _exportConnectionString = ConfigurationManager.ConnectionStrings["ExportConnection"].ConnectionString;

        private ObservableCollection<Client> _clients = new ObservableCollection<Client>();
        private ObservableCollection<Request> _requests = new ObservableCollection<Request>();
        private ObservableCollection<Brigade> _brigades = new ObservableCollection<Brigade>();
        private int _countRed;
        private int _countYellow;
        private int _countGreen;
        private ICommand _openFolder;

        #region Properties
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
        #endregion

        public StatisticsViewModel()
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using (var cmd = new SqlCommand("GetAllClients", conn)  { CommandType = CommandType.StoredProcedure }) {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())  
                    {
                        Clients.Add(new Client
                        {
                            id = (int)reader["id"],
                            firstname = (string)reader["firstname"],
                            patronymic = (string)reader["patronymic"],
                            surname = (string)reader["surname"],
                            email = (string)reader["email"],
                            contactNumber = (string)reader["contactNumber"]
                        });
                    };
                }
            }
            using (var cmd = new SqlCommand("GetAllRequests", conn)  { CommandType = CommandType.StoredProcedure }) {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())  
                    {
                        Requests.Add(new Request
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
                        });
                    };
                }
            }
            using (var cmd = new SqlCommand("GetAllBrigades", conn)  { CommandType = CommandType.StoredProcedure }) {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())  
                    {
                        Brigades.Add( new Brigade
                        {
                            id = (int)reader["id"],
                            userId = reader["userId"] == DBNull.Value ? (Int32?) null : Convert.ToInt32(reader["userId"]),
                            brigadierId = reader["brigadierId"] == DBNull.Value ? (Int32?) null : Convert.ToInt32(reader["brigadierId"]),
                            creationDate = (DateTime)reader["creationDate"]
                        });
                    };
                }
            }
            using (var cmd = new SqlCommand("GetRequestsCountByStatusId", conn)  { CommandType = CommandType.StoredProcedure }) {
                cmd.Parameters.AddWithValue("@statusId", 1); //Red - In processing
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())  
                    {
                        CountRed = (int)reader["rqcount"];
                    };
                }
            }
            using (var cmd = new SqlCommand("GetRequestsCountByStatusId", conn)  { CommandType = CommandType.StoredProcedure }) {
                cmd.Parameters.AddWithValue("@statusId", 2); //Yellow - In progress
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())  
                    {
                        CountYellow = (int)reader["rqcount"];
                    };
                }
            }
            using (var cmd = new SqlCommand("GetRequestsCountByStatusId", conn)  { CommandType = CommandType.StoredProcedure }) {
                cmd.Parameters.AddWithValue("@statusId", 3); //Green - Done
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())  
                    {
                        CountGreen = (int)reader["rqcount"];
                    };
                }
            }
            conn.Close();
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
            using (var cmd = new SqlCommand("ExportTable", conn) {CommandType = CommandType.StoredProcedure})
            {
                cmd.Parameters.AddWithValue("@table", "Schedule");
                cmd.Parameters.AddWithValue("@path", path);
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