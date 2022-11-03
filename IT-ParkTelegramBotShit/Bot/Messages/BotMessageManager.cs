using Telegram.Bot;

namespace IT_ParkTelegramBotShit.Bot;

public class BotMessageManager
{
    private static BotMessageManager _messageManager = null;
    private Dictionary<long, BotChatIdMessageManager> _messageWithChatId;
    
    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;
    
    private BotMessageManager(TelegramBotClient client, CancellationTokenSource token)
    {
        _botClient = client;
        _cancellationTokenSource = token;

        _messageWithChatId = new Dictionary<long, BotChatIdMessageManager>();
    }
    
    public static BotMessageManager GetInstance()
    {
        if (_messageManager == null)
        {
            throw new NotImplementedException();
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

    public BotChatIdMessageManager GetChatIdMessageManager(long chatId)
    {
        if (!_messageWithChatId.ContainsKey(chatId))
        {
            _messageWithChatId[chatId] = new BotChatIdMessageManager(_botClient, _cancellationTokenSource, chatId);
        }
        
        return _messageWithChatId[chatId];
    }
}