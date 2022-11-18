using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Bot.Messages;

public class BotMessageHistory
{
    private static BotMessageHistory _messageHistory;
    private List<int> _messagesIds;
    
    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;

    private long _chatId;

    public BotMessageHistory(TelegramBotClient client, CancellationTokenSource token, long chatId)
    {
        _messagesIds = new List<int>();

        _chatId = chatId;
        _botClient = client;
        _cancellationTokenSource = token;
    }
    
    public void AddMessageId(int messageId)
    {
        _messagesIds.Add(messageId);
    }
    
    public void AddMessagesIds(params int[] messagesId)
    {
        for (int i = 0; i < messagesId.Length; i++)
        {
            _messagesIds.Add(messagesId[i]);
        }
    }
    
    public void AddMessageListIds(List<Message> messageList)
    {
        for (int i = 0; i < messageList.Count; i++)
        {
            _messagesIds.Add(messageList[i].MessageId);
        }
    }
    
    public async void DeleteAllMessages()
    {
        List<int> messagesToDelete = new List<int>(_messagesIds);
        
        _messagesIds.Clear();
        
        for (int i = 0; i < messagesToDelete.Count; i++)
        {
            await DeleteMessage(messagesToDelete[i]);
        }
    }
    
    public Task DeleteLastMessage()
    {
        if (_messagesIds.Count < 1)
        {
            throw new Exception("Messages count < 1");
        }

        int lastMessageId = _messagesIds[_messagesIds.Count - 1];
        
        _messagesIds.Remove(lastMessageId);
        
        return _botClient.DeleteMessageAsync(
            messageId: lastMessageId,
            chatId: _chatId,
            cancellationToken: _cancellationTokenSource.Token);
    }
    
    private Task DeleteMessage(int messageId)
    {
        return _botClient.DeleteMessageAsync(
            messageId: messageId,
            chatId: _chatId,
            cancellationToken: _cancellationTokenSource.Token);
    }
}