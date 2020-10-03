using System;
using System.Collections.Generic;
using System.Text;

namespace NPCE_Client.Model
{

    public enum TipoServizioId
    {
        POSTA1,
        POSTA4,
        ROL,
        COL1,
        COL4,
        MOL1,
        MOL4
    }
    public class TipoServizio
    {

        public int Id { get; set; }

        public string Description { get; set; }
    }
}
