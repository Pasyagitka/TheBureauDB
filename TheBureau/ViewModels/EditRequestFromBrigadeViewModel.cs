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
        private string _connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;

        private int _requestStatus;
        
        private ICommand _updateRequest;
        private Request _requestForEdit;


        public EditRequestFromBrigadeViewModel(Request request)
        {
            RequestForEdit = request;
        }
        public Request RequestForEdit
        {
            get => _requestForEdit;
            set
            {
                _requestForEdit = value;
                OnPropertyChanged("RequestForEdit");
            }
        }

        public ICommand UpdateRequestCommand => _updateRequest ??= new RelayCommand(UpdateRequest);

        private void UpdateRequest(object o)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UpdateRequestByBrigade", conn)  { CommandType = CommandType.StoredProcedure }) {
                        cmd.Parameters.AddWithValue("@id", RequestForEdit.id);
                        cmd.Parameters.AddWithValue("@statusId", Convert.ToInt32(RequestStatus));
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read()) { };
                        }
                    }
                    conn.Close();
                }
                
                // if (Int32.Parse(RequestStatus) == (int) Statuses.InProcessing ||
                //     Int32.Parse(RequestStatus) == (int) Statuses.InProgress ||
                //     Int32.Parse(RequestStatus) == (int) Statuses.Done)
                // {
                //     if (Int32.Parse(RequestStatus) != request.status) isStatusChanged = true;
                //     request.status = Int32.Parse(RequestStatus);
                // }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при редактировании заявки");
                infoWindow.ShowDialog();
            }
        }

        public string RequestStatus
        {
            get => _requestStatus.ToString();
            set
            {
                if (value.Contains("Готово")) _requestStatus = 3; 
                else if (value.Contains("В процессе")) _requestStatus = 2;
                else _requestStatus = 1;
                OnPropertyChanged("RequestStatus");
            }
        }
    }
}