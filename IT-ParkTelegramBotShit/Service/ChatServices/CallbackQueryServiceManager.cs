using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Router.Auxiliary;
using IT_ParkTelegramBotShit.Service.ServiceManager;
using IT_ParkTelegramBotShit.Util;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Service.ChatServices;

public class CallbackQueryServiceManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private TeacherStateManager _teacherStateManager;

    public CallbackQueryServiceManager()
    {
        _teacherStateManager = new TeacherStateManager();
    }

    public MessageToSend ProcessBotUpdate(long chatId, TransmittedData transmittedData, CallbackQuery callback)
    {
        var state = transmittedData.State;
        
        if (state.GlobalState != States.GlobalStates.Other)
        {
            Logger.Debug(LoggerTextsStorage.LostServiceMethod(chatId, transmittedData));
            
            return MessageToSend.Empty();
        }
        else if (state.StudentState != States.StudentStates.None)
        {
            Logger.Debug(LoggerTextsStorage.LostServiceMethod(chatId, transmittedData));
            
            return MessageToSend.Empty();
        }
        else if (state.TeacherState != States.TeacherStates.None)
        {
            return _teacherStateManager.ProcessBotUpdate(chatId, transmittedData, callback.Data);
        }
        else
        {
            Logger.Error("Program logic error (ProcessBotUpdate in MessageServiceManager)");
            return new MessageToSend(ReplyTextsStorage.FatalError);
        }
    }
}