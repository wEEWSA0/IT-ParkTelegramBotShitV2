using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Bot;

public class BotChatIdMessageManager
{
    private Queue<MessageToSend> _messagesToSend;
    
    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;

    private long _chatId;
    
    public BotChatIdMessageManager(TelegramBotClient client, CancellationTokenSource token, long chatId)
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

    public void SendAllMessages()
    {
        while (_messagesToSend.Count > 0)
        {
            SendMessage(_messagesToSend.Dequeue());
        }
    }
    
    private void SendMessage(MessageToSend message)
    {
        Task task = _botClient.SendTextMessageAsync(
            chatId: _chatId,
            text: message.Text,
            replyMarkup: message.InlineKeyboardMarkup,
            cancellationToken: _cancellationTokenSource.Token);
    }
}

public class MessageToSend
{
    public string Text { get; private set; }
    public InlineKeyboardMarkup? InlineKeyboardMarkup;

    public MessageToSend(string text)
    {
        Text = text;
    }
}