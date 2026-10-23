using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CertControl.Data;

namespace CertControl.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private readonly cert_control_dbEntities _context;

        public LoginPage()
        {
            InitializeComponent();
            _context = cert_control_dbEntities.GetContext();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();

            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            string passwordHash = Classes.PasswordHelper.GetHash(password);

            var user = _context.User.Include(u => u.Role).FirstOrDefault(
                        u => u.login == login &&
                             u.password_hash == passwordHash &&
                             u.user_account_state_id == 1);

            if (user == null)
            {
                MessageBox.Show(
                    "Неверный логин или пароль.");

                return;
            }

            Classes.Manager.CurrentUser = user;

            Classes.Manager.MainFrame.Navigate(
                new CertificatesPage());
        }
    }
}
