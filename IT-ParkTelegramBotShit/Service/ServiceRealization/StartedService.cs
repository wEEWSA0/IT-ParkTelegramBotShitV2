using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Bot.Buttons;
using IT_ParkTelegramBotShit.Bot.Notifications;
using IT_ParkTelegramBotShit.DataBase;
using IT_ParkTelegramBotShit.DataBase.Entities;
using IT_ParkTelegramBotShit.Router.Transmitted;
using IT_ParkTelegramBotShit.Util;
using NLog;
using NLog.Fluent;
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
            
            SendOfficialArchoredMessage(chatId);
            
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
        }
        else if (DbManager.GetInstance().TableCourses.TryGetCourseByStudentInviteCode(out Course course, request))
        {
            // todo проверка на уникальность
            // DbManager.GetInstance().TableStudents.CreateStudentAccount(chatId, course.Id, "Default name");
            
            // response = ReplyTextsStorage.MainMenu;
            response = ReplyTextsStorage.Student.InputName;
            
            transmittedData.State.GlobalState = States.GlobalStates.Other;
            // transmittedData.State.StudentState = States.StudentStates.MainMenu;
            transmittedData.State.StudentState = States.StudentStates.InputName;
            
            transmittedData.DataStorage.Add(ConstantsStorage.StudentCourseId, course.Id); // записываем course id

            // keyboard = ReplyKeyboardsStorage.Student.MainMenu;
            //
            // var logIntoAccountMessageToSend = new MessageToSend(ReplyTextsStorage.Student.LogIntoAccount, false);
            //
            // BotMessageManager.GetInstance().GetSender(chatId).AddMessageToStack(logIntoAccountMessageToSend);
        }
        else
        {
            response = ReplyTextsStorage.NotValidCode;
        }

        return new MessageToSend(response, keyboard);
    }

    public static async void Logout(long chatId)
    {
        await BotMessageManager.GetInstance().GetHistory(chatId).DeleteAllMessages();
        
        SendOfficialArchoredMessage(chatId);
    }

    private static async void SendOfficialArchoredMessage(long chatId)
    {
        var message = new MessageToSend(ReplyTextsStorage.OfficialITParkBot);
        
        await BotNotificationSender.GetInstance().SendAnchoredNotificationMessage(message, chatId);
    }
}