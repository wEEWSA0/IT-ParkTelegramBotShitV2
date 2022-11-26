using IT_ParkTelegramBotShit.Bot.Notifications;
using IT_ParkTelegramBotShit.DataBase;
using IT_ParkTelegramBotShit.DataBase.Entities;
using IT_ParkTelegramBotShit.Router;
using IT_ParkTelegramBotShit.Router.Transmitted;
using IT_ParkTelegramBotShit.Util;
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

        ChatsRouter chatsRouter = new ChatsRouter();
        BotRequestHandlers botRequestHandlers = new BotRequestHandlers(chatsRouter);

        _botClient.StartReceiving(
            botRequestHandlers.HandleUpdateAsync,
            botRequestHandlers.HandlePollingErrorAsync,
            receiverOptions,
            _cancellationTokenSource.Token
        );
        
        Logger.Debug("Выполнена инициализация ReceiverOptions и BotRequestHandlers и выполнен TelegramBotClient StartReceiving");
        
        SetupNotifications(chatsRouter);

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

    private void SetupNotifications(ChatsRouter chatsRouter)
    {
        var teachersChatId = DbManager.GetInstance().TableTeachers.GetAllTeachersChatId();
        
        AuthorizationTeachers(teachersChatId, chatsRouter);
        
        BotNotificationSystem.GetInstance().StartNotificationSystem(60000);
    }

    private async void AuthorizationTeachers(List<long> teacherChatIdList, ChatsRouter chatsRouter)
    {
        var firstMessage = new MessageToSend(ReplyTextsStorage.OfficialITParkBot);

        foreach (var chatId in teacherChatIdList)
        {
            await BotNotificationSender.GetInstance().SendAnchoredNotificationMessage(firstMessage, chatId);

            AuthorizeTeacher(chatId, chatsRouter.GetUserTransmittedData(chatId));
            var mainMenuMessage = GetAuthorizedTeacherMessage();
            await BotNotificationSender.GetInstance().SendNotificationMessage(mainMenuMessage, chatId);
        }
    }

    private void AuthorizeTeacher(long chatId, TransmittedData transmittedData)
    {
        transmittedData.State.GlobalState = States.GlobalStates.Other;
        transmittedData.State.TeacherState = States.TeacherStates.MainMenu;

        if (!DbManager.GetInstance().TableTeachers.TryGetTeacherByChatId(out Teacher teacher, chatId))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("AuthorizeTeacher"));
            throw new Exception();
        }
        
        transmittedData.DataStorage.Add(ConstantsStorage.TeacherId, teacher.Id);
    }

    private MessageToSend GetAuthorizedTeacherMessage()
    {
        string response = ReplyTextsStorage.MainMenu;
        var keyboard = ReplyKeyboardsStorage.Teacher.MainMenu;

        return new MessageToSend(response, keyboard);
    }
}