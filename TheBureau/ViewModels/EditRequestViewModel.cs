using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Services;
using TheBureau.Views.Controls;
using static System.Int32;

namespace TheBureau.ViewModels
{
    public class EditRequestViewModel : ViewModelBase
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;
        private ObservableCollection<Brigade> _brigades = new();
        
        private int _selectedBrigadeId;
        private int _requestStatus; 
        
        private Request _requestForEdit;
        
        private ICommand _updateRequest;
        
        public EditRequestViewModel(Request request)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("GetAllBrigades", conn)  { CommandType = CommandType.StoredProcedure }) {
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        while (reader.Read())   {
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
            Brigades.Add(new Brigade{id=0});
            RequestForEdit = request;
            SelectedBrigadeId = request.brigadeId ?? 0;
        }
        
        public Request RequestForEdit
        {
            get => _requestForEdit;
            set { _requestForEdit = value; OnPropertyChanged("RequestForEdit"); }
        }

        public ICommand UpdateRequestCommand => _updateRequest ??= new RelayCommand(UpdateRequest);

        private void UpdateRequest(object o)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UpdateRequestByAdmin", conn)  { CommandType = CommandType.StoredProcedure }) {
                        cmd.Parameters.AddWithValue("@id", RequestForEdit.id);
                        cmd.Parameters.AddWithValue("@statusId", _requestStatus);
                        cmd.Parameters.AddWithValue("@brigadeId", SelectedBrigadeId == 0? DBNull.Value : SelectedBrigadeId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read()) { };
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при редактировании заявки");
                infoWindow.ShowDialog();
            }
        }

        public int SelectedBrigadeId
        {
            get => _selectedBrigadeId;
            set { _selectedBrigadeId = value; OnPropertyChanged("SelectedBrigadeId"); }
        }
        
        public ObservableCollection<Brigade> Brigades 
        { 
            get => _brigades; 
            set { _brigades = value; OnPropertyChanged("Brigades"); } 
        }
        
        public string RequestStatus
        {
            get => _requestStatus.ToString();
            set
            { //1 - В обработке, 2 - в Процессе, 3 - Готово
                if (value.Contains("Готово")) _requestStatus = 3; 
                else if (value.Contains("В процессе")) _requestStatus = 2;
                else _requestStatus = 1;
                OnPropertyChanged("RequestStatus");
            }
        }
    }
}