using System.Diagnostics.CodeAnalysis;
using Infra.Notification.Abstractions;

namespace Infra.Notification.Contexts
{
    [ExcludeFromCodeCoverage]
    public class NotificationContext : Notifiable
    {
    }

    [ExcludeFromCodeCoverage]
    public class NotificationContext<TNotificationMessage> : Notifiable<TNotificationMessage>
        where TNotificationMessage : class
    {
    }
}