using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Router.Auxiliary;
using IT_ParkTelegramBotShit.Service.ServiceRealization;
using NLog;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Service.ServiceManager;

public class GlobalStateManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private Dictionary<States.GlobalStates, Func<long, TransmittedData, string, MessageToSend>>
        _startedStateServiceMethodPairs;

    private StartedService _startedService;

    public GlobalStateManager()
    {
        _startedService = new StartedService();

        _startedStateServiceMethodPairs =
            new Dictionary<States.GlobalStates, Func<long, TransmittedData, string, MessageToSend>>();
        
        _startedStateServiceMethodPairs[States.GlobalStates.CmdStart] = _startedService.ProcessCommandStart;
        _startedStateServiceMethodPairs[States.GlobalStates.EnterCode] = _startedService.ProcessCommandEnterCode;
    }

    public MessageToSend ProcessBotUpdate(long chatId, TransmittedData transmittedData, string request)
    {
        var serviceMethod = _startedStateServiceMethodPairs[transmittedData.State.GlobalState];
        
        Logger.Debug($"Вызван метод ProcessBotUpdate: chatId = {chatId}, " +
                     $"состояние = {transmittedData.State.GetCurrentStateName()}, " +
                     $"функция для обработки = {serviceMethod.Method.Name}");
        
        return serviceMethod.Invoke(chatId, transmittedData, request);
    }
}