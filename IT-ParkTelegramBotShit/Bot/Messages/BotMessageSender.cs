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

    private BotMessageHistory _history;
    private long _chatId;
    
    public BotMessageSender(TelegramBotClient client, CancellationTokenSource token, long chatId)
    {
        _messagesToSend = new Queue<MessageToSend>();
        
        _history = BotMessageManager.GetInstance().GetHistory(chatId);

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

    public async Task<List<Message>> SendAllMessages()
    {
        List<Message> messages = new List<Message>();
        
        while (_messagesToSend.Count > 0)
        {
            var messageToSend = _messagesToSend.Dequeue();
            
            if (!messageToSend.IsLastMessagesHistoryNeeded)
            {
                await _history.DeleteAllOrdinaryMessages();
            }
            
            Message message = SendMessage(messageToSend);
            
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