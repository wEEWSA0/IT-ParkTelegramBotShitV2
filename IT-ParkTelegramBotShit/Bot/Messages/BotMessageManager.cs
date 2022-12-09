using NLog;
using Telegram.Bot;

namespace IT_ParkTelegramBotShit.Bot;

public class BotMessageManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private static BotMessageManager _messageManager = null;
    private Dictionary<long, BotMessageSender> _messageSender;
    private Dictionary<long, BotMessageHistory> _messageHistorie;

    private BotResponder _botResponder;
    
    private BotMessageManager(BotResponder botResponder)
    {
        _botResponder = botResponder;

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

    public static bool Create(BotResponder botResponder)
    {
        if (_messageManager == null)
        {
            _messageManager = new BotMessageManager(botResponder);
            Logger.Debug("BotMessageManager is initialized");
            return true;
        }

        return false;
    }

    public BotMessageSender GetSender(long chatId)
    {
        if (!_messageSender.ContainsKey(chatId))
        {
            _messageSender[chatId] = new BotMessageSender(_botResponder, chatId);
        }
        
        return _messageSender[chatId];
    }
    
    public BotMessageHistory GetHistory(long chatId)
    {
        if (!_messageHistorie.ContainsKey(chatId))
        {
            _messageHistorie[chatId] = new BotMessageHistory(_botResponder, chatId);
        }
        
        return _messageHistorie[chatId];
    }

    public List<long> GetAllHistoryKeys()
    {
        return new List<long>(_messageHistorie.Keys.ToList());
    }
    
    /*
     * Класс для доступа к истории сообщений и возможности отправлять сообщения лично для каждого chatId
     */
}