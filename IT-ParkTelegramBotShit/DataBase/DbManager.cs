using IT_ParkTelegramBotShit.DataBase.Connection;
using IT_ParkTelegramBotShit.DataBase.Tables;
using Npgsql;

namespace IT_ParkTelegramBotShit.DataBase;

public class DbManager
{
    public TableTeachers TableTeachers { get; private set; }
    public TableStudents TableStudents { get; private set; }
    public TableCourses TableCourses { get; private set; }

    public DbManager()
    {
        NpgsqlConnection connection = DbConnector.GetInstance().Connection;

        TableTeachers = new TableTeachers(connection);
        TableStudents = new TableStudents(connection);
        TableCourses = new TableCourses(connection); // todo возможно переделать структуру
    }

    private static DbManager _dbManager = null;

    public static DbManager GetInstance()
    {
        if (_dbManager == null)
        {
            _dbManager = new DbManager();
        }

        return _dbManager;
    }
}