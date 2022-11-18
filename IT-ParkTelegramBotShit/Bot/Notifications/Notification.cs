namespace IT_ParkTelegramBotShit.Bot.Messages.Notifications;

public class Notification
{
    public MessageToSend Message { get; private set; }
    public DateTime Date { get; private set; }

    public Notification(MessageToSend message, DateTime date)
    {
        Message = message;
        Date = date;
    }
}