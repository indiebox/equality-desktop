using System;

using Notification.Wpf;

namespace Equality.Services
{
    public interface INotificationService
    {
        public void Show(NotificationContent content, TimeSpan? expirationTime = null);

        public void Show(string message, TimeSpan? expirationTime = null);

        public void ShowInfo(string message, TimeSpan? expirationTime = null);

        public void ShowWarning(string message, TimeSpan? expirationTime = null);

        public void ShowError(string message, TimeSpan? expirationTime = null);

        public void ShowSuccess(string message, TimeSpan? expirationTime = null);
    }
}
