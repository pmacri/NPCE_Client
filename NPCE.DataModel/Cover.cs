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
    
    public partial class Cover
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cover()
        {
            this.ROL = new HashSet<ROL>();
        }
    
        public string IdCover { get; set; }
        public Nullable<int> IdStatoCover { get; set; }
        public string Denominazione { get; set; }
        public string CodiceContratto { get; set; }
        public string UserId { get; set; }
        public string ServizioNpce { get; set; }
        public byte[] HeaderImage { get; set; }
        public string HeaderText { get; set; }
        public string HeaderXHTML { get; set; }
        public byte[] LogoMittente { get; set; }
        public byte[] CampoLiberoHeaderImage { get; set; }
        public string CampoLiberoHeaderText { get; set; }
        public string CampoLiberoHeaderXHTML { get; set; }
        public byte[] BodyImage { get; set; }
        public string BodyText { get; set; }
        public string BodyXHTML { get; set; }
        public byte[] FooterImage { get; set; }
        public string FooterText { get; set; }
        public string FooterXHTML { get; set; }
        public byte[] CampoLiberoLetteraImage { get; set; }
        public string CampoLiberoLetteraText { get; set; }
        public string CampoLiberoLetteraXHTML { get; set; }
        public Nullable<bool> FooterStandard { get; set; }
        public string ConversionURI { get; set; }
        public string ConversionMessage { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ROL> ROL { get; set; }
    }
}
