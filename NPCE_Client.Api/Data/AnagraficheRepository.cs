using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPCE_Client.Api.Data
{
    public class AnagraficheRepository : IAnagraficheRepository
    {
        private readonly AppDbContext appDbContext;

        public AnagraficheRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public Anagrafica AddAnagrafica(Anagrafica anagrafica)
        {
            var addedEntity = appDbContext.Anagrafiche.Add(anagrafica);
            appDbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public void DeleteAnagrafica(int idAnagrafica)
        {
            var foundAnagrafica = appDbContext.Anagrafiche.FirstOrDefault(e => e.Id == idAnagrafica);
            if (foundAnagrafica == null) return;

            appDbContext.Anagrafiche.Remove(foundAnagrafica);
            appDbContext.SaveChanges();
        }

        public IEnumerable<Anagrafica> GetAllAnagrafiche()
        {
            return appDbContext.Anagrafiche;
        }

        public Anagrafica GetAnagraficaById(int id)
        {
            return appDbContext.Anagrafiche.Where(a => a.Id == id).FirstOrDefault();
        }

        public Anagrafica UpdateAnagrafica(Anagrafica anagrafica)
        {
            var foundAnagrafica = appDbContext.Anagrafiche.FirstOrDefault(e => e.Id == anagrafica.Id);

            if (foundAnagrafica != null)
            {
                foundAnagrafica.Cognome = anagrafica.Cognome;
                foundAnagrafica.Nome = anagrafica.Nome;
                foundAnagrafica.RagioneSociale = anagrafica.RagioneSociale;
                foundAnagrafica.ComplementoNominativo = anagrafica.ComplementoNominativo;
                foundAnagrafica.DUG = anagrafica.DUG;
                foundAnagrafica.Toponimo = anagrafica.Toponimo;
                foundAnagrafica.Esponente = anagrafica.Esponente;
                foundAnagrafica.NumeroCivico = anagrafica.NumeroCivico;
                foundAnagrafica.ComplementoIndirizzo = anagrafica.ComplementoIndirizzo;
                foundAnagrafica.Frazione = anagrafica.Frazione;
                foundAnagrafica.Citta = anagrafica.Citta;
                foundAnagrafica.Cap = anagrafica.Cap;
                foundAnagrafica.Telefono = anagrafica.Telefono;
                foundAnagrafica.CasellaPostale = anagrafica.CasellaPostale;
                foundAnagrafica.Provincia = anagrafica.Provincia;
                foundAnagrafica.Stato = anagrafica.Stato;

                appDbContext.SaveChanges();

                return foundAnagrafica;
            }
            return null;
        }
    }
}
