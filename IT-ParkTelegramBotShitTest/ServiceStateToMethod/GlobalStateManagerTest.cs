using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Router.Transmitted;
using IT_ParkTelegramBotShit.Service.ServiceStateToMethod;

namespace IT_ParkTelegramBotShitTest.ServiceStateToMethod;

public class GlobalStateManagerTest
{
    private GlobalStateManager _globalStateManager = new GlobalStateManager();
    
    [Fact]
    public void IsNotExistingRequestReturnEmptyMessageToSendInProcessCallbackMethod()
    {
        // Подготовка
        TransmittedData transmittedData = new TransmittedData();
        string notExistingRequest = "dsf";
        long chatId = 0;
        MessageToSend expected = MessageToSend.Empty();
        
        // Тестирование
        var actual = _globalStateManager.ProcessBotUpdate(chatId, transmittedData, notExistingRequest);
        
        // Проверка
        Assert.NotStrictEqual(expected, actual);
    }
}