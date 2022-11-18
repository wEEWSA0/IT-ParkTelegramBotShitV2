using IT_ParkTelegramBotShit.Router;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace IT_ParkTelegramBotShit.Bot.Messages;

public class BotRequestHandlers
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    private ChatsRouter _chatsRouter;

    public BotRequestHandlers()
    {
        _chatsRouter = new ChatsRouter();
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        var messageManager = BotMessageManager.GetInstance();
        MessageToSend messageToSend = MessageToSend.Empty();
        long chatId = 0;
        
        switch (update.Type)
        {
            case UpdateType.Message:
                if (update.Message != null)
                {
                    chatId = update.Message.Chat.Id;
                    
                    messageManager.GetHistory(chatId).AddMessageId(update.Message.MessageId);
                    
                    Logger.Info($"Принято входящее сообщение: chatId = {chatId}, UpdateType.Message");
                    
                    messageToSend =
                        await Task.Run(() => _chatsRouter.RouteMessage(chatId, update.Message), cancellationToken);
                }
                break;

            case UpdateType.CallbackQuery:
                if (update.CallbackQuery != null)
                {
                    if (update.CallbackQuery.Message == null)
                    {
                        throw new FormatException();
                    }
                    
                    chatId = update.CallbackQuery.Message.Chat.Id;
                    
                    Logger.Info($"Принято входящее сообщение: chatId = {chatId}, UpdateType.CallbackQuery");

                    messageToSend =
                        await Task.Run(() => _chatsRouter.RouteCallbackQuery(chatId, update.CallbackQuery), cancellationToken);
                }
                break;
        }

        if (messageToSend.Text != MessageToSend.Empty().Text)
        {
            var sender = messageManager.GetSender(chatId);
            var history = messageManager.GetHistory(chatId);
            
            sender.AddMessageToStack(messageToSend);
            
            var messages = sender.SendAllMessages();

            history.AddMessageListIds(messages);
            
            Logger.Info($"Отправлено ответное сообщение: chatId = {chatId}");
        }
        else
        {
            if (update.Type == UpdateType.Message)
            {
                if (update.Message == null)
                {
                    Logger.Error("update.Type is null and UpdateType is Message. It can't be!");
                    throw new Exception();
                }
                
                await messageManager.GetHistory(chatId).DeleteLastMessage();
            }
            else
            {
                Logger.Warn("MessageToSend is Empty and UpdateType isn't Message. It can't be!");
            }
        }
        
        Logger.Debug($"Выполенна обработка входящего сообщения: chatId = {chatId} в методе HandleUpdateAsync");
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