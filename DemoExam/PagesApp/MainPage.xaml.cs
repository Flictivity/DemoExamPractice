using System.Windows.Controls;

namespace DemoExam.PagesApp
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            pageMainFrame.Navigate(new ServicesPage());
        }

        private void BackButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
