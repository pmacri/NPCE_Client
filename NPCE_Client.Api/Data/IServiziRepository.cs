using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPCE_Client.Api.Data
{
    public interface IServiziRepository
    {
        Task<Servizio> AddServizioAsync(Servizio servizio);
        Task<Servizio> GetServizioByIdAsync(int servizioId);
        IEnumerable<Servizio> GetAll();
    }
}
