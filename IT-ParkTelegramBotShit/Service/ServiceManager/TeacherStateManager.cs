using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Router.Auxiliary;
using IT_ParkTelegramBotShit.Service.ServiceRealization;
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
        var serviceMethod = _serviceMethodPairs[transmittedData.State.TeacherState];
        
        Logger.Debug($"Вызван метод ProcessBotUpdate: chatId = {chatId}, " +
                     $"состояние = {transmittedData.State.GetCurrentStateName()}, " +
                     $"функция для обработки = {serviceMethod.Method.Name}");
        
        return serviceMethod.Invoke(chatId, transmittedData, request);
    }
}