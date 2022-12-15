using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Router.Transmitted;
using IT_ParkTelegramBotShit.Service.ServiceStateToMethod;

namespace IT_ParkTelegramBotShitTest.ServiceStateToMethod;

public class TeacherStateManagerTest
{
    private TeacherStateManager _teacherStateManager = new TeacherStateManager();
    
    [Fact]
    public void IsNotExistingRequestReturnEmptyMessageToSendInProcessCallbackMethod()
    {
        // Подготовка
        TransmittedData transmittedData = new TransmittedData();
        string notExistingRequest = "s-asd-";
        long chatId = 0;
        MessageToSend expected = MessageToSend.Empty();
        
        // Тестирование
        var actual = _teacherStateManager.ProcessCallback(chatId, transmittedData, notExistingRequest);
        
        // Проверка
        Assert.NotStrictEqual(expected, actual);
    }
}