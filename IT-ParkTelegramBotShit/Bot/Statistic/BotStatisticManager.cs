using NLog;

public class BotStatisticManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private static BotStatisticManager _statisticManager;
    
    private BotStatisticManager()
    {
        
    }
    
    public static BotStatisticManager GetInstance()
    {
        if (_statisticManager == null)
        {
            _statisticManager = new BotStatisticManager();
        }
        
        return _statisticManager;
    }
    
    // todo make this class
    
    /*
     * Класс не обязателен, но его можно реализовать для большей безопасности отправки сообщений
     * Он будет менять переменную, отвечащую за задержку между отправкой сообщений
     * Большая нагрузка => большая задержка
     * Малая нагрузка => маленькая задержка, или без задержек
     */
}