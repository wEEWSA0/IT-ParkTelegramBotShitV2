using IT_ParkTelegramBotShit.DataBase;
using NLog;

namespace IT_ParkTelegramBotShit.Bot.Messages.Notifications;

public class Notification
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    public NotificationType Type { get; private set; }
    public MessageToSend Message { get; private set; }
    public DateTime Date { get; private set; }
    
    // todo добавить ConstantsStorage.SpecialValue
    
    private List<long> _recieverList = new List<long>();
    private BotMessageSender _messageSender; //private BotMessageManager.GetInstance().GetSender(ConstantsStorage.SpecialValue)
    
    public Notification(MessageToSend message, DateTime date)
    {
        Type = NotificationType.OneTime;
        Message = message;
        Date = date;
    }
    
    public Notification(MessageToSend message, DateTime date, NotificationType type) // todo вспомнить base
    {
        Type = type;
        Message = message;
        Date = date;
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
    
    public void Send()
    {
        if (_recieverList.Count == 0)
        {
            throw new NotImplementedException();
        }

        for (int i = 0; i < _recieverList.Count; i++)
        {
            // todo отправка
        }
    }
}