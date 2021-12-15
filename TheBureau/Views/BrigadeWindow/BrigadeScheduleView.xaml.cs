
using System;
using System.Windows;

namespace TheBureau.Views.BrigadeWindow
{
    /// <summary>
    /// Логика взаимодействия для BrigadeScheduleView.xaml
    /// </summary>
    public partial class BrigadeScheduleView : Window
    {
        public BrigadeScheduleView()
        {
            InitializeComponent();
        }

        public BrigadeScheduleView(int brigadeId)
        {
            InitializeComponent();
            Uri uri = new Uri("http://pasyagitkaasus/ReportServer2019?%2FSSRS%2FBrigadeScheduleReport&rs%3AClearSession=true&rc%3AView=e7366d73-6f3e-4f61-a295-d06774c8546b&rc:Toolbar=false&brigadeId=" + brigadeId);
            Browser.Navigate(uri);
        }
    }
}
