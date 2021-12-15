using System.Windows;
using TheBureau.Models;
using TheBureau.ViewModels;

namespace TheBureau.Views.AdminWindow
{
    public partial class EditAccessoryView : Window
    {
        public EditAccessoryView()
        {
            InitializeComponent();
        }

        public EditAccessoryView(Accessory accessory)
        {
            InitializeComponent();
            EditAccessoryViewModel editAccessoryViewModel = new EditAccessoryViewModel(accessory);
            DataContext = editAccessoryViewModel;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

    }
}