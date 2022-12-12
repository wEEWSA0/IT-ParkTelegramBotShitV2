namespace IT_ParkTelegramBotShit.Util;

public class ReplyStudentTextsStorage
{
    public readonly string LogIntoAccount =
        "Вы вошли в систему за ученика";
    
    public readonly string InputName =
        "Введите свое имя (оно будет отображаться на курсе)";
    
    public readonly string Profile =
        "Профиль";
    
    public readonly string Homework =
        "Домашнее задание:";
    
    public readonly string NextLesson =
        "Следующее занятие будет";
    
    public readonly string NextLessonNotAssigned =
        "Следущее занятие не назначено";
    
    public readonly string QuitAccount =
        "Вы вышли из аккаунта ученика";
    
    public readonly string QuitAccountFinalStep =
        "Вы точно собираетесь выйти из аккаунта ученика? Данные будут стерты";
    
    public string GetNewNameView(string name)
    {
        return $"Новое имя '{name}'" +
               $"\n\nОбновить имя на новое?";
    }
}