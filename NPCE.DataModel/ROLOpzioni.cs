//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NPCE.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ROLOpzioni
    {
        public System.Guid IdRichiesta { get; set; }
        public string Inserto { get; set; }
        public Nullable<bool> InserisciMittenteInserto { get; set; }
        public Nullable<bool> Archiviazione { get; set; }
        public Nullable<bool> FirmaElettronica { get; set; }
        public Nullable<bool> DPM { get; set; }
        public Nullable<bool> SecurPaper { get; set; }
        public Nullable<bool> InserisciMittente { get; set; }
        public Nullable<System.DateTime> DataStampa { get; set; }
        public short TipoArchiviazione { get; set; }
        public Nullable<short> AnniArchiviazione { get; set; }
    
        public virtual ROL ROL { get; set; }
    }
}
