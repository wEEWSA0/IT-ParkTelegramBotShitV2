using IT_ParkTelegramBotShit.Router.Auxiliary;

namespace IT_ParkTelegramBotShit.Util;

public class LoggerTextsStorage
{
    public static string LostServiceMethod(long chatId, TransmittedData transmittedData)
    {
        return $"Вызван метод ProcessBotUpdate: chatId = {chatId}, " +
            $"состояние = {transmittedData.State.GetCurrentStateName()}, " +
            $"функция для обработки не найдена, вернется пустое сообщение";
    }
    
    public static string FoundServiceMethod(long chatId, TransmittedData transmittedData, string methodName)
    {
        return $"Вызван метод ProcessBotUpdate: chatId = {chatId}, " +
               $"состояние = {transmittedData.State.GetCurrentStateName()}, " +
               $"функция для обработки = {methodName}";
    }
    
    public static string FatalLogicError(string methodName)
    {
        return $"В методе {methodName} произошла ошибка, связанная с логикой взаимодействия методов в программе";
    }
}