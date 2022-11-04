using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Router.Auxiliary;
using IT_ParkTelegramBotShit.Util;
using NLog;
using Telegram.Bot.Types;

namespace IT_ParkTelegramBotShit.Service.ServiceRealization;

public class TeacherService
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    public MessageToSend ProcessMainMenu(long chatId, TransmittedData transmittedData, string request)
    {
        string response = ReplyTextsStorage.Empty;

        response = ReplyTextsStorage.InDevelopment + "  " + request;

        switch (request)
        {
            case CallbackQueryStorage.Logout:
            {
                // todo
            }
            break;
            default: throw new NotImplementedException();
        }
        
        return new MessageToSend(response);
    }
}