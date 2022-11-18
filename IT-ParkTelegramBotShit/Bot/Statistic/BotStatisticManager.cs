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
}