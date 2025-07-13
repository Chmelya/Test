using AM.Application.Common.Interfaces.Repositories;
using AM.Domain.Enities;
using AM.Infrastructure.UnitOfWork;
using AM.JobScheduler.DTO;
using AM.JobScheduler.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AM.JobScheduler.Services
{
    public class MeteoriteFetchService(
        HttpClient httpClient,
        IMeteoriteRepository meteoriteRepository,
        IGeolocationRepository geolocationRepository,
        IRecclassRepository recclassRepository,
        IOptions<JobsSettings> options,
        IUnitOfWork unitOfWork)
        : IMeteoriteFetchService
    {
        public async Task FetchMeteorites()
        {
            if (string.IsNullOrEmpty(options.Value.MeteoritesDatasetUrl))
            {
                //TODO Regex
                throw new ArgumentException("MeteoritesDatasetUrl is empty");
            }

            try
            {
                var meteoritesDto = await httpClient.GetFromJsonAsync<List<MeteoriteDto>>(options.Value.MeteoritesDatasetUrl);
                if (meteoritesDto is null)
                {
                    return;
                }

                var recclassesToWrite = meteoritesDto
                    .Select(m => m.Recclass)
                    .Distinct()
                    .Select(recclass => new Recclass { Name = recclass });

                unitOfWork.BeginTransaction();

                await recclassRepository.BulkInsertOrUpdateAsync(recclassesToWrite, config =>
                {
                    config.BatchSize = 200;
                    config.UpdateByProperties =
                    [
                        nameof(Recclass.Name)
                    ];
                });

                var recclasses = (await recclassRepository.GetAllAsListAsync()).ToDictionary(r => r.Name);

                var meteorites = meteoritesDto.Select(dto =>
                {
                    var meteorite = new Meteorite();

                    foreach (var prop in typeof(Meteorite).GetProperties())
                    {
                        if (prop.Name == nameof(Meteorite.Year)
                           || prop.Name == nameof(Meteorite.Recclass))
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
                    meteorite.RecclassId = recclasses[dto.Recclass].Id;

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

                unitOfWork.Commit();
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException("Something went wrong during fetch", ex);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Something went wrong during parsing", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Something went wrong during database update", ex);
            }
            finally
            {
                unitOfWork.Rollback();
            }
        }
    }
}
