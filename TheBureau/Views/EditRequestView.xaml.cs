using System.Windows;
using TheBureau.Models;
using TheBureau.ViewModels;

namespace TheBureau.Views
{
    public partial class EditRequestView : Window
    {
        public EditRequestView()
         {
             InitializeComponent();
         }
        public EditRequestView(Request request)
        {
            InitializeComponent();
            EditRequestViewModel editRequestViewModel = new EditRequestViewModel(request);
            DataContext = editRequestViewModel;
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