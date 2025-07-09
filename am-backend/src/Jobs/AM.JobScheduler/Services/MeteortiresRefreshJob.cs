
using AM.JobScheduler.Interfaces;

namespace AM.JobScheduler.Services;

public class MeteortiresRefreshJob(
      //ILogger<MeteortiresRefreshJob> logger,
      IServiceProvider services
    )
    : IHostedService, IAsyncDisposable
{
    private readonly Task _completedTask = Task.CompletedTask;
    private int _executionCount = 0;
    private Timer? _timer;

    public Task StartAsync(CancellationToken stoppingToken)
    {
    //    logger.LogInformation("{Service} is running.", nameof(MeteortiresRefreshJob));
        _timer = new Timer(DoWorkAsync, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        return _completedTask;
    }

    private async void DoWorkAsync(object? state)
    {
        int count = Interlocked.Increment(ref _executionCount);
        //logger.LogInformation(
        //    "{Service} is working, execution count: {Count:#,0}",
        //    nameof(MeteortiresRefreshJob),
        //    count);
        await using var scope = services.CreateAsyncScope();
        var meteoriteFetchService = scope.ServiceProvider
            .GetRequiredService<IMeteoriteFetchService>();

        await meteoriteFetchService.FetchMeteorites();
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        //logger.LogInformation(
        //    "{Service} is stopping.", nameof(MeteortiresRefreshJob));
        _timer?.Change(Timeout.Infinite, 0);

        return _completedTask;
    }

    public async ValueTask DisposeAsync()
    {
        if (_timer is IAsyncDisposable timer)
        {
            await timer.DisposeAsync();
        }
        else
        {
            _timer?.Dispose();
        }
    }
}
