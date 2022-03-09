using System;

using Notification.Wpf;
using Notification.Wpf.Constants;

namespace Equality.Services
{
    public class NotificationService : INotificationService
    {
        public const string NotificationContainerKey = "NotificationsContainer";

        protected NotificationManager NotificationManager;

        public NotificationService()
        {
            NotificationManager = new NotificationManager();

            NotificationConstants.TitleSize = 16;
            NotificationConstants.MessageSize = 16;
        }

        public void Show(NotificationContent content, TimeSpan? expirationTime = null)
        {
            NotificationManager.Show(content, NotificationContainerKey, expirationTime);
        }

        public void Show(string message, TimeSpan? expirationTime = null)
        {
            NotificationManager.Show(message, NotificationType.None, NotificationContainerKey, expirationTime);
        }

        public void ShowInfo(string message, TimeSpan? expirationTime = null)
        {
            NotificationManager.Show(message, NotificationType.Information, NotificationContainerKey, expirationTime);
        }

        public void ShowWarning(string message, TimeSpan? expirationTime = null)
        {
            NotificationManager.Show(message, NotificationType.Warning, NotificationContainerKey, expirationTime ?? TimeSpan.FromSeconds(7));
        }

        public void ShowError(string message, TimeSpan? expirationTime = null)
        {
            NotificationManager.Show(message, NotificationType.Error, NotificationContainerKey, expirationTime ?? TimeSpan.MaxValue);
        }

        public void ShowSuccess(string message, TimeSpan? expirationTime = null)
        {
            NotificationManager.Show(message, NotificationType.Success, NotificationContainerKey, expirationTime);
        }
    }
}
