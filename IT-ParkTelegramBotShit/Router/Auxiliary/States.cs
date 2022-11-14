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
        
        Groups = 10,
        
        InputGroupName = 100,
        InputGroupInviteCode = 101,
        GroupCreateFinalStep = 102,
        
        ChooseGroupForEdit = 200,
        EditGroup = 201,
        EditGroupName = 2010,
        EditGroupNameFinalStep = 20100,
        EditGroupInviteCode = 2011,
        EditGroupInviteCodeFinalStep = 20110,
        DeleteGroup = 2012,
        DeleteGroupFinalStep = 20120,
        
        InputHomework = 30,
        InputHomeworkFinalStep = 31,
        
        InputDataNextLesson = 40,
        InputTimeNextLessonFinalState = 41
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