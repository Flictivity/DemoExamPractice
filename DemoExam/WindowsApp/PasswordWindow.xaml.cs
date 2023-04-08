using System.Windows;

namespace DemoExam.WindowsApp
{
    /// <summary>
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        public PasswordWindow()
        {
            InitializeComponent();
        }

        private void EnterBtnClick(object sender, RoutedEventArgs e)
        {
            if (tbPassword.Text == "0000")
            {
                App.IsAdministratorMode = true;
            }
            Close();
        }
    }
}
