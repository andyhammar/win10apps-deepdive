// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace UwpDeepDive.MainApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BackgroundPage
    {
        public BackgroundPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private async void TriggerButton_OnClick(object sender, RoutedEventArgs e)
        {
            await App.TriggerBackgroundTask();
        }


        private void ExtendedExecutionButton_OnClick(object sender, RoutedEventArgs e)
        {
            App.AskForExtendedExecutionOnNextSuspend = true;
        }
    }
}
