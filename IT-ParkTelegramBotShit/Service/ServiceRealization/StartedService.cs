using IT_ParkTelegramBotShit.Router.Auxiliary;
using IT_ParkTelegramBotShit.Util;
using NLog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IT_ParkTelegramBotShit.Service.ServiceRealization;

public class StartedService
{
    private static ILogger Logger = LogManager.GetCurrentClassLogger();
    
    public Task ProcessCommandStart(long chatId, TransmittedData transmittedData, Message message,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        Logger.Info($"Старт метода ProcessCommandStart для chatId = {chatId}");

        string requestMessageText = message.Text;

        string responseMessageText = StateStringsStorage.Empty;

        if (requestMessageText != StateStringsStorage.Start)
        {
            responseMessageText = ReplyTextsStorage.CmsStart;

            Logger.Info($"Команда не распознана. Метод ProcessCommandStart. chatId = {chatId}");

            Task taskError = botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
                cancellationToken: cancellationToken);

            Logger.Info($"Отправлено сообщение об ошибке пользователю. Метод ProcessCommandStart. chatId = {chatId}");

            return taskError;
        }

        transmittedData.State = State.EnterCode;
        
        responseMessageText = ReplyTextsStorage.CmsStart;

        Task taskSuccess = botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseMessageText,
            cancellationToken: cancellationToken);

        Logger.Info($"Отправлено корректное сообщение пользователю. Метод ProcessCommandStart. chatId = {chatId}");

        return taskSuccess;
    }
    
    public Task ProcessCommandEnterCode(long chatId, TransmittedData transmittedData, Message message,
        ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        Logger.Info($"Старт метода ProcessCommandStart для chatId = {chatId}");

        string requestMessageText = message.Text;

        string responseMessageText = StateStringsStorage.Empty;

        if (true) // проверка кода в бд
        {
            responseMessageText = ReplyTextsStorage.NotValidCode;

            Logger.Info($"Команда не распознана. Метод ProcessCommandStart. chatId = {chatId}");

            Task taskError = botClient.SendTextMessageAsync(
                chatId: chatId,
                text: responseMessageText,
                cancellationToken: cancellationToken);

            Logger.Info($"Отправлено сообщение об ошибке пользователю. Метод ProcessCommandStart. chatId = {chatId}");

            return taskError;
        }

        transmittedData.State = State.EnterCode;
        
        responseMessageText = ReplyTextsStorage.CmsStart;

        Task taskSuccess = botClient.SendTextMessageAsync(
            chatId: chatId,
            text: responseMessageText,
            cancellationToken: cancellationToken);

        Logger.Info($"Отправлено корректное сообщение пользователю. Метод ProcessCommandStart. chatId = {chatId}");

        return taskSuccess;
    }
}