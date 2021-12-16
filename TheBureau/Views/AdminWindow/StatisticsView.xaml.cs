using System;
using System.Windows.Controls;
using TheBureau.Views.Controls;

namespace TheBureau.Views.AdminWindow
{
    public partial class StatisticsView : Page
    {
        //hack SSRS
        public StatisticsView()
        {
            InitializeComponent();
            Uri uri = new Uri("http://pasyagitkaasus/ReportServer2019/Pages/ReportViewer.aspx?%2FSSRS%2FAllBrigadesScheduleReport&rs%3AClearSession=true&rc%3AView=9d68cd2d-5290-4e1e-bd66-903ba055cc67&rc:Toolbar=false");
            Browser.Navigate(uri);
        }

        private void GetShedule_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Uri uri = new Uri("http://pasyagitkaasus/ReportServer2019?%2FSSRS%2FBrigadeScheduleReport&rs%3AClearSession=true&rc%3AView=e7366d73-6f3e-4f61-a295-d06774c8546b");
            Browser.Navigate(uri);
        }

        private void GetReport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Uri uri = new Uri("http://pasyagitkaasus/ReportServer2019/Pages/ReportViewer.aspx?%2fSSRS%2fRequestReport&rs:Command=Render");
            Browser.Navigate(uri);
        }

        private void GetAllBrigadesReport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Uri uri = new Uri("http://pasyagitkaasus/ReportServer2019/Pages/ReportViewer.aspx?%2FSSRS%2FAllBrigadesScheduleReport&rs%3AClearSession=true&rc%3AView=9d68cd2d-5290-4e1e-bd66-903ba055cc67&rc:Toolbar=false");
            Browser.Navigate(uri);
        }


    }
}
