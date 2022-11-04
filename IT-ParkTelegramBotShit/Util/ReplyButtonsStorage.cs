using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Util;

public class ReplyButtonsStorage
{
    public static InlineKeyboardButton MainMenu = InlineKeyboardButton.WithCallbackData("Main menu", CallbackQueryStorage.MainMenu);
    public static InlineKeyboardButton Logout = InlineKeyboardButton.WithCallbackData("Logout", CallbackQueryStorage.Logout);
}