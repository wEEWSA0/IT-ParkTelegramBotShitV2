using IT_ParkTelegramBotShit.DataBase;
using IT_ParkTelegramBotShit.Util;
using NLog;

namespace IT_ParkTelegramBotShit.Bot.Messages.Notifications;

public class Notification
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    public NotificationType Type { get; private set; }
    public MessageToSend Message { get; private set; }
    public DateTime Date { get; private set; }

    private List<long> _recieverList = new List<long>();
    private BotNotificationSender _notificationSender;

    public Notification(MessageToSend message, DateTime date)
    {
        Type = NotificationType.OneTime;
        Message = message;
        Date = date;
        
        _notificationSender = BotNotificationSender.GetInstance();
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
    
    public void Send()
    {
        if (_recieverList.Count == 0)
        {
            Logger.Error("Нельзя отправить уведомление 0 пользователям");
            throw new Exception();
        }
        
        // todo жестокие тесты на 1000-и запросов (почти 100% шанс на поломку)
        
        for (int i = 0; i < _recieverList.Count; i++)
        {
            _notificationSender.SendNotificationMessage(Message, _recieverList[i]);
        }
    }
}