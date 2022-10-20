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

/*
    public bool AddNew(Teacher teacher)
    {
        if (!CheckInviteCodeUnique(teacher))
        {
            return false;
        }

        string sqlRequest = $"INSERT INTO Teachers (name, invite_code) VALUES ('{teacher.Name}', '{teacher.InviteCode}')";

        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        command.ExecuteNonQuery();

        return true;
    }
*/
    public List<Teacher> GetTeachers() // не используется
    {
        List<Teacher> teachers = new List<Teacher>();

        string sqlRequest = $"SELECT name, invite_code FROM teachers"; // $"SELECT * FROM teachers WHERE id={findId}" 

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

    public Teacher TryGetTeacherByChatId(out bool isEnabled, long chatId)
    {
        isEnabled = true;

        string sqlRequest = $"SELECT * FROM teachers WHERE chat_id = {chatId}";

        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();

        if (!dataReader.HasRows)
        {
            isEnabled = false;

            return new Teacher();
        }
        
        int id = dataReader.GetInt32(dataReader.GetOrdinal("id"));
        string name = dataReader.GetString(dataReader.GetOrdinal("name"));
        string code = dataReader.GetString(dataReader.GetOrdinal("invite_code"));

        Teacher teacher = new Teacher(id, name, chatId, code);
        
        dataReader.Close();

        return teacher;
    }
    
    public Teacher TryJoinTeacherAccountByInviteCode(out bool isEnabled, long chatId, string inviteCode)
    {
        isEnabled = true;

        if (!IsCorrectInviteCode(inviteCode))
        {
            isEnabled = false;

            return new Teacher();
        }

        string sqlRequest = $"SELECT * FROM teachers WHERE invite_code = '{inviteCode}'";

        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();

        int id = dataReader.GetInt32(dataReader.GetOrdinal("id"));
        string name = dataReader.GetString(dataReader.GetOrdinal("name"));
        string code = dataReader.GetString(dataReader.GetOrdinal("invite_code"));

        Teacher teacher = new Teacher(id, name, chatId, code);
        
        UpdateChatIdByTeacherId(id, chatId);
        
        dataReader.Close();

        return teacher;
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

    /*private bool CheckInviteCode(Teacher teacher)
    {
        string sqlRequest = $"SELECT invite_code FROM teachers";

        NpgsqlCommand command = new NpgsqlCommand(sqlRequest, _connection);

        NpgsqlDataReader dataReader = command.ExecuteReader();

        while (dataReader.Read())
        {
            string code = dataReader.GetString(dataReader.GetOrdinal("invite_code"));

            if (teacher.InviteCode == code)
            {
                dataReader.Close();

                return false;
            }
        }

        dataReader.Close();

        return true;
    }*/
}