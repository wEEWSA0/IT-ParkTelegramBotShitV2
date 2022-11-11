using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Bot.Buttons;
using IT_ParkTelegramBotShit.DataBase;
using IT_ParkTelegramBotShit.DataBase.Entities;
using IT_ParkTelegramBotShit.Router.Auxiliary;
using IT_ParkTelegramBotShit.Util;
using NLog;
using NLog.Fluent;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Service.ServiceRealization;

public class TeacherService
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    //private TeacherCallbackQueryStorage _teacherCallbacks = CallbackQueryStorage.Teacher;
    private Dictionary<string, Func<long, TransmittedData, string, MessageToSend>>
        _requestMethodsPairs;
    
    public TeacherService()
    {
        _requestMethodsPairs = new Dictionary<string, Func<long, TransmittedData, string, MessageToSend>>();

        _requestMethodsPairs[CallbackQueryStorage.Teacher.Groups] = ProcessGroups;
    }
    
    public MessageToSend ProcessMainMenu(long chatId, TransmittedData transmittedData, string request)
    {
        if (!_requestMethodsPairs.ContainsKey(request))
        {
            Logger.Error("Not found methods to process MainMenu");
            throw new NotImplementedException();
        }

        return _requestMethodsPairs[request].Invoke(chatId, transmittedData, request);
    }
    
    private MessageToSend ProcessGroups(long chatId, TransmittedData transmittedData, string request)
    {
        string response = ReplyTextsStorage.Groups;
        
        InlineKeyboardMarkup keyboard;

        if (!transmittedData.DataStorage.TryGet(ConstantsStorage.TeacherId, out object obj))
        {
            Logger.Error("Хочу спать");
        }

        int teacherId = (int)obj;

        if (DbManager.GetInstance().TableCourses.TryGetTeacherCourses(out List<Course> courses, teacherId))
        {
            keyboard = BotKeyboardCreator.GetInstance()
                .GetKeyboardMarkup(ReplyButtonsStorage.Teacher.CreateGroup, ReplyButtonsStorage.Teacher.EditGroup);
        }
        else
        {
            keyboard = BotKeyboardCreator.GetInstance()
                .GetKeyboardMarkup(ReplyButtonsStorage.Teacher.CreateGroup);
        }
        // todo Добавить кнопку "В главное меню"
        transmittedData.State.TeacherState = States.TeacherStates.Groups;
        
        return new MessageToSend(response, keyboard, false);
    }
}