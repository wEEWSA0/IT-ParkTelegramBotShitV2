using IT_ParkTelegramBotShit.DataBase;
using IT_ParkTelegramBotShit.Util;
using NLog;

namespace IT_ParkTelegramBotShit.Bot.Notifications;

public class Notification
{
    protected static ILogger Logger = LogManager.GetCurrentClassLogger();

    protected List<long> _recieverList;
    protected MessageToSend _message;
    
    //public NotificationType Type { get; private set; }
    public DateTime Date { get; private set; }
    public DateTime ExpiredDate { get; private set; }

    protected Notification()
    {
        ExpiredDate = DateTime.MinValue;
        _message = MessageToSend.Empty();
        _recieverList = new List<long>();
        Date = DateTime.Now;
    }
    
    public Notification(MessageToSend message)
    {
        //Type = NotificationType.OneTime;
        ExpiredDate = DateTime.MinValue;
        _message = message;
        _recieverList = new List<long>();
        Date = DateTime.Now;
    }
    
    public Notification(MessageToSend message, DateTime date) : this(message)
    {
        Date = date;
    }
    
    public Notification(MessageToSend message, DateTime date, DateTime expiredDate) : this(message, date)
    {
        ExpiredDate = expiredDate;
    }

    // public Notification(MessageToSend message, DateTime date, NotificationType type) : this(message, date)
    // {
    //     Type = type;
    // }
    
    // public Notification(MessageToSend message, DateTime date, DateTime expiredDate, NotificationType type) : this(message, date)
    // {
    //     Type = type;
    //     ExpiredDate = expiredDate;
    // }
    //
    // public Notification(MessageToSend message, DateTime date, DateTime expiredDate, NotificationType type, List<long> recievers) : this(message, date, expiredDate, type)
    // {
    //     AddRecieverList(recievers);
    // }

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
    
    public virtual async void Send()
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
    
    /*
     * Класс, хранящий содержание сообщения-уведомления, которое будет отправлено
     * У уведомления имеется срок годности (до отправки)
     * Уведомление может быть как одноразовым, так и постоянным
     */
}