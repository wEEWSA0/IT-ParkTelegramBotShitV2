using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Bot.Buttons;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Util;

public static class ReplyKeyboardsStorage
{
    public static readonly TeacherReplyKeyboardsStorage Teacher = new TeacherReplyKeyboardsStorage();
    
    public static InlineKeyboardMarkup FinalStep = BotKeyboardCreator.GetInstance()
        .GetKeyboardMarkup(ReplyButtonsStorage.Yes, ReplyButtonsStorage.No);
}

public class TeacherReplyKeyboardsStorage
{
    private static BotKeyboardCreator _creator = BotKeyboardCreator.GetInstance();
    private static TeacherReplyButtonsStorage _replyButtons = ReplyButtonsStorage.Teacher;
    
    public readonly InlineKeyboardMarkup MainMenu = _creator
        .GetKeyboardMarkup(_replyButtons.Groups, _replyButtons.AddHomework, _replyButtons.AddNextLessonDate, _replyButtons.Profile);
    /*
    public readonly InlineKeyboardMarkup Groups = _creator
        .GetKeyboardMarkup(_replyButtons.CreateGroup, _replyButtons.EditGroup);*/
}