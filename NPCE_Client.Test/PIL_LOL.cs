using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE.Library;
using NPCE_Client.Api.Data;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_Client.Test
{
    [TestClass]
    public class PIL_LOL
    {

        // to have the same Configuration object as in Startup	      
        private IConfigurationRoot _configuration;
        // represents database's configuration	      
        private DbContextOptions<AppDbContext> _options;

        public PIL_LOL()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            _configuration = builder.Build(); _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
                .Options;
        }


        [TestMethod]
        public void Invio_1_Destinatario_OK()
        {
            var ctx = new AppDbContext(_options);

            var ambiente = new Ambiente
            {
                Description = "COLLAUDO",
                customerid = "nello.citta.npce",
                costcenter = "UNF",
                billingcenter = "IdCdF",
                idsender = "999988",
                sendersystem = "H2H",
                smuser = "nello.citta.npce",
                contracttype = "PosteWeb",
                contractid="contracoId",
                  codicefiscale="mcrpql64t08f537u",
                   partitaiva="01234567890",
                usertype = "B",
                customer="customer",
                LolUri = "http://10.60.19.36/NPCE_EntryPoint/WsCE.svc",
                Username = "rete\\mic32nv",
                Password = "Passw0rd"
            };

            Anagrafica destinatario;
            Anagrafica mittente;


            destinatario = ctx.Anagrafiche.First();
            mittente = ctx.Anagrafiche.Skip(1).First();

            var documento = ctx.Documenti.First();

            var servizio = new Servizio();

            servizio.ServizioAnagrafiche.Add(
                new ServizioAnagrafica { Anagrafica = destinatario, IsMittente = false });

            servizio.ServizioAnagrafiche.Add(
                new ServizioAnagrafica { Anagrafica = mittente, IsMittente = true });

            servizio.ServizioDocumenti.Add(new ServizioDocumento { Documento = documento });

            servizio.TipoServizioId = (int)TipoServizioId.POSTA4;

            LOLPil service = new LOLPil(servizio, ambiente);
            try
            {
                var result = service.Invia();

                Assert.IsTrue(result.Code == "I");
            }
            catch (System.Exception)
            {
                throw;
            }

        }

        [TestMethod]
        public  async Task InvioAsync_1_Destinatario_OK()
        {
            var ctx = new AppDbContext(_options);

            var ambiente = new Ambiente
            {
                Description = "COLLAUDO",
                customerid = "nello.citta.npce",
                costcenter = "UNF",
                billingcenter = "IdCdF",
                idsender = "999988",
                sendersystem = "H2H",
                smuser = "nello.citta.npce",
                contracttype = "PosteWeb",
                contractid = "contracoId",
                codicefiscale = "mcrpql64t08f537u",
                partitaiva = "01234567890",
                usertype = "B",
                customer = "customer",
                LolUri = "http://10.60.19.36/NPCE_EntryPoint/WsCE.svc",
                Username = "rete\\mic32nv",
                Password = "Passw0rd"
            };

            Anagrafica destinatario;
            Anagrafica mittente;


            destinatario = ctx.Anagrafiche.First();
            mittente = ctx.Anagrafiche.Skip(1).First();

            var documento = ctx.Documenti.First();

            var servizio = new Servizio();

            servizio.ServizioAnagrafiche.Add(
                new ServizioAnagrafica { Anagrafica = destinatario, IsMittente = false });

            servizio.ServizioAnagrafiche.Add(
                new ServizioAnagrafica { Anagrafica = mittente, IsMittente = true });

            servizio.ServizioDocumenti.Add(new ServizioDocumento { Documento = documento });

            servizio.TipoServizioId = (int)TipoServizioId.POSTA4;

            LOLPil service = new LOLPil(servizio, ambiente);
            try
            {
                var result =await  service.InviaAsync();

                Assert.IsTrue(result.Code == "I");
            }
            catch (System.Exception)
            {
                throw;
            }

        }
    }
}
