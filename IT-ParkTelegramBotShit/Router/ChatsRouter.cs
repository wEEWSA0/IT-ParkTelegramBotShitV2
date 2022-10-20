using IT_ParkTelegramBotShit.Router.Auxiliary;
using IT_ParkTelegramBotShit.Router.ChatRouters;
using IT_ParkTelegramBotShit.Service;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Router;

public class ChatsRouter // Распределяет пользователей
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private Dictionary<long, TransmittedData> _chatTransmittedDataPairs;

    private ChatRouterCallbackQuery _chatRouterCallbackQuery;
    public ChatRouterCallbackQuery RouterCallbackQuery
    {
        get => _chatRouterCallbackQuery;
    }
    
    private ChatRouterMessage _chatRouterMessage;
    public ChatRouterMessage RouterMessage
    {
        get => _chatRouterMessage;
    }

    public ChatsRouter()
    {
        _chatTransmittedDataPairs = new Dictionary<long, TransmittedData>();

        _chatRouterCallbackQuery = new ChatRouterCallbackQuery(this);
        _chatRouterMessage = new ChatRouterMessage(this);
    }

    internal TransmittedData GetUserTransmittedData(long chatId)
    {
        if (!_chatTransmittedDataPairs.ContainsKey(chatId))
        {
            _chatTransmittedDataPairs[chatId] = new TransmittedData();
        }

        return _chatTransmittedDataPairs[chatId];
    }
}