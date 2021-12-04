using System.Configuration;
using System.Threading;
using System.Windows;

namespace TheBureau
{
    public partial class App : Application
    {
        const string AppId = "TheBureauByPasyagitka";
        static Mutex mutex = new Mutex(false, AppId);
        static bool mutexAccessed = false;

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Application.Current.Properties["User"] = null;
            Application.Current.Properties["Role"] = null;
            //Application.Current.Properties["CurrentConnectionString"] = ConfigurationManager.ConnectionStrings["AdminConnection"].ConnectionString;
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                if (mutex.WaitOne(0))
                    mutexAccessed = true;
            }
            catch (AbandonedMutexException)
            {
                mutexAccessed = true;
            }
            if (mutexAccessed)
            {
                base.OnStartup(e);
            }
            else
            {
                MessageBox.Show("Приложение уже запущено", "Отмена", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (mutexAccessed) mutex?.ReleaseMutex();
            mutex?.Dispose();
            mutex = null;
            base.OnExit(e);
        }
    }
}
