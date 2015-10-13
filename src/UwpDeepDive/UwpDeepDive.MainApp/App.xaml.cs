using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UwpDeepDive.MainApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private Frame _rootFrame;

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
        }
        
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
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
                    //TODO: Load state from previously suspended application
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
        }

        private void CheckActivationKind(ActivationKind kind)
        {
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
            if (frame == null) return;
            if (frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
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
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            SaveNavigationState();
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
    }
}
