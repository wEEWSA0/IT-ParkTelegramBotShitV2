using Telegram.Bot;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Bot;

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
        for (int i = 0; i < _messages.Count; i++)
        {
            await DeleteMessage(_messages[i]);
        }
        
        _messages.Clear();
    }
    
    private Task DeleteMessage(Message message)
    {
        return _botClient.DeleteMessageAsync(
            messageId: message.MessageId,
            chatId: _chatId,
            cancellationToken: _cancellationTokenSource.Token);
    }
}