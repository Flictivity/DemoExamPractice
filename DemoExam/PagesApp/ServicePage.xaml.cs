using DemoExam.ADOApp;
using DemoExam.Components;
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
        public ServicePage()
        {
            InitializeComponent();

            _services = App.Connection.Service.ToList();
            lvServices.ItemsSource = _services;

            cbSorting.ItemsSource = SortingMethods.Methods;
            cbSorting.SelectedIndex = 0;

            UpdateRecordsCount();
        }

        private void cbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void tbSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Serach();
        }

        private void Serach()
        {
            if (tbSearchBar.Text == "")
            {
                _sorted = _services;
            }
            lvServices.ItemsSource = _sorted
                .Where(x => string.Join(" ", x.Title, x.Description).ToLower()
                .Contains(tbSearchBar.Text.ToLower()))
                .ToList();
            UpdateRecordsCount();
        }

        private void Filter()
        {
            switch (cbSorting.SelectedIndex)
            {
                case 0:
                    _sorted = _services;
                    break;
                case 1:
                    _sorted = _services.OrderBy(x => x.CostWithDiscount).ToList();
                    break;
                case 2:
                    _sorted = _services.OrderByDescending(x => x.CostWithDiscount).ToList();
                    break;
                case 3:
                    _sorted = _services.Where(x => x.Discount >= 0 && x.Discount < 5).ToList();
                    break;
                case 4:
                    _sorted = _services.Where(x => x.Discount >= 5 && x.Discount < 15).ToList();
                    break;
                case 5:
                    _sorted = _services.Where(x => x.Discount >= 15 && x.Discount < 30).ToList();
                    break;
                case 6:
                    _sorted = _services.Where(x => x.Discount >= 30 && x.Discount < 70).ToList();
                    break;
                case 7:
                    _sorted = _services.Where(x => x.Discount >= 70 && x.Discount < 100).ToList();
                    break;
                default:
                    _sorted = _services;
                    break;
            }
            lvServices.ItemsSource = _sorted;
            if (tbSearchBar.Text != "") Serach();
            UpdateRecordsCount();
        }

        private void UpdateRecordsCount()
        {
            tbRecordsCount.Text = $"{lvServices.Items.Count} из {_services.Count()}";
        }
    }
}
