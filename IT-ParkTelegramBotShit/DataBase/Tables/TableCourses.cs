using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.DataBase.Entities;
using Npgsql;

namespace IT_ParkTelegramBotShit.DataBase.Tables;

public class TableCourses : ITables.ITableCourses
{
    private NpgsqlConnection _connection; // todo optimaze code (add help methods)
    // можно добавить класс со статическимим методами (общими для всех Tables)

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
        string sqlFormattedDate = nextLesson.ToString("yyyy-MM-dd HH:mm:ss.fff");
        
        string sqlRequest = $"UPDATE courses SET next_lesson = '{sqlFormattedDate}' WHERE id = {courseId}";

        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);
        
        command.ExecuteNonQuery();
    }

    public void TeacherLogOut(int teacherId)
    {
        string sqlRequest = $"UPDATE teachers SET chat_id = null WHERE id = {teacherId}";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);
        
        command.ExecuteNonQuery();
    }
    
    public void UpdateTeacherName(string newName, int teacherId)
    {
        string sqlRequest = $"UPDATE teachers SET name = '{newName}' WHERE id = {teacherId}";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);
        
        command.ExecuteNonQuery();
    }
    
    public void UpdateCourseName(string courseName, int courseId)
    {
        string sqlRequest = $"UPDATE courses SET course_name = '{courseName}' WHERE id = {courseId}";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);
        
        command.ExecuteNonQuery();
    }
    
    public void UpdateCourseInviteCode(string inviteCode, int courseId)
    {
        string sqlRequest = $"UPDATE courses SET student_invite_code = '{inviteCode}' WHERE id = {courseId}";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);
        
        command.ExecuteNonQuery();
    }

    public bool TryGetCourseHomework(out string homework, int courseId)
    {
        string sqlRequest = $"SELECT homework FROM courses WHERE id = {courseId}";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);
        
        command.ExecuteNonQuery();
        
        NpgsqlDataReader dataReader = command.ExecuteReader();
        
        homework = "";
        
        if (!dataReader.HasRows)
        {
            dataReader.Close();
            
            return false;
        }
        
        if (dataReader.Read())
        {
            int ordinal = dataReader.GetOrdinal("homework");
            
            if (TableHelpMethods.IsOrdinalValueNull(dataReader, ordinal))
            {
                dataReader.Close();
                
                return false;
            }
            
            homework = dataReader.GetString(ordinal);
        }
        else
        {
            throw new Exception();
        }
        
        dataReader.Close();

        return true;
    }
    
    public bool TryGetCourseNextLessonTime(out DateTime nextLesson, int courseId)
    {
        string sqlRequest = $"SELECT next_lesson FROM courses WHERE id = {courseId}";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);
        
        command.ExecuteNonQuery();
        
        NpgsqlDataReader dataReader = command.ExecuteReader();
        
        nextLesson = new DateTime();
        
        if (!dataReader.HasRows)
        {
            dataReader.Close();
            
            return false;
        }

        if (dataReader.Read())
        {
            int ordinal = dataReader.GetOrdinal("next_lesson");
            
            if (TableHelpMethods.IsOrdinalValueNull(dataReader, ordinal))
            {
                dataReader.Close();
                
                return false;
            }

            nextLesson = dataReader.GetDateTime(ordinal);
        }
        else
        {
            throw new Exception();
        }

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
        
        if (dataReader.Read())
        {
            int id = dataReader.GetInt32(dataReader.GetOrdinal("id"));
            string courseName = dataReader.GetString(dataReader.GetOrdinal("course_name"));
            string studentInviteCode = dataReader.GetString(dataReader.GetOrdinal("student_invite_code"));
            int teacherId = dataReader.GetInt32(dataReader.GetOrdinal("teacher_id"));
            
            course = new Course(id, courseName, studentInviteCode, teacherId);
        }
        else
        {
            throw new Exception();
        }

        dataReader.Close();

        return true;
    }
    
    public bool IsCourseNameUnique(string name)
    {
        return !IsExists(name, "course_name");
    }
    
    public bool IsStudentNameInCourseStudentGroupUnique(string name, int courseId)
    {
        string sqlRequest = $"SELECT EXISTS(SELECT * FROM students WHERE course_id = {courseId} AND name = '{name}')";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();

        bool isExist;
        
        if (dataReader.Read())
        {
            isExist = dataReader.GetBoolean(dataReader.GetOrdinal("exists"));
        }
        else
        {
            throw new Exception();
        }
        
        dataReader.Close();

        return !isExist;
    }
    
    public bool IsCourseInviteCodeUnique(string courseName)
    {
        return !IsExists(courseName, "student_invite_code");
    }
    
    private bool IsExists(string value, string tableName)
    {
        string sqlRequest = $"SELECT EXISTS(SELECT * FROM courses WHERE {tableName} = '{value}')";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();

        bool isExist;
        
        if (dataReader.Read())
        {
            isExist = dataReader.GetBoolean(dataReader.GetOrdinal("exists"));
        }
        else
        {
            throw new Exception();
        }
        
        dataReader.Close();

        return isExist;
    }
}