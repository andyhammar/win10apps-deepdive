using System;
using Windows.ApplicationModel;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using UwpDeepDive.MainApp.Helpers;
using UwpDeepDive.MainApp.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UwpDeepDive.MainApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppToAppPage
    {
        public AppToAppPage()
        {
            this.InitializeComponent();
            AppLog.Write($"PFN: {Package.Current.Id.FamilyName}");
        }

        private async void OpenBuddyAppButton_OnClick(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("uwpddbuddyapp://"));
        }
    }
}
