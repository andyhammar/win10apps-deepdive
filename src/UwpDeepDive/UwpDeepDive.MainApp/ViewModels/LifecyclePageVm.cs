namespace UwpDeepDive.MainApp.ViewModels
{
    public class LifecyclePageVm : VmBase
    {
        private string _userText;

        public LifecyclePageVm()
        {
            UserText = "enter text here";
        }

        public string UserText
        {
            get { return _userText; }
            set { _userText = value; OnPropertyChanged();}
        }
    }
}
