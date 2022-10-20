using NLog;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace IT_ParkTelegramBotShit.Bot;

public class Bot
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;

    public Bot()
    {
        Logger.Info("Старт инициализации TelegramBotClient");

        _botClient = new TelegramBotClient("5655889814:AAEdVK___v3pjwLwxycXJaYsFtdFnMOvMyA");
        _cancellationTokenSource = new CancellationTokenSource();

        Logger.Info("Выполнена инициализация TelegramBotClient");
    }

    public void Start()
    {
        Logger.Info("Старт инициализации ReceiverOptions и BotRequestHandlers");
        ReceiverOptions receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        BotRequestHandlers botRequestHandlers = new BotRequestHandlers();

        _botClient.StartReceiving(
            botRequestHandlers.HandleUpdateAsync,
            botRequestHandlers.HandlePollingErrorAsync,
            receiverOptions,
            _cancellationTokenSource.Token
        );

        Logger.Info("Выполнена инициализация ReceiverOptions и BotRequestHandlers и выполнен TelegramBotClient StartReceiving");
    }

    public string GetBotName()
    {
        Logger.Info("Старт получения имени бота");
        string? userName = _botClient.GetMeAsync().Result.Username;
        
        if (userName != null)
        {
            Logger.Info("Выполнено получение имени бота");
        }
        else
        {
            Logger.Error("Ошибка получение имени бота");
        }
        
        return userName;
    }

    public void Stop()
    {
        Logger.Info("Старт остановки бота");
        _cancellationTokenSource.Cancel();
        Logger.Info("Выполнено остановка бота");
    }   
}