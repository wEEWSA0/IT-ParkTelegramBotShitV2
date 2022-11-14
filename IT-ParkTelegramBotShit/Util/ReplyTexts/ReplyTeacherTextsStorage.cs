namespace IT_ParkTelegramBotShit.Util;

public class ReplyTeacherTextsStorage
{
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

    public readonly string InputHomework = 
        "Введите домашнее задание";

    public readonly string HomeworkCreated = 
        "Домашнее задание добавлено";

    public string GetNewHomeworkView(string homework)
    {
        return $"Дз для следующего занятия:"+
               $"\n{homework}" +
               $"\n\nДобавить?";
    }
    
    #endregion
    
    #region NextLessonReply
    
    public readonly string InputDateNextLesson = 
        "Введите время для следующего занятия";
    
    public readonly string DateNextLessonCreated = 
        "Введите время для следующего занятия";
    
    public string GetNewDateNextLessonView(string date)
    {
        return $"Дата и время следующего занятия: {date}"+
               $"\nНазначить?";
    }
    
    #endregion
}