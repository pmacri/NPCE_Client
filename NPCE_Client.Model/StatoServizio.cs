using System;
using System.Collections.Generic;
using System.Text;

namespace NPCE_Client.Model
{

    public enum StatoServizioId
    {
        DA_INVIARE,
        INVIATO,
        CONFERMATO
    }
    public class StatoServizio
    {
        public int Id { get; set; }

        public string Description { get; set; }

    }
}
