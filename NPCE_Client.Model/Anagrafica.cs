using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NPCE_Client.Model
{
    public class Anagrafica
    {
        public int Id { get; set; }

        [Required]
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string RagioneSociale { get; set; }
        public string ComplementoNominativo { get; set; }
        public string DUG { get; set; }
        public string Toponimo { get; set; }
        public string Esponente { get; set; }
        public string NumeroCivico { get; set; }
        public string ComplementoIndirizzo { get; set; }
        public string Frazione { get; set; }
        public string Citta { get; set; }
        public string Cap { get; set; }
        public string Telefono { get; set; }
        public string CasellaPostale { get; set; }
        public string Provincia { get; set; }
        public string Stato { get; set; }

        [NotMapped]
        public bool IsMittente { get; set; }


        [NotMapped]
        public string Indirizzo
        {
            get
            {
                return $"{DUG} {Toponimo} {Esponente??string.Empty} {NumeroCivico??string.Empty}";
            }
        }
    }
}
