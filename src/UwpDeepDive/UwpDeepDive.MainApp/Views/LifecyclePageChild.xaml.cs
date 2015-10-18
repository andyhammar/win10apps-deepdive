// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using Windows.UI.Xaml.Navigation;
using UwpDeepDive.MainApp.Helpers;
using UwpDeepDive.MainApp.ViewModels;

namespace UwpDeepDive.MainApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LifecyclePageChild
    {
        private LifecyclePageVm _vm;

        public LifecyclePageChild()
        {
            InitializeComponent();
            AppLog.Write();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            HandleObjectParameter(e);
            HandleStringParameter(e);
        }

        private void HandleObjectParameter(NavigationEventArgs navigationEventArgs)
        {
            var vm = navigationEventArgs.Parameter as LifecyclePageVm;
            if (vm == null) return;
            DataContext = _vm = vm;
        }

        private void HandleStringParameter(NavigationEventArgs navigationEventArgs)
        {
            var param = navigationEventArgs.Parameter as string;
            if (string.IsNullOrEmpty(param)) return;

            if (param == "loadnote")
            {
                DataContext = _vm = new LifecyclePageVm();
                _vm.InitUserTextFromPersistentStorage();
            }
        }
    }
}
