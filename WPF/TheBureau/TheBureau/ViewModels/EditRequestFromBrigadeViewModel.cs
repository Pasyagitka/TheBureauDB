using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TheBureau.Enums;
using TheBureau.Models;
using TheBureau.Repositories;
using TheBureau.Services;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class EditRequestFromBrigadeViewModel : ViewModelBase
    {
        private readonly RequestRepository _requestRepository = new();
        private int _requestStatus;
        
        private ICommand _updateRequest;
        private Request _requestForEdit;


        public EditRequestFromBrigadeViewModel(Request request)
        {
            RequestForEdit = request;
        }
        public Request RequestForEdit
        {
            get => _requestForEdit;
            set
            {
                _requestForEdit = value;
                OnPropertyChanged("RequestForEdit");
            }
        }

        public ICommand UpdateRequestCommand => _updateRequest ??= new RelayCommand(UpdateRequest);

        private void UpdateRequest(object o)
        {
            try
            {
                bool isStatusChanged = false;
                var request = _requestRepository.Get(RequestForEdit.id);
                if (Int32.Parse(RequestStatus) == (int) Statuses.InProcessing ||
                    Int32.Parse(RequestStatus) == (int) Statuses.InProgress ||
                    Int32.Parse(RequestStatus) == (int) Statuses.Done)
                {
                    if (Int32.Parse(RequestStatus) != request.status) isStatusChanged = true;
                    request.status = Int32.Parse(RequestStatus);
                }

                _requestRepository.Update(request);
                _requestRepository.Save();

            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при редактировании заявки");
                infoWindow.ShowDialog();
            }
        }

        public string RequestStatus
        {
            get => _requestStatus.ToString();
            set
            {
                if (value.Contains("Готово")) _requestStatus = (int)Statuses.Done; 
                else if (value.Contains("В процессе")) _requestStatus = (int)Statuses.InProgress;
                else _requestStatus = (int)Statuses.InProcessing;
                OnPropertyChanged("RequestStatus");
            }
        }
    }
}