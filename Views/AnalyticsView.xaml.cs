using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TypingTutor.Services;

namespace TypingTutor.Views
{
    public class WeakKeyDisplayItem
    {
        public string KeyLabel { get; set; } = string.Empty;
        public string ErrorCountLabel { get; set; } = string.Empty;
    }

    public partial class AnalyticsView : UserControl
    {
        public AnalyticsView()
        {
            InitializeComponent();
            AnalyticsService.Instance.OnHistoryUpdated += RefreshData;
            RefreshData();
        }

        public void RefreshData()
        {
            var analytics = AnalyticsService.Instance;

            CardBestWpm.Value = analytics.GetBestNetWpm().ToString("F1");
            CardAvgWpm.Value = analytics.GetAverageNetWpm().ToString("F1");
            CardAvgAccuracy.Value = $"{analytics.GetAverageAccuracy():F1}%";
            CardTotalTests.Value = analytics.GetTotalTestsCount().ToString();
            var user = AuthService.Instance.CurrentUser;
            if (TxtAnalyticsAvatar != null)
            {
                TxtAnalyticsAvatar.Text = user.AvatarInitials;
            }

            if (TxtAnalyticsUserName != null)
            {
                TxtAnalyticsUserName.Text = user.DisplayName;
            }

            if (TxtAnalyticsUserStatus != null)
            {
                TxtAnalyticsUserStatus.Text = user.IsGuest 
                    ? "Mode: Guest Candidate" 
                    : $"User ID: @{user.UserId}";
            }

            if (BtnAnalyticsSignOut != null)
            {
                BtnAnalyticsSignOut.Visibility = user.IsGuest ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            }

            DgHistory.ItemsSource = analytics.GetAttemptHistory();

            var topWeak = analytics.GetTopWeakKeys(8);
            var displayList = new List<WeakKeyDisplayItem>();
            foreach (var kv in topWeak)
            {
                displayList.Add(new WeakKeyDisplayItem
                {
                    KeyLabel = kv.Key.ToString(),
                    ErrorCountLabel = $"{kv.Value} errors"
                });
            }

            LstWeakKeys.ItemsSource = displayList;
        }

        private void BtnClearHistory_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var user = AuthService.Instance.CurrentUser;
            var result = System.Windows.MessageBox.Show(
                $"Are you sure you want to clear typing test history for {user.DisplayName}?", 
                "Clear History", 
                System.Windows.MessageBoxButton.YesNo, 
                System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                AnalyticsService.Instance.ClearCurrentUserHistory();
            }
        }

        private void BtnAnalyticsSignOut_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var user = AuthService.Instance.CurrentUser;
            if (user.IsGuest) return;

            var result = System.Windows.MessageBox.Show(
                $"Are you sure you want to sign out from '{user.DisplayName}'?", 
                "Sign Out Confirmation", 
                System.Windows.MessageBoxButton.YesNo, 
                System.Windows.MessageBoxImage.Question);

            if (result == System.Windows.MessageBoxResult.Yes)
            {
                AuthService.Instance.Logout();
            }
        }
    }
}
