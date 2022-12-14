using Npgsql;

namespace IT_ParkTelegramBotShit.DataBase.Tables;

public class TableHelpMethods
{
    public static bool IsOrdinalValueNull(NpgsqlDataReader dataReader, int ordinal)
    {
        bool isNull = dataReader.GetValue(ordinal) == DBNull.Value;

        return isNull;
    }
}