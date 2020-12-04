using NPCE_Client.AppComponents.Shared;
using NPCE_Client.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NPCE_Client.Api.Data
{
    public interface IAnagraficaRepository
    {
        IEnumerable<Anagrafica> GetAllAnagrafiche();

        IEnumerable<AnagraficheSelectorViewModel> GetByServizio(int idServizio);

        Anagrafica GetAnagraficaById(int id);

        Anagrafica AddAnagrafica(Anagrafica anagrafica);
        Anagrafica UpdateAnagrafica(Anagrafica anagrafica);
        void DeleteAnagrafica(int idAnagrafica);

        Task UpdateAngraficheServizioAsync(int servizioId, IEnumerable<AnagraficheSelectorViewModel> anagrafiche);
    }
}