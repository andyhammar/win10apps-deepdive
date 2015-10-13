using System;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace UwpDeepDive.Background
{
    public sealed class AppBgTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var toastNotifier = ToastNotificationManager.CreateToastNotifier();
            var templateContent = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            
            toastNotifier.Show();
        }
    }
}
