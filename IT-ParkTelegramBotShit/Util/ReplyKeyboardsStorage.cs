using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Bot.Buttons;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Util;

public class BotKeyboardsStorage
{
    private static BotKeyboardCreator _creator = BotKeyboardCreator.GetInstance();
    
    public static InlineKeyboardMarkup TeacherMainMenu = _creator
        .GetKeyboardMarkup(ReplyButtonsStorage.Groups, ReplyButtonsStorage.AddHomework, ReplyButtonsStorage.AddNextLessonDate, ReplyButtonsStorage.Profile);
}