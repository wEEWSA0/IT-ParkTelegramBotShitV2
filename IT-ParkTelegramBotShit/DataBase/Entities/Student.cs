namespace IT_ParkTelegramBotShit.DataBase.Entities;

public class Student
{
    public long ChatId { get; set; }
    public int CourseId { get; set; }
    public string Name { get; set; }

    public Student()
    {
        ChatId = -1;
        Name = "";
    }
    public Student(long chatId, int courseId, string name)
    {
        ChatId = chatId;
        CourseId = courseId;
        Name = name;
    }
}