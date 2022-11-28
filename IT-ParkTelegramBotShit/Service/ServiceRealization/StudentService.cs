using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Bot.Buttons;
using IT_ParkTelegramBotShit.DataBase;
using IT_ParkTelegramBotShit.DataBase.Entities;
using IT_ParkTelegramBotShit.Router.Transmitted;
using IT_ParkTelegramBotShit.Util;
using NLog;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Service.ServiceRealization;

public class StudentService
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    private Dictionary<string, Func<long, TransmittedData, MessageToSend>>
        _requestMethodsPairs;

    public StudentService()
    {
        _requestMethodsPairs = new Dictionary<string, Func<long, TransmittedData, MessageToSend>>();
    }
    // todo make all logic of this class
    public MessageToSend ProcessButtonExample(long chatId, TransmittedData transmittedData)
    {
        // make something
        
        return MessageToSend.Empty();
    }
}