using System;
using System.Windows;
using System.Windows.Input;
using TheBureau.Views;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class HelloPageViewModel : ViewModelBase
    {
        private ICommand _enterAsClientCommand;
        public ICommand EnterAsClientCommand
        {
            get
            {
                return _enterAsClientCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        var clientMainWindow = new ClientWindowView();
                        Application.Current.Windows[0]?.Close();
                        clientMainWindow.Show();
                    }
                    catch (Exception)
                    {
                        InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при входе как клиент");
                        infoWindow.ShowDialog();
                    }
                });
            }
        }
    }
}