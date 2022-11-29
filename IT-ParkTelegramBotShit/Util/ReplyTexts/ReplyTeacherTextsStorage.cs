namespace IT_ParkTelegramBotShit.Util;

public class ReplyTeacherTextsStorage
{
    public readonly string LogIntoAccount =
        "Вы вошли в учетную запись преподователя";
    
    #region GroupsReply
    
    public readonly string Groups =
        "Группы";
    
    public readonly string EditGroup =
        "Редактирование группы";
    
    public readonly string InputGroupName =
        "Введите название группы";
    
    public readonly string InputGroupInviteCode =
        "Введите код приглашения в группу";
    
    public readonly string InputAnotherGroupName =
        "Такое название уже существует. Введите другое название группы";
    
    public readonly string InputAnotherGroupInviteCode =
        "Такой код уже существует. Введите другой";
    
    public readonly string GroupCreated =
        "Группа создана";
    
    public readonly string GroupNotCreated =
        "Группа не создана";
    
    public string GetGroupFinalStateView(string name, string inviteCode)
    {
        return $"Группа '{name}'" +
               $"\nКод для входа: {inviteCode}" +
               $"\n\nДобавить?";
    }
    
    public string GetNewGroupNameView(string name)
    {
        return $"Новое название группы '{name}'" +
               $"\n\nОбновить название группы на новое?";
    }
    
    public string GetNewGroupInviteCodeView(string inviteCode)
    {
        return $"Новое код приглашения {inviteCode}" +
               $"\n\nОбновить код приглашения на новый?";
    }
    
    #endregion
    
    #region HomeworkReply

    public readonly string GroupHomework =
        "В какую группу добавить дз?";
        
    public readonly string InputHomework = 
        "Введите домашнее задание";

    public readonly string HomeworkCreated = 
        "Домашнее задание добавлено";
    
    public readonly string HomeworkNotCreated = 
        "Домашнее задание не добавлено";

    public string GetNewHomeworkView(string homework)
    {
        return $"Дз для следующего занятия:"+
               $"\n{homework}" +
               $"\n\nДобавить?";
    }
    
    #endregion
    
    #region NextLessonReply

    public readonly string GroupNextLesson =
        "В какой группе указать дату занятия?";
    
    public static DateTime localDate = DateTime.Now;
    public static string dateNow = localDate.ToShortDateString();
    public static string timeNow = localDate.ToShortTimeString();
    
    public string InputNextLessonDate(string date)
    {
        DateTime localDate = DateTime.Now;
        dateNow = localDate.ToShortDateString();
        return "Введите дату следующего занятия" +
               $"\nв формате (время - {dateNow})";
    }
    
    public string InputNextLessonTime(string date)
    {
        DateTime localDate = DateTime.Now;
        timeNow = localDate.ToShortTimeString();
        return "Введите время следующего занятия" +
               $"\nв формате (время - {timeNow})";
    }

    public readonly string NextLessonDateCreated = 
        "Следующее занятие назначено";
    
    public readonly string NextLessonDateNotCreated = 
        "Следующее занятие не назначено";
    
    public string GetNewNextLessonDateView(string date)
    {
        return $"Дата и время следующего занятия:" +
               $"{date}"+
               $"\nНазначить?";
    }
    
    #endregion

    #region Profile

    public readonly string Profile =
        "Профиль";
    
    public readonly string EditName =
        "Введите новые ФИО";
    
    public string ProfileNewNameView(string name)
    {
        return $"Новые ФИО:"+
               $"\n{name}" +
               $"\n\nИзменить?";
    }

    public readonly string NameEdited =
        "ФИО успешно изменены";
    
    public readonly string NameNotEdited =
        "ФИО остались прежними";
    
    public readonly string NotLogOut =
        "Вы решили остаться";
    
    public readonly string LogOut =
        $"Вы вышли из профиля, введите /start" +
        $"\nчтобы начать работу";
    
    public readonly string ProfileLogOut =
        "Вы действительно хотите выйти?";
    
    #endregion
}