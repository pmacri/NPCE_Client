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
    
    public partial class ROLDocumenti
    {
        public System.Guid IdRichiesta { get; set; }
        public int IdPosizione { get; set; }
        public string Uri { get; set; }
        public string Hash_MD5 { get; set; }
    
        public virtual ROL ROL { get; set; }
    }
}
