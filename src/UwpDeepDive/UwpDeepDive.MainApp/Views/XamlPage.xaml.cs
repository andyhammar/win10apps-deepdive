using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using UwpDeepDive.MainApp.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UwpDeepDive.MainApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class XamlPage
    {
        private XamlPageVm _vm;

        public XamlPage()
        {
            this.InitializeComponent();
            DataContext = _vm = new XamlPageVm();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await _vm.Init();
        }
    }
}
