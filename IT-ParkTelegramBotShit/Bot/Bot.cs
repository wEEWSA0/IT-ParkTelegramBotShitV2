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
    private const int CheckNotificationsDelay = 60000;
    private const int CheckStatisticDelay = 10000;
    
    // todo решить проблему с дублированием закрепленного сообщения (приоритет: средний)
    // Пусть сразу посылается сообщение с вводом кода (приходится писать /start)
    
    private TelegramBotClient _botClient;
    private CancellationTokenSource _cancellationTokenSource;

    public Bot()
    {
        _botClient = new TelegramBotClient("5655889814:AAEdVK___v3pjwLwxycXJaYsFtdFnMOvMyA");
        _cancellationTokenSource = new CancellationTokenSource();

        var botResponder = new BotResponder(_botClient, _cancellationTokenSource);
        
        if (!BotMessageManager.Create(botResponder))
        {
            Logger.Error("Problems with BotMessageManager.Create");
            throw new Exception("Not working, error");
        }
        
        if (!BotNotificationSender.Create(botResponder))
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
        BotStatisticManager.GetInstance().StartCollectStatistic(CheckStatisticDelay);

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
        
        var studentsChatId = DbManager.GetInstance().TableStudents.GetAllStudentsChatId();
        AuthorizationStudents(studentsChatId, chatsRouter);
        /*
        // test
        MessageToSend mes = new MessageToSend($"Проверка {DateTime.Now}");
        var date = DateTime.Now;
        date = date.AddMinutes(1);
        Notification not = new Notification(mes, DateTime.Now, date, NotificationType.ExpiredRegular);
        not.AddReciever(1036970909);
        
        BotNotificationSystem.GetInstance().AddNotification(not);
        */
        BotNotificationSystem.GetInstance().StartNotificationSystem(CheckNotificationsDelay);
    }

    #region Authorization

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
    
    private async void AuthorizationStudents(List<long> studentsChatIdList, ChatsRouter chatsRouter)
    {
        var firstMessage = new MessageToSend(ReplyTextsStorage.OfficialITParkBot);

        foreach (var chatId in studentsChatIdList)
        {
            await BotNotificationSender.GetInstance().SendAnchoredNotificationMessage(firstMessage, chatId);

            AuthorizeStudent(chatId, chatsRouter.GetUserTransmittedData(chatId));
            var mainMenuMessage = GetAuthorizedStudentMessage();
            await BotNotificationSender.GetInstance().SendNotificationMessage(mainMenuMessage, chatId);
        }
    }
    
    private void AuthorizeStudent(long chatId, TransmittedData transmittedData)
    {
        transmittedData.State.GlobalState = States.GlobalStates.Other;
        transmittedData.State.StudentState = States.StudentStates.MainMenu;
        
        if (!DbManager.GetInstance().TableStudents.TryGetStudentByChatId(out Student student, chatId))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("AuthorizeStudent"));
            throw new Exception();
        }
        
        transmittedData.DataStorage.Add(ConstantsStorage.StudentCourseId, student.CourseId);
    }

    private MessageToSend GetAuthorizedStudentMessage()
    {
        string response = ReplyTextsStorage.MainMenu;
        var keyboard = ReplyKeyboardsStorage.Student.MainMenu;

        return new MessageToSend(response, keyboard);
    }

    #endregion
}