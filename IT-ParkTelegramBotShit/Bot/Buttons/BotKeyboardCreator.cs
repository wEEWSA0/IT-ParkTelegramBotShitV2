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

        InlineKeyboardButton[][] buttonsInColumn = new InlineKeyboardButton[buttons.Length][];

        for (int i = 0; i < buttons.Length; i++)
        {
            buttonsInColumn[i] = new InlineKeyboardButton[1];
            buttonsInColumn[i][0] = buttons[i];
        }
        
        keyboard = new InlineKeyboardMarkup(buttonsInColumn);

        return keyboard;
    }
    
    public InlineKeyboardMarkup GetKeyboardMarkupRow(params InlineKeyboardButton[] buttons)
    {
        InlineKeyboardMarkup keyboard;

        keyboard = new InlineKeyboardMarkup(buttons);
        
        return keyboard;
    }
}