using System.Windows;
using System.Windows.Input;

namespace TheBureau.Views.ClientWindow
{
    public partial class ClientWindowView : Window
    {
        public ClientWindowView()
        {
            InitializeComponent();
        }
        private void TopGrid_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
