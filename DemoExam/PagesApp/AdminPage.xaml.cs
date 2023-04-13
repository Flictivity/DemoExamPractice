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
            _timer.Tick += new EventHandler(TimerTick);
            _timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            RefreshRecords();
        }
        private void RefreshRecords()
        {
            var addedDay = DateTime.Now.AddDays(1);
            lvRecords.ItemsSource = null;
            lvRecords.ItemsSource = App.Connection.ClientService
                .ToList()
                .Where(x => (x.StartTime.Date == DateTime.Now.Date
                && x.StartTime.TimeOfDay > DateTime.Now.TimeOfDay) ||
                (x.StartTime.Date == addedDay.Date)).OrderBy(x => x.StartTime).ToList();
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
