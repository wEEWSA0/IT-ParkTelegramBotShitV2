namespace IT_ParkTelegramBotShit.Bot.Messages.Notifications;

public class Notification
{
    public NotificationType Type { get; private set; }
    public MessageToSend Message { get; private set; }
    public DateTime Date { get; private set; }

    public Notification(MessageToSend message, DateTime date)
    {
        Type = NotificationType.OneTime;
        Message = message;
        Date = date;
    }
    
    public Notification(MessageToSend message, DateTime date, NotificationType type)
    {
        Type = type;
        Message = message;
        Date = date;
    }
}