using IT_ParkTelegramBotShit.Bot;
using IT_ParkTelegramBotShit.Shutdown;

ShutdownProgram shutdownProgram = new ShutdownProgram();
shutdownProgram.Start();

ManualResetEvent exitEvent = new ManualResetEvent(false);

Console.CancelKeyPress += (sender, eventArgs) =>
{
    eventArgs.Cancel = true;
    exitEvent.Set();
};

BotsManager botsManager = BotsManager.GetInstance();
Bot bot = new Bot();

botsManager.AddBot(bot);
bot.Start();

Console.WriteLine("Press enter for stop");

// todo разбираться
exitEvent.WaitOne();

Console.WriteLine("Bot stopped");