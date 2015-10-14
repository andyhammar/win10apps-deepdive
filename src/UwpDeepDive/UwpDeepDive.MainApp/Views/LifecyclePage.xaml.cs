// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using UwpDeepDive.MainApp.Helpers;
using UwpDeepDive.MainApp.ViewModels;

namespace UwpDeepDive.MainApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LifecyclePage
    {
        private LifecyclePageVm _vm;

        public LifecyclePage()
        {
            InitializeComponent();
            DataContext = _vm = new LifecyclePageVm();
            AppLog.Write();

            NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AppLog.Write();
            base.OnNavigatedTo(e);

            _vm.InitUserText();

            if (e.NavigationMode != NavigationMode.Back)
            {
                //handle re-entry with unwanted parameter?
                HandleObjectParameter(e);
                HandleStringParameter(e);
            }
        }

        private void HandleObjectParameter(NavigationEventArgs navigationEventArgs)
        {
            var vm = navigationEventArgs.Parameter as LifecyclePageVm;
            if (vm == null) return;
            DataContext = _vm = vm;
        }

        private void HandleStringParameter(NavigationEventArgs navigationEventArgs)
        {
            var text = navigationEventArgs.Parameter as string;
            if (text == null) return;

            if (string.IsNullOrEmpty(_vm.UserText))
            {
                _vm.UserText = text;
            }
        }

        private void nextButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (LifecyclePageChild));
        }

        private async void TriggerButton_OnClick(object sender, RoutedEventArgs e)
        {
            await App.TriggerBackgroundTask();
        }
    }
}
