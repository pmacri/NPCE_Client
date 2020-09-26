using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPCE_Client.Api.Data
{
    public class DocumentoRepository : IDocumentiRepository
    {

        private readonly AppDbContext appDbContext;

        public DocumentoRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public Documento AddDocumento(Documento documento)
        {
            var addedEntity = appDbContext.Documenti.Add(documento);
            appDbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public void DeleteDocumento(int idDocumento)
        {
            var foundDocumento = appDbContext.Documenti.FirstOrDefault(e => e.Id == idDocumento);
            if (foundDocumento == null) return;

            appDbContext.Documenti.Remove(foundDocumento);
            appDbContext.SaveChanges();
        }

        public IEnumerable<Documento> GetAllDocumenti()
        {
            return appDbContext.Documenti;
        }

        public Documento GetDocumentoById(int id)
        {
            return appDbContext.Documenti.Where(a => a.Id == id).FirstOrDefault();
        }

        public Documento UpdateDocumento(Documento documento)
        {
            throw new NotImplementedException();
        }
    }
}
