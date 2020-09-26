using Microsoft.EntityFrameworkCore;
using NPCE_Client.Model;

namespace NPCE_Client.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Anagrafica> Anagrafiche { get; set; }

        public DbSet<Documento> Documenti { get; set; }
    }
}
