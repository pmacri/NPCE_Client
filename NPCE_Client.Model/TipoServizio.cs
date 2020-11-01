using Humanizer.Localisation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NPCE_Client.Model
{

    public enum TipoServizioId
    {
        [Display(Name = "Posta 1")]
        POSTA1,
        [Display(Name = "Posta 4")]
        POSTA4,
        [Display(Name = "Raccomandata")]
        ROL,
        [Display(Name = "COL1")]
        COL1,
        [Display(Name = "COL4")]
        COL4,
        [Display(Name = "MOL1")]
        MOL1,
        [Display(Name = "MOL4")]
        MOL4
    }
    public class TipoServizio
    {

        public int Id { get; set; }

        public string Description { get; set; }
    }
}
