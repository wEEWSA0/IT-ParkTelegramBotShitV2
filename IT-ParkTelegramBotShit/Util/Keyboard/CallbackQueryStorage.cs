namespace IT_ParkTelegramBotShit.Util;

public static class CallbackQueryStorage
{
    public static readonly TeacherCallbackQueryStorage Teacher = new TeacherCallbackQueryStorage();
    public static readonly StudentCallbackQueryStorage Student = new StudentCallbackQueryStorage();
    
    public const string MainMenu = "main_menu_callback";
    public const string Yes = "yes_callback";
    public const string No = "no_callback";
}

public class TeacherCallbackQueryStorage
{
    #region MainMenu

    public readonly string Groups = "groups_callback_teacher";
    public readonly string AddHomework = "add_homework_callback_teacher";
    public readonly string Profile = "profile_callback_teacher";
    public readonly string AddNextLessonDate = "add_next_lesson_date_callback_teacher";

    #endregion
    #region Groups

    public readonly string CreateGroup = "create_group_callback_teacher";
    public readonly string EditGroup = "edit_group_callback_teacher";

    #endregion
    #region EditGroup

    public readonly string EditGroupName = "edit_group_name_callback_teacher";
    public readonly string EditGroupInviteCode = "edit_group_invite_code_callback_teacher";
    public readonly string DeleteGroup = "delete_group_callback_teacher";

    #endregion
    #region Profile

    public readonly string QuiteAccount = "quint_account_callback_teacher";
    public readonly string EditName = "edit_name_callback_teacher";

    #endregion
}

public class StudentCallbackQueryStorage
{
    #region NextLesson
        
    public readonly string NextLesson = "next_lesson_callback_student";
    public readonly string SkipNextLesson = "skip_next_lesson_callback_student";
        
    #endregion
        
    #region Homework
        
    public readonly string Homework = "homework_callback_student";
        
    #endregion
        
    #region Profile
        
    public readonly string Profile = "profile_callback_student";
    public readonly string QuitAccount = "quit_account(delete)_callback_student";
    public readonly string ChangeName = "change_name_callback_student";
    
    #endregion
}