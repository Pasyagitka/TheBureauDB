using System.Windows;
using TheBureau.ViewModels;

namespace TheBureau.Views.Controls
{
    public partial class InfoWindow : Window
    {
        private readonly InfoWindowViewModel _viewModel; 
        public InfoWindow()
        {
            InitializeComponent();
            _viewModel = new InfoWindowViewModel("Ошибка","Непредвиденная ошибка");
            DataContext = _viewModel;
        }
        public InfoWindow(string status, string message)
        {
            InitializeComponent();
            _viewModel = new InfoWindowViewModel(status, message);
            DataContext = _viewModel;
        }
    }
}