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

    private TeacherService _teacherService;

    public TeacherStateManager()
    {
        _teacherService = new TeacherService();

        _serviceMethodPairs =
            new Dictionary<States.TeacherStates, Func<long, TransmittedData, string, MessageToSend>>();
        
        _serviceMethodPairs[States.TeacherStates.MainMenu] = _teacherService.ProcessMainMenu;
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