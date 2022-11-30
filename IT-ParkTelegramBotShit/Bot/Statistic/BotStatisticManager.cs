using NLog;

public class BotStatisticManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private static BotStatisticManager _statisticManager;

    private const int CheckTime = 10000;
    private int _workLoad;
    public int SleepValue { get; private set; }

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

    public void AddWorkLoad()
    {
        AddWorkLoad(1);
    }
    
    public void AddWorkLoad(int value)
    {
        _workLoad += value;
    }
    
    private void UpdateSleepValue()
    {
        // в зависимости от workLoad будет задаваться значение SleepValue
        // SleepValue связан с ConstantsStorage.ThreadSleepBetweenSendMessages

        _workLoad = 0;
    }
    
    /*
     * Класс не обязателен, но его можно реализовать для большей безопасности отправки сообщений
     * Он будет менять переменную, отвечащую за задержку между отправкой сообщений
     * Большая нагрузка => большая задержка
     * Малая нагрузка => маленькая задержка, или без задержек
     */
}