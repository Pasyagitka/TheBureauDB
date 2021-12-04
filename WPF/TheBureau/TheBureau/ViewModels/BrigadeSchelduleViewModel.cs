using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LiveCharts;
using LiveCharts.Defaults;
using TheBureau.Models;
using TheBureau.Repositories;

namespace TheBureau.ViewModels
{
    public class BrigadeSchelduleViewModel : ViewModelBase
    {
        private readonly BrigadeRepository _brigadeRepository = new();
        private readonly RequestRepository _requestRepository = new();

        private ObservableCollection<Brigade> _brigades;
        private ObservableCollection<Request> _requests;
        
        private List<string> _brigadeList;
        private ChartValues<HeatPoint> _values;

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
        
        public List<string> Days { get; set; }

        public List<string> BrigadeList
        {
            get => _brigadeList;
            set { _brigadeList = value; OnPropertyChanged("BrigadeList"); }
        }
        public ChartValues<HeatPoint> Values
        {
            get => _values;
            set {  _values = value; OnPropertyChanged("Values"); }
        }

        public BrigadeSchelduleViewModel()
        {
            Brigades = new ObservableCollection<Brigade>(_brigadeRepository.GetAll());
            Requests = new ObservableCollection<Request>(_requestRepository.GetAll());
            
            Days = new List<string> { "пн", "вт", "ср", "чт", "пт", "сб", "вс"};
            BrigadeList = new List<string>();

            var ids = _brigadeRepository.GetAll().Select(x=>x.id).ToList();
            foreach (var id in ids)
            {
                BrigadeList.Add(id.ToString());
            }
            
            Values = new ChartValues<HeatPoint>();

            int idx = 0;
            foreach (var brigade in Brigades)
            {
                Values.Add( new(idx, 0, _requestRepository.RequestForBrigadeForCertainDay(brigade.id, DayOfWeek.Monday)));
                Values.Add( new(idx, 1, _requestRepository.RequestForBrigadeForCertainDay(brigade.id, DayOfWeek.Tuesday)));
                Values.Add( new(idx, 2, _requestRepository.RequestForBrigadeForCertainDay(brigade.id, DayOfWeek.Wednesday)));
                Values.Add( new(idx, 3, _requestRepository.RequestForBrigadeForCertainDay(brigade.id, DayOfWeek.Thursday)));
                Values.Add( new(idx, 4, _requestRepository.RequestForBrigadeForCertainDay(brigade.id, DayOfWeek.Friday)));
                Values.Add( new(idx, 5, _requestRepository.RequestForBrigadeForCertainDay(brigade.id, DayOfWeek.Saturday)));
                Values.Add( new(idx, 6, _requestRepository.RequestForBrigadeForCertainDay(brigade.id, DayOfWeek.Sunday)));
                idx++;
            }
        }
    }
}