using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Services
{
   public interface IDocumentiDataService
    {
        Task<IEnumerable<Documento>> GetAllDocumenti();

        Task<Documento> GetDocumentoDetail(int documentoId);

        Task<Documento> AddDocumento(Documento documento);

        Task EditAnagrafica(Documento documento);

        Task DeleteDocumento(int documentoId);
    }
}
