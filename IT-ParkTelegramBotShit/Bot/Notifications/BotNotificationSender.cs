using IT_ParkTelegramBotShit.Util;
using NLog;
using NLog.Fluent;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Bot;

public class BotNotificationSender
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private static BotNotificationSender _notificationSender;
    
    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;
    // todo regualr notification type is useless
    private BotNotificationSender(TelegramBotClient client, CancellationTokenSource token)
    {
        _botClient = client;
        _cancellationTokenSource = token;
    }

    public static BotNotificationSender GetInstance()
    {
        if (_notificationSender == null)
        {
            Logger.Error("BotNotificationSender not initialized");
            throw new Exception();
        }
        
        return _notificationSender;
    }

    public static bool Create(TelegramBotClient client, CancellationTokenSource token)
    {
        if (_notificationSender == null)
        {
            _notificationSender = new BotNotificationSender(client, token);
            Logger.Debug("BotNotificationSender is initialized");
            return true;
        }

        return false;
    }
    
    public async Task<Message> SendNotificationMessage(MessageToSend messageToSend, long chatId)
    {
        Task<Message> task = SendMessage(messageToSend, chatId);
        
        Message message = task.Result;
        
        BotMessageManager.GetInstance().GetHistory(chatId).AddMessageId(message.MessageId);
        
        var sleepValue = BotStatisticManager.GetInstance().SleepValue;
        await Task.Run(() => Thread.Sleep(sleepValue));
        
        return task.Result;
    }
    
    public async Task<Message> SendAnchoredNotificationMessage(MessageToSend messageToSend, long chatId)
    {
        Task<Message> task = SendMessage(messageToSend, chatId);
        
        Message message = task.Result;
        
        BotMessageManager.GetInstance().GetHistory(chatId).AddAnchoredMessagesId(message.MessageId);

        var sleepValue = BotStatisticManager.GetInstance().SleepValue;
        await Task.Run(() => Thread.Sleep(sleepValue));
        
        return task.Result;
    }
    
    private Task<Message> SendMessage(MessageToSend message, long chatId)
    {
        BotStatisticManager.GetInstance().AddWorkLoad();
        
        return _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: message.Text,
            replyMarkup: message.InlineKeyboardMarkup,
            cancellationToken: _cancellationTokenSource.Token);
    }
}