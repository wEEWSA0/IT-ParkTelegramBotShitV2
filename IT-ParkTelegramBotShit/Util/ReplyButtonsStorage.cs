using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Util;

public class ReplyButtonsStorage
{
    public static InlineKeyboardButton MainMenu = InlineKeyboardButton.WithCallbackData("Главное меню", CallbackQueryStorage.MainMenu);
    public static InlineKeyboardButton Logout = InlineKeyboardButton.WithCallbackData("Выйти", CallbackQueryStorage.Logout);
    
    #region TeacherMainMenu

    public static InlineKeyboardButton Groups = InlineKeyboardButton.WithCallbackData("Группы", CallbackQueryStorage.Groups);
    public static InlineKeyboardButton AddHomework = InlineKeyboardButton.WithCallbackData("Назначить дз", CallbackQueryStorage.AddHomework);
    public static InlineKeyboardButton Profile = InlineKeyboardButton.WithCallbackData("Профиль", CallbackQueryStorage.Profile);
    public static InlineKeyboardButton AddNextLessonDate = InlineKeyboardButton.WithCallbackData("Назначить занятие", CallbackQueryStorage.AddNextLessonDate);

    #endregion
}