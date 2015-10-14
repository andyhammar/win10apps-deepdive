using System;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpDeepDive.BuddyApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void LaunchProtocolButton_OnClick(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("uwpddmainapp:"));
        }

        private async void LaunchForResultsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var uri = new Uri("uwpddmainapp:");
            var options = new LauncherOptions
            {
                TargetApplicationPackageFamilyName = "11fc3805-6c54-417b-916c-a4f6e89876b2_2eqywga5gg4gm"
            };
            var launchUriResult = await Launcher.LaunchUriForResultsAsync(uri, options);
            if (launchUriResult.Status == LaunchUriStatus.Success)
            {
                _resultsTextBox.Text = (launchUriResult.Result.FirstOrDefault().Value as string) ?? "error";
            }
        }
    }
}
