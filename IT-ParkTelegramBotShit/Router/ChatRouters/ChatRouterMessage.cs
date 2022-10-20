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

    public Task Route(long chatId, Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        Logger.Info($"Старт метода Route для chatId = {chatId}");

        TransmittedData transmittedData = _chatsRouter.GetUserTransmittedData(chatId);

        Task task = _servicesManager.ProcessBotUpdate(chatId, transmittedData, message, botClient, cancellationToken);

        Logger.Info($"Выполнен метода Route для chatId = {chatId}");

        return task;
    }
}