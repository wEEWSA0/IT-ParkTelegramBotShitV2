using IT_ParkTelegramBotShit.DataBase.Entities;
using Npgsql;

namespace IT_ParkTelegramBotShit.DataBase.Tables;

public class TableTeachers
{
    private NpgsqlConnection _connection;

    public TableTeachers(NpgsqlConnection connection)
    {
        _connection = connection;
    }
    
    public List<Teacher> GetTeachers() // не используется
    {
        List<Teacher> teachers = new List<Teacher>();

        string sqlRequest = $"SELECT name, invite_code FROM teachers";

        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();

        while (dataReader.Read())
        {
            string name = dataReader.GetString(dataReader.GetOrdinal("name"));
            string code = dataReader.GetString(dataReader.GetOrdinal("invite_code"));

            // NOT WORKING
            //teachers.Add(new Teacher(name, code));
        }

        dataReader.Close();

        return teachers;
    }

    public bool TryGetTeacherByChatId(out Teacher teacher, long chatId)
    {
        string sqlRequest = $"SELECT * FROM teachers WHERE chat_id = {chatId}";

        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();

        if (!dataReader.HasRows)
        {
            teacher = new Teacher();
            
            dataReader.Close();
            
            return false;
        }
        
        int id = dataReader.GetInt32(dataReader.GetOrdinal("id"));
        string name = dataReader.GetString(dataReader.GetOrdinal("name"));
        string code = dataReader.GetString(dataReader.GetOrdinal("invite_code"));

        teacher = new Teacher(id, name, chatId, code);
        
        dataReader.Close();

        return true;
    }
    
    public bool TryJoinTeacherAccountByInviteCode(out Teacher teacher, long chatId, string inviteCode)
    {
        teacher = new Teacher();
        
        if (!IsCorrectInviteCode(inviteCode))
        {
            return false;
        }

        string sqlRequest = $"SELECT * FROM teachers WHERE invite_code = '{inviteCode}'";

        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();
        
        if (!dataReader.HasRows)
        {
            dataReader.Close();
            
            return false;
        }

        while (dataReader.Read())
        {
            int id = dataReader.GetInt32(dataReader.GetOrdinal("id"));
            string name = dataReader.GetString(dataReader.GetOrdinal("name"));
            string code = dataReader.GetString(dataReader.GetOrdinal("invite_code"));
            
            teacher = new Teacher(id, name, chatId, code);
        
            break;
        }
        
        dataReader.Close();

        UpdateChatIdByTeacherId(teacher.Id, chatId);
        
        return true;
    }

    private void UpdateChatIdByTeacherId(int id, long chatId)
    {
        string sqlRequest = $"UPDATE teachers set chat_id = {chatId} where id = {id}";
        
        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        command.ExecuteNonQuery();
    }

    private bool IsCorrectInviteCode(string inviteCode)
    {
        string sqlRequest = $"SELECT invite_code FROM teachers";

        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();

        while (dataReader.Read())
        {
            string code = dataReader.GetString(dataReader.GetOrdinal("invite_code"));

            if (inviteCode == code)
            {
                dataReader.Close();
                
                return true;
            }
        }

        dataReader.Close();

        return false;
    }
}