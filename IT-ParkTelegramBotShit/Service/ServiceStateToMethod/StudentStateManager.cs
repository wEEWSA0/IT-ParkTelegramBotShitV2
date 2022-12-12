using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Router.Transmitted;
using IT_ParkTelegramBotShit.Service.ServiceRealization;
using IT_ParkTelegramBotShit.Util;
using NLog;

namespace IT_ParkTelegramBotShit.Service.ServiceStateToMethod;

public class StudentStateManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private Dictionary<States.StudentStates, Func<long, TransmittedData, string, MessageToSend>>
        _messageMethods;
    
    private Dictionary<States.StudentStates, Func<long, TransmittedData, string, MessageToSend>>
        _callbackMethods;
    
    private Dictionary<string, Func<long, TransmittedData, MessageToSend>>
        _staticButtonsMethods;
    
    public StudentStateManager()
    {
        StudentService studentService = new StudentService();
        #region Messages
        
        _messageMethods =
            new Dictionary<States.StudentStates, Func<long, TransmittedData, string, MessageToSend>>();

        _messageMethods[States.StudentStates.InputName] = studentService.ProcessInputName;
        _messageMethods[States.StudentStates.InputNewName] = studentService.ProcessInputNewName;
        
        #endregion
        #region Callbacks
        
        _callbackMethods =
            new Dictionary<States.StudentStates, Func<long, TransmittedData, string, MessageToSend>>();
        
        _callbackMethods[States.StudentStates.MainMenu] = ProcessStaticButtons;
        _callbackMethods[States.StudentStates.ChangeNameFinalStep] = ProcessStaticButtons;
        _callbackMethods[States.StudentStates.QuitAccountFinalStep] = ProcessStaticButtons;
        
        #endregion
        #region StaticButtons
        _staticButtonsMethods = new Dictionary<string, Func<long, TransmittedData, MessageToSend>>();
        
        _staticButtonsMethods[CallbackQueryStorage.MainMenu] = studentService.ProcessButtonMainMenu;
        _staticButtonsMethods[CallbackQueryStorage.Student.Homework] = studentService.ProcessButtonHomework;
        _staticButtonsMethods[CallbackQueryStorage.Student.NextLesson] = studentService.ProcessButtonNextLesson;
        _staticButtonsMethods[CallbackQueryStorage.Student.Profile] = studentService.ProcessButtonProfile;
        _staticButtonsMethods[CallbackQueryStorage.Student.ChangeName] = studentService.ProcessButtonChangeName;
        _staticButtonsMethods[CallbackQueryStorage.Student.QuitAccount] = studentService.ProcessButtonQuitAccount;
        
        _staticButtonsMethods[CallbackQueryStorage.Yes] = studentService.ProcessButtonYes;
        _staticButtonsMethods[CallbackQueryStorage.No] = studentService.ProcessButtonNo;
        
        #endregion
    }
    
    public MessageToSend ProcessMessage(long chatId, TransmittedData transmittedData, string request)
    {
        if (!_messageMethods.ContainsKey(transmittedData.State.StudentState))
        {
            Logger.Debug(LoggerTextsStorage.LostServiceMethod(chatId, transmittedData));
            
            return MessageToSend.Empty();
        }
        
        var serviceMethod = _messageMethods[transmittedData.State.StudentState];
        
        Logger.Debug(LoggerTextsStorage.FoundServiceMethod(chatId, transmittedData, serviceMethod.Method.Name));
        
        return serviceMethod.Invoke(chatId, transmittedData, request);
    }
    
    public MessageToSend ProcessCallback(long chatId, TransmittedData transmittedData, string request)
    {
        if (!_callbackMethods.ContainsKey(transmittedData.State.StudentState))
        {
            Logger.Debug(LoggerTextsStorage.LostServiceMethod(chatId, transmittedData));
            
            return MessageToSend.Empty();
        }
        
        var serviceMethod = _callbackMethods[transmittedData.State.StudentState];
        
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