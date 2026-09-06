using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TypingTutor.Services;

namespace TypingTutor.Views
{
    public partial class ProUpgradeView : UserControl
    {
        public ProUpgradeView()
        {
            InitializeComponent();
            LicenseService.Instance.OnLicenseChanged += RefreshLicenseState;
            RefreshLicenseState();
        }

        private void RefreshLicenseState()
        {
            var license = LicenseService.Instance.CurrentLicense;
            if (license.IsProActivated)
            {
                TxtLicenseState.Text = "PRO ACTIVATED";
                TxtLicenseState.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10B981"));
                TxtLicenseUser.Text = license.RegisteredUser;
                BdrLicenseStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#064E3B"));
                BtnDeactivate.Visibility = Visibility.Visible;
            }
            else
            {
                TxtLicenseState.Text = "FREE VERSION";
                TxtLicenseState.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F59E0B"));
                TxtLicenseUser.Text = "Offline Candidate";
                BdrLicenseStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B"));
                BtnDeactivate.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnActivate_Click(object sender, RoutedEventArgs e)
        {
            string key = TxtLicenseInput.Text.Trim();
            if (LicenseService.Instance.ActivateLicense(key))
            {
                MessageBox.Show("Congratulations! Pro Version successfully activated. All 800+ passages, custom imports, and AI smart drills are now unlocked!", "License Activated", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Invalid License Key. Please enter a valid activation code (e.g. TYPING-PRO-2026).", "Activation Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDeactivate_Click(object sender, RoutedEventArgs e)
        {
            LicenseService.Instance.DeactivateLicense();
            MessageBox.Show("License deactivated. Reverted to Free tier.", "License Reset", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
