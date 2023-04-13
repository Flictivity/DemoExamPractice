using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DemoExam.PagesApp
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private DispatcherTimer _timer;
        public AdminPage()
        {
            InitializeComponent();
        }

        private void TimerInit()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(30);
            _timer.Tick += TimerTick;
            _timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            RefreshRecords();
        }
        private void RefreshRecords()
        {
            lvRecords.ItemsSource = null;
            lvRecords.ItemsSource = App.Connection.ClientService
                .ToList()
                .Where(x => x.StartTime >= DateTime.Now && x.StartTime < DateTime.Now.AddDays(2));
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            TimerInit();
            RefreshRecords();
        }

        private void Page_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _timer.Stop();
            _timer = null;
        }
    }
}
