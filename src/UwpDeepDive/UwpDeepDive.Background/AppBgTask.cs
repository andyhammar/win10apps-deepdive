using System;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using NotificationsExtensions.Toasts;

namespace UwpDeepDive.Bg
{
    public sealed class AppBgTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var toastNotificationResponse = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;
            if (toastNotificationResponse != null)
            {
                HandleToastNotificationResponse(toastNotificationResponse);
                return;
            }
            SendToast();
            //var toastNotifier = ToastNotificationManager.CreateToastNotifier();
            //var templateContent = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);

            //toastNotifier.Show();
        }

        private void HandleToastNotificationResponse(ToastNotificationActionTriggerDetail toastNotificationResponse)
        {
            var userInput = toastNotificationResponse.UserInput;
            var reply = userInput.Values.FirstOrDefault();

            var note = ApplicationData.Current.LocalSettings.Values["note"] as string;

            var newNote = $"{note}{Environment.NewLine}[toast reply]{reply}";

            ApplicationData.Current.LocalSettings.Values["note"] = newNote;
        }

        private void SendToast()
        {
            ToastContent content = new ToastContent()
            {
                Launch = "toastResponse",

                Visual = new ToastVisual()
                {
                    TitleText = new ToastText
                    {
                        Text = "UwpDeepDive notification"
                    },

                    BodyTextLine1 = new ToastText
                    {
                        Text = "Got a note to add?"
                    }
                },

                Actions = new ToastActionsCustom()
                {
                    Inputs =
                    {
                        new ToastTextBox("tbReply")
                        {
                            PlaceholderContent = "add note here"
                        }
                    },

                    Buttons =
                    {
                        new ToastButton("save", "save")
                        {
                            ActivationType = ToastActivationType.Background
                        },
                        new ToastButton("launch", "launch")
                        {
                            ActivationType = ToastActivationType.Foreground
                        }
                    }
                },

                Audio = new ToastAudio()
                {
                    Src = new Uri("ms-winsoundevent:Notification.IM")
                }
            };

            XmlDocument doc = content.GetXml();

            // Generate WinRT notification
            var toast = new ToastNotification(doc);

            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);

        }


    }
}
