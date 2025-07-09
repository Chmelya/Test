using AM.Application.Common.Interfaces.Repositories;
using AM.Domain.Enities;
using AM.JobScheduler.Interfaces;
using System.Text.Json;

namespace AM.JobScheduler.Services
{
    public class MeteoriteFetchService(
        HttpClient httpClient,
        IMeteoriteRepository meteoriteRepository)
        : IMeteoriteFetchService
    {
        public async Task FetchMeteorites()
        {
            try
            {
                //TODO : To config
                var response = await httpClient.GetFromJsonAsync<IList<Meteorite>>("https://raw.githubusercontent.com/biggiko/nasa-dataset/refs/heads/main/y77d-th95.json");
                await meteoriteRepository.BulkInsertAsync(response);
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
