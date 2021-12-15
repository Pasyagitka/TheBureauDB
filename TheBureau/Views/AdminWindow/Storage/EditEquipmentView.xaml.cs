using System.Windows;
using TheBureau.Models;
using TheBureau.ViewModels;

namespace TheBureau.Views.AdminWindow
{
    public partial class EditEquipmentView : Window
    {
        public EditEquipmentView()
        {
            InitializeComponent();
        }

        public EditEquipmentView(Equipment equipment)
        {
            InitializeComponent();
            EditEquipmentViewModel editEquipmentViewModel = new EditEquipmentViewModel(equipment);
            DataContext = editEquipmentViewModel;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

    }
}