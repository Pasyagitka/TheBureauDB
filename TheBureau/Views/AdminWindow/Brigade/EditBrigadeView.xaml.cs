using System;
using System.Collections.Generic;
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
using TheBureau.ViewModels;

namespace TheBureau.Views.AdminWindow.Brigade
{
    /// <summary>
    /// Логика взаимодействия для EditBrigadeView.xaml
    /// </summary>
    public partial class EditBrigadeView : Window
    {
        public EditBrigadeView()
        {
            InitializeComponent();
        }

        public EditBrigadeView(Models.Brigade brigadeToEdit)
        {
            InitializeComponent();
            EditBrigadeViewModel employeeEditViewModel = new EditBrigadeViewModel(brigadeToEdit);
            DataContext = employeeEditViewModel;
        }
        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
