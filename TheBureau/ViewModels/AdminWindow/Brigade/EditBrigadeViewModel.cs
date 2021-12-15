using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    class EditBrigadeViewModel : ViewModelBase
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;

        private int _selectedEmployeeId;

        private Brigade _currentBrigade;
        public Brigade CurrentBrigade
        {
            get => _currentBrigade;
            set { _currentBrigade = value; OnPropertyChanged("CurrentBrigade"); }
        }
        public int SelectedEmployeeId
        {
            get => _selectedEmployeeId;
            set { _selectedEmployeeId = value; OnPropertyChanged("SelectedEmployeeId"); }
        }

        private ICommand _updateBrigadeCommand;
        public ICommand UpdateBrigadeCommand => _updateBrigadeCommand ??= new RelayCommand(UpdateBrigade);



        public EditBrigadeViewModel(Brigade brigade)
        {
            CurrentBrigade = brigade;
        }

        public void UpdateBrigade(object o)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SetBrigadier", conn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.AddWithValue("@brigadeId", CurrentBrigade.id);
                    cmd.Parameters.AddWithValue("@employeeId", SelectedEmployeeId == 0 ? DBNull.Value : SelectedEmployeeId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) { };
                    }
                }
                conn.Close();
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при редактировании заявки");
                infoWindow.ShowDialog();
            }
        }
    }
}
