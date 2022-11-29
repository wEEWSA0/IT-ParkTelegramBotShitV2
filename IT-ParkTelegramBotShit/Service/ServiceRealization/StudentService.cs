using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Bot.Buttons;
using IT_ParkTelegramBotShit.DataBase;
using IT_ParkTelegramBotShit.DataBase.Entities;
using IT_ParkTelegramBotShit.Router.Transmitted;
using IT_ParkTelegramBotShit.Util;
using NLog;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Service.ServiceRealization;

public class StudentService
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private Dictionary<string, Func<long, TransmittedData, MessageToSend>>
        _requestMethodsPairs;

    public StudentService()
    {
        _requestMethodsPairs = new Dictionary<string, Func<long, TransmittedData, MessageToSend>>();
    }
    
    public MessageToSend ProcessButtonHomework(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Student.Homework;
        
        if (!transmittedData.DataStorage.TryGet(ConstantsStorage.StudentCourseId, out object courseId))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessButtonHomework"));
        }

        if (!DbManager.GetInstance().TableCourses.TryGetCourseHomework(out string homework, (int)courseId))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessButtonHomework"));
        }

        response += "\n" + homework;
        
        InlineKeyboardMarkup keyboard = BotKeyboardCreator
            .GetInstance().GetKeyboardMarkup(ReplyButtonsStorage.MainMenu, ReplyButtonsStorage.Student.NextLesson);
        
        return new MessageToSend(response, keyboard, false);
    }
    
    public MessageToSend ProcessButtonNextLesson(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Student.NextLesson;
        
        if (!transmittedData.DataStorage.TryGet(ConstantsStorage.StudentCourseId, out object courseId))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessButtonNextLesson"));
        }

        if (!DbManager.GetInstance().TableCourses.TryGetCourseNextLessonTime(out DateTime date, (int)courseId))
        {
            response += ReplyTextsStorage.Student.NextLessonNotAssigned;
        }
        else
        {
            response += "\n" + date;
        }

        InlineKeyboardMarkup keyboard = BotKeyboardCreator
            .GetInstance().GetKeyboardMarkup(ReplyButtonsStorage.MainMenu, ReplyButtonsStorage.Student.Homework);
        
        return new MessageToSend(response, keyboard, false);
    }
    
    public MessageToSend ProcessButtonMainMenu(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.MainMenu;

        InlineKeyboardMarkup keyboard = ReplyKeyboardsStorage.Student.MainMenu;
        
        transmittedData.State.StudentState = States.StudentStates.MainMenu;
        
        return new MessageToSend(response, keyboard, false);
    }
}