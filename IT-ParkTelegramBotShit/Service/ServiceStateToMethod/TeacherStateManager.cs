using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Router.Transmitted;
using IT_ParkTelegramBotShit.Service.ServiceRealization;
using IT_ParkTelegramBotShit.Util;
using NLog;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Service.ServiceStateToMethod;

public class TeacherStateManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private Dictionary<States.TeacherStates, Func<long, TransmittedData, string, MessageToSend>>
        _messageMethods;
    
    private Dictionary<States.TeacherStates, Func<long, TransmittedData, string, MessageToSend>>
        _callbackMethods;
    
    private Dictionary<string, Func<long, TransmittedData, MessageToSend>>
        _staticButtonsMethods;
    
    public TeacherStateManager()
    {
        TeacherService teacherService = new TeacherService(); // вызываем ProcessBotCallback и ProcessBotMessage новые метода за место старых
                                                              // благодоря этому метод processButton перейдет в этот класс  
                                                              // => service только с логикой, четкое подразделение на ввод и нажатие
        #region Messages
        _messageMethods =
            new Dictionary<States.TeacherStates, Func<long, TransmittedData, string, MessageToSend>>();

        _messageMethods[States.TeacherStates.InputGroupName] = teacherService.ProcessInputGroupName;
        _messageMethods[States.TeacherStates.InputGroupInviteCode] = teacherService.ProcessInputGroupInviteCode;
        _messageMethods[States.TeacherStates.EditGroupName] = teacherService.ProcessEditGroupName;
        _messageMethods[States.TeacherStates.EditGroupInviteCode] = teacherService.ProcessEditGroupInviteCode;
        _messageMethods[States.TeacherStates.InputHomework] = teacherService.ProcessInputHomework;
        _messageMethods[States.TeacherStates.InputNextLesson] = teacherService.ProcessInputNextLessonDate;
        
        _callbackMethods =
            new Dictionary<States.TeacherStates, Func<long, TransmittedData, string, MessageToSend>>();
        #endregion
        #region Callbacks
        _callbackMethods[States.TeacherStates.MainMenu] = ProcessStaticButtons;
        _callbackMethods[States.TeacherStates.Groups] = ProcessStaticButtons;
        _callbackMethods[States.TeacherStates.GroupCreateFinalStep] = ProcessStaticButtons;
        _callbackMethods[States.TeacherStates.ChooseGroupForEdit] = teacherService.ProcessChooseGroupForEdit;
        _callbackMethods[States.TeacherStates.EditGroup] = ProcessStaticButtons;
        _callbackMethods[States.TeacherStates.EditGroupNameFinalStep] = ProcessStaticButtons;
        _callbackMethods[States.TeacherStates.EditGroupInviteCodeFinalStep] = ProcessStaticButtons;
        _callbackMethods[States.TeacherStates.ChooseGroupForHomework] = teacherService.ProcessChooseGroupForHomework;
        _callbackMethods[States.TeacherStates.HomeworkFinalStep] = ProcessStaticButtons;
        _callbackMethods[States.TeacherStates.ChooseGroupForNextLesson] = teacherService.ProcessChooseGroupForNextLesson;
        _callbackMethods[States.TeacherStates.InputNextLessonFinalStep] = ProcessStaticButtons;
        #endregion
        #region StaticButtons
        _staticButtonsMethods = new Dictionary<string, Func<long, TransmittedData, MessageToSend>>();
        
        _staticButtonsMethods[CallbackQueryStorage.Teacher.Groups] = teacherService.ProcessButtonGroups;
        _staticButtonsMethods[CallbackQueryStorage.Teacher.CreateGroup] = teacherService.ProcessButtonCreateGroup;
        _staticButtonsMethods[CallbackQueryStorage.MainMenu] = teacherService.ProcessButtonMainMenu;
        _staticButtonsMethods[CallbackQueryStorage.Yes] = teacherService.ProcessButtonYes;
        _staticButtonsMethods[CallbackQueryStorage.No] = teacherService.ProcessButtonNo;
        _staticButtonsMethods[CallbackQueryStorage.Teacher.EditGroup] = teacherService.ProcessButtonEditGroup;
        _staticButtonsMethods[CallbackQueryStorage.Teacher.EditGroupName] = teacherService.ProcessButtonEditGroupName;
        _staticButtonsMethods[CallbackQueryStorage.Teacher.EditGroupInviteCode] = teacherService.ProcessButtonEditGroupInviteCode;

        _staticButtonsMethods[CallbackQueryStorage.Teacher.AddHomework] = teacherService.ProcessButtonSetHomework;
        _staticButtonsMethods[CallbackQueryStorage.Teacher.AddNextLessonDate] = teacherService.ProcessButtonDateNextLesson;
        #endregion
    }
    
    public MessageToSend ProcessMessage(long chatId, TransmittedData transmittedData, string request)
    {
        if (!_messageMethods.ContainsKey(transmittedData.State.TeacherState))
        {
            Logger.Debug(LoggerTextsStorage.LostServiceMethod(chatId, transmittedData));
            
            return MessageToSend.Empty();
        }
        
        var serviceMethod = _messageMethods[transmittedData.State.TeacherState];
        
        Logger.Debug(LoggerTextsStorage.FoundServiceMethod(chatId, transmittedData, serviceMethod.Method.Name));
        
        return serviceMethod.Invoke(chatId, transmittedData, request);
    }
    
    public MessageToSend ProcessCallback(long chatId, TransmittedData transmittedData, string request)
    {
        if (!_callbackMethods.ContainsKey(transmittedData.State.TeacherState))
        {
            Logger.Debug(LoggerTextsStorage.LostServiceMethod(chatId, transmittedData));
            
            return MessageToSend.Empty();
        }
        
        var serviceMethod = _callbackMethods[transmittedData.State.TeacherState];
        
        Logger.Debug(LoggerTextsStorage.FoundServiceMethod(chatId, transmittedData, serviceMethod.Method.Name));
        
        return serviceMethod.Invoke(chatId, transmittedData, request);
    }
    
    private MessageToSend ProcessStaticButtons(long chatId, TransmittedData transmittedData, string request)
    {
        if (!_staticButtonsMethods.ContainsKey(request))
        {
            Logger.Error($"Not found methods to process '{request}'");
            throw new Exception();
        }
        
        return _staticButtonsMethods[request].Invoke(chatId, transmittedData);
    }
}