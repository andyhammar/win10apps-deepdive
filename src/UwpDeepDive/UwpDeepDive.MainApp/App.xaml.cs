using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.ExtendedExecution;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using NotificationsExtensions.Toasts;
using UwpDeepDive.MainApp.Helpers;
using UwpDeepDive.MainApp.Views;

namespace UwpDeepDive.MainApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private Frame _rootFrame;
        private static ApplicationTrigger _bgTaskTrigger;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            AppLog.Write();
        }

        public static bool AskForExtendedExecutionOnNextSuspend { get; set; }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            AppLog.Write();

            CheckActivationKind(e.Kind);
            CheckPreviousExecutionState(e.PreviousExecutionState);
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            _rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (_rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                _rootFrame = new Frame();

                _rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //Recover - if it makes sense!
                    RecoverNavigationState(_rootFrame);
                }

                // Place the frame in the current Window
                Window.Current.Content = _rootFrame;
            }

            if (_rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                _rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();

            var systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested += App_BackRequested;

            await RefreshBackgroundTasks();
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            AppLog.Write($"kind: {args.Kind}");

            await HandleToastActivation(args);

            if (HandleProtocolForResultsActivation(args))
                return;

            if (HandleProtocolActivation(args))
                return;
        }

        private static async Task HandleToastActivation(IActivatedEventArgs args)
        {
            var e = args as ToastNotificationActivatedEventArgs;
            if (e == null) return;

            var reply = e?.UserInput.Values.FirstOrDefault() as string;
            AppLog.Write($"arg: {e?.Argument}, reply: {reply}");


            var contentDialog = new ContentDialog()
            {
                Content =
                    new TextBlock { Text = reply ?? "", FontSize = 36, Foreground = new SolidColorBrush(Colors.Coral) },
                PrimaryButtonText = "close",
                IsPrimaryButtonEnabled = true
            };

            await contentDialog.ShowAsync();
        }

        private bool HandleProtocolForResultsActivation(IActivatedEventArgs args)
        {
            var e = args as ProtocolForResultsActivatedEventArgs;
            if (e == null) return false;

            //check against whitelist?
            AppLog.Write($"caller: {e.CallerPackageFamilyName}");

            //var dialog = new MessageDialog($"Will now share your note with the app: {e.CallerPackageFamilyName}?");
            //await dialog.ShowAsync();

            //Current.Exit();
            var page = new ProtocolForResultsPage();
            page.InitProtocolResponse(e);
            Window.Current.Content = page;
            Window.Current.Activate();
            return true;
        }

        private bool HandleProtocolActivation(IActivatedEventArgs args)
        {
            var e = args as ProtocolActivatedEventArgs;
            if (e == null) return false;

            var uiElement = new TextBlock { Text = "protocol activated", FontSize = 42 };
            Window.Current.Content = uiElement;
            Window.Current.Activate();
            return true;
        }

        private async Task RefreshBackgroundTasks()
        {
            foreach (var registration in BackgroundTaskRegistration.AllTasks.Values)
            {
                registration.Unregister(false);
            }
            await RegisterTasks();
        }

        private async Task RegisterTasks()
        {
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            if (status != BackgroundAccessStatus.Denied)
            {
                RegisterTask("timerTask", new TimeTrigger(240, false));
            }
            RegisterTask("toastNotificationActionTask", new ToastNotificationActionTrigger());

            RegisterTask("appTriggerTask", _bgTaskTrigger = new ApplicationTrigger());
        }

        private void RegisterTask(string name, IBackgroundTrigger trigger)
        {
            var taskName = name;

            var builder = new BackgroundTaskBuilder
            {
                Name = taskName,
                TaskEntryPoint = "UwpDeepDive.Bg.AppBgTask"
            };

            builder.SetTrigger(trigger);

            builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
            builder.AddCondition(new SystemCondition(SystemConditionType.UserPresent));

            var task = builder.Register();
            task.Completed += Task_Completed;
        }

        private void Task_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            //task completed (when alive, or during suspend/terminated)
            AppLog.Write($"sender: {sender.Name}");
        }

        public static async Task TriggerBackgroundTask()
        {
            await _bgTaskTrigger.RequestAsync();
        }

        private void CheckActivationKind(ActivationKind kind)
        {
            AppLog.Write(kind.ToString());
            switch (kind)
            {
                case ActivationKind.Launch:
                    break;
                case ActivationKind.Search:
                    break;
                case ActivationKind.ShareTarget:
                    break;
                case ActivationKind.File:
                    break;
                case ActivationKind.Protocol:
                    break;
                case ActivationKind.FileOpenPicker:
                    break;
                case ActivationKind.FileSavePicker:
                    break;
                case ActivationKind.CachedFileUpdater:
                    break;
                case ActivationKind.ContactPicker:
                    break;
                case ActivationKind.Device:
                    break;
                case ActivationKind.PrintTaskSettings:
                    break;
                case ActivationKind.CameraSettings:
                    break;
                case ActivationKind.RestrictedLaunch:
                    break;
                case ActivationKind.AppointmentsProvider:
                    break;
                case ActivationKind.Contact:
                    break;
                case ActivationKind.LockScreenCall:
                    break;
                case ActivationKind.VoiceCommand:
                    break;
                case ActivationKind.LockScreen:
                    break;
                case ActivationKind.PickerReturned:
                    break;
                case ActivationKind.WalletAction:
                    break;
                case ActivationKind.PickFileContinuation:
                    break;
                case ActivationKind.PickSaveFileContinuation:
                    break;
                case ActivationKind.PickFolderContinuation:
                    break;
                case ActivationKind.WebAuthenticationBrokerContinuation:
                    break;
                case ActivationKind.WebAccountProvider:
                    break;
                case ActivationKind.ComponentUI:
                    break;
                case ActivationKind.ProtocolForResults:
                    break;
                case ActivationKind.ToastNotification:
                    break;
                case ActivationKind.DialReceiver:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private void CheckPreviousExecutionState(ApplicationExecutionState previousExecutionState)
        {
            AppLog.Write(previousExecutionState.ToString());
            switch (previousExecutionState)
            {
                case ApplicationExecutionState.NotRunning:
                    break;
                case ApplicationExecutionState.Running:
                    break;
                case ApplicationExecutionState.Suspended:
                    break;
                case ApplicationExecutionState.Terminated:
                    //try to recover state, if it makes sense (time since last run?)
                    break;
                case ApplicationExecutionState.ClosedByUser:
                    //probably revert to clean state
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(previousExecutionState), previousExecutionState, null);
            }
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            if (frame == null || !frame.CanGoBack) return;

            e.Handled = true;
            frame.GoBack();
        }


        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            AppLog.Write();

            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            SaveNavigationState();

            await CheckExtendedExecution();

            deferral.Complete();
        }

        private void SaveNavigationState()
        {
            var naviState = _rootFrame.GetNavigationState();
            ApplicationData.Current.LocalSettings.Values["naviState"] = naviState;
        }

        private void RecoverNavigationState(Frame rootFrame)
        {
            var naviState = ApplicationData.Current.LocalSettings.Values["naviState"] as string;
            if (naviState == null) return;

            rootFrame.SetNavigationState(naviState);
        }

        private async Task CheckExtendedExecution()
        {
            if (AskForExtendedExecutionOnNextSuspend)
            {
                AskForExtendedExecutionOnNextSuspend = false;
                using (var session = new ExtendedExecutionSession() { Reason = ExtendedExecutionReason.SavingData })
                {
                    session.Revoked += (s, args) => { AppLog.Write("extended execution revoked, reason: " + args.Reason); };
                    session.Description = "toasting things up";
                    var extendedExecutionResult = await session.RequestExtensionAsync();
                    if (extendedExecutionResult == ExtendedExecutionResult.Allowed)
                    {
                        while (true)
                        {
                            await Task.Delay(TimeSpan.FromSeconds(10));
                            ShowStillAliveToast();
                        }
                    }
                }
            }
        }

        private void ShowStillAliveToast()
        {
            ToastContent content = new ToastContent()
            {
                Launch = "toastResponse",

                Visual = new ToastVisual()
                {
                    TitleText = new ToastText
                    {
                        Text = "Still alive!"
                    },

                    BodyTextLine1 = new ToastText
                    {
                        Text = DateTime.Now.ToString("G")
                    },
                },

                Actions = new ToastActionsSnoozeAndDismiss()
            };

            var doc = content.GetXml();

            // Generate WinRT notification
            var toast = new ToastNotification(doc);

            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);

        }
    }
}
