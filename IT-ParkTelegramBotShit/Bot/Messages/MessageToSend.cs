using IT_ParkTelegramBotShit.Util;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Bot;

public class MessageToSend
{
    public string Text { get; }
    public InlineKeyboardMarkup? InlineKeyboardMarkup;
    public bool IsLastMessagesHistoryNeeded { get; }
    
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

    public static MessageToSend Empty()
    {
        return new MessageToSend(ConstantsStorage.EmptyMessageToSend);
    }
}