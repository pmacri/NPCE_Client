using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPCE_Client.Api.Data
{
    public interface IAmbientiRepository
    {
        IEnumerable<Ambiente> GetAllAmbienti();

        Ambiente GetAmbienteById(int id);

        Ambiente AddAmbiente(Ambiente Ambiente);
        Ambiente UpdateAmbiente(Ambiente Ambiente);
        void DeleteAmbiente(int idAmbiente);
    }
}
