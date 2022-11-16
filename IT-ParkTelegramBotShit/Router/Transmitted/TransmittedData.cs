namespace IT_ParkTelegramBotShit.Router.Transmitted;

public class TransmittedData
{
    public States State { get; set; }
    public DataStorage DataStorage { get; }

    public TransmittedData()
    {
        State = new States();
        State.GlobalState = States.GlobalStates.CmdStart;
        DataStorage = new DataStorage();
    }
}