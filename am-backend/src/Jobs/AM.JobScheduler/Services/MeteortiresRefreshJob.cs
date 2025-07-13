
using AM.JobScheduler.Interfaces;

namespace AM.JobScheduler.Services;

public class MeteortiresRefreshJob(
      IServiceProvider services
    )
    : IHostedService, IAsyncDisposable
{
    private readonly Task _completedTask = Task.CompletedTask;
    private int _executionCount = 0;
    private Timer? _timer;

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(DoWorkAsync, null, TimeSpan.Zero, TimeSpan.FromSeconds(1000));

        return _completedTask;
    }

    private async void DoWorkAsync(object? state)
    {
        int count = Interlocked.Increment(ref _executionCount);

        await using var scope = services.CreateAsyncScope();
        var meteoriteFetchService = scope.ServiceProvider
            .GetRequiredService<IMeteoriteFetchService>();

        await meteoriteFetchService.FetchMeteorites();
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
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
