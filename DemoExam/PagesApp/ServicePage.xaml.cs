using System.Linq;
using System.Windows.Controls;

namespace DemoExam.PagesApp
{
    /// <summary>
    /// Interaction logic for ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        public ServicePage()
        {
            InitializeComponent();

            lvServices.ItemsSource = App.Connection.Service.ToList();
        }
    }
}
