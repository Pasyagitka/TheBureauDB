using System.Windows;
using TheBureau.Models;
using TheBureau.ViewModels;

namespace TheBureau.Views.AdminWindow
{
    public partial class EditToolView : Window
    {
        public EditToolView()
        {
            InitializeComponent();
        }

        public EditToolView(Tool tool)
        {
            InitializeComponent();
            EditToolViewModel editToolViewModel = new EditToolViewModel(tool);
            DataContext = editToolViewModel;
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

    }
}