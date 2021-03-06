﻿using NPCE_Client.AppComponents.Shared;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<Anagrafica>  AddAnagrafica(Anagrafica anagrafica)
        {
            var anagraficaJson = new StringContent(JsonSerializer.Serialize(anagrafica), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/anagrafiche",anagraficaJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Anagrafica>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task<IEnumerable<AnagraficheSelectorViewModel>> AnagraficheSelectorGetByServizio(int servizioId)
        {
            Stream stream = await httpClient.GetStreamAsync($"api/anagraficheselector/{servizioId}");

            return await JsonSerializer.DeserializeAsync<IEnumerable<AnagraficheSelectorViewModel>>
               (stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task DeleteAnagrafica(int anagraficaId)
        {
            await httpClient.DeleteAsync($"api/anagrafiche/{anagraficaId}");
        }

        public async Task EditAnagrafica(Anagrafica anagrafica)
        {
            var anagraficaJson =
               new StringContent(JsonSerializer.Serialize(anagrafica), Encoding.UTF8, "application/json");

            await httpClient.PutAsync("api/anagrafiche", anagraficaJson);
        }

        public async Task<IEnumerable<Anagrafica>> GetAllAnagrafiche()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Anagrafica>>
                (await httpClient.GetStreamAsync($"api/anagrafiche"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<AnagraficheSelectorViewModel>> AnagraficheSelectorViewModelGetAll()
        {

            return await JsonSerializer.DeserializeAsync<IEnumerable<AnagraficheSelectorViewModel>>
                (await httpClient.GetStreamAsync($"api/AnagraficheSelector"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Anagrafica> GetAnagraficaDetail(int anagraficaId)
        {
            return await JsonSerializer.DeserializeAsync<Anagrafica>
                (await httpClient.GetStreamAsync($"api/anagrafiche/{anagraficaId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task UpdateAnagraficheServizioAsync(int servizioId, IEnumerable<AnagraficheSelectorViewModel> anagrafiche)
        {
            var AnagraficheJson =
               new StringContent(JsonSerializer.Serialize(anagrafiche), Encoding.UTF8, "application/json");
            await httpClient.PutAsync($"api/anagraficheSelector/{servizioId}", AnagraficheJson);
        }
    }
}
