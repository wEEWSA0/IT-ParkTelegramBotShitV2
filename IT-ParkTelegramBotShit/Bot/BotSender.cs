using Telegram.Bot;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Bot;

public class BotSender
{
    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;

    public BotSender(TelegramBotClient botClient, CancellationTokenSource tokenSource)
    {
        _botClient = botClient;
        _cancellationTokenSource = tokenSource; // todo реализовать класс, использовать его
    }
    
    private Task<Message> SendTextMessage(MessageToSend message, long chatId)
    {
        return _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: message.Text,
            replyMarkup: message.InlineKeyboardMarkup,
            cancellationToken: _cancellationTokenSource.Token);
    }
    
    private Task<Message> SendPhoto(MessageToSend message, long chatId)
    {
        return _botClient.SendPhotoAsync(
            chatId: chatId,
            caption: message.Text,
            photo: message.OnlineFile,
            replyMarkup: message.InlineKeyboardMarkup,
            cancellationToken: _cancellationTokenSource.Token);
    }
    // todo найти различия между фото и файлом
    private Task<Message> SendFile(MessageToSend message, long chatId)
    {
        return _botClient.SendDocumentAsync(
            chatId: chatId,
            caption: message.Text,
            document: message.OnlineFile,
            replyMarkup: message.InlineKeyboardMarkup,
            cancellationToken: _cancellationTokenSource.Token);
    }
}