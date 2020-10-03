using NPCE.ServiceReference.LOL;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace NPCE.Library
{
    public class LOLService : ServiceBase<LOLServiceSoap>
    {
        
        public LOLService(Servizio servizio, Ambiente ambiente) : base(servizio, ambiente)
        {
            
        }
        public override Task ConfermaAsync()
        {
            throw new NotImplementedException();
        }

        public override async Task InviaAsync()
        {
            LOLSubmit lolSubmit = new LOLSubmit();
            SetMittente(lolSubmit);
            SetDestinatari(lolSubmit);
            SetDocumenti(lolSubmit);
            SetOpzioni(lolSubmit);

            if (Servizio.TipoServizio.Description == "Posta1")
            {
                SetPosta1(lolSubmit);
            }

            string idRichiesta = await RecuperaIdRichiestaAsync();

            var invioResult = await _proxy.InvioAsync(idRichiesta, string.Empty, lolSubmit);

        }

        private void SetPosta1(LOLSubmit lolSubmit)
        {
            lolSubmit.DescrizioneLettera = new DescrizioneLettera { TipoLettera = "Posta1" };

        }

        public async Task<string> RecuperaIdRichiestaAsync()
        {

            var fake = new OperationContextScope((IContextChannel)_proxy);

            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = _httpHeaders;

            var result = await _proxy.RecuperaIdRichiestaAsync();

            return result.IDRichiesta;

        }

        private void SetOpzioni(LOLSubmit lolSubmit)
        {
            var opzioni = new LOLSubmitOpzioni();

            opzioni.OpzionidiStampa = new OpzionidiStampa
            {
                PageSize = OpzionidiStampaPageSize.A4,
                FronteRetro = GetFronteRetroDescription(Servizio.FronteRetro),
                BW = Servizio.Colore ? "false" : "true"
            };

            lolSubmit.Opzioni = opzioni;
        }

        private void SetMittente(LOLSubmit lolSubmit)
        {
            var mittenteServizio = Servizio.ServizioAnagrafiche.Single(a => a.IsMittente == true);
            var mittente = new Mittente();

            var nominativo = new Nominativo
            {
                Nome = mittenteServizio.Anagrafica.Nome,
                Cognome = mittenteServizio.Anagrafica.Cognome,
                Indirizzo = new Indirizzo
                {
                    DUG = mittenteServizio.Anagrafica.DUG,
                    Toponimo = mittenteServizio.Anagrafica.Toponimo,
                    Esponente = mittenteServizio.Anagrafica.Esponente,
                    NumeroCivico = mittenteServizio.Anagrafica.NumeroCivico
                },
                CAP = mittenteServizio.Anagrafica.Cap,
                CasellaPostale = mittenteServizio.Anagrafica.CasellaPostale,
                Citta = mittenteServizio.Anagrafica.Citta,
                ComplementoIndirizzo = mittenteServizio.Anagrafica.ComplementoIndirizzo,
                ComplementoNominativo = mittenteServizio.Anagrafica.ComplementoNominativo,
                Provincia = mittenteServizio.Anagrafica.Provincia,
                Stato = mittenteServizio.Anagrafica.Stato,
                RagioneSociale = mittenteServizio.Anagrafica.RagioneSociale
            };

            mittente.Nominativo = nominativo;

            lolSubmit.Mittente = mittente;
        }

        private void SetDestinatari(LOLSubmit lolSubmit)
        {
            int count = 0;

            var destinatariServizioList = Servizio.ServizioAnagrafiche.Where(d => d.IsMittente == false).Select(d => d.Anagrafica);

            var listDestinatari = new List<Destinatario>();

            foreach (var destinatarioServizio in destinatariServizioList)
            {
                count++;
                Destinatario newDestinatario = NewDestinatario(destinatarioServizio);
                listDestinatari.Add(newDestinatario);
            }

            lolSubmit.Destinatari = listDestinatari.ToArray();

            lolSubmit.NumeroDestinatari = count;

        }

        private Destinatario NewDestinatario(Anagrafica destinatarioServizio)
        {
            var destinatario = new Destinatario();

            var nominativo = new Nominativo
            {
                Nome = destinatarioServizio.Nome,
                Cognome = destinatarioServizio.Cognome,
                Indirizzo = new Indirizzo
                {
                    DUG = destinatarioServizio.DUG,
                    Toponimo = destinatarioServizio.Toponimo,
                    Esponente = destinatarioServizio.Esponente,
                    NumeroCivico = destinatarioServizio.NumeroCivico
                },
                CAP = destinatarioServizio.Cap,
                CasellaPostale = destinatarioServizio.CasellaPostale,
                Citta = destinatarioServizio.Citta,
                ComplementoIndirizzo = destinatarioServizio.ComplementoIndirizzo,
                ComplementoNominativo = destinatarioServizio.ComplementoNominativo,
                Provincia = destinatarioServizio.Provincia,
                Stato = destinatarioServizio.Stato,
                RagioneSociale = destinatarioServizio.RagioneSociale
            };

            destinatario.Nominativo = nominativo;

            return destinatario;
        }

        private void SetDocumenti(LOLSubmit lolSubmit)
        {

            ServiceReference.LOL.Documento newDocumento;
            var listDocumenti = new List<ServiceReference.LOL.Documento>();

            foreach (var documento in Servizio.ServizioDocumenti.Select(sd => sd.Documento).ToList())
            {
                newDocumento = NewDocumento(documento);
                listDocumenti.Add(newDocumento);
            }

            lolSubmit.Documento = listDocumenti.ToArray();
        }

        private ServiceReference.LOL.Documento NewDocumento(NPCE_Client.Model.Documento documento)
        {
            return new ServiceReference.LOL.Documento { MD5 = GetMD5(documento), Immagine = documento.Content, TipoDocumento = documento.Extension };
        }
    }
}
