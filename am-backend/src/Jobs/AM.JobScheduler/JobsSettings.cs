namespace AM.JobScheduler
{
    public class JobsSettings
    {
        public int MeteoritesFetchInSeconds { get; set; } = 3600;

        public string MeteoritesDatasetUrl { get; set; } = string.Empty;
    }
}
