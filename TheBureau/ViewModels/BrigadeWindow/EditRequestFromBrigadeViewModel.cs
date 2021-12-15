using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Services;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class EditRequestFromBrigadeViewModel : ViewModelBase
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["BrigadeConnection"].ConnectionString;

        private string _requestStatus;
        private ICommand _updateRequest;
        private Request _requestForEdit;
        private ObservableCollection<Employee> _brigadeEmployees;


        public EditRequestFromBrigadeViewModel(Request request)
        {
            RequestForEdit = request;
        }

        #region Properties
        public Request RequestForEdit
        {
            get => _requestForEdit;
            set
            {
                _requestForEdit = value;
                OnPropertyChanged("RequestForEdit");
            }
        }
        public ObservableCollection<Employee> BrigadeEmployees
        {
            get => _brigadeEmployees;
            set { _brigadeEmployees = value; OnPropertyChanged("BrigadeEmployees"); }
        }
        #endregion
        public ICommand UpdateRequestCommand => _updateRequest ??= new RelayCommand(UpdateRequest);

        private void UpdateRequest(object o)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    int statusId = 1;
                    using (SqlCommand cmd = new SqlCommand("GetStatusIdByName", conn) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@status", _requestStatus);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read()) {
                                statusId = (int)reader["id"];
                            };
                        }
                    }
                    using (SqlCommand cmd = new SqlCommand("UpdateRequestByBrigade", conn)  { CommandType = CommandType.StoredProcedure }) {
                        cmd.Parameters.AddWithValue("@id", RequestForEdit.id);
                        cmd.Parameters.AddWithValue("@statusId", statusId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read()) { };
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при редактировании заявки");
                infoWindow.ShowDialog();
            }
        }

        public string RequestStatus
        {
            get => _requestStatus;
            set
            {
                if (value.Contains("Готово")) _requestStatus = "Done"; 
                else if (value.Contains("В процессе")) _requestStatus = "InProgress";
                else _requestStatus = "InProcessing";
                OnPropertyChanged("RequestStatus");
            }
        }
    }
}