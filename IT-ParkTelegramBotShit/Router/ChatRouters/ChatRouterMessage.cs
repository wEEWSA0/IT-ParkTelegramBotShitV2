using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Router.Auxiliary;
using IT_ParkTelegramBotShit.Service.ChatServices;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Router.ChatRouters;

public class ChatRouterMessage
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private ChatsRouter _chatsRouter;
    private MessageServiceManager _servicesManager;

    public ChatRouterMessage(ChatsRouter chatsRouter)
    {
        _chatsRouter = chatsRouter;
        _servicesManager = new MessageServiceManager();
    }

    public MessageToSend Route(long chatId, Message message)
    {
        Logger.Debug($"Старт метода Route: chatId = {chatId}");

        TransmittedData transmittedData = _chatsRouter.GetUserTransmittedData(chatId);

        MessageToSend messageToSend = _servicesManager.ProcessBotUpdate(chatId, transmittedData, message);

        Logger.Debug($"Выполнен метода Route: chatId = {chatId}");

        return messageToSend;
    }
}