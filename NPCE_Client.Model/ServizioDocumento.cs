using System;
using System.Collections.Generic;
using System.Text;

namespace NPCE_Client.Model
{
    public class ServizioDocumento
    {
        public int ServizioId { get; set; }
        public Servizio Servizio { get; set; }

        public int DocumentoId { get; set; }
        public Documento Documento { get; set; }
    }
}
