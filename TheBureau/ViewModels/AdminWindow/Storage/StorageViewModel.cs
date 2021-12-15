using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;
using TheBureau.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using TheBureau.Views;
using TheBureau.Views.Controls;
using TheBureau.Views.AdminWindow;
using TheBureau.Views.AdminWindow.Employee;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace TheBureau.ViewModels
{
    public class StorageViewModel : ViewModelBase
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;
        private readonly string _exportConnectionString =
            ConfigurationManager.ConnectionStrings["ExportConnection"].ConnectionString;
        
        private ObservableCollection<Tool> _tools = new();
        private ObservableCollection<Accessory> _accessories = new();
        private ObservableCollection<Equipment> _equipments = new();

        private Tool _selectedToolItem;
        private Accessory _selectedAccessoryItem;
        private Equipment _selectedEquipmentItem;

        private ICommand _openFileTool;
        private ICommand _openFolderTool;

        private ICommand _deleteToolCommand;
        private ICommand _openEditToolWindowCommand;
        private ICommand _openAddToolWindowCommand;

        private ICommand _deleteAccessoryCommand;
        private ICommand _openEditAccessoryWindowCommand;
        private ICommand _openAddAccessoryWindowCommand;

        private ICommand _deleteEquipmentCommand;
        private ICommand _openEditEquipmentWindowCommand;
        private ICommand _openAddEquipmentWindowCommand;
        private ICommand _openFolderAccessory;
        private ICommand _openFileAccessory;

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
            set
            {
                _tools = value;
                OnPropertyChanged("Tools");
            }
        }

        public ObservableCollection<Accessory> Accessories
        {
            get => _accessories;
            set
            {
                _accessories = value;
                OnPropertyChanged("Accessories");
            }
        }

        public ObservableCollection<Equipment> Equipments
        {
            get => _equipments;
            set
            {
                _equipments = value;
                OnPropertyChanged("Equipments");
            }
        }

        #endregion

        public StorageViewModel()
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();

            using (SqlCommand cmd = new SqlCommand("GetAllTools", conn) {CommandType = CommandType.StoredProcedure})
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tools.Add(new Tool
                        {
                            id = (int) reader["id"],
                            name = (string) reader["name"],
                            stage = (int) reader["stageId"]
                        });
                    }
                    
                    ;
                }
            }

            using (SqlCommand cmd = new SqlCommand("GetAllAccessories", conn)
                {CommandType = CommandType.StoredProcedure})
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Accessories.Add(new Accessory
                        {
                            id = (int) reader["id"],
                            art = Convert.ToString(reader["art"]),
                            equipmentId = (string) reader["equipmentId"],
                            name = (string) reader["name"],
                            price = (decimal) reader["price"]
                        });
                    }

                    ;
                }
            }

            using (SqlCommand cmd = new SqlCommand("GetAllEquipment", conn) {CommandType = CommandType.StoredProcedure})
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Equipments.Add(new Equipment
                        {
                            id = (string) reader["id"],
                            type = (string) reader["type"],
                            mounting = (int) reader["mountingId"]
                        });
                    }

                    ;
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
                                using (SqlCommand cmd = new SqlCommand("DeleteTool", conn)
                                    {CommandType = CommandType.StoredProcedure})
                                {
                                    cmd.Parameters.AddWithValue("@toolId", toolId);
                                    using (SqlDataReader reader = cmd.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                        }

                                        ;
                                    }
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

        public ICommand OpenEditToolWindowCommand =>
            _openEditToolWindowCommand ??= new RelayCommand(OpenEditToolWindow);

        public ICommand OpenAddToolWindowCommand => _openAddToolWindowCommand ??= new RelayCommand(OpenAddToolWindow);

        private void OpenAddToolWindow(object sender)
        {
            try
            {
                AddToolView view = new();
                if (view.ShowDialog() == true)
                {
                }
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
                        using (SqlCommand cmd = new SqlCommand("GetTool", conn)
                            {CommandType = CommandType.StoredProcedure})
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


        #region ImportExport

        public ICommand ImportCommandTool => _openFileTool ??= new RelayCommand(OpenFileTool);
        public ICommand ExportCommandTool => _openFolderTool ??= new RelayCommand(OpenFolderTool);

        private void OpenFileTool(object sender)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog
                {
                    Multiselect = false,
                    DefaultExt = ".json",
                    Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt"
                };
                if (fileDialog.ShowDialog() == true)
                {
                    ImportTools(fileDialog.FileName);
                    InfoWindow infoWindow = new InfoWindow("Импортировано из файла", fileDialog.FileName);
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

        private void ImportTools(string filename)
        {
            using var conn = new SqlConnection(_exportConnectionString);
            conn.Open();
            using (var cmd = new SqlCommand("ImportTools", conn) {CommandType = CommandType.StoredProcedure})
            {
                cmd.Parameters.AddWithValue("@filename", filename);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Tools.Add(new Tool
                        {
                            id = (int) reader["id"],
                            name = (string) reader["name"],
                            stage = (int) reader["stageId"]
                        });
                    }

                    ;
                }
            }

            conn.Close();
        }

        private void OpenFolderTool(object sender)
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
                {
                    RootFolder = Environment.SpecialFolder.Desktop,
                };
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;
                if (ExportTools(folderBrowserDialog.SelectedPath))
                {
                    InfoWindow infoWindow = new InfoWindow("Экспортировано в файл", folderBrowserDialog.SelectedPath);
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
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при отрытии диалогового окна для выбора файла");
                infoWindow.ShowDialog();
            }
        }

        private bool ExportTools(string path)
        {
            var success = 1;
            string filename;
            using var conn = new SqlConnection(_exportConnectionString);
            conn.Open();
            using (var cmd = new SqlCommand("ExportTable", conn) {CommandType = CommandType.StoredProcedure})
            {
                cmd.Parameters.AddWithValue("@table", "Tool");
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
        
        
        public ICommand ImportCommandAccessory => _openFileAccessory ??= new RelayCommand(OpenFileAccessory);
        private void OpenFileAccessory(object sender)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog
                {
                    Multiselect = false,
                    DefaultExt = ".json",
                    Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt"
                };
                if (fileDialog.ShowDialog() == true)
                {
                    ImportAccessory(fileDialog.FileName);
                    InfoWindow infoWindow = new InfoWindow("Импортировано из файла", fileDialog.FileName);
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

        private void ImportAccessory(string filename)
        {
            using var conn = new SqlConnection(_exportConnectionString);
            conn.Open();
            using (var cmd = new SqlCommand("ImportAccessory", conn) {CommandType = CommandType.StoredProcedure})
            {
                cmd.Parameters.AddWithValue("@filename", filename);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Accessories.Add(new Accessory()
                        {
                            id = (int) reader["id"],
                            art = Convert.ToString(reader["art"]),
                            equipmentId = (string) reader["equipmentId"],
                            name = (string) reader["name"],
                            price = (decimal) reader["price"]
                        });
                    }

                    ;
                }
            }
            conn.Close();
        }
        
        public ICommand ExportCommandAccessory => _openFolderAccessory ??= new RelayCommand(OpenFolderAccessory);
        private void OpenFolderAccessory(object sender)
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
                {
                    RootFolder = Environment.SpecialFolder.Desktop,
                };
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;
                if (ExportAccessories(folderBrowserDialog.SelectedPath))
                {
                    InfoWindow infoWindow = new InfoWindow("Экспортировано в файл", folderBrowserDialog.SelectedPath);
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
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при отрытии диалогового окна для выбора файла");
                infoWindow.ShowDialog();
            }
        }
        private bool ExportAccessories(string path)
        {
            var success = 1;
            string filename;
            using var conn = new SqlConnection(_exportConnectionString);
            conn.Open();
            using (var cmd = new SqlCommand("ExportTable", conn) {CommandType = CommandType.StoredProcedure})
            {
                cmd.Parameters.AddWithValue("@table", "Accessory");
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
        
        
        #region Accessory

        public ICommand DeleteAccessoryCommand
        {
            get
            {
                return _deleteAccessoryCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        var accessory = SelectedAccessoryItem;
                        if (accessory != null)
                        {
                            int accessoryId = accessory.id;
                            using (SqlConnection conn = new SqlConnection(_connectionString))
                            {
                                conn.Open();
                                using (SqlCommand cmd = new SqlCommand("DeleteAccessory", conn)
                                    {CommandType = CommandType.StoredProcedure})
                                {
                                    cmd.Parameters.AddWithValue("@accessoryId", accessoryId);
                                    using (SqlDataReader reader = cmd.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                        }

                                        ;
                                    }
                                }
                                conn.Close();
                            }
                            Accessories.Remove(SelectedAccessoryItem);
                            if (Accessories != null) SelectedAccessoryItem = Accessories.First();
                        }
                    }
                    catch (Exception)
                    {
                        InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при удалении комплектующих");
                        infoWindow.ShowDialog();
                    }
                });
            }
        }

        public ICommand OpenEditAccessoryWindowCommand =>
            _openEditAccessoryWindowCommand ??= new RelayCommand(OpenEditAccessoryWindow);

        public ICommand OpenAddAccessoryWindowCommand => _openAddAccessoryWindowCommand ??= new RelayCommand(OpenAddAccessoryWindow);
        
        private void OpenAddAccessoryWindow(object sender)
        {
            try
            {
                AddAccessoryView view = new();
                if (view.ShowDialog() == true)
                {
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при добавлении комплектующих");
                infoWindow.ShowDialog();
            }
        }

        private void OpenEditAccessoryWindow(object sender)
        {
            try
            {
                var accessoryItem = SelectedAccessoryItem;
                EditAccessoryView window = new(accessoryItem);
                if (window.ShowDialog() == true)
                {
                    Accessory accessory = new Accessory();
                    using SqlConnection conn = new SqlConnection(_connectionString);
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetAccessory", conn)
                        {CommandType = CommandType.StoredProcedure})
                    {
                        cmd.Parameters.AddWithValue("@id", accessoryItem.id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                accessory.id = (int) reader["id"];
                                accessory.art = Convert.ToString(reader["art"]);
                                accessory.equipmentId = (string) reader["equipmentId"];
                                accessory.name = (string) reader["name"];
                                accessory.price = (decimal) reader["price"];
                            }
                            ;
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при изменении комплектующего");
                infoWindow.ShowDialog();
            }
        }

        #endregion
        
        
        #region Equipment

        public ICommand DeleteEquipmentCommand
        {
            get
            {
                return _deleteEquipmentCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        var equipment = SelectedEquipmentItem;
                        if (equipment != null)
                        {
                            using (SqlConnection conn = new SqlConnection(_connectionString))
                            {
                                conn.Open();
                                using (SqlCommand cmd = new SqlCommand("DeleteEquipment", conn)
                                    {CommandType = CommandType.StoredProcedure})
                                {
                                    cmd.Parameters.AddWithValue("@equipmentId", SelectedEquipmentItem.id);
                                    using (SqlDataReader reader = cmd.ExecuteReader())
                                    {
                                        while (reader.Read()) {  }  ;
                                    }
                                }
                                conn.Close();
                            }
                            Equipments.Remove(SelectedEquipmentItem);
                            if (Equipments != null) SelectedEquipmentItem = Equipments.First();
                        }
                    }
                    catch (Exception)
                    {
                        InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при удалении комплектующих");
                        infoWindow.ShowDialog();
                    }
                });
            }
        }

        public ICommand OpenEditEquipmentWindowCommand =>
            _openEditEquipmentWindowCommand ??= new RelayCommand(OpenEditEquipmentWindow);

        public ICommand OpenAddEquipmentWindowCommand => _openAddEquipmentWindowCommand ??= new RelayCommand(OpenAddEquipmentWindow);
        
        private void OpenAddEquipmentWindow(object sender)
        {
            try
            {
                AddEquipmentView view = new();
                if (view.ShowDialog() == true)
                { }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при добавлении оборудования");
                infoWindow.ShowDialog();
            }
        }

        private void OpenEditEquipmentWindow(object sender)
        {
            try
            {
                var equipmentItem = SelectedEquipmentItem;
                EditEquipmentView window = new(equipmentItem);
                if (window.ShowDialog() == true)
                {
                    Equipment equipment = new Equipment();
                    using SqlConnection conn = new SqlConnection(_connectionString);
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("GetEquipment", conn)
                        {CommandType = CommandType.StoredProcedure})
                    {
                        cmd.Parameters.AddWithValue("@id", equipmentItem.id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                equipment.id = (string) reader["id"];
                                equipment.type = (string)reader["type"];
                                equipment.mounting = (int) reader["mountingId"];
                            }
                            ;
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при изменении оборудования");
                infoWindow.ShowDialog();
            }
        }

        #endregion
    }
}