using System.Windows;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.ViewModels;

namespace TheBureau.Views
{
    public partial class EditClientView : Window
    {
        public EditClientView()
        {
            InitializeComponent();
        }
        public EditClientView(Client selected)
        {
            InitializeComponent();
            EditClientViewModel _editClientViewModel = new EditClientViewModel(selected);
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