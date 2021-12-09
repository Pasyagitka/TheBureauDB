using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Services;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class EditToolViewModel : ViewModelBase
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;
        private readonly ErrorsViewModel _errorsViewModel = new();

        private string _name;
        //private string _stage;
        private Tool _tool;
        private int _selectedStageId;
     
        private int _id;

        private ICommand _editToolCommand;
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
                OnPropertyChanged("SelectedBrigadeId");
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
                OnPropertyChanged("Employee");
            }
        }

        
        public ICommand EditToolCommand => _editToolCommand ??= new RelayCommand(EditTool, CanEditTool);
        private bool CanEditTool(object sender) => !HasErrors;
        private void EditTool(object sender)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("UpdateTool", conn) {CommandType = CommandType.StoredProcedure})
                    {
                        cmd.Parameters.AddWithValue("@id", Id);
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
        
        
        public EditToolViewModel(Tool selectedTool)
        {
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            Tool = selectedTool;
            
            Stages = new ObservableCollection<Stage>();
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("GetAllStages", conn)  { CommandType = CommandType.StoredProcedure }) {
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