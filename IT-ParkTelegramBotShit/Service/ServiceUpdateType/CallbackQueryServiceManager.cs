using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Router.Transmitted;
using IT_ParkTelegramBotShit.Service.ServiceStateToMethod;
using IT_ParkTelegramBotShit.Util;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Service.ServiceUpdateType;

public class CallbackQueryServiceManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private TeacherStateManager _teacherStateManager;

    public CallbackQueryServiceManager()
    {
        _teacherStateManager = new TeacherStateManager();
    }

    public MessageToSend ProcessBotCallback(long chatId, TransmittedData transmittedData, CallbackQuery callback)
    {
        var state = transmittedData.State;
        
        if (state.GlobalState != States.GlobalStates.Other)
        {
            Logger.Debug(LoggerTextsStorage.LostServiceMethod(chatId, transmittedData));
            
            return MessageToSend.Empty();
        }
        
        if (state.StudentState != States.StudentStates.None)
        {
            Logger.Debug(LoggerTextsStorage.LostServiceMethod(chatId, transmittedData));
            
            return MessageToSend.Empty();
        }
        
        if (state.TeacherState != States.TeacherStates.None)
        {
            return _teacherStateManager.ProcessCallback(chatId, transmittedData, callback.Data);
        }
        
        Logger.Error("Program logic error (ProcessBotUpdate in MessageServiceManager)");
        return new MessageToSend(ReplyTextsStorage.FatalError);
    }
}