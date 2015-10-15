using Windows.ApplicationModel.Activation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UwpDeepDive.MainApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProtocolForResultsPage : Page
    {
        private ProtocolForResultsActivatedEventArgs _args;

        public ProtocolForResultsPage()
        {
            this.InitializeComponent();
        }

        public void InitProtocolResponse(ProtocolForResultsActivatedEventArgs args)
        {
            _args = args;
            _callerNameTextBlock.Text = args.CallerPackageFamilyName;
        }

        private void YesButton_OnClick(object sender, RoutedEventArgs e)
        {
            var result = new ValueSet {{"note", ApplicationData.Current.LocalSettings.Values["note"] as string}};
            _args.ProtocolForResultsOperation.ReportCompleted(result);

            Application.Current.Exit();
        }

        private void NoButton_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
