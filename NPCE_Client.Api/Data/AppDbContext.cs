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

        public DbSet<Ambiente> Ambienti { get; set; }

        public DbSet<Servizio> Servizi { get; set; }

        public DbSet<TipoServizio> TipiServizio { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoServizio>()
                .Property(x => x.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<StatoServizio>()
                .Property(x => x.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<ServizioAnagrafica>()
                        .HasKey(t => new { t.ServizioId, t.AnagraficaId });

            modelBuilder.Entity<ServizioDocumento>()
                       .HasKey(t => new { t.ServizioId, t.DocumentoId });

            modelBuilder.Entity<TipoServizio>()
                .HasData(
                new TipoServizio { Id = (int)TipoServizioId.POSTA1, Description = "Posta 1" },
                new TipoServizio { Id = (int)TipoServizioId.POSTA4, Description = "Posta 4" },
                new TipoServizio { Id = (int)TipoServizioId.ROL, Description = "Raccomandata" },
                new TipoServizio { Id = (int)TipoServizioId.COL1, Description = "PostaContest 1" },
                new TipoServizio { Id = (int)TipoServizioId.COL4, Description = "PostaContest 4" },
                new TipoServizio { Id = (int)TipoServizioId.MOL1, Description = "RaccomandataMarket 1" },
                new TipoServizio { Id = (int)TipoServizioId.MOL4, Description = "RaccomandataMarket 4" }
                );

            modelBuilder.Entity<StatoServizio>()
                .HasData(
                new StatoServizio { Id = (int)StatoServizioId.DA_INVIARE, Description = "Da Inviare" },
                new StatoServizio { Id = (int)StatoServizioId.INVIATO, Description = "Inviato" },
                new StatoServizio { Id = (int)StatoServizioId.CONFERMATO, Description = "Confermato" }
                );
        }

    }
}
