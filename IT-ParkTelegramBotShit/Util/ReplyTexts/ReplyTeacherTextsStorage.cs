namespace IT_ParkTelegramBotShit.Util;

public class ReplyTeacherTextsStorage
{
    public readonly string Groups =
        "Группы";
    
    public readonly string InputGroupName =
        "Введите название группы";
    
    public readonly string InputGroupInviteCode =
        "Введите код приглашения в группы";
    
    public readonly string InputAnotherGroupName =
        "Такое название уже существует. Введите название другое группы";
    
    public readonly string InputAnotherInviteCode =
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
}