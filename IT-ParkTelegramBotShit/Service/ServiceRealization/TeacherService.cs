using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Bot.Buttons;
using IT_ParkTelegramBotShit.DataBase;
using IT_ParkTelegramBotShit.DataBase.Entities;
using IT_ParkTelegramBotShit.Router.Auxiliary;
using IT_ParkTelegramBotShit.Service.ChatServices;
using IT_ParkTelegramBotShit.Util;
using NLog;
using NLog.Fluent;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Service.ServiceRealization;

public class TeacherService
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private Dictionary<string, Func<long, TransmittedData, MessageToSend>>
        _requestMethodsPairs;
    
    public TeacherService()
    {
        _requestMethodsPairs = new Dictionary<string, Func<long, TransmittedData, MessageToSend>>();
        
        _requestMethodsPairs[CallbackQueryStorage.Teacher.Groups] = ProcessButtonGroups;
        _requestMethodsPairs[CallbackQueryStorage.Teacher.CreateGroup] = ProcessButtonCreateGroup;
        _requestMethodsPairs[CallbackQueryStorage.MainMenu] = ProcessButtonMainMenu;
        _requestMethodsPairs[CallbackQueryStorage.Yes] = ProcessButtonYes;
        _requestMethodsPairs[CallbackQueryStorage.No] = ProcessButtonNo;
    }
    
    #region InputMethods

    public MessageToSend ProcessInputGroupName(long chatId, TransmittedData transmittedData, string request)
    {
        string response = ReplyTextsStorage.Teacher.InputGroupInviteCode;

        if (!DbManager.GetInstance().TableCourses.IsCourseNameUnique(request))
        {
            response = ReplyTextsStorage.Teacher.InputAnotherGroupName;
            
            return new MessageToSend(response);
        }
        
        transmittedData.DataStorage.Add(ConstantsStorage.GroupName, request);
        
        transmittedData.State.TeacherState = States.TeacherStates.InputGroupInviteCode;
        
        return new MessageToSend(response);
    }
    
    public MessageToSend ProcessInputGroupInviteCode(long chatId, TransmittedData transmittedData, string request)
    {
        transmittedData.DataStorage.TryGet(ConstantsStorage.GroupName, out Object groupName);
        
        string response = ReplyTextsStorage.FinalStep + "\n" + ReplyTextsStorage.Teacher.GetGroupFinalStateView((string)groupName, request);
        
        var tableCourses = DbManager.GetInstance().TableCourses;
        
        if (!tableCourses.IsCourseInviteCodeUnique(request))
        {
            response = ReplyTextsStorage.Teacher.InputAnotherInviteCode;
            
            return new MessageToSend(response);
        }
        
        transmittedData.DataStorage.Add(ConstantsStorage.GroupInviteCode, request);

        transmittedData.State.TeacherState = States.TeacherStates.GroupCreateFinalStep;

        var keyboard = ReplyKeyboardsStorage.FinalStep;
        
        return new MessageToSend(response, keyboard, false);
    }

    #endregion
    #region ButtonsMethods

    public MessageToSend ProcessButtons(long chatId, TransmittedData transmittedData, string request)
    {
        if (!_requestMethodsPairs.ContainsKey(request))
        {
            Logger.Error($"Not found methods to process '{request}'");
            throw new NotImplementedException();
        }
        
        return _requestMethodsPairs[request].Invoke(chatId, transmittedData);
    }
    
    private MessageToSend ProcessButtonGroups(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Teacher.Groups;
        
        InlineKeyboardMarkup keyboard;

        if (!transmittedData.DataStorage.TryGet(ConstantsStorage.TeacherId, out object obj))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessGroups"));
        }

        int teacherId = (int)obj;

        if (DbManager.GetInstance().TableCourses.TryGetTeacherCourses(out List<Course> courses, teacherId))
        {
            keyboard = BotKeyboardCreator.GetInstance()
                .GetKeyboardMarkup(ReplyButtonsStorage.Teacher.CreateGroup, ReplyButtonsStorage.Teacher.EditGroup, ReplyButtonsStorage.MainMenu);
        }
        else
        {
            keyboard = BotKeyboardCreator.GetInstance()
                .GetKeyboardMarkup(ReplyButtonsStorage.Teacher.CreateGroup, ReplyButtonsStorage.MainMenu);
        }
        
        transmittedData.State.TeacherState = States.TeacherStates.Groups;
        
        return new MessageToSend(response, keyboard, false);
    }
    
    private MessageToSend ProcessButtonMainMenu(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.MainMenu;

        InlineKeyboardMarkup keyboard = ReplyKeyboardsStorage.Teacher.MainMenu;
        
        transmittedData.State.TeacherState = States.TeacherStates.MainMenu;
        
        return new MessageToSend(response, keyboard, false);
    }
    
    private MessageToSend ProcessButtonCreateGroup(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Teacher.InputGroupName;

        transmittedData.State.TeacherState = States.TeacherStates.InputGroupName;
        
        return new MessageToSend(response, false);
    }
    
    private MessageToSend ProcessButtonYes(long chatId, TransmittedData transmittedData)
    {
        var state = transmittedData.State;
        var storage = transmittedData.DataStorage;
        
        switch (transmittedData.State.TeacherState)
        {
            case States.TeacherStates.GroupCreateFinalStep:
            {
                var tableCourses = DbManager.GetInstance().TableCourses;
                
                bool isGroupNameEnab = storage.TryGet(ConstantsStorage.GroupName, out Object groupName);
                bool isTeacherIdEnab = storage.TryGet(ConstantsStorage.TeacherId, out Object teacherId);
                bool isInviteCodeEnab = storage.TryGet(ConstantsStorage.GroupInviteCode, out Object inviteCode);
        
                if (!isGroupNameEnab || !isTeacherIdEnab || !isInviteCodeEnab)
                {
                    Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessInputGroupInviteCode"));
                }
                
                tableCourses.CreateCourse((string)groupName, (string)inviteCode, (int)teacherId);
                
                MessageToSend messageToSend = new MessageToSend(ReplyTextsStorage.Teacher.GroupCreated, false);
                
                BotMessageManager.GetInstance().GetSender(chatId).AddMessageToStack(messageToSend);
                
                state.TeacherState = States.TeacherStates.MainMenu;
            }
                break;
            default:
            {
                Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessButtonYes"));
                throw new NotImplementedException();
            }
        }
        
        string response = ReplyTextsStorage.MainMenu;
        var keyboard = ReplyKeyboardsStorage.Teacher.MainMenu;
        
        return new MessageToSend(response, keyboard); // todo: возможно переделать логику перекидывания MessageToSend для всех методов
    }
    
    private MessageToSend ProcessButtonNo(long chatId, TransmittedData transmittedData)
    {
        var state = transmittedData.State;
        
        switch (transmittedData.State.TeacherState)
        {
            case States.TeacherStates.GroupCreateFinalStep:
            {
                MessageToSend messageToSend = new MessageToSend(ReplyTextsStorage.Teacher.GroupNotCreated, false);
                
                BotMessageManager.GetInstance().GetSender(chatId).AddMessageToStack(messageToSend);

                state.TeacherState = States.TeacherStates.MainMenu;
            }
                break;
            default:
            {
                Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessButtonNo"));
                throw new NotImplementedException();
            }
        }
        
        string response = ReplyTextsStorage.MainMenu;
        var keyboard = ReplyKeyboardsStorage.Teacher.MainMenu;
        
        return new MessageToSend(response, keyboard);
    }

    #endregion
}