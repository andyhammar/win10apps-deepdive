// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using UwpDeepDive.MainApp.Helpers;

namespace UwpDeepDive.MainApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LifecyclePageChild
    {
        public LifecyclePageChild()
        {
            InitializeComponent();
            AppLog.Write();
        }
    }
}
