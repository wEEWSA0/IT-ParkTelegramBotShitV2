using IT_ParkTelegramBotShit.Bot;
using Microsoft.Extensions.Hosting;
namespace IT_ParkTelegramBotShit.Shutdown;

public class ShutdownService : IHostedService
{
    private const int CheckTime = 50;
    
    private bool _isShouldStop;
    private Task _backgroundTask;
    private readonly IHostApplicationLifetime _applicationLifetime;

    public ShutdownService(IHostApplicationLifetime applicationLifetime)
    {
        _applicationLifetime = applicationLifetime;
    }

    public Task StartAsync(CancellationToken _)
    {
        Console.WriteLine("Starting service");

        _backgroundTask = Task.Run(async () =>
        {
            while (!_isShouldStop)
            {
                await Task.Delay(CheckTime);
            }

            Console.WriteLine("Background task gracefully stopped");
        });

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stopping service");

        Stopping();
        
        _isShouldStop = true;
        await _backgroundTask;
        
        Console.WriteLine("Service stopped");
    }

    private void Stopping()
    {
        BotsManager.GetInstance().Stop();
    }
}