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
    
    private StartedServiceManager _startedServiceManager;

    public MessageServiceManager()
    {
        _startedServiceManager = new StartedServiceManager();
    }

    public MessageToSend ProcessBotUpdate(long chatId, TransmittedData transmittedData, Message message)
    {
        var state = transmittedData.State;
        
        if (state.GlobalState != States.GlobalStates.Other)
        {
            return _startedServiceManager.ProcessBotUpdate(chatId, transmittedData, message);
        }
        else if (state.StudentState != States.StudentStates.None)
        {
            throw new NotImplementedException();
            // st
        }
        else if (state.TeacherState != States.TeacherStates.None)
        {
            throw new NotImplementedException();
            // th
        }
        else
        {
            Logger.Error("Program logic error (ProcessBotUpdate in MessageServiceManager)");
            return new MessageToSend(ReplyTextsStorage.FatalError);
        }
    }
}