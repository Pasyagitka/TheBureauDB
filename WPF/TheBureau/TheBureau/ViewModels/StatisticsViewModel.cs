using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LiveCharts;
using TheBureau.Models;
using TheBureau.Repositories;

namespace TheBureau.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {
        private readonly ClientRepository _clientRepository = new();
        private readonly RequestRepository _requestRepository = new();
        private readonly BrigadeRepository _brigadeRepository = new();

        private ObservableCollection<Client> _clients;
        private ObservableCollection<Request> _requests;
        private ObservableCollection<Brigade> _brigades;
        private int _countRed;
        private int _countYellow;
        private int _countGreen;

        private ChartValues<int> _redValues;
        private ChartValues<int> _yellowValues;
        private ChartValues<int> _greenValues;

        public ChartValues<int> RedValues
        {
            get => _redValues;
            set { _redValues = value; OnPropertyChanged("RedValues");}
        }

        public ChartValues<int> YellowValues
        {
            get => _yellowValues;
            set { _yellowValues = value;  OnPropertyChanged("YellowValues");}
        }

        public ChartValues<int> GreenValues
        {
            get => _greenValues;
            set { _greenValues = value; OnPropertyChanged("GreenValues");}
        }
        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set { _clients = value; OnPropertyChanged("Clients"); }
        }
        public ObservableCollection<Request> Requests
        {
            get => _requests;
            set { _requests = value; OnPropertyChanged("Requests"); }
        }
        public ObservableCollection<Brigade> Brigades
        {
            get => _brigades;
            set { _brigades = value; OnPropertyChanged("Brigades"); }
        }
        public string CountRedRequests
        {
            get => $" Новые заявки ( {_countRed} )";
            set { _countRed = Int32.Parse(value); OnPropertyChanged("CountRedRequests"); }
        }
        public int CountRed
        {
            get => _countRed;
            set { _countRed = value; OnPropertyChanged("CountRed"); }
        }
        public int CountYellow
        {
            get => _countYellow;
            set { _countYellow = value; OnPropertyChanged("CountYellow"); }
        }
        public int CountGreen
        {
            get => _countGreen;
            set { _countGreen = value; OnPropertyChanged("CountGreen"); }
        }

        public StatisticsViewModel()
        {
            Clients = new ObservableCollection<Client>(_clientRepository.GetAll());
            Requests = new ObservableCollection<Request>(_requestRepository.GetAll());
            Brigades = new ObservableCollection<Brigade>(_brigadeRepository.GetAll());
            
            CountGreen = _requestRepository.GetGreenRequestsCount();
            CountRed = _requestRepository.GetRedRequestsCount();
            CountYellow = _requestRepository.GetYellowRequestsCount();

            GreenValues = new ChartValues<int>(new[] {CountGreen});
            RedValues = new ChartValues<int>(new[] {CountRed});
            YellowValues = new ChartValues<int>(new[] {CountYellow});
        }
    }
}