using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Util;

public static class ReplyButtonsStorage
{
    public static readonly TeacherReplyButtonsStorage Teacher = new TeacherReplyButtonsStorage();
    
    public static InlineKeyboardButton MainMenu = InlineKeyboardButton.WithCallbackData("Главное меню", CallbackQueryStorage.MainMenu);
    public static InlineKeyboardButton Logout = InlineKeyboardButton.WithCallbackData("Выйти", CallbackQueryStorage.Logout);
}

public class TeacherReplyButtonsStorage
{
    private static TeacherCallbackQueryStorage _callbacks = CallbackQueryStorage.Teacher;
    
    #region MainMenu

    public readonly InlineKeyboardButton Groups = InlineKeyboardButton.WithCallbackData("Группы", _callbacks.Groups);
    public readonly InlineKeyboardButton AddHomework = InlineKeyboardButton.WithCallbackData("Назначить дз", _callbacks.AddHomework);
    public readonly InlineKeyboardButton Profile = InlineKeyboardButton.WithCallbackData("Профиль", _callbacks.Profile);
    public readonly InlineKeyboardButton AddNextLessonDate = InlineKeyboardButton.WithCallbackData("Назначить занятие", _callbacks.AddNextLessonDate);

    #endregion
    #region Groups

    public readonly InlineKeyboardButton EditGroup = InlineKeyboardButton.WithCallbackData("Редактировать группу", _callbacks.EditGroup);
    public readonly InlineKeyboardButton CreateGroup = InlineKeyboardButton.WithCallbackData("Создать группу", _callbacks.CreateGroup);

    #endregion
    #region EditGroup

    public readonly InlineKeyboardButton EditGroupName = InlineKeyboardButton.WithCallbackData("Изменить название группы", _callbacks.EditGroupName);
    public readonly InlineKeyboardButton DeleteGroup = InlineKeyboardButton.WithCallbackData("Удалить группу", _callbacks.DeleteGroup);

    #endregion
}