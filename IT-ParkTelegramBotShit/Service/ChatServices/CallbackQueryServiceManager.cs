using IT_ParkTelegramBotShit.Router.Auxiliary;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Service.ChatServices;

public class CallbackQueryServiceManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private Dictionary<States, Func<long, TransmittedData, CallbackQuery, ITelegramBotClient, CancellationToken, Task>>
        _stateServiceMethodPairs;

    //private MainMenuService _mainMenuService;

    public CallbackQueryServiceManager()
    {
        // _mainMenuService = new MainMenuService();

        _stateServiceMethodPairs =
            new Dictionary<States, Func<long, TransmittedData, CallbackQuery, ITelegramBotClient, CancellationToken, Task>>();
/*
        _stateServiceMethodPairs[State.WaitingCommandStart] = _mainMenuService.ProcessCommandStart;
        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonInMenuMain] =
            _mainMenuService.ProcessClickOnInlineButtonInMenuMain;
        _stateServiceMethodPairs[State.WaitingClickOnInlineButtonInMenuAdd] =
            _mainMenuService.ProcessClickOnInlineButtonInMenuAddChoosing;*/
    }

    public Task ProcessBotUpdate(long chatId, TransmittedData transmittedData, CallbackQuery callback,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        try
        {
            var serviceMethod = _stateServiceMethodPairs[transmittedData.State];
            
            Logger.Info($"Вызван метод ProcessBotUpdate Для chatId = {chatId} состояние системы = {transmittedData.State} функция для обработки = {serviceMethod.Method.Name}");
            
            return serviceMethod.Invoke(chatId, transmittedData, callback, botClient, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new Task(() => { });
        }
    }
}