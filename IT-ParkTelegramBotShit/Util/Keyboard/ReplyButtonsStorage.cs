using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Util;

public static class ReplyButtonsStorage
{
    public static readonly TeacherReplyButtonsStorage Teacher = new TeacherReplyButtonsStorage();
    public static readonly StudentReplyButtonsStorage Student = new StudentReplyButtonsStorage();
    
    public static InlineKeyboardButton MainMenu = InlineKeyboardButton.WithCallbackData("Главное меню", CallbackQueryStorage.MainMenu);
    public static InlineKeyboardButton Yes = InlineKeyboardButton.WithCallbackData("Да", CallbackQueryStorage.Yes);
    public static InlineKeyboardButton No = InlineKeyboardButton.WithCallbackData("Нет", CallbackQueryStorage.No);
}

public class TeacherReplyButtonsStorage
{
    private static TeacherCallbackQueryStorage _callbacks = CallbackQueryStorage.Teacher;
    
    #region MainMenu

    public readonly InlineKeyboardButton Groups = InlineKeyboardButton.WithCallbackData("Группы", _callbacks.Groups);
    public readonly InlineKeyboardButton AddHomework = InlineKeyboardButton.WithCallbackData("Назначить дз", _callbacks.AddHomework);
    public readonly InlineKeyboardButton Profile = InlineKeyboardButton.WithCallbackData("Профиль (не трожь)", _callbacks.Profile);
    public readonly InlineKeyboardButton AddNextLessonDate = InlineKeyboardButton.WithCallbackData("Назначить занятие (не трожь)", _callbacks.AddNextLessonDate);

    #endregion
    #region Groups

    public readonly InlineKeyboardButton EditGroup = InlineKeyboardButton.WithCallbackData("Редактировать группу", _callbacks.EditGroup);
    public readonly InlineKeyboardButton CreateGroup = InlineKeyboardButton.WithCallbackData("Создать группу", _callbacks.CreateGroup);

    #endregion
    #region EditGroup

    public readonly InlineKeyboardButton EditGroupName = InlineKeyboardButton.WithCallbackData("Изменить название группы", _callbacks.EditGroupName);
    public readonly InlineKeyboardButton EditGroupInviteCode = InlineKeyboardButton.WithCallbackData("Изменить код приглашения", _callbacks.EditGroupInviteCode);
    public readonly InlineKeyboardButton DeleteGroup = InlineKeyboardButton.WithCallbackData("Удалить группу", _callbacks.DeleteGroup);

    #endregion
}

public class StudentReplyButtonsStorage
{
    private static StudentCallbackQueryStorage _callbacks = CallbackQueryStorage.Student;
    
    #region NextLesson
    
    public readonly InlineKeyboardButton NextLesson = InlineKeyboardButton.WithCallbackData("Следущее занятие", _callbacks.NextLesson);
    public readonly InlineKeyboardButton SkipNextLesson = InlineKeyboardButton.WithCallbackData("Сообщить о пропуске занятия", _callbacks.SkipNextLesson);
        
    #endregion
        
    #region Homework
        
    public readonly InlineKeyboardButton Homework = InlineKeyboardButton.WithCallbackData("Домашнее задание", _callbacks.Homework);
        
    #endregion
        
    #region Profile
        
    public readonly InlineKeyboardButton Profile = InlineKeyboardButton.WithCallbackData("Профиль", _callbacks.Profile);
    public readonly InlineKeyboardButton QuitAccount = InlineKeyboardButton.WithCallbackData("Выйти из профиля", _callbacks.QuitAccount);
    public readonly InlineKeyboardButton ChangeName = InlineKeyboardButton.WithCallbackData("Поменять ФИО", _callbacks.ChangeName);
    
    #endregion
        
    #region Payment
        
    public readonly InlineKeyboardButton Payment = InlineKeyboardButton.WithUrl("Оплата", "https://itpark32.ru/profile");
        
    #endregion
}