using Npgsql;

namespace IT_ParkTelegramBotShit.DataBase.Connection;

public class DbConnector
{
    private const string _connectionString =
        "Host=194.67.105.79;Username=itparktelegrambot_user;Password=alexfuckkids;Database=itparktelegrambot_db";

    public NpgsqlConnection Connection { private set; get; }

    private DbConnector()
    {
        Connection = new NpgsqlConnection(_connectionString);
        Connection.Open();
    }

    private static DbConnector _dbConnector = null;
    
    public static DbConnector GetInstance() // параметры
    {
        if (_dbConnector == null)
        {
            _dbConnector = new DbConnector(); // todo List для Connection
        }

        return _dbConnector;
    }
}