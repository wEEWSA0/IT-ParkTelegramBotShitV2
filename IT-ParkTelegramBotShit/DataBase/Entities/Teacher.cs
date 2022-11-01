namespace IT_ParkTelegramBotShit.DataBase.Entities;

public class Teacher
{
    public int Id { get; set; }
    public string Name { get; set; }
    public long ChatId { get; set; }
    public string InviteCode { get; set; }

    public Teacher()
    {
        Id = -1;
        Name = "";
        InviteCode = "";
    }
    public Teacher(int id, string name, long chatId, string inviteCode)
    {
        Id = id;
        Name = name;
        ChatId = chatId;
        InviteCode = inviteCode;
    }
}