using System;
using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Repositories;
using TheBureau.Services;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class EditClientViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly ErrorsViewModel _errorsViewModel = new();
        private readonly ClientRepository _clientRepository = new();
        
        private int _id;
        private string _surname;
        private string _firstname;
        private string _patronymic;
        private string _email;
        private decimal _contactNumber;
        private Client _client;
        
        private RelayCommand _editClientCommand;
        
        #region ClientProperties

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged("Id"); }
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
                var regex = new Regex(ValidationConst.LettersHyphenRegex);
                if (!regex.IsMatch(_surname!))
                {
                    _errorsViewModel.AddError("Surname", ValidationConst.IncorrectSurname);
                }
                OnPropertyChanged("Surname");
            }

        }

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
                var regex = new Regex(ValidationConst.LettersHyphenRegex);
                if (!regex.IsMatch(_firstname!))
                {
                    _errorsViewModel.AddError("Firstname",  ValidationConst.IncorrectFirstname);
                }
                OnPropertyChanged("Firstname");
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
                var regex = new Regex(ValidationConst.LettersHyphenRegex);
                if (!regex.IsMatch(_patronymic!))
                {
                    _errorsViewModel.AddError("Patronymic", ValidationConst.IncorrectPatronymic);
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
                var regex = new Regex(ValidationConst.EmailRegex);
                if (!regex.IsMatch(_email!))
                {
                    _errorsViewModel.AddError("Email", ValidationConst.IncorrectEmailStructure);
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
                _contactNumber = decimal.Parse(value!); 
                

                var regex = new Regex(ValidationConst.ContactNumberRegex);
                if (!regex.IsMatch(_contactNumber.ToString()))
                {
                    _errorsViewModel.AddError("ContactNumber", ValidationConst.IncorrectNumberStructure);
                }
                OnPropertyChanged("ContactNumber");
            }
        }
        #endregion propetries

        public Client Client
        {
            get => _client;
            set
            { 
                _client = value;
                Id = Client.id;
                Firstname = Client.firstname;
                Surname = Client.surname;
                Patronymic = Client.patronymic;
                Email = Client.email;
                ContactNumber = Client.contactNumber.ToString();
                OnPropertyChanged("Client");
            }
        }

        public ICommand EditClientCommand => _editClientCommand ??= new RelayCommand(EditClient, CanEditClient);

        private void EditClient(object sender)
        {
            try
            {
                var clientUpdate = _clientRepository.Get(Id);
                clientUpdate.firstname = Firstname;
                clientUpdate.surname = Surname;
                clientUpdate.patronymic = Patronymic;
                clientUpdate.email = Email;
                clientUpdate.contactNumber = decimal.Parse(ContactNumber);
                _clientRepository.Update(clientUpdate);
                _clientRepository.SaveChanges();
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при редактировании клиента");
                infoWindow.ShowDialog();
            }
        }

        private bool CanEditClient(object sender)
        {
            return !HasErrors;
        }
        public EditClientViewModel(Client selectedClient)
        {
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
            if (selectedClient != null)
            {
                Client = selectedClient;
            }
        }
        
        #region Validation
        public IEnumerable GetErrors(string propertyName) => _errorsViewModel.GetErrors(propertyName);
        public bool HasErrors => _errorsViewModel.HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        
        private void ErrorsViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged("CanEditClient");
        }
        #endregion
    }
}