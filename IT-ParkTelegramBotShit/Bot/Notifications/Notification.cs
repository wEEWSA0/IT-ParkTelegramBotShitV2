using IT_ParkTelegramBotShit.DataBase;
using IT_ParkTelegramBotShit.Util;
using NLog;

namespace IT_ParkTelegramBotShit.Bot.Notifications;

public class Notification
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private List<long> _recieverList = new List<long>();
    
    private MessageToSend _message;
    public NotificationType Type { get; private set; }
    public DateTime Date { get; private set; }

    public Notification(MessageToSend message)
    {
        Type = NotificationType.OneTime;
        _message = message;
        Date = DateTime.Now;
    }
    
    public Notification(MessageToSend message, DateTime date) : this(message)
    {
        Date = date;
    }

    public Notification(MessageToSend message, DateTime date, NotificationType type) : this(message, date)
    {
        Type = type;
    }

    public void AddReciever(long chatId)
    {
        if (_recieverList.Contains(chatId))
        {
            Logger.Warn($"Notification already contains user with chat id: {chatId}");
            
            return;
        }
        
        _recieverList.Add(chatId);
    }
    
    public void AddRecieverList(List<long> recieverList)
    {
        for (int i = 0; i < recieverList.Count; i++)
        {
            AddReciever(recieverList[i]);
        }
    }
    
    public async void Send()
    {
        if (_recieverList.Count == 0)
        {
            Logger.Error("Нельзя отправить уведомление 0 пользователям");
            throw new Exception();
        }
        
        var notificationSender = BotNotificationSender.GetInstance();
        
        for (int i = 0; i < _recieverList.Count; i++)
        {
            await notificationSender.SendNotificationMessage(_message, _recieverList[i]);
        }
    }
}