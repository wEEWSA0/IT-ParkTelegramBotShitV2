using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Bot.Notifications;
using NLog;
using NLog.Fluent;

public class BotNotificationSystem
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    private static BotNotificationSystem _notificationSystem;
    
    private List<Notification> _notifications;

    private Task _checkingNotifications;
    private bool _isStarted;

    private BotNotificationSystem()
    {
        _isStarted = false;
        _notifications = new List<Notification>();
    }
    
    public static BotNotificationSystem GetInstance()
    {
        if (_notificationSystem == null)
        {
            _notificationSystem = new BotNotificationSystem();
            Logger.Debug("BotNotificationSystem is initialized");
        }
        
        return _notificationSystem;
    }
    
    public void AddNotification(Notification notification)
    {
        if (_notifications.Contains(notification))
        {
            Logger.Warn("Notification already added");
        }
        
        _notifications.Add(notification);
    }

    public void StartNotificationSystem(int checkRateInMiliseconds)
    {
        if (_isStarted)
        {
            Logger.Warn("CheckingNotifications already started");
            return;
        }

        _isStarted = true;

        if (_checkingNotifications != null)
        {
            if (_checkingNotifications.Status != TaskStatus.RanToCompletion)
            {
                Logger.Error("NotificationSystem has some undeifined errors");
                return;
            }
        }

        _checkingNotifications = CheckingNotifications(checkRateInMiliseconds);
        
        _checkingNotifications.Start();
    }

    public void StopNotificationSystem()
    {
        if (!_isStarted)
        {
            Logger.Warn("CheckingNotifications already stopped or not started");
            return;
        }

        _isStarted = false;
    }

    private void SendExpiredNotifications()
    {
        for (int i = 0; i < _notifications.Count; i++)
        {
            var notification = _notifications[i];

            if (IsNotificationExpired(notification))
            {
                _notifications.Remove(notification);
            }
            
            if (IsNotificationReadyToSend(notification))
            {
                notification.Send();

                if (notification.Type == NotificationType.OneTime)
                {
                    _notifications.Remove(notification);
                }
            }
        }
    }

    private Task CheckingNotifications(int checkRateInMiliseconds)
    {
        return new Task(() =>
        {
            Logger.Debug("CheckingNotifications started");
            
            while (_isStarted)
            {
                SendExpiredNotifications();
                Thread.Sleep(checkRateInMiliseconds);
            }

            Logger.Debug("CheckingNotifications finished");
        });
    }

    private bool IsNotificationExpired(Notification notification)
    {
        if (notification.Type == NotificationType.Regular || notification.Type == NotificationType.OneTime)
        {
            return false;
        }
        
        return notification.ExpiredDate <= DateTime.Now;
    }
    
    private bool IsNotificationReadyToSend(Notification notification)
    {
        return notification.Date <= DateTime.Now;
    }
}