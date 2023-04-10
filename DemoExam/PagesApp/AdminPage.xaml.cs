using System.Linq;
using System.Windows.Controls;

namespace DemoExam.PagesApp
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            lvRecords.ItemsSource = App.Connection.ClientService.ToList();
        }
    }
}
