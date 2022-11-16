using IT_ParkTelegramBotShit.Bot.Messages;
using IT_ParkTelegramBotShit.Router.Transmitted;
using IT_ParkTelegramBotShit.Service;
using IT_ParkTelegramBotShit.Service.ServiceUpdateType;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Router;

public class ChatsRouter
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private Dictionary<long, TransmittedData> _chatTransmittedDataPairs;

    private MessageServiceManager _messageService;
    private CallbackQueryServiceManager _callbackQueryService;

    public ChatsRouter()
    {
        _chatTransmittedDataPairs = new Dictionary<long, TransmittedData>();

        _messageService = new MessageServiceManager();
        _callbackQueryService = new CallbackQueryServiceManager();
    }
    
    public MessageToSend RouteMessage(long chatId, Message message)
    {
        Logger.Debug($"Старт метода Route в ChatsRouter: chatId = {chatId}");

        TransmittedData transmittedData = GetUserTransmittedData(chatId);

        MessageToSend messageToSend = _messageService.ProcessBotMessage(chatId, transmittedData, message);

        Logger.Debug($"Выполнен метод Route в ChatsRouter: chatId = {chatId}");

        return messageToSend;
    }
    
    public MessageToSend RouteCallbackQuery(long chatId, CallbackQuery callback)
    {
        Logger.Debug($"Старт метода Route в ChatsRouter: chatId = {chatId}");

        TransmittedData transmittedData = GetUserTransmittedData(chatId);

        MessageToSend messageToSend = _callbackQueryService.ProcessBotCallback(chatId, transmittedData, callback);

        Logger.Debug($"Выполнен метод Route в ChatsRoute: chatId = {chatId}");

        return messageToSend;
    }
    
    private TransmittedData GetUserTransmittedData(long chatId)
    {
        if (!_chatTransmittedDataPairs.ContainsKey(chatId))
        {
            _chatTransmittedDataPairs[chatId] = new TransmittedData();
        }

        return _chatTransmittedDataPairs[chatId];
    }
}