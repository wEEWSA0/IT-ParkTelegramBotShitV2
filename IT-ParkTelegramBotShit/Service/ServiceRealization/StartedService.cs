using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.DataBase;
using IT_ParkTelegramBotShit.DataBase.Entities;
using IT_ParkTelegramBotShit.Router.Auxiliary;
using IT_ParkTelegramBotShit.Util;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Service.ServiceRealization;

public class StartedService
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    public MessageToSend ProcessCommandStart(long chatId, TransmittedData transmittedData, Message message)
    {
        string requestMessageText = message.Text;

        string responseMessageText = StateStringsStorage.Empty;

        if (requestMessageText != StateStringsStorage.Start)
        {
            responseMessageText = ReplyTextsStorage.ErrorInput;
        }
        else
        {
            transmittedData.State.GlobalState = States.GlobalStates.EnterCode;
        
            responseMessageText = ReplyTextsStorage.CmsStart;
        }
        
        return new MessageToSend(responseMessageText);
    }
    
    public MessageToSend ProcessCommandEnterCode(long chatId, TransmittedData transmittedData, Message message)
    {
        string requestMessageText = message.Text;

        string responseMessageText = StateStringsStorage.Empty;

        if (DbManager.GetInstance().TableTeachers.TryJoinTeacherAccountByInviteCode(out Teacher teacher, chatId, requestMessageText))
        {
            // учитель
        }
        else if (DbManager.GetInstance().TableCourses.TryGetCourseByStudentInviteCode(out Course course, requestMessageText))
        {
            // ученик
        }
        else
        {
            responseMessageText = ReplyTextsStorage.NotValidCode;
        }

        return new MessageToSend(responseMessageText);
    }
}