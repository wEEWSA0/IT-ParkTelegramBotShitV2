namespace IT_ParkTelegramBotShit.Util;

public class ReplyTextsStorage
{
    public static ReplyTeacherTestsStorage Teacher = new ReplyTeacherTestsStorage();

    public static ReplyStudentTestsStorage Student = new ReplyStudentTestsStorage();
    
    public static string ErrorInput =
        "Команда не распознана. Для начала работы с ботом введите /start";

    public static string NotValidCode =
        "Код не распознан";
    
    public static string CmsStart =
        "Для начала работы с ботом введите код ";
}