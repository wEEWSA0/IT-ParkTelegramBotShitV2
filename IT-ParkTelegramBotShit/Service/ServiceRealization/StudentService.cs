using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Bot.Buttons;
using IT_ParkTelegramBotShit.DataBase;
using IT_ParkTelegramBotShit.DataBase.Entities;
using IT_ParkTelegramBotShit.Router.Transmitted;
using IT_ParkTelegramBotShit.Util;
using NLog;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Service.ServiceRealization;

public class StudentService // TODO GetInstance classes in _var
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    #region InputAndChooseMethods

    public MessageToSend ProcessInputName(long chatId, TransmittedData transmittedData, string request)
    {
        if (!transmittedData.DataStorage.TryGet(ConstantsStorage.StudentCourseId, out Object courseIdObj))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessInputName"));
        }

        int courseId = (int)courseIdObj;
        
        var name = InputName(request, courseId);

        DbManager.GetInstance().TableStudents.CreateStudentAccount(chatId, courseId, name);
        
        var response = ReplyTextsStorage.MainMenu;
        var keyboard = ReplyKeyboardsStorage.Student.MainMenu;

        transmittedData.State.StudentState = States.StudentStates.MainMenu;

        var logIntoAccountMessageToSend = new MessageToSend(ReplyTextsStorage.Student.LogIntoAccount, false);
        
        BotMessageManager.GetInstance().GetSender(chatId).AddMessageToStack(logIntoAccountMessageToSend);

        return new MessageToSend(response, keyboard);
    }

    public MessageToSend ProcessInputNewName(long chatId, TransmittedData transmittedData, string request)
    {
        if (!transmittedData.DataStorage.TryGet(ConstantsStorage.StudentCourseId, out Object courseIdObj))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessInputNewName"));
        }
        
        int courseId = (int)courseIdObj;
        
        var name = InputName(request, courseId);
        
        transmittedData.DataStorage.Add(ConstantsStorage.StudentName, name);
        
        transmittedData.State.StudentState = States.StudentStates.ChangeNameFinalStep;

        return GetReplyFinalStepMessageToSend(ReplyTextsStorage.Student.GetNewNameView(name));
    }

    #endregion
    #region ButtonsMethods

    public MessageToSend ProcessButtonYes(long chatId, TransmittedData transmittedData)
    {
        var state = transmittedData.State;      //вызываем состояния
        var storage = transmittedData.DataStorage;      //вызываем storage 
        MessageToSend messageToSend;
        
        switch (transmittedData.State.StudentState)
        {
            case States.StudentStates.ChangeNameFinalStep:
            {
                var tableStudents = DbManager.GetInstance().TableStudents;
                
                bool isNewNameEnab = storage.TryGet(ConstantsStorage.StudentName, out Object nameObj);
        
                if (!isNewNameEnab)
                {
                    Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessButtonYes"));
                }

                string name = (string)nameObj;
                tableStudents.UpdateNameByChatId(chatId, name);
                
                messageToSend = new MessageToSend(ReplyTextsStorage.AppliedChanges, false);
            }
                break;
            case States.StudentStates.QuitAccountFinalStep:
            {
                var tableStudents = DbManager.GetInstance().TableStudents;
                
                tableStudents.DeleteStudentByChatId(chatId);
        
                transmittedData.State.Reset();
        
                var quitAccountMessageToSend = new MessageToSend(ReplyTextsStorage.Student.QuitAccount);
            
                BotMessageManager.GetInstance().GetSender(chatId).AddMessageToStack(quitAccountMessageToSend);
                
                StartedService.Logout(chatId);

                return new MessageToSend(ReplyTextsStorage.CmdStart);
            }
                break;
            default:
            {
                Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessButtonYes"));
                throw new Exception();
            }
        }
        
        BotMessageManager.GetInstance().GetSender(chatId).AddMessageToStack(messageToSend);
                
        state.StudentState = States.StudentStates.MainMenu;
        
        string response = ReplyTextsStorage.MainMenu;
        var keyboard = ReplyKeyboardsStorage.Student.MainMenu;
        
        return new MessageToSend(response, keyboard);
    }
    
    public MessageToSend ProcessButtonNo(long chatId, TransmittedData transmittedData)
    {
        var state = transmittedData.State;
        MessageToSend messageToSend;
        
        switch (transmittedData.State.StudentState)
        {
            case States.StudentStates.ChangeNameFinalStep:
            case States.StudentStates.QuitAccountFinalStep:
            {
                messageToSend = new MessageToSend(ReplyTextsStorage.DeclinedChanges, false);
            }
                break;
            default:
            {
                Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessButtonNo"));
                throw new Exception();
            }
        }
        
        BotMessageManager.GetInstance().GetSender(chatId).AddMessageToStack(messageToSend);
                
        state.StudentState = States.StudentStates.MainMenu;
        
        string response = ReplyTextsStorage.MainMenu;
        var keyboard = ReplyKeyboardsStorage.Student.MainMenu;
        
        return new MessageToSend(response, keyboard);
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
            response = ReplyTextsStorage.Student.HomeworkNotAssigned;
        }
        else
        {
            response += "\n" + homework;
        }

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
            response = ReplyTextsStorage.Student.NextLessonNotAssigned;
        }
        else
        {
            response += "\n" + date;
        }

        InlineKeyboardMarkup keyboard = BotKeyboardCreator
            .GetInstance().GetKeyboardMarkup(ReplyButtonsStorage.MainMenu, ReplyButtonsStorage.Student.Homework);
        
        return new MessageToSend(response, keyboard, false);
    }
    
    public MessageToSend ProcessButtonProfile(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Student.Profile;

        InlineKeyboardMarkup keyboard = ReplyKeyboardsStorage.Student.Profile;

        if (!DbManager.GetInstance().TableStudents.TryGetStudentByChatId(out Student student, chatId))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessButtonProfile"));
        }

        response += "\n" + $"Ученик: {student.Name}";
        
        return new MessageToSend(response, keyboard, false);
    }
    
    public MessageToSend ProcessButtonChangeName(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Student.InputName;
        
        transmittedData.State.StudentState = States.StudentStates.InputNewName;
        
        return new MessageToSend(response, false);
    }
    
    public MessageToSend ProcessButtonQuitAccount(long chatId, TransmittedData transmittedData)
    {
        transmittedData.State.StudentState = States.StudentStates.QuitAccountFinalStep;

        return GetReplyFinalStepMessageToSend(ReplyTextsStorage.Student.QuitAccountFinalStep);
    }
    
    public MessageToSend ProcessButtonMainMenu(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.MainMenu;

        InlineKeyboardMarkup keyboard = ReplyKeyboardsStorage.Student.MainMenu;
        
        transmittedData.State.StudentState = States.StudentStates.MainMenu;
        
        return new MessageToSend(response, keyboard, false);
    }

    #endregion
    #region HelpMethods
    
    private string InputName(string inputName, int courseId)
    {
        var name = inputName;
        
        int doubleNameI = 1;

        var tableCourses = DbManager.GetInstance().TableCourses;
        
        while (!tableCourses.IsStudentNameInCourseStudentGroupUnique(name, courseId))
        {
            name = inputName + " " + doubleNameI;
            doubleNameI++;

            if (doubleNameI > 5 && doubleNameI < 1000000) // todo clear magic numbers
            {
                doubleNameI += DateTime.Now.Millisecond;
            }
        }

        return name;
    }

    private MessageToSend GetReplyFinalStepMessageToSend(string value)
    {
        var response = ReplyTextsStorage.FinalStep + "\n" + value;
        var keyboard = ReplyKeyboardsStorage.FinalStep;
        
        return new MessageToSend(response, keyboard, false);
    }
    
    #endregion
}