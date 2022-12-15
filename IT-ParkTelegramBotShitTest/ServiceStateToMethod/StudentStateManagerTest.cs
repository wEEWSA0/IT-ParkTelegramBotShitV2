using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Router.Transmitted;
using IT_ParkTelegramBotShit.Service.ServiceStateToMethod;

namespace IT_ParkTelegramBotShitTest.ServiceStateToMethod;

public class StudentStateManagerTest
{
    private StudentStateManager _studentStateManager = new StudentStateManager();
    
    [Fact]
    public void IsNotExistingRequestReturnEmptyMessageToSendInProcessCallbackMethod()
    {
        // Подготовка
        TransmittedData transmittedData = new TransmittedData();
        string notExistingRequest = "sadfsfdasgregdf";
        long chatId = 0;
        MessageToSend expected = MessageToSend.Empty();
        
        // Тестирование
        var actual = _studentStateManager.ProcessCallback(chatId, transmittedData, notExistingRequest);
        
        // Проверка
        Assert.NotStrictEqual(expected, actual);
    }
    
    [Fact]
    public void IsNotExistingStateThrowExeptionEmptyMessageToSendInProcessCallbackMethod()
    {
        // Подготовка
        TransmittedData transmittedData = new TransmittedData();
        transmittedData.State.GlobalState = States.GlobalStates.Other;
        
        string request = "7868678";
        long chatId = 0;
        MessageToSend expected = MessageToSend.Empty();
        
        // Тестирование
        // Проверка
        Assert.Throws<Exception>(() => _studentStateManager.ProcessCallback(chatId, transmittedData, request));
    }
}