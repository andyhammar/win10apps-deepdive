// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml.Navigation;
using static Windows.Phone.UI.Input.HardwareButtons;

namespace UwpDeepDive.MainApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdaptivePage
    {
        public AdaptivePage()
        {
            this.InitializeComponent();
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(400, 400));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var status = "phone API not found";
            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                CameraPressed += HardwareButtons_CameraPressed;
                status = "phone API found!";
            }
            _adaptiveCodeStatusTextBlock.Text = status;
        }

        private void HardwareButtons_CameraPressed(object sender, CameraEventArgs e)
        {
            _adaptiveCodeStatusTextBlock.Text = "camera button pressed!";
        }
    }
}
