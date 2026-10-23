using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CertControl.Classes;
using CertControl.Data;

namespace CertControl.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditCertificatePage.xaml
    /// </summary>
    public partial class AddEditCertificatePage : Page
    {
        private readonly cert_control_dbEntities _context;
        private Certificate _currentCert = new Certificate();
        public AddEditCertificatePage(cert_control_dbEntities context, Certificate certificate = null)
        {
            InitializeComponent();

            _context = context;
            if (certificate != null)
            {
                _currentCert = certificate;
            }

            DataContext = _currentCert;

            Init();
        }

        private void Init()
        {
            OwnerComboBox.ItemsSource = _context.Owner.ToList();
            TypeComboBox.ItemsSource = _context.CertificateType.ToList();
            StatusComboBox.ItemsSource = _context.CertificateStatus.ToList();
            AuthorityComboBox.ItemsSource = _context.CertificateAuthority.ToList();
        }

        private void SaveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_currentCert.serial_number))
                {
                    MessageBox.Show("Введите номер сертификата.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(_currentCert.name))
                {
                    MessageBox.Show("Введите название.");
                    return;
                }

                if (_currentCert.end_date < _currentCert.issue_date)
                {
                    MessageBox.Show("Дата окончания не может быть раньше даты выдачи.");
                    return;
                }

                if (_currentCert.owner_id == 0)
                {
                    MessageBox.Show("Выберите владельца.");
                    return;
                }

                if (_currentCert.certificate_type_id == 0)
                {
                    MessageBox.Show("Выберите тип сертификата.");
                    return;
                }

                if (_currentCert.certificate_type_id == 0)
                {
                    MessageBox.Show("Выберите статус сертификата.");
                    return;
                }

                if (_currentCert.certificate_authority_id == 0)
                {
                    MessageBox.Show("Выберите центр сертификации.");
                    return;
                }

                if (_currentCert.id == 0)
                {
                    _context.Certificate.Add(_currentCert);
                }

                _context.SaveChanges();

                MessageBox.Show("Данные сохранены.");

                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка");
            }
        }


        private void BackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Manager.MainFrame.GoBack();
        }
    }
}
