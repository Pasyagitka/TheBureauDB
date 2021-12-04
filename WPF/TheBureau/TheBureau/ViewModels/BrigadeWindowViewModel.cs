using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.UI.WebControls.Expressions;
using System.Windows;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Repositories;
using TheBureau.Views;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class BrigadeWindowViewModel : ViewModelBase
    {
        private RequestRepository _requestRepository = new();
        private readonly BrigadeRepository _brigadeRepository = new();
        private readonly RequestEquipmentRepository _requestEquipmentRepository = new();


        private ObservableCollection<Request> _brigadeRequests;
        private ObservableCollection<RequestEquipment> _requestEquipments; 
        private Brigade _currentBrigade;
        private Request _selectedItem;
        
        private string _findRequestText;
        
        private WindowState _windowState;
        
        private ICommand _logOutCommand;
        private ICommand _updateRequest;
        private ICommand _closeWindowCommand;
        private ICommand _minimizeWindowCommand;
        public ICommand UpdateRequestCommand => _updateRequest ??= new RelayCommand(OpenEditRequest);
        public ICommand CloseWindowCommand => _closeWindowCommand ??= new RelayCommand(obj => { Application.Current.Shutdown(); });
        public ICommand MinimizeWindowCommand => _minimizeWindowCommand ??= new RelayCommand(obj => { WindowState = WindowState.Minimized; });
        public ICommand LogOutCommand
        {
            get
            {
                return _logOutCommand ??= new RelayCommand(obj =>
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
            }
        }
        public WindowState  WindowState
        {
            get => _windowState;
            set { _windowState = value; OnPropertyChanged("WindowState"); }
        }
        public string FindRequestText
        {
            get => _findRequestText;
            set
            {
                _findRequestText = value;
                Search();
                OnPropertyChanged("FindRequestText");
            }
        }
        
        public Brigade CurrentBrigade
        {
            get => _currentBrigade;
            set { _currentBrigade = value; OnPropertyChanged("CurrentBrigade"); }
        }
        
        public ObservableCollection<Request> BrigadeRequests
        {
            get => _brigadeRequests;
            set { _brigadeRequests = value; OnPropertyChanged("BrigadeRequests"); }
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
        private void OpenEditRequest(object o)
        {
            try
            {
                var requestToEdit = SelectedItem;
                EditRequestFromBrigadeView window = new(requestToEdit);
                if (window.ShowDialog() == true)
                {
                    _requestRepository = new RequestRepository();
                    BrigadeRequests =
                        new ObservableCollection<Request>(_requestRepository.GetRequestsByBrigadeId(CurrentBrigade.id)
                            .Reverse());
                    SelectedItem = _requestRepository.Get(requestToEdit.id);
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при открытии окна изменения заявки");
                infoWindow.ShowDialog();
            }
        }
        public BrigadeWindowViewModel()
        {
            try
            {
                WindowState = WindowState.Normal;
                var user = Application.Current.Properties["User"] as User;
                if (user != null)
                {
                    CurrentBrigade = _brigadeRepository.GetAll().FirstOrDefault(x => x.userId == user.id);
                    if (CurrentBrigade != null)
                    {
                        BrigadeRequests =
                            new ObservableCollection<Request>(_requestRepository
                                .GetRequestsByBrigadeId(CurrentBrigade.id).Reverse());
                        SelectedItem = BrigadeRequests.First();
                    }
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при открытии окна бригады");
                infoWindow.ShowDialog();
            }
        }
        
        public ObservableCollection<RequestEquipment> RequestEquipments
        {
            get => _requestEquipments;
            set { _requestEquipments = value; OnPropertyChanged("RequestEquipments");}
        }

        private void SetEquipment()
        {
            if (SelectedItem != null)
            {
                RequestEquipments = new ObservableCollection<RequestEquipment>(_requestEquipmentRepository.GetAllByRequestId(SelectedItem.id));
            }
        }
        
        private void Search()
        {
            try
            {
                BrigadeRequests = new ObservableCollection<Request>(_requestRepository
                    .FindRequestsForBrigadeByCriteria(FindRequestText, _currentBrigade.id).Reverse());
                SelectedItem = BrigadeRequests.First();
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Не удалось отобразить заявки");
                infoWindow.ShowDialog();
            }
        }
    }
}