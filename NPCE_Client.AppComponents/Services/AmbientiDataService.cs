using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Services
{
    public class AmbientiDataService : IAmbientiDataService
    {
        private readonly HttpClient httpClient;

        public AmbientiDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Ambiente> AddAmbiente(Ambiente Ambiente)
        {
            var AmbienteJson = new StringContent(JsonSerializer.Serialize(Ambiente), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/ambienti", AmbienteJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Ambiente>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }


        public async Task DeleteAmbiente(int AmbienteId)
        {
            await httpClient.DeleteAsync($"api/ambienti/{AmbienteId}");
        }

        public async Task EditAmbiente(Ambiente Ambiente)
        {
            var AmbienteJson =
               new StringContent(JsonSerializer.Serialize(Ambiente), Encoding.UTF8, "application/json");

            await httpClient.PutAsync("api/ambienti", AmbienteJson);
        }

        public async Task<IEnumerable<Ambiente>> GetAllAmbienti()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Ambiente>>
                (await httpClient.GetStreamAsync($"api/ambienti"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

    

        public async Task<Ambiente> GetAmbienteDetail(int AmbienteId)
        {
            return await JsonSerializer.DeserializeAsync<Ambiente>
                (await httpClient.GetStreamAsync($"api/ambienti/{AmbienteId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
