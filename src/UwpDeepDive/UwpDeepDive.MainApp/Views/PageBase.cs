using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UwpDeepDive.MainApp.Views
{
    public class PageBase : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            UpdateBackButton();
        }

        private void UpdateBackButton()
        {
            var frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                frame = Frame;
                if (frame == null)
                    return;
            }

            var canGoBack = frame.CanGoBack;
            var appViewBackButtonVisibility = canGoBack
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                appViewBackButtonVisibility;
        }
    }
}
