using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace IT_ParkTelegramBotShit.Shutdown;

public class ShutdownProgram
{
    public async void Start()
    {
        var host = new HostBuilder()
            .ConfigureServices((hostContext, services) => services.AddHostedService<ShutdownService>())
            .UseConsoleLifetime()
            .Build();

        await host.RunAsync();
    }
}