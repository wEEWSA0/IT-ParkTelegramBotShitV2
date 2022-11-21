using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Bot.Messages;

public class BotNotificationSender
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private static BotNotificationSender _notificationSender;
    
    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;

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

            return true;
        }

        return false;
    }

    public Message SendNotificationMessage(MessageToSend message, long chatId)
    { 
        Task<Message> task = _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: message.Text,
            replyMarkup: message.InlineKeyboardMarkup,
            cancellationToken: _cancellationTokenSource.Token);

        return task.Result;
    }
}