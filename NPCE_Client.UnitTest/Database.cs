using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE_Client.Api.Data;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace NPCE_Client.Test
{
    [TestClass]
    public class Database
    {

        // to have the same Configuration object as in Startup	      
        private IConfigurationRoot _configuration;	
        // represents database's configuration	      
        private DbContextOptions<AppDbContext> _options;

        public Database()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            _configuration = builder.Build(); _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
                .Options;
        }


        [TestMethod]
        public void InsertAmbiente()
        {
            var ctx= new AppDbContext(_options);

            ctx.Ambienti.Add(new Ambiente
            {
                Description = "COLLAUDO",
                customerid = "nello.citta.npce",
                costcenter = "UNF",
                billingcenter = "IdCdF",
                idsender = "999988",
                sendersystem = "H2H",
                smuser = "nello.citta.npce",
                contracttype = "PosteWeb",
                usertype = "B",
                LolUri = "http://10.60.19.12/LOLGC/LolService.svc",
                Username = "rete\\mic32nv",
                Password = "Passw0rd"
            });

            ctx.SaveChanges();
        }
    }

}
