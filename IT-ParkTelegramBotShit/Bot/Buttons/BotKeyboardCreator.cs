using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Bot.Buttons;

public class BotKeyboardCreator
{
    private static BotKeyboardCreator _keyboardCreator = null;

    private BotKeyboardCreator()
    {
        
    }
    
    public static BotKeyboardCreator GetInstance()
    {
        if (_keyboardCreator == null)
        {
            _keyboardCreator = new BotKeyboardCreator();
        }
        
        return _keyboardCreator;
    }

    public InlineKeyboardMarkup GetKeyboardMarkup(params InlineKeyboardButton[] buttons)
    {
        InlineKeyboardMarkup keyboard;

        keyboard = new InlineKeyboardMarkup(buttons);
        
        return keyboard;
    }
}