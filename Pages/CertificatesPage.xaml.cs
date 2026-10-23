using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CertControl.Classes;
using CertControl.Data;
using CertControl.Dtos;

namespace CertControl.Pages
{
    /// <summary>
    /// Логика взаимодействия для CertificatesPage.xaml
    /// </summary>
    public partial class CertificatesPage : Page
    {
        private readonly cert_control_dbEntities _context;

        public CertificatesPage()
        {
            InitializeComponent();
            _context = cert_control_dbEntities.GetContext();
            ApplyPermissions();
            CurrentUserTextBlock.Text = Manager.CurrentUser.login;
            LoadCertificates();
        }

        private void LoadCertificates()
        {
            var certs = _context.Certificate
                .Include(c => c.CertificateStatus)
                .Include(c => c.CertificateType)
                .Include(c => c.CertificateAuthority)
                .Include(c => c.Owner)
                .Include(c => c.Server)
                .ToList();

            CertificatesDataGrid.ItemsSource = certs.Select(c => DtoMapper.GetCertificateDto(c)).ToList();
        }

        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditCertificatePage(_context));
        }

        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (CertificatesDataGrid.SelectedItem is CertificateDto selectedDto)
            {
                var cert = _context.Certificate.First(c => c.id == selectedDto.Id);
                Manager.MainFrame.Navigate(new AddEditCertificatePage(_context, cert));
            }
            else
                MessageBox.Show("Выберите сертификат.");
        }

        private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (CertificatesDataGrid.SelectedItem is CertificateDto selectedDto)
            {
                var cert = _context.Certificate.First(c => c.id == selectedDto.Id);
                var result = MessageBox.Show(
                                "Удалить сертификат?",
                                "Подтверждение",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                    return;

                try
                {
                    _context.Certificate.Remove(cert);
                    _context.SaveChanges();

                    LoadCertificates();

                    MessageBox.Show(
                        "Сертификат удалён.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Ошибка");
                }

            }
            else
                MessageBox.Show("Выберите сертификат.");
        }

        private void RefreshButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadCertificates();
        }

        private void Page_IsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == System.Windows.Visibility.Visible)
            {
                _context.ChangeTracker.Entries().ToList().ForEach(entry => entry.Reload());
                LoadCertificates();
            }
        }

        private void ApplyPermissions()
        {
            var user = Classes.Manager.CurrentUser;

            if (user == null)
                return;

            string role = user.Role.role1;

            if (role == "Администратор")
            {
                return;
            }

            if (role == "Оператор")
            {
                DeleteButton.Visibility = Visibility.Collapsed;
            }

            if (role == "Менеджер")
            {
                AddButton.Visibility = Visibility.Collapsed;
                EditButton.Visibility = Visibility.Collapsed;
                DeleteButton.Visibility = Visibility.Collapsed;
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.CurrentUser = null;

            Manager.MainFrame.Navigate(
                new LoginPage());
        }
    }
}
