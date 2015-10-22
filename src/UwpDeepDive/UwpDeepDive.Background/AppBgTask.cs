using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;
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

            if (HandleAppServiceCall(taskInstance)) return;

            SendToast(taskInstance.TriggerDetails?.GetType()?.Name);
        }

        private bool HandleAppServiceCall(IBackgroundTaskInstance taskInstance)
        {
            var appServiceTriggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            if (appServiceTriggerDetails == null) return false;

            if (appServiceTriggerDetails.Name == "uwpdeepdive-appservice")
            {
                appServiceTriggerDetails.AppServiceConnection.RequestReceived += AppServiceConnection_RequestReceived;
            }
            return true;
        }

        private async void AppServiceConnection_RequestReceived(
            AppServiceConnection sender,
            AppServiceRequestReceivedEventArgs args)
        {
            var appServiceDeferral = args.GetDeferral();

            var msg = args.Request.Message;
            var cmd = msg["cmd"] as string;
            if (cmd == null) return;

            var response = string.Empty;
            if (cmd == "time")
            {
                response = DateTime.Now.ToString("T");
            }
            else if (cmd == "note")
            {
                response = ApplicationData.Current.LocalSettings.Values["note"] as string;
            }
            var result = new ValueSet { { cmd, response } };
            await args.Request.SendResponseAsync(result);

            appServiceDeferral.Complete();
        }

        private static bool CheckBackgroundWorkCost()
        {
            if (BackgroundWorkCost.CurrentBackgroundWorkCost 
                == BackgroundWorkCostValue.High)
            {
                //being a good citizen
                return true;
            }
            return false;
        }

        private bool HandleToastResponse(IBackgroundTaskInstance taskInstance)
        {
            var toastNotificationResponse = taskInstance.TriggerDetails
                as ToastNotificationActionTriggerDetail;
            if (toastNotificationResponse == null) return false;

            SaveToastResponse(toastNotificationResponse);
            return true;
        }

        private void SaveToastResponse(ToastNotificationActionTriggerDetail toastNotificationResponse)
        {
            var userInput = toastNotificationResponse.UserInput;
            var reply = userInput.Values.FirstOrDefault();

            var note = ApplicationData.Current.LocalSettings.Values["note"] as string;

            var newNote = $"{note}{Environment.NewLine}[toast reply] {reply}";

            ApplicationData.Current.LocalSettings.Values["note"] = newNote;
        }

        private void SendToast(string text = null)
        {
            //This is using NotificationsExtensions
            var content = new ToastContent()
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

                Audio = new ToastAudio
                {
                    Src = new Uri("ms-winsoundevent:Notification.IM")
                }
            };

            var doc = content.GetXml();

            // Generate WinRT notification
            var toast = new ToastNotification(doc);

            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);

            //Debug.WriteLine(content.GetContent());
            /*
            <?xml version="1.0" encoding="UTF-8"?>
                <toast launch="toastResponse">
                    <visual>
                        <binding template="ToastGeneric">
                            <text>UwpDeepDive notification</text>
                            <text>Got a note to add?</text>
                            <text>ApplicationTriggerDetails</text>
                        </binding>
                    </visual>
                    <audio src="ms-winsoundevent:Notification.IM" />
                    <actions>
                        <input id="tbReply" type="text" placeHolderContent="add note here" />
                        <action content="save" arguments="save" activationType="background" />
                        <action content="launch" arguments="launch" />
                    </actions>
                </toast>
            </xml>
            */
        }
    }
}
