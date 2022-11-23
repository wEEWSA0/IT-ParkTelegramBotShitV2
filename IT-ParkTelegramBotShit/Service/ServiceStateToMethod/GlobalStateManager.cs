using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Router.Transmitted;
using IT_ParkTelegramBotShit.Service.ServiceRealization;
using IT_ParkTelegramBotShit.Util;
using NLog;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Service.ServiceStateToMethod;

public class GlobalStateManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private Dictionary<States.GlobalStates, Func<long, TransmittedData, string, MessageToSend>>
        _serviceMethodPairs;

    private StartedService _startedService;

    public GlobalStateManager()
    {
        _startedService = new StartedService();

        _serviceMethodPairs =
            new Dictionary<States.GlobalStates, Func<long, TransmittedData, string, MessageToSend>>();
        
        _serviceMethodPairs[States.GlobalStates.CmdStart] = _startedService.ProcessCommandStart;
        _serviceMethodPairs[States.GlobalStates.EnterCode] = _startedService.ProcessCommandEnterCode;
    }

    public MessageToSend ProcessBotUpdate(long chatId, TransmittedData transmittedData, string request)
    {
        if (!_serviceMethodPairs.ContainsKey(transmittedData.State.GlobalState))
        {
            Logger.Debug(LoggerTextsStorage.LostServiceMethod(chatId, transmittedData));
            
            return MessageToSend.Empty();
        }
        
        var serviceMethod = _serviceMethodPairs[transmittedData.State.GlobalState];
        
        Logger.Debug(LoggerTextsStorage.FoundServiceMethod(chatId, transmittedData, serviceMethod.Method.Name));
        
        return serviceMethod.Invoke(chatId, transmittedData, request);
    }
}