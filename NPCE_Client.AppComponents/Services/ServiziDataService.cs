using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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
        public Task<Servizio> AddServizio(Servizio servizio)
        {
            throw new NotImplementedException();
        }

        public Task DeleteServizio(int servizioId)
        {
            throw new NotImplementedException();
        }

        public Task EditServizio(Servizio servizio)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Servizio>> GetAllServizi()
        {
            throw new NotImplementedException();
        }

        public Task<Servizio> GetServizioDetail(int servizioId)
        {
            throw new NotImplementedException();
        }
    }
}
