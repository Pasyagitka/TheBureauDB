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
    public class RequestViewModel : ViewModelBase
    {
        private RequestRepository _requestRepository = new();
        private BrigadeRepository _brigadeRepository = new();
        private RequestEquipmentRepository _requestEquipmentRepository = new();

        private ObservableCollection<Request> _requests;
        private ObservableCollection<Brigade> _brigades;
        private ObservableCollection<RequestEquipment> _requestEquipments;

        private Request _selectedItem;

        private ICommand _updateRequest;
        private ICommand _hideGreenRequests;
        private ICommand _showAllRequests;
        public ICommand UpdateRequestCommand => _updateRequest ??= new RelayCommand(OpenEditRequest);
        public ICommand HideGreenRequestsCommand =>
            _hideGreenRequests ??= new RelayCommand(o =>
            {
                try
                {
                    Requests = new ObservableCollection<Request>(_requestRepository.GetToDoRequests().Reverse());
                    SelectedItem = Requests.First();
                }
                catch (Exception)
                {
                    InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при выполнении команды");
                    infoWindow.ShowDialog();
                }
            });

        public ICommand ShowAllRequestsCommand =>
            _showAllRequests ??= new RelayCommand(o =>
            {
                try
                {
                    Requests = new ObservableCollection<Request>(_requestRepository.GetAll().Reverse());
                    SelectedItem = Requests.First();
                }
                catch (Exception)
                {
                    InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при выполнении команды");
                    infoWindow.ShowDialog();
                }
            });

        public RequestViewModel()
        {
            try
            {
                Requests = new ObservableCollection<Request>(_requestRepository.GetAll().Reverse());
                Brigades = new ObservableCollection<Brigade>(_brigadeRepository.GetAll());
                SelectedItem = Requests.First();
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при открытии страницы заявок");
                infoWindow.ShowDialog();
            }
        }
        
        public Request SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                SetEquipment();
                OnPropertyChanged("SelectedItem");
            }
        }

        public ObservableCollection<Brigade> Brigades 
        { 
            get => _brigades; 
            set { _brigades = value; OnPropertyChanged("Brigades"); } 
        }
        public ObservableCollection<Request> Requests 
        { 
            get => _requests; 
            set { _requests = value; OnPropertyChanged("Requests"); } 
        }

        public ObservableCollection<RequestEquipment> RequestEquipments
        {
            get => _requestEquipments;
            set { _requestEquipments = value; OnPropertyChanged("RequestEquipments");}
        }

        private void OpenEditRequest(object o)
        {
            try
            {
                if (SelectedItem != null)
                {
                    var requestToEdit = SelectedItem;
                    EditRequestView window = new(requestToEdit);
                    if (window.ShowDialog() == true)
                    {
                        _requestRepository = new RequestRepository();
                        _brigadeRepository = new BrigadeRepository();
                        _requestEquipmentRepository = new RequestEquipmentRepository();
                        Requests = new ObservableCollection<Request>(_requestRepository.GetAll().Reverse());
                        Brigades = new ObservableCollection<Brigade>(_brigadeRepository.GetAll());
                        SelectedItem = _requestRepository.Get(requestToEdit.id);
                    }
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при редактировании заявки");
                infoWindow.ShowDialog();
            }
        }

        private void SetEquipment()
        {
            if (SelectedItem != null)
            {
                RequestEquipments = new ObservableCollection<RequestEquipment>(_requestEquipmentRepository.GetAllByRequestId(SelectedItem.id));
            }
        }
    }
}