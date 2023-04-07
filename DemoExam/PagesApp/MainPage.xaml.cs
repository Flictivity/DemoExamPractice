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
            pageMainFrame.Navigate(new ServicePage());
        }
    }
}
