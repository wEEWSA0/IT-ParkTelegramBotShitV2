namespace IT_ParkTelegramBotShit.Bot.Notifications.Types;

public interface INotificationType
{
    //bool IsWorthSendNotification(); // проверка времени и срока годности, стоит ли 
    bool IsTimeForSendNotification(DateTime currentDateTime);
    bool IsNotificationExpired(DateTime currentDateTime);
}