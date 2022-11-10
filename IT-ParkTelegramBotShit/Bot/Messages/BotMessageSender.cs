using IT_ParkTelegramBotShit.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Bot;

public class BotMessageSender
{
    private Queue<MessageToSend> _messagesToSend;
    
    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;

    private long _chatId;
    
    public BotMessageSender(TelegramBotClient client, CancellationTokenSource token, long chatId)
    {
        _messagesToSend = new Queue<MessageToSend>();

        _chatId = chatId;
        _botClient = client;
        _cancellationTokenSource = token;
    }

    public void AddMessageToStack(string text)
    {
        MessageToSend message = new MessageToSend(text);

        _messagesToSend.Enqueue(message);
    }
    
    public void AddMessageToStack(string text, InlineKeyboardMarkup inlineKeyboard)
    {
        MessageToSend message = new MessageToSend(text);
        
        message.InlineKeyboardMarkup = inlineKeyboard;

        _messagesToSend.Enqueue(message);
    }
    
    public void AddMessageToStack(MessageToSend message)
    {
        _messagesToSend.Enqueue(message);
    }

    public List<Message> SendAllMessages()
    {
        List<Message> messages = new List<Message>();
        
        while (_messagesToSend.Count > 0)
        {
            Message message = SendMessage(_messagesToSend.Dequeue());
            
            messages.Add(message);
        }

        return messages;
    }
    
    private Message SendMessage(MessageToSend message)
    { 
        Task<Message> task = _botClient.SendTextMessageAsync(
            chatId: _chatId,
            text: message.Text,
            replyMarkup: message.InlineKeyboardMarkup,
            cancellationToken: _cancellationTokenSource.Token);

        return task.Result;
    }
}

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