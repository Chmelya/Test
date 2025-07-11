using AM.Application.Common.Interfaces.Repositories;
using AM.Domain.Enities;
using AM.JobScheduler.DTO;
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
                var meteoritesDto = await httpClient.GetFromJsonAsync<List<MeteoriteDto>>("https://raw.githubusercontent.com/biggiko/nasa-dataset/refs/heads/main/y77d-th95.json");
                if (meteoritesDto is null)
                {
                    return;
                }

                var meteorites = meteoritesDto.Select(dto =>
                {
                    var meteorite = new Meteorite();

                    foreach (var prop in typeof(Meteorite).GetProperties())
                    {
                        if(prop.Name == nameof(Meteorite.Year))
                        {
                            continue;
                        }

                        var dtoProp = typeof(MeteoriteDto).GetProperty(prop.Name);
                        if (dtoProp != null && prop.CanWrite)
                        {
                            var value = dtoProp.GetValue(dto);
                            prop.SetValue(meteorite, value);
                        }
                    }

                    meteorite.Year = dto.Year.Year;

                    return meteorite;
                }).ToList();

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
