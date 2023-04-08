﻿using DemoExam.ADOApp;
using Microsoft.Win32;
using System.Data.Entity.Migrations;
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

        private void AddNewServiceBtnClick(object sender, RoutedEventArgs e)
        {
            App.Connection.Service.AddOrUpdate(_service);
            App.Connection.SaveChanges();
            MessageBox.Show($"Успешно");
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
