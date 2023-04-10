using DemoExam.ADOApp;
using DemoExam.Components;
using DemoExam.WindowsApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DemoExam.PagesApp
{
    /// <summary>
    /// Interaction logic for ServicePage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        private List<Service> _services;
        private List<Service> _sorted;

        private Predicate<Service> _filterQuery = x => true;
        private Func<Service, object> _sortQuery = x => x.ID;
        public ServicesPage()
        {
            InitializeComponent();
        }

        private void cbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortingMethodChange();
        }

        private void tbSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        private void FilterAndSort()
        {
            _sorted = _services.Where(x => _filterQuery(x)).OrderBy(x => _sortQuery(x)).ToList();
            lvServices.ItemsSource = _sorted;
            if (tbSearchBar.Text != "") Search();

            UpdateRecordsCount();
        }

        private void Search()
        {
            lvServices.ItemsSource = _sorted
                .Where(x => string.Join(" ", x.Title, x.Description).ToLower()
                .Contains(tbSearchBar.Text.ToLower()))
                .ToList();
            UpdateRecordsCount();
        }

        private void SortingMethodChange()
        {
            switch (cbSorting.SelectedIndex)
            {
                case 0:
                    _sortQuery = x => x.ID;
                    break;
                case 1:
                    _sortQuery = x => x.CostWithDiscount;
                    break;
                case 2:
                    _sortQuery = x => -x.CostWithDiscount;
                    break;
            }
            FilterAndSort();
        }

        private void FilterMethodChange()
        {
            switch (cbFiltering.SelectedIndex)
            {
                case 0:
                    _filterQuery = x => true;
                    break;
                case 1:
                    _filterQuery = x => x.Discount >= 0 && x.Discount < 5;
                    break;
                case 2:
                    _filterQuery = x => x.Discount >= 5 && x.Discount < 15;
                    break;
                case 3:
                    _filterQuery = x => x.Discount >= 15 && x.Discount < 30;
                    break;
                case 4:
                    _filterQuery = x => x.Discount >= 30 && x.Discount < 70;
                    break;
                case 5:
                    _filterQuery = x => x.Discount >= 70 && x.Discount < 100;
                    break;
                default:
                    _filterQuery = x => true;
                    break;
            }
            FilterAndSort();
        }

        private void UpdateRecordsCount()
        {
            tbRecordsCount.Text = $"{lvServices.Items.Count} из {_services.Count()}";
        }

        private void cbFiltering_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterMethodChange();
        }

        private void DeleteButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            int serviceId = (int)((Button)sender).Tag;

            var res = App.Connection.ClientService.Any(x => x.ServiceID == serviceId);

            if (res)
            {
                MessageBox.Show("Удаление не возможно, так как существуют записи",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                var deleteService = App.Connection.Service.FirstOrDefault(x => x.ID == serviceId);
                if (deleteService == null)
                {
                    MessageBox.Show($"Не удалось найти услугус ID {serviceId}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                var clientServices = App.Connection.ClientService.Where(x => x.ServiceID == serviceId).ToList();

                foreach (var service in clientServices)
                {
                    App.Connection.ClientService.Remove(service);
                }
                App.Connection.Service.Remove(deleteService);

                App.Connection.SaveChanges();

                MessageBox.Show("Успешно", "Сообщение", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                UpdateServices();
                return;
            }
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            int serviceId = (int)((Button)sender).Tag;
            var editService = App.Connection.Service.FirstOrDefault(x => x.ID == serviceId);

            NavigationService.Navigate(new ServicePage(editService));
            UpdateServices();
        }

        private void UpdateServices()
        {
            _services = App.Connection.Service.ToList();
            lvServices.ItemsSource = _services;
            FilterAndSort();
        }

        private void CreateNewServiceBtnClick(object sender, RoutedEventArgs e)
        {
            var serviceWindow = new ServicePage(null);
            NavigationService.Navigate(new ServicePage(null));
        }

        private void AdminModeBtnClick(object sender, RoutedEventArgs e)
        {
            if (btnAdminModeEnter.Content == "Выход")
            {
                App.IsAdministratorMode = false;
                UpdateAdminComponents();
                return;
            }

            var window = new PasswordWindow();
            window.ShowDialog();
            UpdateAdminComponents();
        }

        private void UpdateAdminComponents()
        {
            btnNewService.Visibility = App.IsAdministratorMode ? Visibility.Visible : Visibility.Collapsed;
            btnRegistrations.Visibility = App.IsAdministratorMode ? Visibility.Visible : Visibility.Collapsed;
            btnAdminModeEnter.Content = App.IsAdministratorMode ? "Выход" : "Войти как администратор";
            UpdateServices();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _services = App.Connection.Service.ToList();
            lvServices.ItemsSource = _services;

            cbSorting.ItemsSource = SortingMethods.Methods;
            cbFiltering.ItemsSource = FilterMethods.Methods;
            cbSorting.SelectedIndex = 0;
            cbFiltering.SelectedIndex = 0;

            UpdateAdminComponents();
            UpdateRecordsCount();
        }

        private void lvServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!App.IsAdministratorMode)
            {
                return;
            }
            NavigationService.Navigate(new ClientServiceRegistrationPage((Service)lvServices.SelectedItem));
        }

        private void ShowRegistrationsBtnClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminPage());
        }
    }
}
