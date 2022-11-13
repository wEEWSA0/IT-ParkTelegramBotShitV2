namespace IT_ParkTelegramBotShit.Util;

public static class CallbackQueryStorage
{
    public static readonly TeacherCallbackQueryStorage Teacher = new TeacherCallbackQueryStorage();
    
    public const string MainMenu = "main_menu_callback";
    public const string Yes = "yes_callback";
    public const string No = "no_callback";
    //public const string Logout = "logout_from_system_callback";
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