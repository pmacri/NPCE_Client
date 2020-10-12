using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPCE_Client.Api.Data
{
    public class AmbientiRepository: IAmbientiRepository
    {
        private readonly AppDbContext appDbContext;

        public AmbientiRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public Ambiente AddAmbiente(Ambiente Ambiente)
        {
            var addedEntity = appDbContext.Ambienti.Add(Ambiente);
            appDbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public void DeleteAmbiente(int idAmbiente)
        {
            var foundAmbiente = appDbContext.Anagrafiche.FirstOrDefault(e => e.Id == idAmbiente);
            if (foundAmbiente == null) return;

            appDbContext.Anagrafiche.Remove(foundAmbiente);
            appDbContext.SaveChanges();
        }

        public IEnumerable<Ambiente> GetAllAmbienti()
        {
            return appDbContext.Ambienti;
        }

        public Ambiente GetAmbienteById(int id)
        {
            return appDbContext.Ambienti.Where(a => a.Id == id).FirstOrDefault();
        }

        public Ambiente UpdateAmbiente(Ambiente Ambiente)
        {
            var foundAmbiente = appDbContext.Ambienti.FirstOrDefault(e => e.Id == Ambiente.Id);

            if (foundAmbiente != null)
            {
                foundAmbiente.billingcenter= Ambiente.billingcenter;
                foundAmbiente.codicefiscale = Ambiente.codicefiscale;
                foundAmbiente.ColUri = Ambiente.ColUri;
                foundAmbiente.contractid = Ambiente.contractid;
                foundAmbiente.contracttype = Ambiente.contracttype;
                foundAmbiente.Contratto = Ambiente.Contratto;
                foundAmbiente.ContrattoCOL = Ambiente.ContrattoCOL;
                foundAmbiente.ContrattoMOL = Ambiente.ContrattoMOL;
                foundAmbiente.costcenter = Ambiente.costcenter;
                foundAmbiente.customer = Ambiente.customer;
                foundAmbiente.customerid = Ambiente.customerid;
                foundAmbiente.Description = Ambiente.Description;
                foundAmbiente.idsender = Ambiente.idsender;
                foundAmbiente.IsPil = Ambiente.IsPil;
                foundAmbiente.LolUri = Ambiente.LolUri;
                foundAmbiente.MolUri = Ambiente.MolUri;
                foundAmbiente.partitaiva = Ambiente.partitaiva;
                foundAmbiente.Password = Ambiente.Password;
                foundAmbiente.RolUri = Ambiente.RolUri;
                foundAmbiente.sendersystem = Ambiente.sendersystem;
                foundAmbiente.smuser = Ambiente.smuser;
                foundAmbiente.Username = Ambiente.Username;
                foundAmbiente.usertype = Ambiente.usertype;
                foundAmbiente.VolUri = Ambiente.VolUri;


                appDbContext.SaveChanges();

                return foundAmbiente;
            }
            return null;
        }
    }
}
