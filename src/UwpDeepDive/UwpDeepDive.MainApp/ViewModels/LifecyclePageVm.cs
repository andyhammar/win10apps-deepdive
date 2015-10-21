using Windows.Storage;

namespace UwpDeepDive.MainApp.ViewModels
{
    public class LifecyclePageVm : VmBase
    {
        private string _userText;

        public LifecyclePageVm()
        {
            _userText = "placeholder text";
        }

        public void InitUserTextFromPersistentStorage()
        {
            var userText = ApplicationData.Current.LocalSettings.Values["note"] as string;
            if (string.IsNullOrEmpty(userText))
                return;
            UserText = userText;
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
