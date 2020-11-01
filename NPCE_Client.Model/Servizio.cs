using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace NPCE_Client.Model
{
    public class Servizio
    {
        public Servizio()
        {
            ServizioAnagrafiche = new Collection<ServizioAnagrafica>();
            ServizioDocumenti = new Collection<ServizioDocumento>();
        }

        public int Id { get; set; }

        public DateTime DataCreazione { get; set; }
        public bool AvvisoRicevimento { get; set; }
        public bool ArchiviazioneDigitale { get; set; }
        public bool Autoconferma { get; set; }
        public string IdRichiesta { get; set; }
        public bool AttestazioneConsegna { get; set; }
        public bool FronteRetro { get; set; }
        public bool SecondoTentativoRecapito { get; set; }
        public bool Colore { get; set; }
        public string GuidUtente { get; set; }
        public string IdOrdine { get; set; }
        public string TipoArchiviazione { get; set; } // NESSUNA, SEMPLICE, STORICA
        public int AnniArchiviazione { get; set; }

        // Navigation

        private TipoServizioId tipoServizioEnum;
        [NotMapped]
        public TipoServizioId TipoServizioEnum
        {
            get { return tipoServizioEnum; }
            set { 
                tipoServizioEnum = value;
                TipoServizioId = (int)value;
            }
        }

        public TipoServizio TipoServizio { get; set; }
        public int? TipoServizioId { get; set; }

        public Ambiente Ambiente { get; set; }
        public int? AmbienteId { get; set; }

        public int StatoServizioId { get; set; }
        public StatoServizio StatoServizio { get; set; }

        [NotMapped]
        public Anagrafica Mittente { get
            {
                return ServizioAnagrafiche.Where(a => a.IsMittente == true).Select( a => a.Anagrafica).FirstOrDefault();
            }
        }


        public Collection<ServizioAnagrafica> ServizioAnagrafiche { get; }
        public Collection<ServizioDocumento> ServizioDocumenti { get; }
    }
}
