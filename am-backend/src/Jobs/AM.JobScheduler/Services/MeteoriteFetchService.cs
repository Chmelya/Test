using AM.Application.Common.Interfaces.Repositories;
using AM.Domain.Enities;
using AM.JobScheduler.Interfaces;
using System.Text.Json;

namespace AM.JobScheduler.Services
{
    public class MeteoriteFetchService(
        HttpClient httpClient,
        IMeteoriteRepository meteoriteRepository,
        IGeolocationRepository geolocationRepository)
        : IMeteoriteFetchService
    {
        public async Task FetchMeteorites()
        {
            try
            {
                //TODO : To config
                var meteorites = await httpClient.GetFromJsonAsync<IList<Meteorite>>("https://raw.githubusercontent.com/biggiko/nasa-dataset/refs/heads/main/y77d-th95.json");
                if (meteorites is null)
                {
                    return;
                }

                await meteoriteRepository.BulkInsertOrUpdateAsync(meteorites, config =>
                {
                    config.BatchSize = 1000;
                    config.SetOutputIdentity = false;
                });

                var geolocations = meteorites
                    .Where(m => m.Geolocation != null)
                    .Select(m =>
                    {
                        m.Geolocation!.MeteoriteId = m.Id;
                        return m.Geolocation;
                    })
                    .ToList();

                await geolocationRepository.BulkInsertOrUpdateAsync(geolocations, config =>
                {
                    config.BatchSize = 1000;
                    config.UpdateByProperties =
                    [
                        nameof(Geolocation.MeteoriteId)
                    ];
                });
            }

            catch (HttpRequestException ex)
            {
                throw;
            }
            catch (JsonException ex)
            {
                throw;
            }
        }
    }
}
