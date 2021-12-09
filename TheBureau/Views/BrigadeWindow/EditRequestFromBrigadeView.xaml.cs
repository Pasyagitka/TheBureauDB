using System.Windows;
using System.Windows.Controls;
using TheBureau.Models;
using TheBureau.ViewModels;

namespace TheBureau.Views.BrigadeWindow
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

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (DataContext as EditRequestFromBrigadeViewModel)?.BrigadeEmployees.Clear();
            foreach (var i in ListBoxEmployeesAttendance.SelectedItems) (DataContext as EditRequestFromBrigadeViewModel)?.BrigadeEmployees.Add(i as Employee);
        }
    }
}