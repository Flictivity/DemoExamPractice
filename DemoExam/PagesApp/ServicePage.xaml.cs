using DemoExam.ADOApp;
using DemoExam.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace DemoExam.PagesApp
{
    /// <summary>
    /// Interaction logic for ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        private List<Service> _services;
        private List<Service> _sorted;

        private Predicate<Service> _filterQuery = x => true;
        private Func<Service, object> _sortQuery = x => x.ID;
        public ServicePage()
        {
            InitializeComponent();

            _services = App.Connection.Service.ToList();
            lvServices.ItemsSource = _services;

            cbSorting.ItemsSource = SortingMethods.Methods;
            cbFiltering.ItemsSource = FilterMethods.Methods;
            cbSorting.SelectedIndex = 0;
            cbFiltering.SelectedIndex = 0;

            UpdateRecordsCount();
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
            if (tbSearchBar.Text == "0000") App.IsAdministratorMode = true;

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
    }
}
