using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Services
{
    public class AnagraficheDataService : IAnagraficheDataService
    {
        private readonly HttpClient httpClient;

        public AnagraficheDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<Anagrafica>> GetAllAnagrafiche()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Anagrafica>>
                (await httpClient.GetStreamAsync($"api/anagrafiche"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
