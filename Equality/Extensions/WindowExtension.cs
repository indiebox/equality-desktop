using System.Windows;

namespace Equality.Extensions
{
    public static class WindowExtension
    {
        /// <summary>
        /// Set current window as new application MainWindow.
        /// </summary>
        /// <param name="this">Current window.</param>
        /// <param name="closePreviousMainWindow">Close previous main window.</param>
        public static void SetAsMainWindow(this Window @this, bool closePreviousMainWindow = true)
        {
            var temp = App.Current.MainWindow;

            App.Current.MainWindow = @this;

            if (closePreviousMainWindow) {
                temp.Close();
            }
        }
    }
}
