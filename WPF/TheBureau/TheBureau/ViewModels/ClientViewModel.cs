using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Repositories;
using TheBureau.Views;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class ClientViewModel : ViewModelBase
    {
        private readonly AddressRepository _addressRepository = new();
        private readonly RequestEquipmentRepository _requestEquipmentRepository = new();
        private readonly RequestRepository _requestRepository = new();
        private ClientRepository _clientRepository = new();

        
        private ObservableCollection<Client> _clients;
        private ObservableCollection<Request> _clientsRequests;

        Client _selectedItem;
        private string _findClientsText;
        
        private ICommand _deleteCommand;
        private ICommand _openEditClientWindowCommand;

        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        if (SelectedItem != null)
                        {
                            int clientid = SelectedItem.id;
                            var clientRequests = _requestRepository.FindByClientId(clientid);
                            var addresses = clientRequests.Select(x => x.addressId).ToList();
                            var requestsid = clientRequests.Select(x => x.id).ToList();

                            foreach (var id in requestsid)
                            {
                                _requestEquipmentRepository.DeleteByRequestId(id);
                            }
                            _requestEquipmentRepository.Save();

                            foreach (var request in clientRequests.ToList())
                            {
                                _requestRepository.Delete(request.id);
                            }
                            _requestRepository.Save();

                            foreach (var id in addresses)
                            {
                                _addressRepository.Delete(id);
                            }
                            _addressRepository.Save();
                            
                            _clientRepository.Delete(clientid);
                            _clientRepository.Save();
                            Clients = new ObservableCollection<Client>(_clientRepository.GetAll());
                            SelectedItem = Clients.First();
                        }
                    }
                    catch (Exception)
                    {
                        InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при удалении клиента");
                        infoWindow.ShowDialog();
                    }
                });
            }
        }
        public ICommand OpenEditClientWindowCommand => _openEditClientWindowCommand ??= new RelayCommand(OpenEditClientWindow);

        private void OpenEditClientWindow(object sender)
        {
            try
            {
                if (SelectedItem != null)
                {
                    var clientToEdit = SelectedItem;
                    EditClientView window = new(clientToEdit);
                    if (window.ShowDialog() == true)
                    {
                        _clientRepository = new ClientRepository();
                        Clients = new ObservableCollection<Client>(_clientRepository.GetAll());
                        SelectedItem = _clientRepository.Get(clientToEdit.id);
                    }
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при редактировании клиента");
                infoWindow.ShowDialog();
            }
        }
        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set { _clients = value; OnPropertyChanged("Clients"); }
        }
        public Client SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                SetClientsRequests();
                OnPropertyChanged("SelectedItem");
            }
        }
        public ObservableCollection<Request> ClientRequests
        {
            get => _clientsRequests;
            set {_clientsRequests = value; OnPropertyChanged("ClientRequests"); }
        }
        public string FindClientText
        {
            get => _findClientsText;
            set
            {
                _findClientsText = value;
                Search(_findClientsText); 
                OnPropertyChanged("FindClientText");
            }
        }
        public ClientViewModel()
        {
            try
            {
                _clientRepository = new ClientRepository();
                Clients = new ObservableCollection<Client>(_clientRepository.GetAll());
                SelectedItem = Clients.First();
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при открытии страницы клиента");
                infoWindow.ShowDialog();
            }
        }
        void SetClientsRequests()
        {
            if (SelectedItem != null)
            {
                ClientRequests = new ObservableCollection<Request>(_requestRepository.GetAll()
                    .Where(x => x.clientId == SelectedItem.id));
            }
        }
        private void Search(string criteria)
        {
            try
            {
                Clients = new ObservableCollection<Client>(_clientRepository.FindClientsByCriteria(criteria));
                SelectedItem = Clients.First();
            }
            catch (Exception)
            {
                ClientRequests = new ObservableCollection<Request>();
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Не удалось отобразить клиентов");
                infoWindow.ShowDialog();
            }
        }
    }
}