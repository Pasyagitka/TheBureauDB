using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;
using TheBureau.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows.Input;
using TheBureau.Views;
using TheBureau.Views.Controls;
using Microsoft.Win32;
using TheBureau.Views.AdminWindow;
using TheBureau.Views.AdminWindow.Employee;

namespace TheBureau.ViewModels
{
    public class StorageViewModel : ViewModelBase
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;

        private ObservableCollection<Tool> _tools = new();
        private ObservableCollection<Accessory> _accessories = new();
        private ObservableCollection<Equipment> _equipments = new();
        
        private Tool _selectedToolItem;
        private Accessory _selectedAccessoryItem;
        private Equipment _selectedEquipmentItem;

        private ICommand _openFile;
        
        private ICommand _deleteToolCommand;
        private ICommand _openEditToolWindowCommand;
        private ICommand _openAddToolWindowCommand;
        
        private ICommand _deleteAccessoryCommand;
        private ICommand _openEditAccessoryWindowCommand;
        private ICommand _openAddAccessoryWindowCommand;
        
        private ICommand _deleteEquipmentCommand;
        private ICommand _openEditEquipmentWindowCommand;
        private ICommand _openAddEquipmentWindowCommand;
        
        #region Properties
        public Tool SelectedToolItem
        {
            get => _selectedToolItem;
            set
            {
                _selectedToolItem = value;
                OnPropertyChanged("SelectedToolItem");
            }
        }
        public Accessory SelectedAccessoryItem
        {
            get => _selectedAccessoryItem;
            set
            {
                _selectedAccessoryItem = value;
                OnPropertyChanged("SelectedAccessoryItem");
            }
        }
        public Equipment SelectedEquipmentItem
        {
            get => _selectedEquipmentItem;
            set
            {
                _selectedEquipmentItem = value;
                OnPropertyChanged("SelectedEquipmentItem");
            }
        }

        public ObservableCollection<Tool> Tools 
        { 
            get => _tools; 
            set  {  _tools = value;  OnPropertyChanged("Tools"); } 
        }
        public ObservableCollection<Accessory> Accessories 
        { 
            get => _accessories; 
            set  {  _accessories = value;  OnPropertyChanged("Accessories");  } 
        }
        public ObservableCollection<Equipment> Equipments
        {
            get => _equipments;
            set { _equipments = value;  OnPropertyChanged("Equipments");  }
        }
        #endregion

        public StorageViewModel()
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();

            using (SqlCommand cmd = new SqlCommand("GetAllTools", conn)  { CommandType = CommandType.StoredProcedure }) {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())  
                    {
                        Tools.Add(new Tool
                        {
                            id = (int)reader["id"],
                            name = (string)reader["name"],
                            stage = (int)reader["stageId"]
                        });
                    };
                }
            }
            using (SqlCommand cmd = new SqlCommand("GetAllAccessories", conn)  { CommandType = CommandType.StoredProcedure }) {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())  
                    {
                        Accessories.Add(new Accessory
                        {
                            id = (int)reader["id"],
                            art = Convert.ToString(reader["art"]),
                            equipmentId = (string)reader["equipmentId"],
                            name = (string)reader["name"],
                            price = (decimal)reader["price"]
                        });
                    };
                }
            }
            using (SqlCommand cmd = new SqlCommand("GetAllEquipment", conn)  { CommandType = CommandType.StoredProcedure }) {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())  
                    {
                        Equipments.Add(new Equipment
                        {
                            id = (string)reader["id"],
                            type = (string)reader["type"],
                            mounting = (int)reader["mountingId"]
                        });
                    };
                }
            }
            conn.Close();
        }



        #region Tool
        public ICommand DeleteToolCommand
        {
            get
            {
                return _deleteToolCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        var tool = SelectedToolItem;
                        if (tool != null)
                        {
                            int toolId = tool.id;
                            using (SqlConnection conn = new SqlConnection(_connectionString))
                            {
                                conn.Open();
                                using (SqlCommand cmd = new SqlCommand("DeleteTool", conn) {CommandType = CommandType.StoredProcedure})
                                {
                                    cmd.Parameters.AddWithValue("@toolId", toolId);
                                    using (SqlDataReader reader = cmd.ExecuteReader()) { while (reader.Read()) { }; }
                                }
                                conn.Close();
                            }
                            Tools.Remove(SelectedToolItem);
                            if (Tools != null) SelectedToolItem = Tools.First();
                        }
                    }
                    catch (Exception)
                    {
                        InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при удалении инструмента");
                        infoWindow.ShowDialog();
                    }
                });
            }
        }
        public ICommand OpenEditToolWindowCommand => _openEditToolWindowCommand ??= new RelayCommand(OpenEditToolWindow);
        public ICommand OpenAddToolWindowCommand => _openAddToolWindowCommand ??= new RelayCommand(OpenAddToolWindow);
        private void OpenAddToolWindow(object sender)
        {
            try
            {
                AddEmployeeView view = new();
                if (view.ShowDialog() == true) { }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при добавлении инструмента");
                infoWindow.ShowDialog();
            }
        }
        
        private void OpenEditToolWindow(object sender)
        {
            try
            {
                var toolToEdit = SelectedToolItem;
                EditToolView window = new(toolToEdit);
                if (window.ShowDialog() == true)
                {
                    Tool tool = new Tool();
                    using (SqlConnection conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("GetTool", conn) {CommandType = CommandType.StoredProcedure})
                        {
                            cmd.Parameters.AddWithValue("@id", toolToEdit.id);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read()) 
                                {
                                    tool.id = (int) reader["id"];
                                    tool.name = (string) reader["name"];
                                    tool.stage = (int) reader["stageId"];
                                   //todo Stage object - generate new model
                                }
                                ;
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при изменении инструмента");
                infoWindow.ShowDialog();
            }
        }
        #endregion



        public ICommand ImportCommand => _openFile ??= new RelayCommand(OpenFile);
        private void OpenFile(object sender)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog
                {
                    Multiselect = false,
                    DefaultExt = ".json",
                    Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt"
                };
                if (fileDialog.ShowDialog() == true) {
                    ImportTools(fileDialog.FileName);
                    InfoWindow infoWindow = new InfoWindow("Импортировано из файла", fileDialog.FileName);
                    infoWindow.ShowDialog();
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при отрытии диалогового окна для выбора файла");
                infoWindow.ShowDialog();
            }
        }

        private void ImportTools(string filename)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using (var cmd = new SqlCommand("ImportTools", conn)  { CommandType = CommandType.StoredProcedure }) {
                cmd.Parameters.AddWithValue("@filename", filename);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                    };
                }
            }
            conn.Close();
        }
    }
}