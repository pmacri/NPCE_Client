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
        public IEnumerable<Anagrafica> GetAllAnagrafiche()
        {
            return appDbContext.Anagrafiche;
        }
    }
}
