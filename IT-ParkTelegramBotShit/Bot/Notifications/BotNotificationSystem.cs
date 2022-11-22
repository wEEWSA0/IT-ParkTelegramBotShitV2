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
        // Объявление системы ТЕСТИРОВАНИЕ

        var message = new MessageToSend("Тестирование");
        
        DateTime date = DateTime.Now;
        
        AddNotification(new Notification(message, date));
        
        CheckNotifications(); // todo Найти, куда встроить проверку уведомлений
    }
    
    public static BotNotificationSystem GetInstance()
    {
        if (_notificationSystem == null)
        {
            _notificationSystem = new BotNotificationSystem();
        }
        
        return _notificationSystem;
    }
    
    public void AddNotification(Notification notification)
    {
        // возможна проверка на уникальность с Warn
        
        _notifications.Add(notification);
    }
    
    public void CheckNotifications()
    {
        for (int i = 0; i < _notifications.Count; i++)
        {
            var notification = _notifications[i];

            if (notification.Date <= DateTime.Now)
            {
                notification.Send();
            }
        }
    }
}