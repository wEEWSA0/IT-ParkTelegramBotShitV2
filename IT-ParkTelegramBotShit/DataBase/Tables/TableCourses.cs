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

    public void UpdateCourseHomework(string homework, int courseId)
    {
        string sqlRequest = $"UPDATE courses SET homework = '{homework}' WHERE id = {courseId}";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);
        
        command.ExecuteNonQuery();
    }
    
    public void UpdateCourseNextLessonTime(DateTime nextLesson, int courseId)
    {
        string sqlRequest = $"UPDATE courses SET next_lesson = {nextLesson} WHERE id = {courseId}";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);
        
        command.ExecuteNonQuery();
    }

    public void UpdateCourseChangeInfo(string courseName, string inviteCode, int courseId)
    {
        string sqlRequest = $"UPDATE courses SET course_name = '{courseName}', student_invite_code = '{inviteCode}' WHERE id = {courseId}";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);
        
        command.ExecuteNonQuery();
    }

    public bool TryGetCourseHomework(out string homework, int courseId)
    {
        string sqlRequest = $"SELECT homework FROM courses WHERE id = {courseId}";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);
        
        command.ExecuteNonQuery();
        
        NpgsqlDataReader dataReader = command.ExecuteReader();
        
        if (!dataReader.HasRows)
        {
            homework = "";
            
            dataReader.Close();
            
            return false;
        }
        
        homework = dataReader.GetString(dataReader.GetOrdinal("homework"));
        
        dataReader.Close();

        return true;
    }
    
    public bool TryGetCourseNextLessonTime(out DateTime nextLesson, int courseId)
    {
        string sqlRequest = $"SELECT next_lesson FROM courses WHERE id = {courseId}";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);
        
        command.ExecuteNonQuery();
        
        NpgsqlDataReader dataReader = command.ExecuteReader();
        
        if (!dataReader.HasRows)
        {
            nextLesson = new DateTime();
            
            dataReader.Close();
            
            return false;
        }
        
        nextLesson = dataReader.GetDateTime(dataReader.GetOrdinal("next_lesson"));
        
        dataReader.Close();

        return true;
    }

    public bool TryGetTeacherCourses(out List<Course> courses, int teacherId)
    {
        string sqlRequest = $"SELECT * FROM courses WHERE teacher_id = {teacherId}";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();
        
        if (!dataReader.HasRows)
        {
            courses = new List<Course>();
            
            dataReader.Close();
            
            return false;
        }
        
        courses = new List<Course>();
        
        while (dataReader.Read())
        {
            int id = dataReader.GetInt32(dataReader.GetOrdinal("id"));
            string courseName = dataReader.GetString(dataReader.GetOrdinal("course_name"));
            string studentInviteCode = dataReader.GetString(dataReader.GetOrdinal("student_invite_code"));

            Course course = new Course(id, courseName, studentInviteCode, teacherId);
            
            courses.Add(course);
        }

        dataReader.Close();

        return true;
    }

    public bool TryGetCourseByStudentInviteCode(out Course course, string inviteCode)
    {
        string sqlRequest = $"SELECT * FROM courses WHERE student_invite_code = '{inviteCode}'";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();
        
        if (!dataReader.HasRows)
        {
            course = new Course();
            
            dataReader.Close();
            
            return false;
        }
        
        int id = dataReader.GetInt32(dataReader.GetOrdinal("id"));
        string courseName = dataReader.GetString(dataReader.GetOrdinal("course_name"));
        string studentInviteCode = dataReader.GetString(dataReader.GetOrdinal("student_invite_code"));
        int teacherId = dataReader.GetInt32(dataReader.GetOrdinal("teacher_id"));

        course = new Course(id, courseName, studentInviteCode, teacherId);
        
        dataReader.Close();

        return true;
    }
}