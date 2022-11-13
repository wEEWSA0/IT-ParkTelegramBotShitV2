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
        _requestMethodsPairs[CallbackQueryStorage.Teacher.EditGroup] = ProcessButtonEditGroup;
        _requestMethodsPairs[CallbackQueryStorage.Teacher.EditGroupName] = ProcessButtonEditGroupName;
        _requestMethodsPairs[CallbackQueryStorage.Teacher.EditGroupInviteCode] = ProcessButtonEditGroupInviteCode;
    }
    
    #region InputAndChooseMethods

    public MessageToSend ProcessInputGroupName(long chatId, TransmittedData transmittedData, string request)
    {
        if (InputGroupName(out MessageToSend messageToSend, request, ReplyTextsStorage.Teacher.InputGroupInviteCode))
        {
            transmittedData.DataStorage.Add(ConstantsStorage.GroupName, request);

            transmittedData.State.TeacherState = States.TeacherStates.InputGroupInviteCode;
        }

        return messageToSend;
    }
    
    public MessageToSend ProcessInputGroupInviteCode(long chatId, TransmittedData transmittedData, string request)
    {
        transmittedData.DataStorage.TryGet(ConstantsStorage.GroupName, out Object groupName);
        
        string response = ReplyTextsStorage.FinalStep + "\n" + ReplyTextsStorage.Teacher.GetGroupFinalStateView((string)groupName, request);

        if (InputGroupInviteCode(out MessageToSend messageToSend, request, response))
        {
            transmittedData.DataStorage.Add(ConstantsStorage.GroupInviteCode, request);

            transmittedData.State.TeacherState = States.TeacherStates.GroupCreateFinalStep;
            
            return new MessageToSend(messageToSend.Text, ReplyKeyboardsStorage.FinalStep);
        }

        return messageToSend;
    }
    
    public MessageToSend ProcessChooseGroupForEdit(long chatId, TransmittedData transmittedData, string request)
    {
        string response = ReplyTextsStorage.Teacher.EditGroup;

        transmittedData.State.TeacherState = States.TeacherStates.EditGroup;
        
        if (!DbManager.GetInstance().TableCourses.TryGetCourseByStudentInviteCode(out Course course, request))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessChooseGroupForEdit"));
            throw new NotImplementedException();
        }
        
        transmittedData.DataStorage.Add(ConstantsStorage.Course, course);

        var keyboard = ReplyKeyboardsStorage.Teacher.EditGroup;

        return new MessageToSend(response, keyboard, false);
    }

    public MessageToSend ProcessEditGroupName(long chatId, TransmittedData transmittedData, string request)
    {
        string response = ReplyTextsStorage.FinalStep + "\n" + ReplyTextsStorage.Teacher.GetNewGroupNameView(request);
        
        if (InputGroupName(out MessageToSend messageToSend, request, response))
        {
            transmittedData.DataStorage.Add(ConstantsStorage.GroupName, request);
            
            transmittedData.State.TeacherState = States.TeacherStates.EditGroupNameFinalStep;
            
            var keyboard = ReplyKeyboardsStorage.FinalStep;
        
            return new MessageToSend(messageToSend.Text, keyboard);
        }

        return messageToSend;
    }
    
    public MessageToSend ProcessEditGroupInviteCode(long chatId, TransmittedData transmittedData, string request)
    {
        string response = ReplyTextsStorage.FinalStep + "\n" + ReplyTextsStorage.Teacher.GetNewGroupInviteCodeView(request);
        
        if (InputGroupInviteCode(out MessageToSend messageToSend, request, response))
        {
            transmittedData.DataStorage.Add(ConstantsStorage.GroupInviteCode, request);
            
            transmittedData.State.TeacherState = States.TeacherStates.EditGroupInviteCodeFinalStep;
            
            var keyboard = ReplyKeyboardsStorage.FinalStep;
        
            return new MessageToSend(messageToSend.Text, keyboard);
        }
        
        return messageToSend;
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
        MessageToSend messageToSend;
        
        switch (transmittedData.State.TeacherState)
        {
            case States.TeacherStates.GroupCreateFinalStep:
            {
                var tableCourses = DbManager.GetInstance().TableCourses;
                
                bool isGroupNameEnab = storage.TryGet(ConstantsStorage.GroupName, out Object groupName);
                bool isInviteCodeEnab = storage.TryGet(ConstantsStorage.GroupInviteCode, out Object inviteCode);
                bool isTeacherIdEnab = storage.TryGet(ConstantsStorage.TeacherId, out Object teacherId);
        
                if (!isGroupNameEnab || !isInviteCodeEnab || !isTeacherIdEnab)
                {
                    Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessInputGroupInviteCode"));
                }
                
                tableCourses.CreateCourse((string)groupName, (string)inviteCode, (int)teacherId);
                
                messageToSend = new MessageToSend(ReplyTextsStorage.Teacher.GroupCreated, false);
            }
                break;
            case States.TeacherStates.EditGroupNameFinalStep:
            {
                var tableCourses = DbManager.GetInstance().TableCourses;
                
                bool isGroupNameEnab = storage.TryGet(ConstantsStorage.GroupName, out Object groupName);
                bool isCourseEnab = storage.TryGet(ConstantsStorage.Course, out Object course);
        
                if (!isGroupNameEnab || !isCourseEnab)
                {
                    Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessInputGroupInviteCode"));
                }
                
                tableCourses.UpdateCourseName((string)groupName, ((Course)course).Id);
                
                messageToSend = new MessageToSend(ReplyTextsStorage.AppliedChanges, false);
            }
                break;
            case States.TeacherStates.EditGroupInviteCodeFinalStep:
            {
                var tableCourses = DbManager.GetInstance().TableCourses;
                
                bool isInviteCodeEnab = storage.TryGet(ConstantsStorage.GroupInviteCode, out Object inviteCode);
                bool isCourseEnab = storage.TryGet(ConstantsStorage.Course, out Object course);
                
                if (!isInviteCodeEnab || !isCourseEnab)
                {
                    Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessInputGroupInviteCode"));
                }
                
                tableCourses.UpdateCourseInviteCode((string)inviteCode, ((Course)course).Id);
                
                messageToSend = new MessageToSend(ReplyTextsStorage.AppliedChanges, false);
            }
                break;
            default:
            {
                Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessButtonYes"));
                throw new NotImplementedException();
            }
        }
        
        BotMessageManager.GetInstance().GetSender(chatId).AddMessageToStack(messageToSend);
                
        state.TeacherState = States.TeacherStates.MainMenu;
        
        string response = ReplyTextsStorage.MainMenu;
        var keyboard = ReplyKeyboardsStorage.Teacher.MainMenu;
        
        return new MessageToSend(response, keyboard); // todo: возможно переделать логику перекидывания MessageToSend для всех методов
    }
    
    private MessageToSend ProcessButtonNo(long chatId, TransmittedData transmittedData)
    {
        var state = transmittedData.State;
        MessageToSend messageToSend;
        
        switch (transmittedData.State.TeacherState)
        {
            case States.TeacherStates.GroupCreateFinalStep:
            {
                messageToSend = new MessageToSend(ReplyTextsStorage.Teacher.GroupNotCreated, false);
            }
                break;
            case States.TeacherStates.EditGroupNameFinalStep:
            case States.TeacherStates.EditGroupInviteCodeFinalStep:
            {
                messageToSend = new MessageToSend(ReplyTextsStorage.DeclinedChanges, false);
            }
                break;
            default:
            {
                Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessButtonNo"));
                throw new NotImplementedException();
            }
        }
        
        BotMessageManager.GetInstance().GetSender(chatId).AddMessageToStack(messageToSend);

        state.TeacherState = States.TeacherStates.MainMenu;
        
        string response = ReplyTextsStorage.MainMenu;
        var keyboard = ReplyKeyboardsStorage.Teacher.MainMenu;
        
        return new MessageToSend(response, keyboard);
    }
    
    private MessageToSend ProcessButtonEditGroup(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Teacher.Groups;

        transmittedData.State.TeacherState = States.TeacherStates.ChooseGroupForEdit;

        var keyboard = GetTeacherCoursesButtons(chatId, transmittedData.DataStorage);
        
        return new MessageToSend(response, keyboard, false);
    }
    
    private MessageToSend ProcessButtonEditGroupName(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Teacher.InputGroupName;

        transmittedData.State.TeacherState = States.TeacherStates.EditGroupName;

        return new MessageToSend(response, false);
    }
    
    private MessageToSend ProcessButtonEditGroupInviteCode(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Teacher.InputGroupInviteCode;

        transmittedData.State.TeacherState = States.TeacherStates.EditGroupInviteCode;

        return new MessageToSend(response, false);
    }

    #endregion
    #region HelpMethods

    private InlineKeyboardMarkup GetTeacherCoursesButtons(long chatId, DataStorage dataStorage)
    {
        int teacherId;
        
        if (dataStorage.TryGet(ConstantsStorage.TeacherId, out Object objTeacherId))
        {
            teacherId = (int)objTeacherId;
        }
        else
        {
            DbManager.GetInstance().TableTeachers.TryGetTeacherByChatId(out Teacher teacher, chatId);

            teacherId = teacher.Id;
        }

        if (!DbManager.GetInstance().TableCourses.TryGetTeacherCourses(out List<Course> courses, teacherId))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessButtonEditGroup"));
            throw new Exception();
        }

        List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
        for (int i = 0; i < courses.Count; i++)
        {
            buttons.Add(InlineKeyboardButton.WithCallbackData(courses[i].CourseName, courses[i].StudentInviteCode));
        }

        return BotKeyboardCreator.GetInstance().GetKeyboardMarkup(buttons.ToArray());
    }
    
    private bool InputGroupName(out MessageToSend messageToSend, string request, string succesReplyText)
    {
        if (!DbManager.GetInstance().TableCourses.IsCourseNameUnique(request))
        {
            messageToSend = new MessageToSend(ReplyTextsStorage.Teacher.InputAnotherGroupName);

            return false;
        }
        
        messageToSend = new MessageToSend(succesReplyText);

        return true;
    }
    
    private bool InputGroupInviteCode(out MessageToSend messageToSend, string request, string succesReplyText)
    {
        if (!DbManager.GetInstance().TableCourses.IsCourseInviteCodeUnique(request))
        {
            messageToSend = new MessageToSend(ReplyTextsStorage.Teacher.InputAnotherGroupInviteCode);

            return false;
        }
        
        messageToSend = new MessageToSend(succesReplyText);

        return true;
    }

    #endregion
}