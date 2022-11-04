using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Bot.Buttons;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Util;

public class BotKeyboardsStorage
{
    private static BotKeyboardCreator _creator = BotKeyboardCreator.GetInstance();
    
    public static InlineKeyboardMarkup MainMenu = _creator
        .GetKeyboardMarkup(ReplyButtonsStorage.MainMenu, ReplyButtonsStorage.Logout);
}