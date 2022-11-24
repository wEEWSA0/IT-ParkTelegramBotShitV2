using IT_ParkTelegramBotShit.Bot.Notifications;
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
        _botClient = new TelegramBotClient("5655889814:AAEdVK___v3pjwLwxycXJaYsFtdFnMOvMyA");
        _cancellationTokenSource = new CancellationTokenSource();

        if (!BotMessageManager.Create(_botClient, _cancellationTokenSource))
        {
            Logger.Error("Problems with BotMessageManager.Create");
            throw new Exception("Not working, error");
        }
        
        if (!BotNotificationSender.Create(_botClient, _cancellationTokenSource))
        {
            Logger.Error("Problems with BotNotificationSender.Create");
            throw new Exception("Not working, error");
        }
        
        Logger.Debug("Выполнена инициализация TelegramBotClient");
    }

    public void Start()
    {
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
        
        Logger.Debug("Выполнена инициализация ReceiverOptions и BotRequestHandlers и выполнен TelegramBotClient StartReceiving");
        
        SetupNotifications();
        
        Logger.Info("Выполнен запуск бота");
    }

    public string GetBotName()
    {
        string? userName = _botClient.GetMeAsync().Result.Username;
        
        if (userName != null)
        {
            Logger.Debug("Выполнено получение имени бота");
        }
        else
        {
            Logger.Warn("Ошибка получение имени бота");
            userName = "";
        }
        
        return userName;
    }

    public async void Stop()
    {
        Logger.Debug("Начата остановка бота");
        BotNotificationSystem.GetInstance().StopNotificationSystem();

        var keyList = BotMessageManager.GetInstance().GetAllHistoryKeys();

        Logger.Debug("Подготовка к остановке бота");
        
        for (int i = 0; i < keyList.Count; i++)
        {
            await BotMessageManager.GetInstance().GetHistory(keyList[i]).DeleteAllMessages();
        }

        _cancellationTokenSource.Cancel();
        Logger.Info("Выполнена остановка бота");

        Console.WriteLine("Press any button to finished");
    }

    private void SetupNotifications()
    {
        // var message = new MessageToSend("Как ваши дела?");
        //
        // DateTime date = DateTime.Now;
        //
        // var notification = new Notification(message, date);
        //
        // for (int i = 0; i < 10; i++)
        //     notification.AddReciever(247021014);
        //
        // BotNotificationSystem.GetInstance().AddNotification(notification);
        
        BotNotificationSystem.GetInstance().StartNotificationSystem(60000);
    }
}