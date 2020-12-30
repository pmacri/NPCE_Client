using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_Client.UnitTest
{
    public class Ambiente
    {
        public int Id { get; set; }

        public string Description { get; set; }
        public string customerid { get; set; }
        public string costcenter { get; set; }
        public string billingcenter { get; set; }
        public string idsender { get; set; }
        public string sendersystem { get; set; }
        public string smuser { get; set; }
        public string contracttype { get; set; }
        public string usertype { get; set; }
        public string codicefiscale { get; set; }
        public string partitaiva { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string LolUri { get; set; }
        public string RolUri { get; set; }
        public string ColUri { get; set; }
        public string MolUri { get; set; }
        public string Contratto { get; set; }
        public string VolUri { get; set; }
        public string contractid { get; set; }
        public string customer { get; set; }
        public bool IsPil { get; set; }
        public string ContrattoMOL { get; set; }
        public string ContrattoCOL { get; set; }

        public string PostaEvoConnectionString { get; set; }

        public string NomeProprioMol { get; set; }

        public bool FromIAM { get; set; }
        public string UrlEntryPoint { get; internal set; }
        public string PathDocument { get; internal set; }
        public string HashMD5Document { get; internal set; }
        public string PathCov { get; internal set; }
        public string HashMD5Cov { get; internal set; }

        public string PathLoggingFile { get; set; }
    }

}
