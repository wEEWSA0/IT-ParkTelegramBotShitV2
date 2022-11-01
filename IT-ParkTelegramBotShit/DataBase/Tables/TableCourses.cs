using IT_ParkTelegramBotShit.DataBase.Entities;
using Npgsql;

namespace IT_ParkTelegramBotShit.DataBase.Tables;

public class TableCourses
{
    private NpgsqlConnection _connection;

    public TableCourses(NpgsqlConnection connection)
    {
        _connection = connection;
    }
    
    public void CreateCourse(string courseName, string studentInviteCode, int teacherId)
    {
        string sqlRequest = $"INSERT INTO courses (course_name, student_invite_code, teacher_id) VALUES ('{courseName}', '{studentInviteCode}', {teacherId})";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);
        
        command.ExecuteNonQuery();
    }
}