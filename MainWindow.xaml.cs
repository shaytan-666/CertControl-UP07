using System.Windows;

namespace CertControl
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Classes.Manager.MainFrame = MainFrame;

            Classes.Manager.MainFrame.Navigate(
                new Pages.LoginPage());
        }
    }
}
