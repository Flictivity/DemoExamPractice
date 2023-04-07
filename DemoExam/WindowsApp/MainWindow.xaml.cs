using DemoExam.PagesApp;
using System.Windows;

namespace DemoExam.WindowsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Navigate(new MainPage());
        }
    }
}
