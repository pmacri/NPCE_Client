using System;
using System.Collections.Generic;
using System.Text;

namespace NPCE_Client.Model
{
    public class ServizioAnagrafica
    {
        public int ServizioId { get; set; }
        public Servizio Servizio { get; set; }

        public int AnagraficaId { get; set; }
        public Anagrafica   Anagrafica  { get; set; }

        public bool IsMittente { get; set; }
    }
}
