using System.Collections;
using System.Collections.ObjectModel;
using UwpDeepDive.MainApp.Views;

namespace UwpDeepDive.MainApp.ViewModels
{
    public class MainPageVm
    {
        public MainPageVm()
        {
            Pages = new ObservableCollection<PageVmi>
            {
                new PageVmi("Adaptive", typeof(AdaptivePage)),
                new PageVmi("XAML", typeof(XamlPage)),
                new PageVmi("Navigation & Lifecycle", typeof(LifecyclePage)),
                new PageVmi("Background", typeof(BackgroundPage)),
            };
        }
        public ObservableCollection<PageVmi> Pages { get; set; }
    }
}
