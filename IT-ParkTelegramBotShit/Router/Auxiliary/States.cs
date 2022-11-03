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
        MainMenu = 1
    }
    
    public TeacherStates TeacherState;

    public void Reset()
    {
        GlobalState = GlobalStates.CmdStart;
        StudentState = StudentStates.None;
        TeacherState = TeacherStates.None;
    }
}