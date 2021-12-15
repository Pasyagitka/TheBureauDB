using System.Windows;
using TheBureau.Models;
using TheBureau.ViewModels;

namespace TheBureau.Views.AdminWindow
{
    public partial class AddToolView : Window
    {
        public AddToolView()
        {
            InitializeComponent();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

    }
}