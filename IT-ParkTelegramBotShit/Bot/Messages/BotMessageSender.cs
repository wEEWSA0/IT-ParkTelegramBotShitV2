using IT_ParkTelegramBotShit.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Bot;

public class BotMessageSender
{
    private Queue<MessageToSend> _messagesToSend;
    
    private BotResponder _botResponder;

    private BotMessageHistory _history;
    private long _chatId;
    
    public BotMessageSender(BotResponder botResponder, long chatId)
    {
        _messagesToSend = new Queue<MessageToSend>();
        
        _history = BotMessageManager.GetInstance().GetHistory(chatId);

        _chatId = chatId;
        _botResponder = botResponder;
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
        BotStatisticManager.GetInstance().AddWorkLoad();

        Task<Message> task = _botResponder.SendText(message, _chatId);

        return task.Result;
    }
}