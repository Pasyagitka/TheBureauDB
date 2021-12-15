using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TheBureau.Models;
using TheBureau.ViewModels;
using TheBureau.Views.Controls;

namespace TheBureau.Views
{
    /// <summary>
    /// Логика взаимодействия для BrigadeMainWindow.xaml
    /// </summary>
    public partial class BrigadeWindowView : Window
    {
        BrigadeWindowViewModel vm;

        public BrigadeWindowView()
        {
            InitializeComponent();
            vm = new BrigadeWindowViewModel();
            DataContext = vm;
        }

        private void TopGrid_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetSchedule();
            
        }
        private void SetSchedule()
        {
            try
            {
                ObservableCollection<Employee> observableCollection = new();
                foreach (Employee item in A.SelectedItems)
                {
                    observableCollection.Add(item);
                }
                vm.SetEmployeesSchedule(observableCollection);
            }
            catch (Exception e)
            {
                InfoWindow i = new InfoWindow("Ошибка", "Не удалось установить работников для этой заявки");
                i.ShowDialog();
            }
        }
    }
}
