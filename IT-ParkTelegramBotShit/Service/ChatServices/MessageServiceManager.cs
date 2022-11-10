using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Router.Auxiliary;
using IT_ParkTelegramBotShit.Service.ServiceManager;
using IT_ParkTelegramBotShit.Service.ServiceRealization;
using IT_ParkTelegramBotShit.Util;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Service.ChatServices;

public class MessageServiceManager
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    private GlobalStateManager _globalStateManager;

    public MessageServiceManager()
    {
        _globalStateManager = new GlobalStateManager();
    }

    public MessageToSend ProcessBotUpdate(long chatId, TransmittedData transmittedData, Message message)
    {
        var state = transmittedData.State;
        
        if (state.GlobalState != States.GlobalStates.Other)
        {
            return _globalStateManager.ProcessBotUpdate(chatId, transmittedData, message.Text);
        }
        else if (state.StudentState != States.StudentStates.None)
        {
            Logger.Debug(LoggerTextsStorage.LostServiceMethod(chatId, transmittedData));
            
            return MessageToSend.Empty();
        }
        else if (state.TeacherState != States.TeacherStates.None)
        {
            Logger.Debug(LoggerTextsStorage.LostServiceMethod(chatId, transmittedData));
            
            return MessageToSend.Empty();
        }
        else
        {
            Logger.Error("Program logic error (ProcessBotUpdate in MessageServiceManager)");
            return new MessageToSend(ReplyTextsStorage.FatalError);
        }
    }
}