using IT_ParkTelegramBotShit.Bot.Messages;
using IT_ParkTelegramBotShit.Bot.Messages.Notifications;
using NLog;

public class BotNotificationSystem
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private List<Notification> _notifications;
    private BotMessageSender _sender;
    
    private static BotNotificationSystem _notificationSystem;
    
    private BotNotificationSystem()
    {
        
    }
    
    public static BotNotificationSystem GetInstance()
    {
        if (_notificationSystem == null)
        {
            _notificationSystem = new BotNotificationSystem();
        }
        
        return _notificationSystem;
    }
    
    // todo BotNotificationSystem class

    public void AddNotification(Notification notification)
    {
        // возможна проверка на уникальность с Warn
        
        _notifications.Add(notification);
    }
    
    // CheckNotifications()
}