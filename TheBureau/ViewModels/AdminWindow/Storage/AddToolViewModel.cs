using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Services;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    class AddToolViewModel : ViewModelBase
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;
        private readonly ErrorsViewModel _errorsViewModel = new();

        private string _name;
        private Tool _tool;
        private int _selectedStageId;

        private int _id;

        private ICommand _addToolCommand;
        private ObservableCollection<Stage> _stages;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                _errorsViewModel.ClearErrors("Name");

                if (string.IsNullOrWhiteSpace(_name))
                {
                    _errorsViewModel.AddError("Name", ValidationConst.FieldCannotBeEmpty);
                }
                if (_name?.Length is > 30 or < 2)
                {
                    _errorsViewModel.AddError("Name", ValidationConst.MaxLength30);
                }
                OnPropertyChanged("Surname");
            }
        }
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged("Id"); }
        }
        public ObservableCollection<Stage> Stages
        {
            get => _stages;
            set
            {
                _stages = value;
                OnPropertyChanged("Stages");
            }
        }

        public int SelectedStageId
        {
            get => _selectedStageId;
            set
            {
                _selectedStageId = value;
                OnPropertyChanged("SelectedStageId");
            }
        }
        public Tool Tool
        {
            get => _tool;
            set
            {
                _tool = value;
                Id = Tool.id;
                Name = Tool.name;
                SelectedStageId = Tool.stage;
                OnPropertyChanged("Tool");
            }
        }


        public ICommand AddToolCommand => _addToolCommand ??= new RelayCommand(AddTool, CanAddTool);
        private bool CanAddTool(object sender) => !HasErrors;
        private void AddTool(object sender)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("AddTool", conn) { CommandType = CommandType.StoredProcedure })
                    {
                        cmd.Parameters.AddWithValue("@name", Name);
                        cmd.Parameters.AddWithValue("@stageId", SelectedStageId);
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
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при редактировании инструмента");
                infoWindow.ShowDialog();
            }
        }


        public AddToolViewModel()
        {
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            _errorsViewModel.AddError("Name", ValidationConst.FieldCannotBeEmpty);
            
            Stages = new ObservableCollection<Stage>();
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("GetAllStages", conn) { CommandType = CommandType.StoredProcedure })
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Stage a = new Stage
                        {
                            id = (int)reader["id"],
                            stage = (string)reader["stage"]
                        };
                        Stages.Add(a);
                    };
                }
            }
            conn.Close();
            if (Stages != null) SelectedStageId = Stages.First().id;
        }

        #region Validation
        public IEnumerable GetErrors(string propertyName) => _errorsViewModel.GetErrors(propertyName);
        public bool HasErrors => _errorsViewModel.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged("CanEditTool");
        }
        #endregion
    }
}
