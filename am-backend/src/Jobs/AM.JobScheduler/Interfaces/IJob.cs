namespace AM.JobScheduler.Interfaces
{
    public interface IJob
    {
        Task ScheduleJob(CancellationToken cancellationToken);

        Task StartAsync(CancellationToken cancellationToken);

        Task StopJob(CancellationToken cancellationToken);
    }
}
