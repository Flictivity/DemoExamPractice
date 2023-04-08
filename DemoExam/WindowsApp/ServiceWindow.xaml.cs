using DemoExam.ADOApp;
using Microsoft.Win32;
using System.Data.Entity.Migrations;
using System.Windows;

namespace DemoExam.WindowsApp
{
    /// <summary>
    /// Interaction logic for ServiceWindow.xaml
    /// </summary>
    public partial class ServiceWindow : Window
    {
        private Service _service;
        private bool _isEdit;
        public ServiceWindow(Service service)
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

        private void AddNewServiceBtnClick(object sender, RoutedEventArgs e)
        {
            App.Connection.Service.AddOrUpdate(_service);
            App.Connection.SaveChanges();
            MessageBox.Show($"Успешно");
            Close();
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
