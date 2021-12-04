using System.Windows;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.ViewModels;

namespace TheBureau.Views
{
    public partial class EditRequestFromBrigadeView : Window
    {
        public EditRequestFromBrigadeView()
        {
            InitializeComponent();
        }
        
        public EditRequestFromBrigadeView(Request request)
        {
            InitializeComponent();
            EditRequestFromBrigadeViewModel editRequestViewModel = new EditRequestFromBrigadeViewModel(request);
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