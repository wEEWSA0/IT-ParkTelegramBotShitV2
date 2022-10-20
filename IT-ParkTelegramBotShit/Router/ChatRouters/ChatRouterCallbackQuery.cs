using IT_ParkTelegramBotShit.Router.Auxiliary;
using IT_ParkTelegramBotShit.Service;
using IT_ParkTelegramBotShit.Service.ChatServices;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Router.ChatRouters;

public class ChatRouterCallbackQuery
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private ChatsRouter _chatsRouter;
    private CallbackQueryServiceManager _servicesManager;

    public ChatRouterCallbackQuery(ChatsRouter chatsRouter)
    {
        _chatsRouter = chatsRouter;
        _servicesManager = new CallbackQueryServiceManager();
    }

    public Task Route(long chatId, CallbackQuery callback, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        Logger.Info($"Старт метода Route для chatId = {chatId}");
        
        TransmittedData transmittedData = _chatsRouter.GetUserTransmittedData(chatId);

        Task task = _servicesManager.ProcessBotUpdate(chatId, transmittedData, callback, botClient, cancellationToken);

        Logger.Info($"Выполнен метода Route для chatId = {chatId}");

        return task;
    }
}