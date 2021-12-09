using System;
using System.Windows;
using System.Windows.Input;
using TheBureau.Views;
using TheBureau.Views.AdminWindow;
using TheBureau.Views.AdminWindow.Employee;
using TheBureau.Views.AuthWindow;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int _selectedIndex;
        private string _mainTopText;
        private object _content;
        private int _countRed;

        private WindowState _windowState;

        private ICommand _logOutCommand;
        private ICommand _closeWindowCommand;
        private ICommand _minimizeWindowCommand;
        private ICommand _maximizeWindowCommand;


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
                        InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при выходе из аккаунта");
                        infoWindow.ShowDialog();
                    }
                });
            }
        }

        #region Resize

        public WindowState WindowState
        {
            get => _windowState;
            set { _windowState = value; OnPropertyChanged("WindowState"); }
        }

        public ICommand CloseWindowCommand =>
            _closeWindowCommand ??= new RelayCommand(obj => { Application.Current.Shutdown(); });

        public ICommand MinimizeWindowCommand =>
            _minimizeWindowCommand ??= new RelayCommand(obj => { WindowState = WindowState.Minimized; });

        public ICommand MaximizeWindowCommand => _maximizeWindowCommand ??= new RelayCommand(obj =>
            { WindowState = WindowState != WindowState.Maximized ? WindowState.Maximized : WindowState.Normal; });

        #endregion
        public object Content
        {
            get => _content;
            set
            {
                _content = value;
                CountRed = 0; //
                OnPropertyChanged("Content");
            }
        }

        #region Properties
        public string MainTopText
        {
            get => _mainTopText;
            set { _mainTopText = value; OnPropertyChanged("MainTopText"); }
        }
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                SetPage(_selectedIndex);
                OnPropertyChanged("SelectedIndex");
            }
        }
        public int CountRed
        {
            get => _countRed;
            set { _countRed = value; OnPropertyChanged("CountRed"); }
        }
        #endregion
        
        public MainWindowViewModel()
        {
            Content = new StatisticsView();
            WindowState = WindowState.Normal;
            CountRed = 0; //
            SelectedIndex = 1;
        }

        private void SetPage(int index)
        {
            switch (index)
            {
                case 0:
                    Content = new StatisticsView();
                    MainTopText = "БЮРО МОНТАЖНИКА";
                    break;
                case 1:
                    Content = new RequestView();
                    MainTopText = "ЗАЯВКИ";
                    break;
                case 2:
                    Content = new BrigadeView();
                    MainTopText = "БРИГАДЫ";
                    break;
                case 3:
                    Content = new ClientView();
                    MainTopText = "КЛИЕНТЫ";
                    break;
                case 4:
                    Content = new EmployeeView();
                    MainTopText = "РАБОТНИКИ";
                    break;
                case 5:
                    Content = new StorageView();
                    MainTopText = "СКЛАД";
                    break;
                default:
                    Content = new StatisticsView();
                    MainTopText = "БЮРО МОНТАЖНИКА";
                    break;
            }
        }
    }
}