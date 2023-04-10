using DemoExam.ADOApp;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DemoExam.PagesApp
{
    /// <summary>
    /// Interaction logic for ClientServiceRegistrationPage.xaml
    /// </summary>
    public partial class ClientServiceRegistrationPage : Page
    {
        private Service _service = new Service();
        private TimeSpan _startTime;
        public ClientServiceRegistrationPage(Service service)
        {
            InitializeComponent();
            _service = service;

            this.DataContext = _service;
            cbClients.ItemsSource = App.Connection.Client.ToList();
        }

        private void SaveBtnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!IsTimeCorrect())
            {
                return;
            }
            if (cbClients.SelectedItem == null)
            {
                return;
            }
            var startTimeDate = new DateTime(dpDate.SelectedDate.Value.Year, dpDate.SelectedDate.Value.Month, dpDate.SelectedDate.Value.Day);
            var newRegistration = new ClientService
            {
                Client = (Client)cbClients.SelectedItem,
                Service = _service,
                StartTime = startTimeDate + _startTime,
                Comment = tbComment.Text
            };

            App.Connection.ClientService.Add(newRegistration);
            App.Connection.SaveChanges();
            MessageBox.Show("Успешно", "Сообщение", MessageBoxButton.OK,
                MessageBoxImage.Information);
            NavigationService.GoBack();
        }

        private bool IsTimeCorrect()
        {
            if (tbTimeStart.Text == "" || !TimeSpan.TryParse(tbTimeStart.Text, out _startTime))
            {
                MessageBox.Show("Введено не корректное время", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void tbTimeStart_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsTimeCorrect())
            {
                return;
            }
            tbTimeEnd.Text = _startTime.Add(TimeSpan.FromSeconds(_service.DurationInSeconds)).ToString(@"hh\:mm");
        }
    }
}
