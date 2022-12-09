using IT_ParkTelegramBotShit.DataBase.Entities;
using NLog;
using Npgsql;

namespace IT_ParkTelegramBotShit.DataBase.Tables;

public class TableStudents
{
    private NpgsqlConnection _connection;

    public TableStudents(NpgsqlConnection connection)
    {
        _connection = connection;
    }
    
    public List<long> GetAllStudentsChatId()
    {
        List<long> studentsChatId = new List<long>();

        string sqlRequest = $"SELECT chat_id FROM students";

        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();

        while (dataReader.Read())
        {
            int ordinal = dataReader.GetOrdinal("chat_id");

            bool isNull = dataReader.GetValue(ordinal) == DBNull.Value;

            if (isNull)
            {
                continue;
            }
            
            long chatId = dataReader.GetInt64(ordinal);
            
            studentsChatId.Add(chatId);
        }

        dataReader.Close();

        return studentsChatId;
    }

    public bool TryGetStudentByChatId(out Student student, long chatId)
    {
        string sqlRequest = $"SELECT * FROM students WHERE chat_id = {chatId}";

        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();

        if (!dataReader.HasRows)
        {
            student = new Student();

            return false;
        }

        if (dataReader.Read())
        {
            long chat_id = dataReader.GetInt64(dataReader.GetOrdinal("chat_id"));
            int course_id = dataReader.GetInt32(dataReader.GetOrdinal("course_id"));
            string name = dataReader.GetString(dataReader.GetOrdinal("name"));

            student = new Student(chat_id, course_id, name);
            
            dataReader.Close();
            
            return true;
        }
        else
        {
            dataReader.Close();
            
            throw new Exception();
        }
    }
    
    public void SetStudent(Student student)
    {
        string sqlRequest = $"INSERT INTO students (chat_id, course_id, name) VALUES ({student.ChatId}, {student.CourseId}, '{student.Name}')";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        command.ExecuteNonQuery();
    }
    
    public void DeleteStudentByChatId(long chatId)
    {
        string sqlRequest = $"DELETE FROM students WHERE chat_id = {chatId}";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        command.ExecuteNonQuery();
    }
    
    public void SetStudent(long chatId, int courseId, string name)
    {
        string sqlRequest = $"INSERT INTO students (chat_id, course_id, name) VALUES ({chatId}, {courseId}, '{name}')";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        command.ExecuteNonQuery();
    }
    
    private void UpdateNameByChatId(int chatId, string name)
    {
        string sqlRequest = $"UPDATE students set name = '{name}' where chat_id = {chatId}";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        command.ExecuteNonQuery();
    }
    
    public void CreateStudentAccount(long chatId, int courseId, string name)
    {
        // todo проверить в будущем (приоритет: высокий)
        
        string sqlRequest = $"INSERT INTO students (chat_id, course_id, name) VALUES ({chatId}, {courseId}, '{name}')";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        command.ExecuteNonQuery();
    }
}