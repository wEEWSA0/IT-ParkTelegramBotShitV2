namespace IT_ParkTelegramBotShit.Bot.Notifications;

public enum NotificationType // todo notification type functional realization
{
    OneTime,
    Regular,
    UntilAction, // актуально только сегодня до 8:30 (после просто удаляется), а вызов назначен на 5:00
    WhileAction // актуально в течение 1 часа
}