using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Router.Auxiliary;
using IT_ParkTelegramBotShit.Service.ServiceRealization;
using IT_ParkTelegramBotShit.Util;
using NLog;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Service.ServiceManager;

public class TeacherStateManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private Dictionary<States.TeacherStates, Func<long, TransmittedData, string, MessageToSend>>
        _serviceMethodPairs;
    
    public TeacherStateManager()
    {
        TeacherService teacherService = new TeacherService();

        _serviceMethodPairs =
            new Dictionary<States.TeacherStates, Func<long, TransmittedData, string, MessageToSend>>();

        _serviceMethodPairs[States.TeacherStates.MainMenu] = teacherService.ProcessButtons;
        _serviceMethodPairs[States.TeacherStates.Groups] = teacherService.ProcessButtons;
        _serviceMethodPairs[States.TeacherStates.InputGroupName] = teacherService.ProcessInputGroupName;
        _serviceMethodPairs[States.TeacherStates.InputGroupInviteCode] = teacherService.ProcessInputGroupInviteCode;
        _serviceMethodPairs[States.TeacherStates.GroupCreateFinalStep] = teacherService.ProcessButtons;
        _serviceMethodPairs[States.TeacherStates.ChooseGroupForEdit] = teacherService.ProcessChooseGroupForEdit;
        _serviceMethodPairs[States.TeacherStates.EditGroup] = teacherService.ProcessButtons;
        _serviceMethodPairs[States.TeacherStates.EditGroupName] = teacherService.ProcessEditGroupName;
        _serviceMethodPairs[States.TeacherStates.EditGroupInviteCode] = teacherService.ProcessEditGroupInviteCode;
        _serviceMethodPairs[States.TeacherStates.EditGroupNameFinalStep] = teacherService.ProcessButtons;
        _serviceMethodPairs[States.TeacherStates.EditGroupInviteCodeFinalStep] = teacherService.ProcessButtons;
        _serviceMethodPairs[States.TeacherStates.InputHomework] = teacherService.ProcessInputHomework;
        _serviceMethodPairs[States.TeacherStates.ChooseGroupForHomework] = teacherService.ProcessChooseGroupForHomework;
        _serviceMethodPairs[States.TeacherStates.InputHomework] = teacherService.ProcessInputHomework;
        _serviceMethodPairs[States.TeacherStates.HomeworkFinalStep] = teacherService.ProcessButtons;
    }

    public MessageToSend ProcessBotUpdate(long chatId, TransmittedData transmittedData, string request)
    {
        if (!_serviceMethodPairs.ContainsKey(transmittedData.State.TeacherState))
        {
            Logger.Debug(LoggerTextsStorage.LostServiceMethod(chatId, transmittedData));
            
            return MessageToSend.Empty();
        }
        
        var serviceMethod = _serviceMethodPairs[transmittedData.State.TeacherState];
        
        Logger.Debug(LoggerTextsStorage.FoundServiceMethod(chatId, transmittedData, serviceMethod.Method.Name));
        
        return serviceMethod.Invoke(chatId, transmittedData, request);
    }
}