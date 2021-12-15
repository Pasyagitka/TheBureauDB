using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Services;
using TheBureau.Views;
using TheBureau.Views.AdminWindow;
using TheBureau.Views.AuthWindow;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class HelloViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ClientConnection"].ConnectionString;
        
        private readonly ErrorsViewModel _errorsViewModel = new();
        private string _admin = "admin";
        private string _login;
        private string _password;
        private string _info;
        
        private ICommand _closeWindowCommand;
        private ICommand _clientPageSetCommand;
        private ICommand _authPageSetCommand;
        private ICommand _signinCommand;
        
        private object _frameContent;
        
        public ICommand ClientViewSetCommand => _clientPageSetCommand ??= 
            new RelayCommand(obj => { FrameContent = new HelloPageView(); });
        public ICommand AuthViewSetCommand => _authPageSetCommand ??= 
            new RelayCommand(obj => { FrameContent = new AuthenticationPageView(); });
        public ICommand CloseWindowCommand => _closeWindowCommand ??= 
            new RelayCommand(obj => { Application.Current.Shutdown(); });
        
        public HelloViewModel()
        {
            FrameContent = new HelloPageView();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        }

        #region Properties
        public object FrameContent
        {
            get => _frameContent;
            set { _frameContent = value; OnPropertyChanged("FrameContent"); }
        }
        
        public string Info
        {
            get => _info;
            set { _info = value; OnPropertyChanged("Info");}
        }
        
        public string Login
        {
            get => _login;
            set
            {
                _login = value; 
                _errorsViewModel.ClearErrors("Login");
        
                if (string.IsNullOrWhiteSpace(_login))
                {
                    _errorsViewModel.AddError("Login", ValidationConst.LoginEmpty);
                }
                if (_login?.Length > 20)
                {
                    _errorsViewModel.AddError("Login", ValidationConst.LoginLengthExceeded);
                }
                if (_login != null)
                {
                    var regex = new Regex(ValidationConst.LoginRegex);
                    if (!regex.IsMatch(_login))
                    {
                        _errorsViewModel.AddError("Login", ValidationConst.IncorrectLoginStructure);
                    }
                }
                OnPropertyChanged("Login");
            }
        }
        
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged("Password"); }
        }
        #endregion
        public ICommand SigninCommand => _signinCommand ??= new RelayCommand(Auth, CanAuth);
        private bool CanAuth(object obj) => !HasErrors;

        private void Auth(object obj)
        {
            try
            {
                var passwordBox = obj as PasswordBox;
                if (passwordBox == null) return;
                Password = passwordBox.Password;

                if (TryLogin())
                {
                    var user = Application.Current.Properties["User"] as User;
                    if (user?.login == _admin)
                    {
                        var mainWindow = new MainWindowView();
                        mainWindow.Show();
                        Application.Current.Windows[0]?.Close();
                    }
                    else
                    {
                        var brigadeWindow = new BrigadeWindowView();
                        brigadeWindow.Show();
                        Application.Current.Windows[0]?.Close();
                    }
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при авторизации");
                infoWindow.ShowDialog();
            }
        }
        private bool TryLogin()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Password))
                {
                    Info = ValidationConst.FieldCannotBeEmpty;
                    return false;
                }

                if (Password?.Length is < 5 or > 20)
                {
                    Info = ValidationConst.PasswordLengthExceeded;
                    return false;
                }
                
                //todo check login
                User user = new User();
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    //User user = _userRepository.Login(Login, Password);
                    using (SqlCommand cmd = new SqlCommand("LoginUser", conn) {CommandType = CommandType.StoredProcedure})
                    {
                        cmd.Parameters.AddWithValue("@login", Login);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user.id = (int) reader["id"];
                                user.login = (string) reader["login"];
                                user.password = (string) reader["password"];
                            }
                            ;
                        }
                    }
                    conn.Close();
                }
                var currentUser = PasswordHash.ValidatePassword(Password, user.password) ? user : null;
                if (currentUser == null)
                {
                    Info = ValidationConst.WrongLoginOrPassword;
                    return false;
                }
                Application.Current.Properties["User"] = currentUser;
                return true;
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при попытке входа. Данные некорректны");
                infoWindow.ShowDialog();
                return false;
            }
        }
 
        #region Validation
        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }
        public bool HasErrors => _errorsViewModel.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged("CanAuth");
        }
        #endregion
    }
}