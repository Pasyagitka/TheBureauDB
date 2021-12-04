namespace TheBureau.ViewModels
{
    public class InfoWindowViewModel : ViewModelBase
    {
        private string _status;
        private string _information;

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged("Status"); }
        }

        public string Information
        {
            get => _information;
            set { _information = value; OnPropertyChanged("Information"); }
        }

        public InfoWindowViewModel(string status, string information)
        {
            Status = status;
            Information = information;
        }
    }
}