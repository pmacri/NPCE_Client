using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Services
{
    public class ServiziDataService : IServiziDataService
    {

        private readonly HttpClient httpClient;

        public ServiziDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<Servizio> AddServizioAsync(Servizio servizio)
        {
            try
            {
                var servizioJson = new StringContent(JsonSerializer.Serialize(servizio), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("api/servizi", servizioJson);

                if (response.IsSuccessStatusCode)
                {
                    return await JsonSerializer.DeserializeAsync<Servizio>(await response.Content.ReadAsStreamAsync());
                }
            }
            catch (Exception ex)
            {

                throw;
            }


            return null;
        }

        public Task DeleteServizioAsync(int servizioId)
        {
            throw new NotImplementedException();
        }

        public Task EditServizioAsync(Servizio servizio)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Servizio>> GetAllServiziAsync()
        {
            try
            {
                var stream = await httpClient.GetStreamAsync($"api/servizi");
                return await JsonSerializer.DeserializeAsync<IEnumerable<Servizio>> (stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }

        public async Task<Servizio> GetServizioDetailAsync(int ServizioId)
        {
            try
            {
                var stream = await httpClient.GetStreamAsync($"api/servizi/{ServizioId}");
                return await JsonSerializer.DeserializeAsync<Servizio>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            }
            catch (Exception ex)
            {

                throw (ex);
            }
        }
    }
}
