namespace IT_ParkTelegramBotShit.Util;

public static class ReplyTextsStorage
{
    public static readonly ReplyTeacherTextsStorage Teacher = new ReplyTeacherTextsStorage();

    public static readonly ReplyStudentTextsStorage Student = new ReplyStudentTextsStorage();

    public static string Empty =
        "Сообщите в поддержку о том, что увидели данное сообщение. Это поможет исправлению проблемы";
    
    public static string ErrorInput =
        "Команда не распознана. Для начала работы с ботом введите /start";

    public static string NotValidCode =
        "Код не распознан";
    
    public static string CmdStart =
        "Для начала работы с ботом введите код ";
    
    public static string FatalError =
        "Сбои в работе бота! Сообщите в поддержку о том, что увидели данное сообщение. Это поможет исправлению данной проблемы";
    
    public static string InDevelopment =
        "В данный момент функция недоступна";

    public static string MainMenu =
        "Главное меню";
    
    public static string FinalStep =
        "Заключение";
}