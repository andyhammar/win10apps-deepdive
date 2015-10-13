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
                new PageVmi("Navigation (string)", typeof(LifecyclePage), "initial text (via string)"),
                new PageVmi("Navigation (object)", typeof(LifecyclePage), new LifecyclePageVm {UserText = "initial text (via object)"})
            };
        }
        public ObservableCollection<PageVmi> Pages { get; set; }
    }
}
