using IT_ParkTelegramBotShit.Util;
using NLog;

public class BotStatisticManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private static BotStatisticManager _statisticManager;
    private const int StandartThreadSleepBetweenSendMessages = 160;
    private const int StandartMessageWeight = 4;

    private Task _collectingStatistic;
    private bool _isStarted;
    
    private int _workLoad; // todo add ban for spam (on 1 hour for example)
    private int _messageWeight;
    private int _minSleepValue;
    public int SleepValue { get; private set; }

    private BotStatisticManager()
    {
        SleepValue = StandartThreadSleepBetweenSendMessages;
    }
    
    public static BotStatisticManager GetInstance()
    {
        if (_statisticManager == null)
        {
            _statisticManager = new BotStatisticManager();
            Logger.Debug("BotStatisticManager is initialized");
        }
        
        return _statisticManager;
    }
    
    public void AddWorkLoad()
    {
        AddWorkLoad(1);
    }
    
    public void AddWorkLoad(int value)
    {
        _workLoad += value;
    }
    
    public void StartCollectStatistic(int checkRateInMiliseconds)
    {
        if (_isStarted)
        {
            Logger.Warn("Statistic already started collecting");
            return;
        }

        _messageWeight = StandartMessageWeight;
        _minSleepValue = StandartThreadSleepBetweenSendMessages / 2;
        _isStarted = true;

        if (_collectingStatistic != null)
        {
            if (_collectingStatistic.Status != TaskStatus.RanToCompletion)
            {
                Logger.Error("BotStatisticManager has some undeifined errors");
                return;
            }
        }

        _collectingStatistic = CheckingStatistic(checkRateInMiliseconds);
        
        _collectingStatistic.Start();
    }
    public void StartCollectStatistic(int checkRateInMiliseconds, int messageWeight)
    {
        _messageWeight = messageWeight;
        StartCollectStatistic(checkRateInMiliseconds);
    }
    public void StartCollectStatistic(int checkRateInMiliseconds, int messageWeight, int minSleepValue)
    {
        StartCollectStatistic(checkRateInMiliseconds, messageWeight);
        _minSleepValue = minSleepValue;
    }

    public void StopCollectStatistic()
    {
        if (!_isStarted)
        {
            Logger.Warn("Statistic already stopped or not started collecting");
            return;
        }

        _isStarted = false;
    }
    
    private Task CheckingStatistic(int checkRateInMiliseconds)
    {
        return new Task(() =>
        {
            Logger.Debug("CheckingStatistic started");
            
            while (_isStarted)
            {
                UpdateSleepValue(_messageWeight, _minSleepValue, checkRateInMiliseconds);
                Thread.Sleep(checkRateInMiliseconds);
            }

            Logger.Debug("CheckingStatistic finished");
        });
    }
    
    private void UpdateSleepValue(int messageWeight, int minSleepValue, int checkRate)
    {
        var newSleep = minSleepValue;
        
        newSleep += _workLoad * messageWeight;

        Logger.Debug($"Нагрузка: {_workLoad} сообщений за {Math.Round(checkRate / 1000f)} секунд");

        SleepValue = newSleep;
        _workLoad = 0;
    }
    
    /*
     * Класс для большей безопасности отправки сообщений типа "уведомление"
     * Он меняет переменную, отвечащую за задержку между отправкой сообщений
     * Большая нагрузка => большая задержка
     * Малая нагрузка => маленькая задержка
     */
}