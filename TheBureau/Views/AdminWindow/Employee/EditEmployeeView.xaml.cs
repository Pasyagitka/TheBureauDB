using System.Windows;
using System.Windows.Input;
using TheBureau.ViewModels;

namespace TheBureau.Views.AdminWindow.Employee
{
    public partial class EditEmployeeView : Window
    {
        public EditEmployeeView()
        {
            InitializeComponent();
        }
        public EditEmployeeView(Models.Employee selected)
        {
            InitializeComponent();
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel(selected);
            DataContext = employeeEditViewModel;
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