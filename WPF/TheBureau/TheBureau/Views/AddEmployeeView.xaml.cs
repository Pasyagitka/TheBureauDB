using System.Windows;
using System.Windows.Input;

namespace TheBureau.Views
{
    public partial class AddEmployeeView : Window
    {
        public AddEmployeeView()
        {
            InitializeComponent();
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