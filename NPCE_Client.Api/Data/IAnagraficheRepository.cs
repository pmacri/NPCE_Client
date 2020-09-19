using NPCE_Client.Model;
using System.Collections.Generic;

namespace NPCE_Client.Api.Data
{
    public interface IAnagraficheRepository
    {
        IEnumerable<Anagrafica> GetAllAnagrafiche();

        Anagrafica GetAnagraficaById(int id);

        Anagrafica AddAnagrafica(Anagrafica anagrafica);
        Anagrafica UpdateAnagrafica(Anagrafica anagrafica);
        void DeleteAnagrafica(int idAnagrafica);
    }
}