using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Bot.Buttons;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Util;

public static class ReplyKeyboardsStorage
{
    public static readonly TeacherReplyKeyboardsStorage Teacher = new TeacherReplyKeyboardsStorage();
    public static readonly StudentReplyKeyboardsStorage Student = new StudentReplyKeyboardsStorage();
    
    public static InlineKeyboardMarkup FinalStep = BotKeyboardCreator.GetInstance()
        .GetKeyboardMarkup(ReplyButtonsStorage.Yes, ReplyButtonsStorage.No);
}

public class TeacherReplyKeyboardsStorage
{
    private static BotKeyboardCreator _creator = BotKeyboardCreator.GetInstance();
    private static TeacherReplyButtonsStorage _replyButtons = ReplyButtonsStorage.Teacher;
    
    public readonly InlineKeyboardMarkup MainMenu = _creator
        .GetKeyboardMarkup(_replyButtons.Groups, _replyButtons.AddHomework, _replyButtons.AddNextLessonDate, _replyButtons.Profile);
    
    public readonly InlineKeyboardMarkup EditGroup = _creator
        .GetKeyboardMarkup(_replyButtons.EditGroupName, _replyButtons.EditGroupInviteCode/*, _replyButtons.DeleteGroup*/, ReplyButtonsStorage.MainMenu); // todo DeleteGroup
}

public class StudentReplyKeyboardsStorage
{
    private static BotKeyboardCreator _creator = BotKeyboardCreator.GetInstance();
    private static StudentReplyButtonsStorage _replyButtons = ReplyButtonsStorage.Student;
    
    public readonly InlineKeyboardMarkup MainMenu = _creator
        .GetKeyboardMarkup(_replyButtons.NextLesson, _replyButtons.Homework, _replyButtons.Profile, _replyButtons.Payment);
    
    public readonly InlineKeyboardMarkup Profile = _creator
        .GetKeyboardMarkup(_replyButtons.ChangeName, _replyButtons.QuitAccount, ReplyButtonsStorage.MainMenu);
}