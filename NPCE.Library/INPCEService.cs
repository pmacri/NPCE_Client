using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NPCE.Library
{
    public interface INPCEService
    {
         Ambiente Ambiente { get; set; }
         Servizio Servizio { get; set; }
        Task InviaAsync();
        void Invia();
        Task ConfermaAsync();
    }
}
