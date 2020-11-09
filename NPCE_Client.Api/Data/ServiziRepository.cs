using Microsoft.EntityFrameworkCore;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NPCE_Client.Api.Data
{
    public class ServiziRepository : IServiziRepository
    {
        private readonly AppDbContext appDbContext;

        public ServiziRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<Servizio> AddServizioAsync(Servizio servizio)
        {
            try
            {

            var addedEntity = appDbContext.Servizi.Add(servizio);
            await appDbContext.SaveChangesAsync();
            return addedEntity.Entity;
            }
            catch (Exception ex)
            {

                throw(ex);
            }
        }

        public IEnumerable<Servizio> GetAll()
        {
            try
            {

            return  appDbContext.Servizi
                                .Include(s=> s.StatoServizio)
                                .Include(s => s.Ambiente)
                                .Include(s => s.TipoServizio);
            }
            catch (Exception ex)
            {

                throw(ex);
            }
        }

        public Task<Servizio> GetServizioByIdAsync(int servizioId)
        {
            return appDbContext.Servizi
                               .Where(s => s.Id == servizioId)
                               .Include(s => s.StatoServizio)
                               .Include(s => s.Ambiente)
                               .Include(s => s.TipoServizio)
                               .FirstOrDefaultAsync();
        }
    }
}
