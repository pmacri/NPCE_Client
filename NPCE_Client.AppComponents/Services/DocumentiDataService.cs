using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Services
{
    public class DocumentiDataService : IDocumentiDataService
    {
        private readonly HttpClient httpClient;

        public DocumentiDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<Documento> AddDocumento(Documento documento)
        {
            var documentoJson = new StringContent(JsonSerializer.Serialize(documento), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/documenti", documentoJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Documento>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public Task DeleteDocumento(int documentoId)
        {
            throw new NotImplementedException();
        }

        public Task EditAnagrafica(Documento documento)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Documento>> GetAllDocumenti()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Documento>>
               (await httpClient.GetStreamAsync($"api/documenti"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public Task<Documento> GetDocumentoDetail(int documentoId)
        {
            throw new NotImplementedException();
        }
    }
}
