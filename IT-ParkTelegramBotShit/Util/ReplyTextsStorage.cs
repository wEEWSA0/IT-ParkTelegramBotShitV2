namespace IT_ParkTelegramBotShit.Util;

public class ReplyTextsStorage
{
    public static ReplyTeacherTestsStorage Teacher = new ReplyTeacherTestsStorage();

    public static ReplyStudentTextsStorage Student = new ReplyStudentTextsStorage();

    public static string Empty =
        "Empty";
    
    public static string ErrorInput =
        "Команда не распознана. Для начала работы с ботом введите /start";

    public static string NotValidCode =
        "Код не распознан";
    
    public static string CmdStart =
        "Для начала работы с ботом введите код ";
    
    public static string FatalError =
        "Сбои в работе бота!";
    
    public static string InDevelopment =
        "В данный момент функция недоступна";

    public static string MainMenu =
        "Главное меню";
}