// using IT_ParkTelegramBotShit.Bot;
// using IT_ParkTelegramBotShit.Shutdown;
//
// ShutdownProgram shutdownProgram = new ShutdownProgram();
// shutdownProgram.Start();

var tcs = new TaskCompletionSource();

File.WriteAllText("start.txt","start");

AppDomain.CurrentDomain.ProcessExit += (_, _) =>
{
    File.WriteAllText("SIGTERM.txt","SIGTERM");
    tcs.SetResult();
};

await tcs.Task;

File.WriteAllText("finish.txt","finish");



// BotsManager botsManager = BotsManager.GetInstance();
// Bot bot = new Bot();
//
// botsManager.AddBot(bot);
// bot.Start();
//
// Console.WriteLine("Press enter for stop");
//
// // todo разбираться
// exitEvent.WaitOne();
//
// Console.WriteLine("Bot stopped");