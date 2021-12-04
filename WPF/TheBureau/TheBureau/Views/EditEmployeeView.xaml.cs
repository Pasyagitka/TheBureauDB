using System.Windows;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.ViewModels;

namespace TheBureau.Views
{
    public partial class EditEmployeeView : Window
    {
        public EditEmployeeView()
        {
            InitializeComponent();
        }
        public EditEmployeeView(Employee selected)
        {
            InitializeComponent();
            EmployeeEditViewModel _editClientViewModel = new EmployeeEditViewModel(selected);
            DataContext = _editClientViewModel;
        }
        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void NumericTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
           if(!char.IsDigit(e.Text, 0))
           {
               e.Handled = true;
           }
        }
    }
}