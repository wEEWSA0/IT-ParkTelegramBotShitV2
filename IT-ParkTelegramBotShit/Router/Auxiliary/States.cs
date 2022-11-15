namespace IT_ParkTelegramBotShit.Router.Auxiliary;

public class States
{
    public enum GlobalStates
    {
        CmdStart = 1,
        EnterCode = 2,
        Other = 0
    }

    public GlobalStates GlobalState;
    
    public enum StudentStates
    {
        None = 0,
        MainMenu = 1
    }
    
    public StudentStates StudentState;
    
    public enum TeacherStates
    {
        None = 0,
        MainMenu = 1,
        
        #region Groups
        
        Groups = 10,
        
        #region AddGroup
        
        InputGroupName = 100,
        InputGroupInviteCode = 1000,
        GroupCreateFinalStep = 10000,
        
        #endregion
        
        #region EditGroup

        ChooseGroupForEdit = 101,
        EditGroup = 1010,
        
        EditGroupName = 10100,
        EditGroupInviteCode = 10101,
        DeleteGroup = 10102,
        
        EditGroupNameFinalStep = 101000,
        EditGroupInviteCodeFinalStep = 101010,
        DeleteGroupFinalStep = 10102,
        
        #endregion
        
        #endregion

        #region AddHomework

        InputHomework = 30,
        ChooseGroupForHomework = 300,
        HomeworkFinalStep = 3000,

        #endregion

        #region DateNextLesson

        InputNextLesson = 40,
        ChooseGroupForNextLesson = 400,
        InputNextLessonFinalStep = 4000

        #endregion
    }
    
    public TeacherStates TeacherState;

    public void Reset()
    {
        GlobalState = GlobalStates.CmdStart;
        StudentState = StudentStates.None;
        TeacherState = TeacherStates.None;
    }

    public string GetCurrentStateName()
    {
        if (GlobalState != GlobalStates.Other)
        {
            return GlobalState.ToString() + " in GlobalState";
        }
        else if (StudentState != StudentStates.None)
        {
            return StudentState.ToString() + " in StudentState";
        }
        else if (TeacherState != TeacherStates.None)
        {
            return TeacherState.ToString() + " in TeacherState";
        }
        else
        {
            throw new Exception("Logic error in States");
        }
    }
}