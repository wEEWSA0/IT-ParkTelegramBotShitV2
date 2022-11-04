using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Bot.Buttons;
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
    
    public MessageToSend ProcessCommandStart(long chatId, TransmittedData transmittedData, string request)
    {
        string response = ReplyTextsStorage.Empty;

        if (request != RecieveCommandsStorage.Start)
        {
            response = ReplyTextsStorage.ErrorInput;
        }
        else
        {
            transmittedData.State.GlobalState = States.GlobalStates.EnterCode;
        
            response = ReplyTextsStorage.CmsStart;
        }
        
        return new MessageToSend(response);
    }
    
    public MessageToSend ProcessCommandEnterCode(long chatId, TransmittedData transmittedData, string request)
    {
        string response = ReplyTextsStorage.Empty;

        InlineKeyboardMarkup keyboard = InlineKeyboardMarkup.Empty();
        
        if (DbManager.GetInstance().TableTeachers.TryJoinTeacherAccountByInviteCode(out Teacher teacher, chatId, request))
        {
            // учитель
            transmittedData.State.GlobalState = States.GlobalStates.Other;
            transmittedData.State.TeacherState = States.TeacherStates.MainMenu;

            keyboard = BotKeyboardsStorage.MainMenu;
        }
        else if (DbManager.GetInstance().TableCourses.TryGetCourseByStudentInviteCode(out Course course, request))
        {
            // ученик
            response = ReplyTextsStorage.InDevelopment;
        }
        else
        {
            response = ReplyTextsStorage.NotValidCode;
        }

        return new MessageToSend(response, keyboard);
    }
}