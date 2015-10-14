using Windows.Storage;

namespace UwpDeepDive.MainApp.ViewModels
{
    public class LifecyclePageVm : VmBase
    {
        private string _userText;

        public LifecyclePageVm()
        {
            _userText = "initial user text";
        }

        public void InitUserText()
        {
            UserText = ApplicationData.Current.LocalSettings.Values["note"] as string;
        }

        public string UserText
        {
            get { return _userText; }
            set
            {
                _userText = value;
                OnPropertyChanged();
                ApplicationData.Current.LocalSettings.Values["note"] = value;
            }
        }
    }
}
