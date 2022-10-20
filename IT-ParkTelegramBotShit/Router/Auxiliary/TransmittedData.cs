namespace IT_ParkTelegramBotShit.Router.Auxiliary;

public class TransmittedData
{
    public State State { get; set; }
    public DataStorage DataStorage { get; }

    public TransmittedData()
    {
        State = State.CmdStart;
        DataStorage = new DataStorage();
    }
}