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

            AppLog.Write();

            DataContext = _vm = new LifecyclePageVm();
            _vm.InitUserTextFromPersistentStorage();

            NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AppLog.Write();
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
                NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        private void nextButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (LifecyclePageChild), ((FrameworkElement)sender).Tag);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (PageOfNoReturn));
        }
    }
}
