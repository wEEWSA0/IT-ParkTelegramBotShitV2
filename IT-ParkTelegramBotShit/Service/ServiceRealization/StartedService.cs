using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Bot.Buttons;
using IT_ParkTelegramBotShit.Bot.Notifications;
using IT_ParkTelegramBotShit.DataBase;
using IT_ParkTelegramBotShit.DataBase.Entities;
using IT_ParkTelegramBotShit.Router.Transmitted;
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
        if (request != RecieveCommandsStorage.Start)
        {
            return new MessageToSend(ReplyTextsStorage.ErrorInput);
        }
        else
        {
            transmittedData.State.GlobalState = States.GlobalStates.EnterCode;
            
            var message = new MessageToSend(ReplyTextsStorage.OfficialITParkBot);
            
            BotNotificationSender.GetInstance().SendAnchoredNotificationMessage(message, chatId);
            
            return new MessageToSend(ReplyTextsStorage.CmdStart, false);
        }
    }
    
    public MessageToSend ProcessCommandEnterCode(long chatId, TransmittedData transmittedData, string request)
    {
        string response = ReplyTextsStorage.Empty;

        InlineKeyboardMarkup keyboard = InlineKeyboardMarkup.Empty();
        
        if (DbManager.GetInstance().TableTeachers.TryJoinTeacherAccountByInviteCode(out Teacher teacher, chatId, request))
        {
            response = ReplyTextsStorage.MainMenu;
            
            transmittedData.State.GlobalState = States.GlobalStates.Other;
            transmittedData.State.TeacherState = States.TeacherStates.MainMenu;
            
            transmittedData.DataStorage.Add(ConstantsStorage.TeacherId, teacher.Id); // записываем teacher id

            keyboard = ReplyKeyboardsStorage.Teacher.MainMenu;

            var logIntoAccountMessageToSend = new MessageToSend(ReplyTextsStorage.Teacher.LogIntoAccount, false);
            
            BotMessageManager.GetInstance().GetSender(chatId).AddMessageToStack(logIntoAccountMessageToSend);
            
            return new MessageToSend(response, keyboard);
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