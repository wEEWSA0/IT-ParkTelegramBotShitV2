namespace IT_ParkTelegramBotShit.Bot;

public class BotsManager
{
    private static BotsManager _botsManager;
    private List<Bot> _bots;

    private BotsManager()
    {
        _bots = new List<Bot>();
    }

    public static BotsManager GetInstance()
    {
        if (_botsManager == null)
        {
            _botsManager = new BotsManager();
        }

        return _botsManager;
    }
    
    public void AddBot(Bot bot)
    {
        _bots.Add(bot);
    }
    
    public void AddBotList(List<Bot> botList)
    {
        _bots.AddRange(botList);
    }

    /*public void Start()
    {
        foreach (var bot in _bots)
        {
            bot.Start();
            
            Console.WriteLine($"Bot @{bot.GetBotName()} started");
        }
    }*/
    
    public void Stop()
    {
        foreach (var bot in _bots)
        {
            bot.Stop();
            
            Console.WriteLine($"Bot @{bot.GetBotName()} stopped");
        }
    }
}