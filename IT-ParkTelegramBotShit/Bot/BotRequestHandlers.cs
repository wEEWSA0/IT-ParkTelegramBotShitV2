using IT_ParkTelegramBotShit.Router;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace IT_ParkTelegramBotShit.Bot;

public class BotRequestHandlers
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    private ChatsRouter _chatsRouter;

    public BotRequestHandlers()
    {
        Logger.Info("Старт инициализации ChatsRouter");
        _chatsRouter = new ChatsRouter();
        Logger.Info("Выволнена инициализация ChatsRouter");
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        Logger.Info("Старт обработки входящего сообщения от клиента в методе HandleUpdateAsync");
        
        long chatId = 0;
        
        switch (update.Type)
        {
            case UpdateType.Message:
                if (update.Message != null)
                {
                    chatId = update.Message.Chat.Id;
                    Logger.Debug($"Тип входящего сообщения от chatId = {chatId} - UpdateType.Message");
                    
                    MessageToSend messageToSend = _chatsRouter.RouterMessage.Route(chatId, update.Message);
                    
                    BotMessageManager.GetInstance().GetChatIdMessageManager(chatId).AddMessageToStack(messageToSend);
                }
                break;

            case UpdateType.CallbackQuery:
                if (update.CallbackQuery != null)
                {
                   /* if (update.CallbackQuery.Message == null) {return;}
                    
                    chatId = update.CallbackQuery.Message.Chat.Id;
                    Logger.Debug($"Тип входящего сообщения chatId = {chatId} - UpdateType.CallbackQuery");

                    await _chatsRouter.RouterCallbackQuery.Route(chatId, update.CallbackQuery, botClient, cancellationToken);*/
                }
                break;
        }
        
        BotMessageManager.GetInstance().GetChatIdMessageManager(chatId).SendAllMessages();
        
        Logger.Info($"Выполенна обработка входящего сообщения от chatId = {chatId} в методе HandleUpdateAsync");
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        string errorMessage = "empty";
        switch (exception)
        {
            case ApiRequestException:
            {
                var ex = exception as ApiRequestException;
                errorMessage = $"Telegram API Error:\n[{ex.ErrorCode}]\n{ex.Message}";
            }
                break;
            default:
            {
                errorMessage = exception.ToString();
            }
                break;
        }

        Logger.Error($"Ошибка поймана в методе HandlePollingErrorAsync, {errorMessage}");
        return Task.CompletedTask;
    }
}