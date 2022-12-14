using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Bot.Buttons;
using IT_ParkTelegramBotShit.DataBase;
using IT_ParkTelegramBotShit.DataBase.Entities;
using IT_ParkTelegramBotShit.DataBase.Tables;
using IT_ParkTelegramBotShit.Router.Transmitted;
using IT_ParkTelegramBotShit.Service.ServiceUpdateType;
using IT_ParkTelegramBotShit.Util;
using NLog;
using NLog.Fluent;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Service.ServiceRealization;

public class TeacherService
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();

    #region InputAndChooseMethods

    public MessageToSend ProcessInputGroupName(long chatId, TransmittedData transmittedData, string request)
    {
        if (InputGroupName(out MessageToSend messageToSend, request, ReplyTextsStorage.Teacher.InputGroupInviteCode))
        {
            transmittedData.DataStorage.Add(ConstantsStorage.GroupName, request);

            transmittedData.State.TeacherState = States.TeacherStates.InputGroupInviteCode;
        }

        return messageToSend;
    }

    public MessageToSend ProcessInputHomework(long chatId, TransmittedData transmittedData, string request)
    {
        transmittedData.DataStorage.Add(ConstantsStorage.Homework, request);

        transmittedData.State.TeacherState = States.TeacherStates.HomeworkFinalStep;

        var response = GetReplyFinalStepText(ReplyTextsStorage.Teacher.GetNewHomeworkView(request));
        
        var keyboard = ReplyKeyboardsStorage.FinalStep;

        return new MessageToSend(response, keyboard, false);
    }
    
    public MessageToSend ProcessInputName(long chatId, TransmittedData transmittedData, string request)
    {
        transmittedData.DataStorage.Add(ConstantsStorage.TeacherName, request);

        transmittedData.State.TeacherState = States.TeacherStates.EditProfileFinalStep;

        var response = GetReplyFinalStepText(ReplyTextsStorage.Teacher.ProfileNewNameView(request));
        
        var keyboard = ReplyKeyboardsStorage.FinalStep;

        return new MessageToSend(response, keyboard, false);
    }
    
    public MessageToSend ProcessInputNextLessonDate(long chatId, TransmittedData transmittedData, string request)
    {
        if (!DateOnly.TryParse(request, out DateOnly date))
        {
            return new MessageToSend(ReplyTextsStorage.Teacher.ErrorDateTimeInput);
        }
        
        transmittedData.DataStorage.Add(ConstantsStorage.NextLessonDate, date);

        transmittedData.State.TeacherState = States.TeacherStates.InputNextLessonTime;

        var response = ReplyTextsStorage.Teacher.InputNextLessonTime(request);

        return new MessageToSend(response, false);
    }
    
    public MessageToSend ProcessInputNextLessonTime(long chatId, TransmittedData transmittedData, string request)
    {
        if (!TimeOnly.TryParse(request, out TimeOnly time))
        {
            return new MessageToSend(ReplyTextsStorage.Teacher.ErrorDateTimeInput);
        }

        if (!transmittedData.DataStorage.TryGet(ConstantsStorage.NextLessonDate, out Object lessonDateObj))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessInputNextLessonTime"));
        }
        
        DateOnly date = (DateOnly)lessonDateObj;

        var fullDate = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        
        transmittedData.DataStorage.Add(ConstantsStorage.NextLessonDateFull, fullDate);

        transmittedData.State.TeacherState = States.TeacherStates.InputNextLessonFinalStep;
        
        var response = GetReplyFinalStepText(ReplyTextsStorage.Teacher.GetNewNextLessonDateView(fullDate.ToString()));
        
        var keyboard = ReplyKeyboardsStorage.FinalStep;

        return new MessageToSend(response, keyboard, false);
    }

    public MessageToSend ProcessInputGroupInviteCode(long chatId, TransmittedData transmittedData, string request)
    {
        transmittedData.DataStorage.TryGet(ConstantsStorage.GroupName, out Object groupName);
        
        string response = GetReplyFinalStepText(ReplyTextsStorage.Teacher.GetGroupFinalStateView((string)groupName, request));

        if (InputGroupInviteCode(out MessageToSend messageToSend, request, response))
        {
            transmittedData.DataStorage.Add(ConstantsStorage.GroupInviteCode, request);

            transmittedData.State.TeacherState = States.TeacherStates.GroupCreateFinalStep;
            
            return new MessageToSend(messageToSend.Text, ReplyKeyboardsStorage.FinalStep);
        }

        return messageToSend;
    }
    
    public MessageToSend ProcessChooseGroupForEdit(long chatId, TransmittedData transmittedData, string request)
    {
        string response = ReplyTextsStorage.Teacher.EditGroup;

        transmittedData.State.TeacherState = States.TeacherStates.EditGroup;
        
        if (!DbManager.GetInstance().TableCourses.TryGetCourseByStudentInviteCode(out Course course, request))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessChooseGroupForEdit"));
            throw new Exception();
        }
        
        transmittedData.DataStorage.Add(ConstantsStorage.Course, course);

        var keyboard = ReplyKeyboardsStorage.Teacher.EditGroup;

        return new MessageToSend(response, keyboard, false);
    }
    
    public MessageToSend ProcessChooseGroupForHomework(long chatId, TransmittedData transmittedData, string request)  
    {
        string response = ReplyTextsStorage.Teacher.InputHomework;

        transmittedData.State.TeacherState = States.TeacherStates.InputHomework;
        
        if (!DbManager.GetInstance().TableCourses.TryGetCourseByStudentInviteCode(out Course course, request))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessChooseGroupForEdit"));
            throw new Exception();
        }
        
        transmittedData.DataStorage.Add(ConstantsStorage.Course, course);

        return new MessageToSend(response, false);
    }
    
    public MessageToSend ProcessChooseGroupForNextLesson(long chatId, TransmittedData transmittedData, string request)
    {
        string response = ReplyTextsStorage.Teacher.InputNextLessonDate(request);

        transmittedData.State.TeacherState = States.TeacherStates.InputNextLesson;
        
        if (!DbManager.GetInstance().TableCourses.TryGetCourseByStudentInviteCode(out Course course, request))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessChooseGroupForEdit"));
            throw new Exception();
        }
        
        transmittedData.DataStorage.Add(ConstantsStorage.Course, course);

        return new MessageToSend(response, false);
    }

    public MessageToSend ProcessEditGroupName(long chatId, TransmittedData transmittedData, string request)
    {
        string response = GetReplyFinalStepText(ReplyTextsStorage.Teacher.GetNewGroupNameView(request));
        
        if (InputGroupName(out MessageToSend messageToSend, request, response))
        {
            transmittedData.DataStorage.Add(ConstantsStorage.GroupName, request);
            
            transmittedData.State.TeacherState = States.TeacherStates.EditGroupNameFinalStep;
            
            var keyboard = ReplyKeyboardsStorage.FinalStep;
        
            return new MessageToSend(messageToSend.Text, keyboard);
        }

        return messageToSend;
    }
    
    public MessageToSend ProcessEditGroupInviteCode(long chatId, TransmittedData transmittedData, string request)
    {
        string response = GetReplyFinalStepText(ReplyTextsStorage.Teacher.GetNewGroupInviteCodeView(request));
        
        if (InputGroupInviteCode(out MessageToSend messageToSend, request, response))
        {
            transmittedData.DataStorage.Add(ConstantsStorage.GroupInviteCode, request);
            
            transmittedData.State.TeacherState = States.TeacherStates.EditGroupInviteCodeFinalStep;
            
            var keyboard = ReplyKeyboardsStorage.FinalStep;
        
            return new MessageToSend(messageToSend.Text, keyboard);
        }
        
        return messageToSend;
    }
    
    /*public MessageToSend ProcessEditName(long chatId, TransmittedData transmittedData, string request)         //
    {
        string response = GetReplyFinalStepText(ReplyTextsStorage.Teacher.GetNewGroupNameView(request));
        
        if (InputGroupName(out MessageToSend messageToSend, request, response))
        {
            transmittedData.DataStorage.Add(ConstantsStorage.GroupName, request);
            
            transmittedData.State.TeacherState = States.TeacherStates.EditGroupNameFinalStep;
            
            var keyboard = ReplyKeyboardsStorage.FinalStep;
        
            return new MessageToSend(messageToSend.Text, keyboard);
        }

        return messageToSend;
    }
    
    public MessageToSend ProcessProfileLogOut(long chatId, TransmittedData transmittedData, string request)         //
    {
        string response = GetReplyFinalStepText(ReplyTextsStorage.Teacher.GetNewGroupNameView(request));
        
        if (InputGroupName(out MessageToSend messageToSend, request, response))
        {
            transmittedData.DataStorage.Add(ConstantsStorage.GroupName, request);
            
            transmittedData.State.TeacherState = States.TeacherStates.EditGroupNameFinalStep;
            
            var keyboard = ReplyKeyboardsStorage.FinalStep;
        
            return new MessageToSend(messageToSend.Text, keyboard);
        }

        return messageToSend;
    }*/
    
    #endregion
    #region ButtonsMethods

    // public MessageToSend ProcessButtons(long chatId, TransmittedData transmittedData, string request)
    // {
    //     if (!_requestMethodsPairs.ContainsKey(request))
    //     {
    //         Logger.Error($"Not found methods to process '{request}'");
    //         throw new Exception();
    //     }
    //     
    //     return _requestMethodsPairs[request].Invoke(chatId, transmittedData);
    // }

    public MessageToSend ProcessButtonSetHomework(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Teacher.GroupHomework;

        transmittedData.State.TeacherState = States.TeacherStates.ChooseGroupForHomework;

        var keyboard = GetTeacherCoursesButtons(chatId, transmittedData.DataStorage);

        return new MessageToSend(response, keyboard, false);
    }
    
    public MessageToSend ProcessButtonDateNextLesson(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Teacher.GroupNextLesson;

        transmittedData.State.TeacherState = States.TeacherStates.ChooseGroupForNextLesson;

        var keyboard = GetTeacherCoursesButtons(chatId, transmittedData.DataStorage);
        
        return new MessageToSend(response, keyboard, false);
    }
    
    public MessageToSend ProcessButtonGroups(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Teacher.Groups;
        
        InlineKeyboardMarkup keyboard;

        if (!transmittedData.DataStorage.TryGet(ConstantsStorage.TeacherId, out object obj))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessGroups"));
        }

        int teacherId = (int)obj;

        if (DbManager.GetInstance().TableCourses.TryGetTeacherCourses(out List<Course> courses, teacherId))
        {
            keyboard = BotKeyboardCreator.GetInstance()
                .GetKeyboardMarkup(ReplyButtonsStorage.Teacher.CreateGroup, ReplyButtonsStorage.Teacher.EditGroup, ReplyButtonsStorage.MainMenu);
        }
        else
        {
            keyboard = BotKeyboardCreator.GetInstance()
                .GetKeyboardMarkup(ReplyButtonsStorage.Teacher.CreateGroup, ReplyButtonsStorage.MainMenu);
        }
        
        transmittedData.State.TeacherState = States.TeacherStates.Groups;
        
        return new MessageToSend(response, keyboard, false);
    }
    
    public MessageToSend ProcessButtonProfile(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Teacher.Profile;
        
        if (!DbManager.GetInstance().TableTeachers.TryGetTeacherByChatId(out Teacher teacher, chatId))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessButtonProfile"));
        }
        
        response += "\n" + $"??????????????: {teacher.Name}";
        
        InlineKeyboardMarkup keyboard = ReplyKeyboardsStorage.Teacher.Profile;
        
        transmittedData.State.TeacherState = States.TeacherStates.Groups;
        
        return new MessageToSend(response, keyboard, false);
    }
    
    public MessageToSend ProcessButtonEditProfile(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Teacher.EditName;

        transmittedData.State.TeacherState = States.TeacherStates.EditProfile;

        return new MessageToSend(response, false);
    }
    
    public MessageToSend ProcessButtonProfileLogOut(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Teacher.ProfileLogOut;

        transmittedData.State.TeacherState = States.TeacherStates.LogOutFinalStep;     //?????????????????????????? ?? ?????????? None

        var keyboard = ReplyKeyboardsStorage.FinalStep;

        return new MessageToSend(response, keyboard, false);
    }
    
    public MessageToSend ProcessButtonMainMenu(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.MainMenu;

        InlineKeyboardMarkup keyboard = ReplyKeyboardsStorage.Teacher.MainMenu;
        
        transmittedData.State.TeacherState = States.TeacherStates.MainMenu;
        
        return new MessageToSend(response, keyboard, false);
    }
    
    public MessageToSend ProcessButtonCreateGroup(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Teacher.InputGroupName;

        transmittedData.State.TeacherState = States.TeacherStates.InputGroupName;
        
        return new MessageToSend(response, false);
    }
    
    public MessageToSend ProcessButtonYes(long chatId, TransmittedData transmittedData)
    {
        var state = transmittedData.State;      //???????????????? ??????????????????
        var storage = transmittedData.DataStorage;      //???????????????? storage 
        MessageToSend messageToSend;
        
        switch (transmittedData.State.TeacherState)
        {
            case States.TeacherStates.GroupCreateFinalStep:
            {
                var tableCourses = DbManager.GetInstance().TableCourses;
                
                bool isGroupNameEnab = storage.TryGet(ConstantsStorage.GroupName, out Object groupName);
                bool isInviteCodeEnab = storage.TryGet(ConstantsStorage.GroupInviteCode, out Object inviteCode);
                bool isTeacherIdEnab = storage.TryGet(ConstantsStorage.TeacherId, out Object teacherId);
        
                if (!isGroupNameEnab || !isInviteCodeEnab || !isTeacherIdEnab)
                {
                    Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessInputGroupInviteCode"));
                }
                
                tableCourses.CreateCourse((string)groupName, (string)inviteCode, (int)teacherId);
                
                messageToSend = new MessageToSend(ReplyTextsStorage.Teacher.GroupCreated, false);
            }
                break;
            case States.TeacherStates.EditGroupNameFinalStep:
            {
                var tableCourses = DbManager.GetInstance().TableCourses;
                
                bool isGroupNameEnab = storage.TryGet(ConstantsStorage.GroupName, out Object groupName);
                bool isCourseEnab = storage.TryGet(ConstantsStorage.Course, out Object course);
        
                if (!isGroupNameEnab || !isCourseEnab)
                {
                    Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessInputGroupInviteCode"));
                }
                
                tableCourses.UpdateCourseName((string)groupName, ((Course)course).Id);
                
                messageToSend = new MessageToSend(ReplyTextsStorage.AppliedChanges, false);
            }
                break;
            case States.TeacherStates.EditGroupInviteCodeFinalStep:
            {
                var tableCourses = DbManager.GetInstance().TableCourses;
                
                bool isInviteCodeEnab = storage.TryGet(ConstantsStorage.GroupInviteCode, out Object inviteCode);
                bool isCourseEnab = storage.TryGet(ConstantsStorage.Course, out Object course);
                
                if (!isInviteCodeEnab || !isCourseEnab)
                {
                    Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessInputGroupInviteCode"));
                }
                
                tableCourses.UpdateCourseInviteCode((string)inviteCode, ((Course)course).Id);
                
                messageToSend = new MessageToSend(ReplyTextsStorage.AppliedChanges, false);
            }
                break;
            case States.TeacherStates.HomeworkFinalStep:
            {
                var tableCourses = DbManager.GetInstance().TableCourses;
                
                bool isHomeworkEnab = storage.TryGet(ConstantsStorage.Homework, out Object homework);
                bool isCourseEnab = storage.TryGet(ConstantsStorage.Course, out Object course);
        
                if (!isHomeworkEnab || !isCourseEnab)
                {
                    Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessInputHomework"));
                }
                
                tableCourses.UpdateCourseHomework((string)homework, ((Course)course).Id);
                
                messageToSend = new MessageToSend(ReplyTextsStorage.Teacher.HomeworkCreated, false);
            }
                break;
            case States.TeacherStates.InputNextLessonFinalStep:
            {
                var tableCourses = DbManager.GetInstance().TableCourses;
                
                bool isDateNextLessonEnab = storage.TryGet(ConstantsStorage.NextLessonDateFull, out Object nextLessonObj);
                bool isCourseEnab = storage.TryGet(ConstantsStorage.Course, out Object course);
        
                if (!isDateNextLessonEnab || !isCourseEnab)
                {
                    Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessInputNextLessonTime ???????? ProcessInputNextLessonDate"));
                }

                tableCourses.UpdateCourseNextLessonTime((DateTime)nextLessonObj, ((Course)course).Id);
                
                messageToSend = new MessageToSend(ReplyTextsStorage.Teacher.NextLessonDateCreated, false);
            }
                break;
            case States.TeacherStates.EditProfileFinalStep:
            {
                var tableCourses = DbManager.GetInstance().TableCourses;
                
                bool isTeacherIdEnab = storage.TryGet(ConstantsStorage.TeacherId, out Object teacherId);
                bool isNeacherNameEnab = storage.TryGet(ConstantsStorage.TeacherName, out Object teacherName);
        
                if (!isTeacherIdEnab || !isNeacherNameEnab)
                {
                    Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessInputHomework"));
                }
                
                tableCourses.UpdateTeacherName((string)teacherName, (int)teacherId); //
                
                messageToSend = new MessageToSend(ReplyTextsStorage.Teacher.NameEdited, false);
            }
                break;
            case States.TeacherStates.LogOutFinalStep:
            {
                var tableCourses = DbManager.GetInstance().TableCourses;
                
                bool isTeacherIdEnab = storage.TryGet(ConstantsStorage.TeacherId, out Object teacherId);
                
                if (!isTeacherIdEnab)
                {
                    Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessInputHomework"));
                }
                
                transmittedData.State.Reset();
                
                var quitAccountMessageToSend = new MessageToSend(ReplyTextsStorage.Teacher.QuitAccount);
                
                BotMessageManager.GetInstance().GetSender(chatId).AddMessageToStack(quitAccountMessageToSend);
                
                tableCourses.TeacherLogOut((int)teacherId);
                
                StartedService.Logout(chatId);
                
                return new MessageToSend(ReplyTextsStorage.CmdStart);
            }
                break;
            default:
            {
                Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessButtonYes"));
                throw new Exception();
            }
        }
        
        BotMessageManager.GetInstance().GetSender(chatId).AddMessageToStack(messageToSend);
                
        state.TeacherState = States.TeacherStates.MainMenu;
        
        string response = ReplyTextsStorage.MainMenu;
        var keyboard = ReplyKeyboardsStorage.Teacher.MainMenu;
        
        return new MessageToSend(response, keyboard);
    }
    
    public MessageToSend ProcessButtonNo(long chatId, TransmittedData transmittedData)
    {
        var state = transmittedData.State;
        MessageToSend messageToSend;
        
        switch (transmittedData.State.TeacherState)
        {
            case States.TeacherStates.GroupCreateFinalStep:
            {
                messageToSend = new MessageToSend(ReplyTextsStorage.Teacher.GroupNotCreated, false);
            }
                break;
            case States.TeacherStates.EditGroupNameFinalStep:
            case States.TeacherStates.EditGroupInviteCodeFinalStep:
            {
                messageToSend = new MessageToSend(ReplyTextsStorage.DeclinedChanges, false);
            }
                break;
            case States.TeacherStates.HomeworkFinalStep:
            {
                messageToSend = new MessageToSend(ReplyTextsStorage.Teacher.HomeworkNotCreated, false);
            }
                break;
            case States.TeacherStates.InputNextLessonFinalStep:
            {
                messageToSend = new MessageToSend(ReplyTextsStorage.Teacher.NextLessonDateNotCreated, false);
            }
                break;
            case States.TeacherStates.EditProfileFinalStep:
            {
                messageToSend = new MessageToSend(ReplyTextsStorage.Teacher.NameNotEdited, false);
            }
                break;
            case States.TeacherStates.LogOutFinalStep:
            {
                messageToSend = new MessageToSend(ReplyTextsStorage.Teacher.NotLogOut, false);
            }
                break;
            default:
            {
                Logger.Error(LoggerTextsStorage.FatalLogicError("ProcessButtonNo"));
                throw new Exception();
            }
        }
        
        BotMessageManager.GetInstance().GetSender(chatId).AddMessageToStack(messageToSend);

        state.TeacherState = States.TeacherStates.MainMenu;     //?????????????? ?????????????????? ?? ???????????????? ????????
        
        string response = ReplyTextsStorage.MainMenu;   //... ???????????????? ??????????????????
        var keyboard = ReplyKeyboardsStorage.Teacher.MainMenu;      //... ????????????????????
        
        return new MessageToSend(response, keyboard);
    }
    
    public MessageToSend ProcessButtonEditGroup(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Teacher.Groups;

        transmittedData.State.TeacherState = States.TeacherStates.ChooseGroupForEdit;

        var keyboard = GetTeacherCoursesButtons(chatId, transmittedData.DataStorage);
        
        return new MessageToSend(response, keyboard, false);
    }
    
    public MessageToSend ProcessButtonEditGroupName(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Teacher.InputGroupName;

        transmittedData.State.TeacherState = States.TeacherStates.EditGroupName;

        return new MessageToSend(response, false);
    }
    
    public MessageToSend ProcessButtonEditGroupInviteCode(long chatId, TransmittedData transmittedData)
    {
        string response = ReplyTextsStorage.Teacher.InputGroupInviteCode;

        transmittedData.State.TeacherState = States.TeacherStates.EditGroupInviteCode;

        return new MessageToSend(response, false);
    }

    #endregion
    #region HelpMethods

    private InlineKeyboardMarkup GetTeacherCoursesButtons(long chatId, DataStorage dataStorage)
    {
        int teacherId;
        
        if (dataStorage.TryGet(ConstantsStorage.TeacherId, out Object objTeacherId))
        {
            teacherId = (int)objTeacherId;
        }
        else
        {
            DbManager.GetInstance().TableTeachers.TryGetTeacherByChatId(out Teacher teacher, chatId);

            teacherId = teacher.Id;
        }

        if (!DbManager.GetInstance().TableCourses.TryGetTeacherCourses(out List<Course> courses, teacherId))
        {
            Logger.Error(LoggerTextsStorage.FatalLogicError("GetTeacherCoursesButtons"));
            throw new Exception();
        }

        List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
        for (int i = 0; i < courses.Count; i++)
        {
            buttons.Add(InlineKeyboardButton.WithCallbackData(courses[i].CourseName, courses[i].StudentInviteCode));
        }

        return BotKeyboardCreator.GetInstance().GetKeyboardMarkup(buttons.ToArray());
    }
    
    private bool InputGroupName(out MessageToSend messageToSend, string request, string succesReplyText)
    {
        if (!DbManager.GetInstance().TableCourses.IsCourseNameUnique(request))
        {
            messageToSend = new MessageToSend(ReplyTextsStorage.Teacher.InputAnotherGroupName);

            return false;
        }
        
        messageToSend = new MessageToSend(succesReplyText);

        return true;
    }
    
    private bool InputGroupInviteCode(out MessageToSend messageToSend, string request, string succesReplyText)
    {
        if (!DbManager.GetInstance().TableCourses.IsCourseInviteCodeUnique(request))
        {
            messageToSend = new MessageToSend(ReplyTextsStorage.Teacher.InputAnotherGroupInviteCode);

            return false;
        }
        
        messageToSend = new MessageToSend(succesReplyText);

        return true;
    }

    private string GetReplyFinalStepText(string value) // todo ???????????????? ?????????????????? ?????????????????? HelpMethods ?? StartedService (like public static)
    {
        return ReplyTextsStorage.FinalStep + "\n" + value;
    }

    #endregion
}