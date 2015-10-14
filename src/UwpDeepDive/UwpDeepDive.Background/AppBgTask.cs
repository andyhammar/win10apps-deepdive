using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Notifications;
using NotificationsExtensions.Toasts;

namespace UwpDeepDive.Bg
{
    public sealed class AppBgTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            if (CheckBackgroundWorkCost()) return;

            if (HandleToastResponse(taskInstance)) return;

            SendToast(taskInstance.TriggerDetails?.GetType()?.Name);
        }

        private static bool CheckBackgroundWorkCost()
        {
            if (BackgroundWorkCost.CurrentBackgroundWorkCost == BackgroundWorkCostValue.High)
            {
                //being a good citizen
                return true;
            }
            return false;
        }

        private bool HandleToastResponse(IBackgroundTaskInstance taskInstance)
        {
            var toastNotificationResponse = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;
            if (toastNotificationResponse != null)
            {
                SaveToastResponse(toastNotificationResponse);
                return true;
            }
            return false;
        }

        private void SaveToastResponse(ToastNotificationActionTriggerDetail toastNotificationResponse)
        {
            var userInput = toastNotificationResponse.UserInput;
            var reply = userInput.Values.FirstOrDefault();

            var note = ApplicationData.Current.LocalSettings.Values["note"] as string;

            var newNote = $"{note}{Environment.NewLine}[toast reply]{reply}";

            ApplicationData.Current.LocalSettings.Values["note"] = newNote;
        }

        private void SendToast(string text = null)
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
                    },
                    BodyTextLine2 = new ToastText()
                    {
                        Text = text ?? ""
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

            var doc = content.GetXml();

            // Generate WinRT notification
            var toast = new ToastNotification(doc);

            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);

        }
    }
}
