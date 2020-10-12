using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Services
{

    public interface IAmbientiDataService
    {
        Task<IEnumerable<Ambiente>> GetAllAmbienti();

        Task<Ambiente> GetAmbienteDetail(int ambienteId);

        Task<Ambiente> AddAmbiente(Ambiente ambiente);

        Task EditAmbiente(Ambiente ambiente);

        Task DeleteAmbiente(int ambienteId);
    }

}
