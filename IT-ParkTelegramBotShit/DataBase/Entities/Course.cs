namespace IT_ParkTelegramBotShit.DataBase.Entities;

public class Course
{
    public int Id { get; set; }
    public string CourseName { get; set; }
    public string StudentInviteCode { get; set; }
    public int TeacherId { get; set; }
    public DateTime? NextLesson { get; set; }
    public string? Homework { get; set; }

    public Course()
    {
        Id = -1;
        CourseName = "";
        StudentInviteCode = "";
    }
    public Course(int id, string courseName, string studentInviteCode, int teacherId)
    {
        Id = id;
        CourseName = courseName;
        StudentInviteCode = studentInviteCode;
        TeacherId = teacherId;
    }
}