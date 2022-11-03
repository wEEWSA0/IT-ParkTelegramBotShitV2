using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Router.Auxiliary;
using IT_ParkTelegramBotShit.Service.ServiceRealization;
using NLog;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Service.ServiceManager;

public class StartedServiceManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private Dictionary<States.GlobalStates, Func<long, TransmittedData, Message, MessageToSend>>
        _startedStateServiceMethodPairs;

    private StartedService _startedService;

    public StartedServiceManager()
    {
        _startedService = new StartedService();

        _startedStateServiceMethodPairs =
            new Dictionary<States.GlobalStates, Func<long, TransmittedData, Message, MessageToSend>>();
        
        _startedStateServiceMethodPairs[States.GlobalStates.CmdStart] = _startedService.ProcessCommandStart;
        _startedStateServiceMethodPairs[States.GlobalStates.EnterCode] = _startedService.ProcessCommandEnterCode;
    }

    public MessageToSend ProcessBotUpdate(long chatId, TransmittedData transmittedData, Message message)
    {
        var serviceMethod = _startedStateServiceMethodPairs[transmittedData.State.GlobalState];
        
        Logger.Info($"Вызван метод ProcessBotUpdate Для chatId = {chatId} состояние системы = {transmittedData.State} функция для обработки = {serviceMethod.Method.Name}");
        
        return serviceMethod.Invoke(chatId, transmittedData, message);
    }
}