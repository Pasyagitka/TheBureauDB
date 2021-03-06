using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Services;
using TheBureau.Views;
using TheBureau.Views.AuthWindow;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class ClientWindowViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        //todo add stage and status
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ClientConnection"].ConnectionString;

        private readonly ErrorsViewModel _errorsViewModel = new();
        
       private ObservableCollection<Request> _requests = new();
       private ICommand _sendRequestCommand;
       private WindowState _windowState;
        
       private string _findRequestText;
       private string _firstname;
       private string _surname;
       private string _patronymic;
       private string _contactNumber;
       private string _email;
        
       private string _city;
       private string _street;
       private int _house;
       private string _corpus;
       private int _flat;
        
       private bool _radiatorCheckBox;
       private bool _convectorCheckBox;
       private bool _convectorVpCheckBox;
       private int _rpQuantity;
       private int _rsQuantity;
       private int _kpQuantity;
       private int _ksQuantity;
       private int _vpQuantity;
        
       private bool _isRough;
       private bool _isClean;
        
       private string _comment;
       private DateTime _mountingDate;
       private ICommand _closeWindowCommand;
       private ICommand _minimizeWindowCommand;


       public ICommand CloseWindowCommand => _closeWindowCommand ??= new RelayCommand(obj => { Application.Current.Shutdown(); });
       public ICommand MinimizeWindowCommand => _minimizeWindowCommand ??= new RelayCommand(obj => { WindowState = WindowState.Minimized; });

       public WindowState  WindowState
       {
           get => _windowState;
           set { _windowState = value; OnPropertyChanged("WindowState"); }
       }

       public DateTime MountingDate
       {
           get => _mountingDate;
           set { _mountingDate = value; OnPropertyChanged("MountingDate"); }
       }

       public string Comment
       {
           get => _comment;
           set
           {
               _comment = value;
               _errorsViewModel.ClearErrors("Comment");
               if (_comment.Length > 200)
               {
                   _errorsViewModel.AddError("Comment", ValidationConst.CommentLengthExceeded);
               }
               OnPropertyChanged("Comment");
           }
       }
        
        
       #region Оборудование
       public bool RadiatorCheckBox
       {
           get => _radiatorCheckBox;
           set
           {
               _radiatorCheckBox = value; 
               if (!_radiatorCheckBox)
               {
                   RpQuantity = "0";
                   RsQuantity = "0";
               }
               OnPropertyChanged("RadiatorCheckBox");
           }
       }
       public bool ConvectorCheckBox
       {
           get => _convectorCheckBox;
           set
           {
               _convectorCheckBox = value;
               if (!_convectorCheckBox)
               {
                   KpQuantity = "0";
                   KsQuantity = "0";
               }
               OnPropertyChanged("ConvectorCheckBox");
           }
       }
       public bool ConvectorVPCheckBox
       {
           get => _convectorVpCheckBox;
           set 
           {
               _convectorVpCheckBox = value;
               if (!_convectorVpCheckBox)
               {
                   VpQuantity = "0";
               }
               OnPropertyChanged("ConvectorVPCheckBox");
           }
       }
       public string RpQuantity
       {
           get => _rpQuantity.ToString();
           set
           {
               _errorsViewModel.ClearErrors("RpQuantity");
               if (string.IsNullOrWhiteSpace(value))
               {
                   _errorsViewModel.AddError("RpQuantity", ValidationConst.FieldCannotBeEmpty);
               }
               _rpQuantity = Int32.Parse(value);
               if (_rpQuantity > 100)
               {
                   _errorsViewModel.AddError("RpQuantity", ValidationConst.QuantityExceeded);
               }
               OnPropertyChanged("RpQuantity");
           }
       }

       public string RsQuantity
       {
           get => _rsQuantity.ToString();
           set
           {
               _errorsViewModel.ClearErrors("RsQuantity");
               if (string.IsNullOrWhiteSpace(value))
               {
                   _errorsViewModel.AddError("RsQuantity", ValidationConst.FieldCannotBeEmpty);
               }
               _rsQuantity = Int32.Parse(value); 
               if (_rsQuantity > 100)
               {
                   _errorsViewModel.AddError("RsQuantity", ValidationConst.QuantityExceeded);
               }
               OnPropertyChanged("RsQuantity");
           }
       }

       public string KpQuantity
       {
           get => _kpQuantity.ToString();
           set
           {
               _errorsViewModel.ClearErrors("KpQuantity");
               if (string.IsNullOrWhiteSpace(value))
               {
                   _errorsViewModel.AddError("KpQuantity", ValidationConst.FieldCannotBeEmpty);
               }

               _kpQuantity = Int32.Parse(value); 
               if (_kpQuantity > 100)
               {
                   _errorsViewModel.AddError("KpQuantity", ValidationConst.QuantityExceeded);
               }
               OnPropertyChanged("KpQuantity");
           }
       }

       public string KsQuantity
       {
           get => _ksQuantity.ToString();
           set
           {
               _errorsViewModel.ClearErrors("KsQuantity");
               if (string.IsNullOrWhiteSpace(value))
               {
                   _errorsViewModel.AddError("KsQuantity", ValidationConst.FieldCannotBeEmpty);
               }
               _ksQuantity = Int32.Parse(value); 
               if (_ksQuantity > 100)
               {
                   _errorsViewModel.AddError("KsQuantity", ValidationConst.QuantityExceeded);
               }                
               OnPropertyChanged("KsQuantity");
           }
       }

       public string VpQuantity
       {
           get => _vpQuantity.ToString();
           set
           {
               _errorsViewModel.ClearErrors("VpQuantity");
               if (string.IsNullOrWhiteSpace(value))
               {
                   _errorsViewModel.AddError("VpQuantity", ValidationConst.FieldCannotBeEmpty);
               }
               _vpQuantity = Int32.Parse(value); 
               if (_vpQuantity > 100)
               {
                   _errorsViewModel.AddError("VpQuantity", ValidationConst.QuantityExceeded);
               }
               OnPropertyChanged("VpQuantity");
           }
       }
       #endregion

       public bool IsRough
       {
           get => _isRough;
           set { _isRough = value; OnPropertyChanged("IsRough"); }
       }

       public bool IsClean
       {
           get => _isClean;
           set { _isClean = value; OnPropertyChanged("IsClean"); }
       }

        public string Stage
        {
            get
            {
                if (IsRough)
                {
                    return IsClean ? "Both" : "Rough";
                    //Черновая + чистовая - 3, черновая - 1
                }
                if (IsClean) return "Clean"; //2 = clean
                return "Both"; //3 = both
            }
        }

       public ICommand SendRequestCommand => _sendRequestCommand ??= new RelayCommand(SendRequest, CanSendRequest);
       private bool CanSendRequest(object sender) => !HasErrors;
       private void SendRequest(object sender)
       {
            try
            {
                int newRequestId = 0;

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        int stage = 3;
                        using (SqlCommand cmd = new SqlCommand("GetStageIdByName", conn) { Transaction = transaction, CommandType = CommandType.StoredProcedure })
                        {
                            cmd.Parameters.AddWithValue("@stage", Stage);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    stage = (int)reader["id"];
                                };
                            }
                        }


                        using (SqlCommand cmd = new SqlCommand("LeaveRequest", conn) { Transaction = transaction, CommandType = CommandType.StoredProcedure })
                        {
                            cmd.Parameters.AddWithValue("@client_firstname", Firstname);
                            cmd.Parameters.AddWithValue("@client_patronymic", Patronymic);
                            cmd.Parameters.AddWithValue("@client_surname", Surname);
                            cmd.Parameters.AddWithValue("@client_email", Email);
                            cmd.Parameters.AddWithValue("@client_contactNumber", ContactNumber);

                            cmd.Parameters.AddWithValue("@address_country", "Беларусь");
                            cmd.Parameters.AddWithValue("@address_city", City);
                            cmd.Parameters.AddWithValue("@address_street", Street);
                            cmd.Parameters.AddWithValue("@address_house", House);
                            cmd.Parameters.AddWithValue("@address_corpus", Corpus);
                            cmd.Parameters.AddWithValue("@address_flat", Flat);

                            cmd.Parameters.AddWithValue("@stageId", stage);
                            cmd.Parameters.AddWithValue("@statusId", 1);//1 = InProcessing
                            cmd.Parameters.AddWithValue("@mountingDate", MountingDate);
                            cmd.Parameters.AddWithValue("@comment", Comment);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    newRequestId = (int)reader["id"];
                                };
                            }
                        }

                        if (Int32.Parse(RpQuantity) != 0 && RadiatorCheckBox)
                        {
                            using SqlCommand cmd = new SqlCommand("AddRequestEquipment", conn) { Transaction = transaction, CommandType = CommandType.StoredProcedure };
                            cmd.Parameters.AddWithValue("@requestId", Convert.ToInt32(newRequestId));
                            cmd.Parameters.AddWithValue("@equipmentId", "RP");
                            cmd.Parameters.AddWithValue("@quantity", Int32.Parse(RpQuantity));
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                };
                            }
                        }

                        if (Int32.Parse(RsQuantity) != 0 && RadiatorCheckBox)
                        {
                            using SqlCommand cmd = new SqlCommand("AddRequestEquipment", conn) { Transaction = transaction, CommandType = CommandType.StoredProcedure };
                            cmd.Parameters.AddWithValue("@requestId", Convert.ToInt32(newRequestId));
                            cmd.Parameters.AddWithValue("@equipmentId", "RS");
                            cmd.Parameters.AddWithValue("@quantity", Int32.Parse(RsQuantity));
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                };
                            }
                        }

                        if (Int32.Parse(KpQuantity) != 0 && ConvectorCheckBox)
                        {
                            using SqlCommand cmd = new SqlCommand("AddRequestEquipment", conn)
                            { Transaction = transaction, CommandType = CommandType.StoredProcedure };
                            cmd.Parameters.AddWithValue("@requestId", Convert.ToInt32(newRequestId));
                            cmd.Parameters.AddWithValue("@equipmentId", "HP");
                            cmd.Parameters.AddWithValue("@quantity", Int32.Parse(KpQuantity));
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                };
                            }
                        }

                        if (Int32.Parse(KsQuantity) != 0 && ConvectorCheckBox)
                        {
                            using SqlCommand cmd = new SqlCommand("AddRequestEquipment", conn)
                            { Transaction = transaction, CommandType = CommandType.StoredProcedure };
                            cmd.Parameters.AddWithValue("@requestId", Convert.ToInt32(newRequestId));
                            cmd.Parameters.AddWithValue("@equipmentId", "HS");
                            cmd.Parameters.AddWithValue("@quantity", Int32.Parse(KsQuantity));
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                };
                            }
                        }

                        if (Int32.Parse(VpQuantity) != 0 && ConvectorVPCheckBox)
                        {
                            using SqlCommand cmd = new SqlCommand("AddRequestEquipment", conn)
                            { Transaction = transaction, CommandType = CommandType.StoredProcedure };
                            cmd.Parameters.AddWithValue("@requestId", Convert.ToInt32(newRequestId));
                            cmd.Parameters.AddWithValue("@equipmentId", "VP");
                            cmd.Parameters.AddWithValue("@quantity", Int32.Parse(VpQuantity));
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                };
                            }
                        }

                        {
                            using SqlCommand cmd = new SqlCommand("UpdateRequestSetProceeds", conn)  { Transaction = transaction, CommandType = CommandType.StoredProcedure };
                            cmd.Parameters.AddWithValue("@requestId", Convert.ToInt32(newRequestId));
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read()) {};
                            }
                        }

                        transaction.Commit();
                        conn.Close();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw;
                    }
                    InfoWindow infoWindow = new InfoWindow("Успех", "Номер вашей заявки: " + newRequestId);
                    infoWindow.ShowDialog();
                    Update();
                    ResetFields();
                }
            }
            catch (Exception e)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при отправке заявки");
                infoWindow.ShowDialog();
            }
       }

        
        #region Данные Клиента (ФИО, телефон,почта)
        public string Firstname
        {
            get => _firstname;
            set
            {
                _firstname = value;
                _errorsViewModel.ClearErrors("Firstname");
                
                if (string.IsNullOrWhiteSpace(_firstname))
                {
                    _errorsViewModel.AddError("Firstname", ValidationConst.FieldCannotBeEmpty);
                }
                if (_firstname?.Length is > 20 or < 2)
                {
                    _errorsViewModel.AddError("Firstname", ValidationConst.NameLengthExceeded);
                }

                if (_firstname != null)
                {
                    var regex = new Regex(ValidationConst.LettersHyphenRegex);
                    if (!regex.IsMatch(_firstname))
                    {
                        _errorsViewModel.AddError("Firstname",  ValidationConst.IncorrectFirstname);
                    }
                }
                OnPropertyChanged("Firstname");
            }
        }
        public string Surname
        {
            get => _surname;
            set
            { 
                _surname = value; 
                _errorsViewModel.ClearErrors("Surname");
                
                if (string.IsNullOrWhiteSpace(_surname))
                {
                    _errorsViewModel.AddError("Surname", ValidationConst.FieldCannotBeEmpty);
                }
                if (_surname?.Length is > 20 or < 2)
                {
                    _errorsViewModel.AddError("Surname", ValidationConst.NameLengthExceeded);
                }

                if (_surname != null)
                {
                      var regex = new Regex(ValidationConst.LettersHyphenRegex);
                    if (!regex.IsMatch(_surname))
                    {
                        _errorsViewModel.AddError("Surname", ValidationConst.IncorrectSurname);
                    }
                }
                OnPropertyChanged("Surname");
            }

        }
        public string Patronymic
        {
            get => _patronymic;
            set
            {
                _patronymic = value; 
                _errorsViewModel.ClearErrors("Patronymic");
                
                if (string.IsNullOrWhiteSpace(_patronymic))
                {
                    _errorsViewModel.AddError("Patronymic", ValidationConst.FieldCannotBeEmpty);
                }
                if (_patronymic?.Length is > 20 or < 2)
                {
                    _errorsViewModel.AddError("Patronymic", ValidationConst.NameLengthExceeded);
                }

                if (_patronymic != null)
                {
                    var regex = new Regex(ValidationConst.LettersHyphenRegex);
                    if (!regex.IsMatch(_patronymic))
                    {
                        _errorsViewModel.AddError("Patronymic", ValidationConst.IncorrectPatronymic);
                    }
                }
                OnPropertyChanged("Patronymic");
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value; 
                _errorsViewModel.ClearErrors("Email");
                if (string.IsNullOrWhiteSpace(_email))
                {
                    _errorsViewModel.AddError("Email", ValidationConst.FieldCannotBeEmpty);
                }
                if (_email?.Length > 255)
                {
                    _errorsViewModel.AddError("Email", ValidationConst.EmailLengthExceeded);
                }
                if (_email != null)
                {
                    var regex = new Regex(ValidationConst.EmailRegex);
                    if (!regex.IsMatch(_email))
                    {
                        _errorsViewModel.AddError("Email", ValidationConst.IncorrectEmailStructure);
                    }
                }
                OnPropertyChanged("Email");
            }
        }
        public string ContactNumber
        {
            get => _contactNumber.ToString();
            set
            {
                _errorsViewModel.ClearErrors("ContactNumber");
                if (string.IsNullOrWhiteSpace(value))
                {
                    _errorsViewModel.AddError("ContactNumber", ValidationConst.FieldCannotBeEmpty);
                }
                _contactNumber = value;

          
                var regex = new Regex(ValidationConst.ContactNumberRegex);
                if (!regex.IsMatch(_contactNumber.ToString()))
                {
                    _errorsViewModel.AddError("ContactNumber", ValidationConst.IncorrectNumberStructure);
                }
                OnPropertyChanged("ContactNumber");
            }
        }
        #endregion

       #region Адрес 
        public string City
            {
                get => _city;
                set
                {
                    _city = value;
                    
                    _errorsViewModel.ClearErrors("City");

                    if (string.IsNullOrWhiteSpace(_city))
                    {
                        _errorsViewModel.AddError("City", ValidationConst.FieldCannotBeEmpty);
                    }
                    if (_city?.Length > 30)
                    {
                        _errorsViewModel.AddError("City", ValidationConst.MaxLength30);
                    }

                    if (_city != null)
                    {
                        var regex = new Regex(ValidationConst.LettersHyphenRegex);
                        if (!regex.IsMatch(_city))
                        {
                            _errorsViewModel.AddError("City", ValidationConst.IncorrectCity);
                        }
                    }
                   
                    OnPropertyChanged("City"); 
                }
            }
        public string Street
        {
            get => _street;
            set 
            {
                _street = value;  
                
                _errorsViewModel.ClearErrors("Street");

                if (string.IsNullOrWhiteSpace(_street))
                {
                    _errorsViewModel.AddError("Street", ValidationConst.FieldCannotBeEmpty);
                }
                if (_street?.Length > 30)
                {
                    _errorsViewModel.AddError("Street", ValidationConst.MaxLength30);
                }
                if (_street != null)
                {
                    var regex = new Regex(ValidationConst.LettersHyphenRegex);
                    if (!regex.IsMatch(_street))
                    {
                        _errorsViewModel.AddError("Street", ValidationConst.IncorrectStreet);
                    }
                }
                OnPropertyChanged("Street"); 
            }
        }
        public string House
        {
            get => _house.ToString();
            set { 
                
                _errorsViewModel.ClearErrors("House");
                if (string.IsNullOrWhiteSpace(value))
                {
                    _errorsViewModel.AddError("House", ValidationConst.FieldCannotBeEmpty);
                }

                if (value != null)
                {
                    _house = int.Parse(value);
                    if (_house > 300 || _house == 0)
                    {
                        _errorsViewModel.AddError("House", ValidationConst.IncorrectHouse);
                    }
                    
                }
                OnPropertyChanged("House"); 
            }
        }
        public string Corpus
        {
            get => _corpus;
            set
            {
                _errorsViewModel.ClearErrors("Corpus");
                if (string.IsNullOrWhiteSpace(value))
                {
                    _errorsViewModel.AddError("Corpus", ValidationConst.FieldCannotBeEmpty);
                }
                _corpus = value;  
                if (_corpus != null && (_corpus.Length > 10 || _corpus.Equals("0")))
                {
                    _errorsViewModel.AddError("Corpus", ValidationConst.IncorrectExceeded);
                }

                if (_corpus != null)
                {
                    var regex = new Regex(ValidationConst.LettersHyphenDigitsRegex);
                    if (!regex.IsMatch(_corpus))
                    {
                        _errorsViewModel.AddError("Corpus", ValidationConst.IncorrectCorpus);
                    }
                }
                OnPropertyChanged("Corpus");
            }
        }
        public string Flat
        {
            get => _flat.ToString();
            set
            {
                _errorsViewModel.ClearErrors("Flat");
                if (string.IsNullOrWhiteSpace(value))
                {
                    _errorsViewModel.AddError("Flat", ValidationConst.FieldCannotBeEmpty);
                }
                if (value != null)
                {
                    _flat = int.Parse(value); 

                    if (_flat > 1011 || _flat == 0)
                    {
                        _errorsViewModel.AddError("Flat", ValidationConst.IncorrectFlat);
                    }
                }
                OnPropertyChanged("Flat");
            }
        }
       #endregion

        public ObservableCollection<Request> Requests
        {
            get => _requests;
            set { _requests = value; OnPropertyChanged("Requests"); }
        }

        public string FindRequestText
        {
            get => _findRequestText;
            set
            {
                _findRequestText = value;
                SetClientsRequests();
                OnPropertyChanged("FindRequestText");
            }
        }
        void SetClientsRequests()
        {
            Requests.Clear();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("GetRequestsForClientById", conn) {CommandType = CommandType.StoredProcedure})
                {
                    cmd.Parameters.AddWithValue("@requestId", FindRequestText);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Request a = new Request
                            {
                                id = (int) reader["requestId"],
                                mountingDate = (DateTime) reader["mountingDate"],
                                clientId = (int)reader["clientId"],
                                brigadeId = reader["brigadeId"] == DBNull.Value ? (Int32?) null : Convert.ToInt32(reader["brigadeId"]),
                                comment = (string) reader["comment"],
                                stage = (int)reader["stageId"],
                                status = (int)reader["statusId"],
                                Client = new Client
                                {
                                    id = (int)reader["clientId"],
                                    firstname = (string) reader["firstname"],
                                    patronymic = (string) reader["patronymic"],
                                    surname = (string) reader["surname"],
                                    email = (string) reader["email"],
                                    contactNumber = (string) reader["contactNumber"]
                                },
                                Address = new Address
                                {
                                    country = (string) reader["country"],
                                    city = (string) reader["city"],
                                    street = (string) reader["street"],
                                    house = (int) reader["house"],
                                    corpus = reader["corpus"] == DBNull.Value
                                        ? null
                                        : Convert.ToString(reader["corpus"]),
                                    flat = reader["flat"] == DBNull.Value
                                        ? (Int32?) null
                                        : Convert.ToInt32(reader["flat"])
                                }
                            };
                            Requests.Add(a);
                        }
                        ;
                    }
                }
                conn.Close();
            }
        }

        public ClientWindowViewModel()
        {
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            Update();
            ResetFields();
            WindowState = WindowState.Normal;
            IsClean = true;
            IsRough = true;
        }

        public ICommand LogOutCommand => new RelayCommand(obj =>
            {
                try
                {
                    Application.Current.Properties["User"] = null;
                    var helloWindow = new HelloWindowView();
                    helloWindow.Show();
                    Application.Current.Windows[0]?.Close();
                }
                catch (Exception)
                {
                    InfoWindow infoWindow = new InfoWindow("Ошибка", "Не удалось выйти из аккаунта");
                    infoWindow.ShowDialog();
                }
            });

        public void Update()
        {
            // _requestRepository = new RequestRepository();
            // _clientRepository = new ClientRepository();
            // _addressRepository = new AddressRepository();
            // _requestEquipmentRepository = new RequestEquipmentRepository();
        }

        private void ResetFields()
        {
            Firstname = String.Empty;
            Surname = String.Empty;
            Patronymic = String.Empty;
            ContactNumber = Decimal.Zero.ToString();
            Email = String.Empty;
            
            City = String.Empty;
            Street = String.Empty;
            House = "0";
            Corpus = "0";
            Flat = "0";

            RadiatorCheckBox = false;
            RadiatorCheckBox = false;
            RadiatorCheckBox = false;

            IsClean = false;
            IsRough = false;

            Comment = String.Empty;
            MountingDate = DateTime.Today;
        }

        #region Validation
        public IEnumerable GetErrors(string propertyName) => _errorsViewModel.GetErrors(propertyName);
        public bool HasErrors => _errorsViewModel.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        
        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged("SendRequestCommand");
        }
        #endregion
    }
}