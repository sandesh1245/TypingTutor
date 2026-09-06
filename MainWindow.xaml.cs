using System;
using System.Windows;
using System.Windows.Media;
using TypingTutor.Models;
using TypingTutor.Services;
using TypingTutor.Views;

namespace TypingTutor
{
    public partial class MainWindow : Window
    {
        private ExamSimulationView? _examView;
        private CoursesView? _coursesView;
        private VocabBuilderView? _vocabView;
        private TypingGamesView? _gamesView;
        private PassageCatalogView? _catalogView;
        private AnalyticsView? _analyticsView;
        private ProUpgradeView? _proView;

        public MainWindow()
        {
            InitializeComponent();

            LicenseService.Instance.OnLicenseChanged += UpdateLicenseBadge;
            SyncService.Instance.OnSyncStatusChanged += UpdateSyncStatus;
            ThemeService.Instance.OnThemeChanged += ApplyTheme;
            AuthService.Instance.OnUserChanged += (u) => UpdateUserProfileUI();

            UpdateLicenseBadge();
            UpdateSyncStatus();
            ApplyTheme();
            UpdateUserProfileUI();

            NavigateToExam();

            // Bring window to foreground on launch & show login if guest
            Loaded += (s, e) =>
            {
                WindowState = WindowState.Normal;
                Show();
                Activate();
                Focus();

                // If not logged in, show Auth modal with option to login/register or skip
                if (!AuthService.Instance.IsLoggedIn)
                {
                    OpenAuthModal(isRegister: false);
                }
            };
        }

        private void UpdateUserProfileUI()
        {
            var user = AuthService.Instance.CurrentUser;
            if (TxtUserAvatarInitials != null)
            {
                TxtUserAvatarInitials.Text = user.AvatarInitials;
            }

            if (TxtUserDisplayName != null)
            {
                TxtUserDisplayName.Text = user.DisplayName;
            }

            if (TxtUserStatusLabel != null)
            {
                TxtUserStatusLabel.Text = user.IsGuest 
                    ? "Guest Mode • Click to Login" 
                    : $"@{user.UserId} • Click to Switch";
            }

            if (BtnSignOut != null)
            {
                BtnSignOut.Visibility = user.IsGuest ? Visibility.Collapsed : Visibility.Visible;
            }

            if (PanelActiveUserSignOut != null && TxtModalActiveUser != null)
            {
                PanelActiveUserSignOut.Visibility = user.IsGuest ? Visibility.Collapsed : Visibility.Visible;
                TxtModalActiveUser.Text = $"Signed in as: {user.DisplayName} (@{user.UserId})";
            }
        }

        private void BtnSignOut_Click(object sender, RoutedEventArgs e)
        {
            var user = AuthService.Instance.CurrentUser;
            if (user.IsGuest) return;

            var result = MessageBox.Show(
                $"Are you sure you want to sign out from account '{user.DisplayName}'?", 
                "Sign Out Confirmation", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                AuthService.Instance.Logout();
                if (BdrAuthModal != null)
                {
                    BdrAuthModal.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void BtnUserProfile_Click(object sender, RoutedEventArgs e)
        {
            OpenAuthModal(isRegister: false);
        }

        private void OpenAuthModal(bool isRegister)
        {
            if (BdrAuthModal == null) return;

            BdrAuthError.Visibility = Visibility.Collapsed;
            BdrAuthSuccess.Visibility = Visibility.Collapsed;

            PbAuthPassword.Password = string.Empty;
            PbAuthConfirmPassword.Password = string.Empty;

            if (isRegister)
            {
                RadTabRegister.IsChecked = true;
            }
            else
            {
                RadTabLogin.IsChecked = true;
            }

            if (AuthService.Instance.IsLoggedIn)
            {
                TxtAuthUserId.Text = AuthService.Instance.CurrentUserId;
            }
            else
            {
                TxtAuthUserId.Text = string.Empty;
            }

            BdrAuthModal.Visibility = Visibility.Visible;
            TxtAuthUserId.Focus();
        }

        private void BtnCloseAuthModal_Click(object sender, RoutedEventArgs e)
        {
            BdrAuthModal.Visibility = Visibility.Collapsed;
        }

        private void AuthTab_Checked(object sender, RoutedEventArgs e)
        {
            if (BdrAuthError == null || BdrAuthSuccess == null) return;
            BdrAuthError.Visibility = Visibility.Collapsed;
            BdrAuthSuccess.Visibility = Visibility.Collapsed;

            if (RadTabRegister?.IsChecked == true)
            {
                TxtAuthModalTitle.Text = "Create Free Account";
                PanelAuthDisplayName.Visibility = Visibility.Visible;
                PanelAuthConfirmPassword.Visibility = Visibility.Visible;
                BtnAuthSubmit.Content = "Create Account & Sign In ▶";
            }
            else
            {
                TxtAuthModalTitle.Text = "Account Sign In";
                PanelAuthDisplayName.Visibility = Visibility.Collapsed;
                PanelAuthConfirmPassword.Visibility = Visibility.Collapsed;
                BtnAuthSubmit.Content = "Sign In to Account ▶";
            }
        }

        private void BtnAuthSubmit_Click(object sender, RoutedEventArgs e)
        {
            BdrAuthError.Visibility = Visibility.Collapsed;
            BdrAuthSuccess.Visibility = Visibility.Collapsed;

            string userId = TxtAuthUserId.Text.Trim();
            string password = PbAuthPassword.Password;

            if (RadTabRegister?.IsChecked == true)
            {
                string displayName = TxtAuthDisplayName.Text.Trim();
                string confirmPassword = PbAuthConfirmPassword.Password;

                if (string.IsNullOrWhiteSpace(password) || password != confirmPassword)
                {
                    TxtAuthError.Text = "Passwords do not match. Please verify.";
                    BdrAuthError.Visibility = Visibility.Visible;
                    return;
                }

                if (AuthService.Instance.Register(userId, password, displayName, out string error))
                {
                    TxtAuthSuccess.Text = $"Welcome, {AuthService.Instance.CurrentUser.DisplayName}! Account registered.";
                    BdrAuthSuccess.Visibility = Visibility.Visible;
                    BdrAuthModal.Visibility = Visibility.Collapsed;
                }
                else
                {
                    TxtAuthError.Text = error;
                    BdrAuthError.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (AuthService.Instance.Login(userId, password, out string error))
                {
                    TxtAuthSuccess.Text = $"Welcome back, {AuthService.Instance.CurrentUser.DisplayName}!";
                    BdrAuthSuccess.Visibility = Visibility.Visible;
                    BdrAuthModal.Visibility = Visibility.Collapsed;
                }
                else
                {
                    TxtAuthError.Text = error;
                    BdrAuthError.Visibility = Visibility.Visible;
                }
            }
        }

        private void BtnAuthSkip_Click(object sender, RoutedEventArgs e)
        {
            AuthService.Instance.ContinueAsGuest();
            BdrAuthModal.Visibility = Visibility.Collapsed;
        }

        private void BtnToggleTheme_Click(object sender, RoutedEventArgs e)
        {
            ThemeService.Instance.ToggleTheme();
        }

        private void ApplyTheme()
        {
            var theme = ThemeService.Instance;
            Background = theme.WindowBackgroundBrush;
            BdrHeader.Background = theme.HeaderBackgroundBrush;
            BdrHeader.BorderBrush = theme.BorderBrush;
            BdrSidebar.Background = theme.HeaderBackgroundBrush;
            BdrSidebar.BorderBrush = theme.BorderBrush;

            if (theme.IsDarkTheme)
            {
                TxtThemeIcon.Text = "🌙 ";
                TxtThemeStatus.Text = "Dark Mode";
            }
            else
            {
                TxtThemeIcon.Text = "☀️ ";
                TxtThemeStatus.Text = "Light Mode";
            }
        }

        private void Nav_Checked(object sender, RoutedEventArgs e)
        {
            if (ViewHost == null) return;

            if (NavExam?.IsChecked == true) NavigateToExam();
            else if (NavCourses?.IsChecked == true) NavigateToCourses();
            else if (NavVocab?.IsChecked == true) NavigateToVocab();
            else if (NavGames?.IsChecked == true) NavigateToGames();
            else if (NavCatalog?.IsChecked == true) NavigateToCatalog();
            else if (NavAnalytics?.IsChecked == true) NavigateToAnalytics();
            else if (NavPro?.IsChecked == true) NavigateToPro();
        }

        private void NavigateToExam()
        {
            _examView ??= new ExamSimulationView();
            ViewHost.Content = _examView;
        }

        private void NavigateToCourses()
        {
            _coursesView ??= new CoursesView();
            ViewHost.Content = _coursesView;
        }

        private void NavigateToVocab()
        {
            _vocabView ??= new VocabBuilderView();
            ViewHost.Content = _vocabView;
        }

        private void NavigateToGames()
        {
            _gamesView ??= new TypingGamesView();
            ViewHost.Content = _gamesView;
        }

        private void NavigateToCatalog()
        {
            if (_catalogView == null)
            {
                _catalogView = new PassageCatalogView();
                _catalogView.OnSelectPassageForPractice += (passage) =>
                {
                    NavigateToExam();
                    _examView?.SelectPassage(passage);
                    if (NavExam != null) NavExam.IsChecked = true;
                };
            }
            ViewHost.Content = _catalogView;
        }

        private void NavigateToAnalytics()
        {
            _analyticsView ??= new AnalyticsView();
            _analyticsView.RefreshData();
            ViewHost.Content = _analyticsView;
        }

        private void NavigateToPro()
        {
            _proView ??= new ProUpgradeView();
            ViewHost.Content = _proView;
        }

        private void UpdateLicenseBadge()
        {
            var license = LicenseService.Instance.CurrentLicense;
            if (license.IsProActivated)
            {
                TxtProBadge.Text = "PRO ACTIVATED";
                TxtProBadge.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10B981"));
                BdrProBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#064E3B"));
            }
            else
            {
                TxtProBadge.Text = "FREE VERSION";
                TxtProBadge.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F59E0B"));
                BdrProBadge.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B"));
            }
        }

        private void UpdateSyncStatus()
        {
            TxtSyncStatus.Text = SyncService.Instance.SyncStatusMessage;
        }

        private async void BtnSyncCloud_Click(object sender, RoutedEventArgs e)
        {
            await SyncService.Instance.TriggerSyncAsync();
        }
    }
}