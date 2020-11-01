using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Services
{
    public interface IServiziDataService
    {
        Task<IEnumerable<Servizio>> GetAllServizi();

        Task<Servizio> GetServizioDetail(int servizioId);

        Task<Servizio> AddServizio(Servizio servizio);

        Task EditServizio(Servizio servizio);

        Task DeleteServizio(int servizioId);
    }
}
