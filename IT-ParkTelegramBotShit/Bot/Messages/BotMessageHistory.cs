using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Bot.Messages;

public class BotMessageHistory
{
    private static BotMessageHistory _messageHistory = null;
    private List<Message> _messages;
    
    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;

    private long _chatId;

    public BotMessageHistory(TelegramBotClient client, CancellationTokenSource token, long chatId)
    {
        _messages = new List<Message>();

        _chatId = chatId;
        _botClient = client;
        _cancellationTokenSource = token;
    }
    
    public void AddMessage(Message message)
    {
        _messages.Add(message);
    }
    
    public void AddMessages(List<Message> messages)
    {
        for (int i = 0; i < messages.Count; i++)
        {
            _messages.Add(messages[i]);
        }
    }
    
    public async void DeleteAllMessages()
    {
        List<Message> messages = new List<Message>(_messages);
        
        _messages.Clear();
        
        for (int i = 0; i < messages.Count; i++)
        {
            await DeleteMessage(messages[i]);
        }
    }
    
    public Task DeleteLastMessage()
    {
        if (_messages.Count < 1)
        {
            throw new Exception("Messages count < 1");
        }

        Message message = _messages[_messages.Count - 1];
        
        _messages.Remove(message);
        
        return _botClient.DeleteMessageAsync(
            messageId: message.MessageId,
            chatId: _chatId,
            cancellationToken: _cancellationTokenSource.Token);
    }
    
    private Task DeleteMessage(Message message)
    {
        return _botClient.DeleteMessageAsync(
            messageId: message.MessageId,
            chatId: _chatId,
            cancellationToken: _cancellationTokenSource.Token);
    }
}