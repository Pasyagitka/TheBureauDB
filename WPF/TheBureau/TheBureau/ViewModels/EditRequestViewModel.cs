using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TheBureau.Enums;
using TheBureau.Models;
using TheBureau.Repositories;
using TheBureau.Services;
using TheBureau.Views.Controls;
using static System.Int32;

namespace TheBureau.ViewModels
{
    public class EditRequestViewModel : ViewModelBase
    {
        private readonly RequestRepository _requestRepository = new();
        private readonly BrigadeRepository _brigadeRepository = new();

        private ObservableCollection<Brigade> _brigades;
        
        private int _selectedBrigadeId;
        private int _requestStatus;
        private bool _sendEmail;
        
        private Request _requestForEdit;
        
        private ICommand _updateRequest;


        public bool SendEmail
        {
            get => _sendEmail;
            set { _sendEmail = value; OnPropertyChanged("SendEmail"); }
        }

        public EditRequestViewModel(Request request)
        {
            Brigades = new ObservableCollection<Brigade>(_brigadeRepository.GetAll());
            Brigades.Add(new Brigade{id=0});
            RequestForEdit = request;
            SelectedBrigadeId = request.brigadeId ?? 0;
        }
        
        public Request RequestForEdit
        {
            get => _requestForEdit;
            set { _requestForEdit = value; OnPropertyChanged("RequestForEdit"); }
        }

        public ICommand UpdateRequestCommand => _updateRequest ??= new RelayCommand(UpdateRequest);

        private void UpdateRequest(object o)
        {
            try
            {
                bool isStatusChanged = false;
                var request = _requestRepository.Get(RequestForEdit.id);
                if (Parse(RequestStatus) == (int) Statuses.InProcessing ||
                    Parse(RequestStatus) == (int) Statuses.InProgress || Parse(RequestStatus) == (int) Statuses.Done)
                {
                    if (Parse(RequestStatus) != request.status) isStatusChanged = true;
                    request.status = Parse(RequestStatus);
                }

                if (SelectedBrigadeId == 0) request.brigadeId = null;
                else
                    request.brigadeId = SelectedBrigadeId;
                _requestRepository.Update(request);
                _requestRepository.Save();

            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при редактировании заявки");
                infoWindow.ShowDialog();
            }
        }

        public int SelectedBrigadeId
        {
            get => _selectedBrigadeId;
            set { _selectedBrigadeId = value; OnPropertyChanged("SelectedBrigadeId"); }
        }
        
        public ObservableCollection<Brigade> Brigades 
        { 
            get => _brigades; 
            set { _brigades = value; OnPropertyChanged("Brigades"); } 
        }
        
        public string RequestStatus
        {
            get => _requestStatus.ToString();
            set
            { //1 - В обработке, 2 - в Процессе, 3 - Готово
                if (value.Contains("Готово")) _requestStatus = (int)Statuses.Done; 
                else if (value.Contains("В процессе")) _requestStatus = (int)Statuses.InProgress;
                else _requestStatus = (int)Statuses.InProcessing;
                OnPropertyChanged("RequestStatus");
            }
        }
        
    }
}