using DemoExam.ADOApp;
using Microsoft.Win32;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DemoExam.PagesApp
{
    /// <summary>
    /// Interaction logic for ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        private Service _service;
        private bool _isEdit;
        public ServicePage(Service service)
        {
            InitializeComponent();
            _service = service;
            if (_service != null)
            {
                _isEdit = true;
            }
            else
            {
                _service = new Service();
            }
            this.DataContext = _service;

            tblID.Visibility = _isEdit ? Visibility.Visible : Visibility.Collapsed;
            tbID.Visibility = _isEdit ? Visibility.Visible : Visibility.Collapsed;
        }

        private void MainImageChangeButtonClick(object sender, RoutedEventArgs e)
        {
            var window = new OpenFileDialog();
            if (window.ShowDialog() == true)
            {
                MessageBox.Show($"Успешно{window.FileName}");
                //var newPath = Environment.CurrentDirectory + $@"\Услуги школы\\{Path.GetFileName(window.FileName)}";

                //File.Copy(window.FileName, newPath);
                //_service.MainImagePath = $@"Услуги школы\{Path.GetFileName(window.FileName)}";
            }
        }

        private void SaveBtnClick(object sender, RoutedEventArgs e)
        {
            if (tbCost.Text == "" || tbDiscount.Text == "" || tbName.Text == ""
                || tbDuration.Text == "" || _service.MainImagePath == "")
            {
                MessageBox.Show("Необходимо заполнить все поля", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int duration = 0;
            if (!int.TryParse(tbDuration.Text, out duration) || !int.TryParse(tbDiscount.Text, out int discount))
            {
                MessageBox.Show("Скидка и длительность должны быть целыми числами", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (duration / 3600 > 4)
            {
                MessageBox.Show("Длительность проведения услуги не может быть больше 4 часов", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!_isEdit)
            {
                var checkServiceExists = App.Connection.Service.Any(x => x.Title == _service.Title);
                if (checkServiceExists)
                {
                    MessageBox.Show("Услуга с таким названием уже существует", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            App.Connection.Service.AddOrUpdate(_service);
            App.Connection.SaveChanges();
            MessageBox.Show("Успешно", "Сообщение", MessageBoxButton.OK,
                MessageBoxImage.Information);
            NavigationService.GoBack();
        }

        private void AddNewImageBtnClick(object sender, RoutedEventArgs e)
        {
            var window = new OpenFileDialog();
            if (window.ShowDialog() == true)
            {
                MessageBox.Show($"Успешно{window.FileName}");
            }
        }
    }
}
