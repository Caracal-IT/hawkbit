using Microsoft.Extensions.Hosting;

namespace Caracal.Device.UpdateManager.Services;

public sealed class UpdateService: BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Running UpdateService");
            await Task.Delay(3000, stoppingToken);
        }
    }
}