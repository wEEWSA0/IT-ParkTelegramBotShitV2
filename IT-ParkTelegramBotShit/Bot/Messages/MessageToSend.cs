using IT_ParkTelegramBotShit.Util;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Bot;

public class MessageToSend
{
    public string Text { get; }
    public InlineKeyboardMarkup? InlineKeyboardMarkup;
    public InputOnlineFile OnlineFile;
    public bool IsLastMessagesHistoryNeeded { get; }
    // todo add work with images and stickers
    
    public MessageToSend(string text)
    {
        Text = text;
        IsLastMessagesHistoryNeeded = true;
    }
    
    public MessageToSend(string text, InlineKeyboardMarkup? markup)
    {
        Text = text;
        InlineKeyboardMarkup = markup;
        IsLastMessagesHistoryNeeded = true;
    }
    
    public MessageToSend(string text, bool isLastMessagesHistoryNeeded)
    {
        Text = text;
        IsLastMessagesHistoryNeeded = isLastMessagesHistoryNeeded;
    }
    
    public MessageToSend(string text, InlineKeyboardMarkup? markup, bool isLastMessagesHistoryNeeded)
    {
        Text = text;
        InlineKeyboardMarkup = markup;
        IsLastMessagesHistoryNeeded = isLastMessagesHistoryNeeded;
    }
    
    public MessageToSend(string text, InputOnlineFile onlineFile, bool isLastMessagesHistoryNeeded)
    {
        Text = text;
        IsLastMessagesHistoryNeeded = isLastMessagesHistoryNeeded;
        OnlineFile = onlineFile;
    }
    
    public MessageToSend(string text, InlineKeyboardMarkup? markup, InputOnlineFile onlineFile, bool isLastMessagesHistoryNeeded)
    {
        Text = text;
        InlineKeyboardMarkup = markup;
        IsLastMessagesHistoryNeeded = isLastMessagesHistoryNeeded;
    }

    public static MessageToSend Empty()
    {
        return new MessageToSend(ConstantsStorage.EmptyMessageToSend);
    }
    
    /*
     * Класс, хранящий содержание сообщения, которое будет отправлено
     * Работает с текстом и встроенной в сообщение клавиатурой
     */
}