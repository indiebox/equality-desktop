using System;
using System.Threading;
using System.Windows;

namespace Equality.Data
{
    /// <see href="https://github.com/it3xl/WPF-app-Single-Instance-in-one-line-of-code/blob/master/WpfSingleInstanceByEventWaitHandle/WpfSingleInstance.cs"/>
    internal class SingleInstance
    {
        private static bool AlreadyProcessedOnThisInstance;

        private const string APP_NAME = "Equality";

        internal static void Make(bool uniquePerUser = true)
        {
            if (AlreadyProcessedOnThisInstance) {
                return;
            }

            AlreadyProcessedOnThisInstance = true;

            Application app = Application.Current;
            bool isSecondaryInstance = true;
            string eventName = uniquePerUser
                ? $"{APP_NAME}-{Environment.MachineName}-{Environment.UserDomainName}-{Environment.UserName}"
                : $"{APP_NAME}-{Environment.MachineName}";

            EventWaitHandle eventWaitHandle = null;
            try {
                eventWaitHandle = EventWaitHandle.OpenExisting(eventName);
            } catch {
                // This code only runs on the first instance.
                isSecondaryInstance = false;
            }

            if (isSecondaryInstance) {
                ActivateFirstInstanceWindow(eventWaitHandle);

                // Let's produce a non-interceptable exit.
                Environment.Exit(0);
            }

            RegisterFirstInstanceWindowActivation(app, eventName);
        }

        private static void ActivateFirstInstanceWindow(EventWaitHandle eventWaitHandle)
        {
            // Let's notify the first instance to activate its main window.
            _ = eventWaitHandle.Set();
        }

        private static void RegisterFirstInstanceWindowActivation(Application app, string eventName)
        {
            var eventWaitHandle = new EventWaitHandle(
                false,
                EventResetMode.AutoReset,
                eventName
            );

            _ = ThreadPool.RegisterWaitForSingleObject(eventWaitHandle, WaitOrTimerCallback, app, Timeout.Infinite, false);

            eventWaitHandle.Close();
        }

        private static void WaitOrTimerCallback(object state, bool timedOut)
        {
            var app = (Application)state;
            app.Dispatcher.BeginInvoke(new Action(() =>
            {
                Application.Current.MainWindow.SetCurrentValue(Window.WindowStateProperty, WindowState.Normal);
                Application.Current.MainWindow.Activate();
            }));
        }
    }
}
