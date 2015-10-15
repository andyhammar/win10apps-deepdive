using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpDeepDive.BuddyApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BuddyMainPage : Page
    {
        public BuddyMainPage()
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
                _resultsTextBox.Text = (launchUriResult.Result?.FirstOrDefault().Value as string) ?? "no response";
            }
        }

        private async void AppServiceButton_OnClick(object sender, RoutedEventArgs e)
        {
            var cmd = _commandComboBox.SelectedValue as string;
            if (string.IsNullOrEmpty(cmd)) return;

            await LaunchAppService(cmd);
        }

        private async Task LaunchAppService(string cmd)
        {
            var connection = new AppServiceConnection
            {
                AppServiceName = "uwpdeepdive-appservice",
                PackageFamilyName = "11fc3805-6c54-417b-916c-a4f6e89876b2_2eqywga5gg4gm"
            };
            var appServiceConnectionStatus = await connection.OpenAsync();
            if (appServiceConnectionStatus != AppServiceConnectionStatus.Success)
            {
                _resultsTextBox.Text = appServiceConnectionStatus.ToString();
                return;
            }

            var msg = new ValueSet {["cmd"] = cmd};
            var appServiceResponse = await connection.SendMessageAsync(msg);
            if (appServiceResponse.Status != AppServiceResponseStatus.Success)
            {
                _resultsTextBox.Text = appServiceResponse.Status.ToString();
                return;
            }
            var time = appServiceResponse.Message[cmd] as string;
            _resultsTextBox.Text = time ?? "";
        }
    }
}
