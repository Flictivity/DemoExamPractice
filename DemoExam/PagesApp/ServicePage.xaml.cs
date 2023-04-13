using DemoExam.ADOApp;
using Microsoft.Win32;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DemoExam.Components;

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
        }

        private void MainImageChangeButtonClick(object sender, RoutedEventArgs e)
        {
            var window = new OpenFileDialog();
            if (window.ShowDialog() == true)
            {
                var bytePhoto = File.ReadAllBytes(window.FileName);
                _service.PhotoByte = bytePhoto;

                try
                {
                    imgMainImage.Source = ByteImageConverter.ByteToImage(_service.PhotoByte);
                    MessageBox.Show("Успешно", "Сообщение", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка", "Ошибка", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
                
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
            if (!int.TryParse(tbDuration.Text, out duration)
                || !double.TryParse(tbDiscount.Text, out double discount)
                || !decimal.TryParse(tbCost.Text, out decimal cost))
            {
                MessageBox.Show("Скидка, длительность и стоимость должны быть целыми числами", "Ошибка",
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
                var bytePhoto = File.ReadAllBytes(window.FileName);

                _service.ServicePhoto.Add(new ServicePhoto{Service = _service, PhotoByte = bytePhoto, PhotoPath = "/"});
                lvImages.ItemsSource = null;
                lvImages.ItemsSource = _service.ServicePhoto.ToList();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _service;
            tblID.Visibility = _isEdit ? Visibility.Visible : Visibility.Collapsed;
            tbID.Visibility = _isEdit ? Visibility.Visible : Visibility.Collapsed;
            lvImages.ItemsSource = _service.ServicePhoto.ToList();
        }

        private void RemoveServiceImageBtnClick(object sender, RoutedEventArgs e)
        {
            var tag = (byte[]) ((Button) sender).Tag;
            var image = _service.ServicePhoto.FirstOrDefault(x => x.PhotoByte == tag);
            _service.ServicePhoto.Remove(image);
            lvImages.ItemsSource = null;
            lvImages.ItemsSource = _service.ServicePhoto.ToList();
            if (_isEdit)
            {
                var deleteImage = App.Connection.ServicePhoto.ToList().FirstOrDefault(x => x.ID == image?.ID);
                if(deleteImage != null) App.Connection.ServicePhoto.Remove(deleteImage);
                App.Connection.SaveChanges();
            }
        }
    }
}
