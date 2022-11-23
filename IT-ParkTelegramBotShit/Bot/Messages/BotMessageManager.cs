using NLog;
using Telegram.Bot;

namespace IT_ParkTelegramBotShit.Bot;

public class BotMessageManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private static BotMessageManager _messageManager = null;
    private Dictionary<long, BotMessageSender> _messageSender;
    private Dictionary<long, BotMessageHistory> _messageHistorie;
    
    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;
    
    private BotMessageManager(TelegramBotClient client, CancellationTokenSource token)
    {
        _botClient = client;
        _cancellationTokenSource = token;

        _messageSender = new Dictionary<long, BotMessageSender>();
        _messageHistorie = new Dictionary<long, BotMessageHistory>();
    }
    
    public static BotMessageManager GetInstance()
    {
        if (_messageManager == null)
        {
            Logger.Error("BotMessageManager not initialized");
            throw new Exception();
        }
        
        return _messageManager;
    }

    public static bool Create(TelegramBotClient client, CancellationTokenSource token)
    {
        if (_messageManager == null)
        {
            _messageManager = new BotMessageManager(client, token);

            return true;
        }

        return false;
    }

    public BotMessageSender GetSender(long chatId)
    {
        if (!_messageSender.ContainsKey(chatId))
        {
            _messageSender[chatId] = new BotMessageSender(_botClient, _cancellationTokenSource, chatId);
        }
        
        return _messageSender[chatId];
    }
    
    public BotMessageHistory GetHistory(long chatId)
    {
        if (!_messageHistorie.ContainsKey(chatId))
        {
            _messageHistorie[chatId] = new BotMessageHistory(_botClient, _cancellationTokenSource, chatId);
        }
        
        return _messageHistorie[chatId];
    }
}