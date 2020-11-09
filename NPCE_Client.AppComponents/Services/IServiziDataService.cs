using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Services
{
    public interface IServiziDataService
    {
        Task<IEnumerable<Servizio>> GetAllServiziAsync();

        Task<Servizio> GetServizioDetailAsync(int servizioId);

        Task<Servizio> AddServizioAsync(Servizio servizio);

        Task EditServizioAsync(Servizio servizio);

        Task DeleteServizioAsync(int servizioId);
    }
}
