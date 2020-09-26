using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPCE_Client.Api.Data
{
    public interface IDocumentiRepository
    {
        IEnumerable<Documento> GetAllDocumenti();
        Documento GetDocumentoById(int id);
        Documento AddDocumento(Documento documento);
        Documento UpdateDocumento(Documento documento);
        void DeleteDocumento(int idDocumento);
    }
}
