using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Router.Auxiliary;
using IT_ParkTelegramBotShit.Service.ServiceRealization;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Service.ChatServices;

public class MessageServiceManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private Dictionary<State, Func<long, TransmittedData, Message, MessageToSend>>
        _startedStateServiceMethodPairs;

    private StartedService _startedService;

    public MessageServiceManager()
    {
        _startedService = new StartedService();

        _startedStateServiceMethodPairs =
            new Dictionary<State, Func<long, TransmittedData, Message, MessageToSend>>();

        _startedStateServiceMethodPairs[State.CmdStart] = _startedService.ProcessCommandStart;
        _startedStateServiceMethodPairs[State.EnterCode] = _startedService.ProcessCommandEnterCode;
    }

    public MessageToSend ProcessBotUpdate(long chatId, TransmittedData transmittedData, Message message)
    {
        var serviceMethod = _startedStateServiceMethodPairs[transmittedData.State];
        
        Logger.Info($"Вызван метод ProcessBotUpdate Для chatId = {chatId} состояние системы = {transmittedData.State} функция для обработки = {serviceMethod.Method.Name}");
        
        return serviceMethod.Invoke(chatId, transmittedData, message);
    }
}