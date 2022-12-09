using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Bot;

public class BotMessageHistory
{
    private static BotMessageHistory _messageHistory;
    private List<int> _ordinaryMessagesIds;
    private List<int> _anchoredMessagesIds;

    private BotResponder _botResponder;

    private long _chatId;

    public BotMessageHistory(BotResponder botResponder, long chatId)
    {
        _ordinaryMessagesIds = new List<int>();
        _anchoredMessagesIds = new List<int>();

        _chatId = chatId;
        _botResponder = botResponder;
    }

    public void AddMessageId(int messageId)
    {
        _ordinaryMessagesIds.Add(messageId);
    }

    public void AddAnchoredMessagesId(params int[] messagesId)
    {
        for (int i = 0; i < messagesId.Length; i++)
        {
            _anchoredMessagesIds.Add(messagesId[i]);
        }
    }

    public void AddMessagesIds(params int[] messagesId)
    {
        for (int i = 0; i < messagesId.Length; i++)
        {
            _ordinaryMessagesIds.Add(messagesId[i]);
        }
    }
    
    public void AddMessageListIds(List<Message> messageList)
    {
        for (int i = 0; i < messageList.Count; i++)
        {
            _ordinaryMessagesIds.Add(messageList[i].MessageId);
        }
    }
    
    public async Task DeleteAllOrdinaryMessages()
    {
        await DeleteMessages(_ordinaryMessagesIds);
        
        _ordinaryMessagesIds.Clear();
    }
    
    public async Task DeleteAllMessages()
    {
        await DeleteAllOrdinaryMessages();
        
        await DeleteMessages(_anchoredMessagesIds);
        
        _anchoredMessagesIds.Clear();
    }
    
    public Task DeleteLastMessage()
    {
        if (_ordinaryMessagesIds.Count < 1)
        {
            throw new Exception("Messages count < 1");
        }

        int lastMessageId = _ordinaryMessagesIds[_ordinaryMessagesIds.Count - 1];
        
        _ordinaryMessagesIds.Remove(lastMessageId);

        return DeleteMessage(lastMessageId);
    }
    
    private Task DeleteMessage(int messageId)
    {
        return _botResponder.DeleteMessage(messageId, _chatId);
    }

    private async Task DeleteMessages(List<int> messageIdList)
    {
        List<int> messageIdsToDelete = new List<int>(messageIdList);
        
        for (int i = 0; i < messageIdsToDelete.Count; i++)
        {
            await DeleteMessage(messageIdsToDelete[i]);
        }
    }
}