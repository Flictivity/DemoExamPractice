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
        private TimeSpan _duration;
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


        }

        private bool IsTimeCorrect()
        {
            if (tbTimeStart.Text == "" || !TimeSpan.TryParse(tbTimeStart.Text, out _duration))
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
            tbTimeEnd.Text = _duration.Add(TimeSpan.FromSeconds(_service.DurationInSeconds)).ToString(@"hh\:mm");
        }
    }
}
