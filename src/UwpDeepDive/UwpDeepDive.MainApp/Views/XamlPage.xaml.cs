using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
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
        private readonly XamlPageVm _vm;

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

        private void button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _lazyImage.Source =
                new BitmapImage(
                    new Uri(
                        "http://www.extremetech.com/wp-content/uploads/2014/10/windows-10-logo-windows-91-640x353.jpg",
                        UriKind.Absolute));
        }
    }
}
