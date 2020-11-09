using NPCE_Client.AppComponents.Shared;
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

        Task<Anagrafica> AddAnagrafica(Anagrafica anagrafica);

        Task EditAnagrafica(Anagrafica anagrafica);

        Task DeleteAnagrafica(int anagraficaId);

        Task<IEnumerable<AnagraficheSelectorViewModel>> AnagraficheSelectorGetByServizio(int servizioId);

        Task<IEnumerable<AnagraficheSelectorViewModel>> AnagraficheSelectorViewModelGetAll();

        Task UpdateAnagraficheServizioAsync(int servizioId, IEnumerable<AnagraficheSelectorViewModel> anagrafiche);
    }
}
