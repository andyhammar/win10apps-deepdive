using Windows.UI.Xaml.Controls;
using UwpDeepDive.MainApp.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UwpDeepDive.MainApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new MainPageVm();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var page = e.ClickedItem as PageVmi;
            if (page == null) return;

            Frame.Navigate(page.Type, page.Param);
        }
    }
}
