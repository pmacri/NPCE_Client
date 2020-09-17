using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Services
{
    public interface IAnagraficheDataService
    {
        Task<IEnumerable<Anagrafica>> GetAllAnagrafiche();

        Task<Anagrafica> GetAnagraficaDetail(int anagraficaId);
    }
}
