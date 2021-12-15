using System.Windows;
using TheBureau.Models;
using TheBureau.ViewModels;

namespace TheBureau.Views.AdminWindow
{
    public partial class AddAccessoryView : Window
    {
        public AddAccessoryView()
        {
            InitializeComponent();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

    }
}