using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.Common.Proxy;
using ComunicazioniElettroniche.Common.Serialization;
using ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL;
using ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitResponse;
using ComunicazioniElettroniche.LOL.Web.DataContracts;
using NPCE_Client.Model;
using PosteItaliane.OrderManagement.Schema.SchemaDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace NPCE.Library
{
    public class LOLPil : PilBase
    {
        public LOLPil(NPCE_Client.Model.Servizio servizio, Ambiente ambiente) : base(servizio, ambiente)
        {
            IdRichiesta = Guid.NewGuid().ToString();
        }

        //public LOLPil(NPCE_Client.Model.Servizio servizio, Ambiente ambiente, string idRichiesta) : base(servizio, ambiente, idRichiesta)
        //{
        //}

        public override async Task<NPCEResult> ConfermaAsync(string idRichiesta)
        {

            OrderResponse preConfirmResult;

            string idOrdine;

            

            try
            {

                OrderRequest preconfirmRequest = GetPreConfirmRequest(idRichiesta);
                CE.Header.GUIDMessage = IdRichiesta;
                CE.Body = SerializationUtility.SerializeToXmlElement(preconfirmRequest);

                SubmitRequestRequest request = new SubmitRequestRequest(CE);
                var response = await WsCEClient.SubmitRequestAsync(request);
                preConfirmResult = SerializationUtility.Deserialize<OrderResponse>(response.CE.Body);
                idOrdine = preConfirmResult.IdOrder;

                ConfirmOrder confirmRequest = GetConfirmRequest(idOrdine, preConfirmResult.PaymentTypes[0].TypeDescription);
                CE.Header.GUIDMessage = IdRichiesta;
                CE.Body = SerializationUtility.SerializeToXmlElement(confirmRequest);
                request = new SubmitRequestRequest(CE);
                response = await WsCEClient.SubmitRequestAsync(request);
                

                return new NPCEResult { Code = "I", Description = "Conferma avvenuta correttamente", IdOrdine = idOrdine, IdRichiesta = idRichiesta };

            }
            finally
            {
                WsCEClient.InnerChannel.Close();
            }
        }

        public async override Task<NPCEResult> InviaAsync()
        {
            LetteraSubmit letteraBE = SetLetteraSubmit();
            letteraBE.IdRichiesta = IdRichiesta;
            LetteraResponse letteraResult = null;


            CE.Header.GUIDMessage = IdRichiesta;


            CE.Body = SerializationUtility.SerializeToXmlElement(letteraBE);

            try
            {

                SubmitRequestRequest request = new SubmitRequestRequest(CE);
                var response = await WsCEClient.SubmitRequestAsync(request);
                try
                {
                    letteraResult = SerializationUtility.Deserialize<LetteraResponse>(response.CE.Body);

                    return letteraResult.CreateResult(response.CE);
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            finally
            {
                WsCEClient.InnerChannel.Close();
            }
        }


        private LetteraSubmit SetLetteraSubmit()
        {
            LetteraSubmit letteraBE = new LetteraSubmit();

            letteraBE.IdRichiesta = IdRichiesta;

            letteraBE.NumeroDestinatari = Servizio.ServizioAnagrafiche.Where(d => d.IsMittente == false).Count();
            SetOpzioni(letteraBE);
            SetOpzioniStampa(letteraBE);
            SetMittente(letteraBE);
            SetDestinatari(letteraBE);
            SetDocumenti(letteraBE);
            SetPosta1(letteraBE);

            return letteraBE;
        }

        private static void SetOpzioniStampa(LetteraSubmit letteraBE)
        {

            var tipoStampa = (letteraBE.Opzioni == null || letteraBE.Opzioni.OpzionidiStampa == null) ? OpzionidiStampaTipoStampa.BW : OpzionidiStampaTipoStampa.Color;
            var fronteRetro = (letteraBE.Opzioni == null || letteraBE.Opzioni.OpzionidiStampa == null) ? false : letteraBE.Opzioni.OpzionidiStampa.FronteRetro;
            letteraBE.Opzioni = letteraBE.Opzioni??new Opzioni();
            letteraBE.Opzioni.OpzionidiStampa = new OpzionidiStampa
            {
                FronteRetro = fronteRetro,
                TipoStampa = tipoStampa
            };
        }

        private void SetPosta1(LetteraSubmit lolSubmit)
        {
            if (Servizio?.TipoServizioId == (int)TipoServizioId.POSTA1)
            {
                lolSubmit.Tipo = "LOL_PRO";
            }
            else
            {
                lolSubmit.Tipo = "LOL";
            }
        }

        private void SetDocumenti(LetteraSubmit lolSubmit)
        {

            lolSubmit.Documenti = GetDocumenti();
        }

        private List<LetteraSubmitDocumento> GetDocumenti()
        {
            LetteraSubmitDocumento documento = null;

            LetteraSubmitDocumento cover = null;

            if (Ambiente.Description.Contains("COLLAUDO"))
            {
                documento = new LetteraSubmitDocumento
                {
                    FileHash = "AB8EF323B64C85C8DFCCCD4356E4FB9B",
                    Uri = @"\\FSSVIL-b451.rete.testposte\ShareFS\InputDocument\ROL_db56a17c-12b2-402a-ad51-9e309f895e79.doc",
                    IdPosizione = 2
                };

                cover = new LetteraSubmitDocumento
                {
                    FileHash = "5FBA263B3420664720BB6A15F92ED247",
                    Uri = @"\\FSSVIL-b451.rete.testposte\ShareFS\inputdocument\20201127\80ac0000-3d01-c952-0000-000000019a68.cov",
                IdPosizione = 1
                };

            }

            return new List<LetteraSubmitDocumento> { documento, cover };
        }

        private void SetDestinatari(LetteraSubmit lolSubmit)
        {
            int count = 0;

            var destinatariServizioList = Servizio.ServizioAnagrafiche.Where(d => d.IsMittente == false).Select(d => d.Anagrafica).ToList();

            var listDestinatari = new List<ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.LetteraDestinatario>();

            foreach (var destinatarioServizio in destinatariServizioList)
            {
                count++;
                ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.LetteraDestinatario newDestinatario = NewDestinatario(destinatarioServizio, lolSubmit);
                listDestinatari.Add(newDestinatario);
            }

            lolSubmit.LetteraDestinatario = listDestinatari.ToArray();

            lolSubmit.NumeroDestinatari = count;
        }

        private void SetMittente(LetteraSubmit letteraBE)
        {
            var mittenteServizio = Servizio.ServizioAnagrafiche.Where(d => d.IsMittente == true).FirstOrDefault();
            var mittente = new ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.Mittente();

            var nominativo = new ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.Nominativo
            {
                Nome = mittenteServizio.Anagrafica.Nome,
                Cognome = mittenteServizio.Anagrafica.Cognome,
                Indirizzo = new ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.Indirizzo
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

            letteraBE.Mittente = mittente;
        }

        private void SetOpzioni(LetteraSubmit letteraBE)
        {
            var opzioni = new Opzioni();

            opzioni.DataStampa = DateTime.Now;

            opzioni.ArchiviazioneDocumenti = Servizio.TipoArchiviazione;

            if (Servizio.AnniArchiviazione > 0)
            {
                opzioni.AnniArchiviazione = Servizio.AnniArchiviazione;
                opzioni.AnniArchiviazioneSpecified = true;
            }

            letteraBE.Opzioni = opzioni;
        }

        private ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.LetteraDestinatario NewDestinatario(Anagrafica destinatarioServizio, LetteraSubmit lolSubmit)
        {
            var destinatario = new ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.LetteraDestinatario();

            var nominativo = new ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.Nominativo
            {
                Nome = destinatarioServizio.Nome,
                Cognome = destinatarioServizio.Cognome,
                Indirizzo = new ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.Indirizzo
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


            // TODO
            destinatario.IdLettera = lolSubmit.IdRichiesta;

            int countDestinatari = (lolSubmit.LetteraDestinatario == null) ? 0 : lolSubmit.LetteraDestinatario.Count();
            destinatario.NumeroDestinatarioCorrente = countDestinatari + 1;

            destinatario.Destinatario = new ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.Destinatario();

            destinatario.Destinatario.Nominativo = nominativo;

            return destinatario;
        }

        public override NPCEResult Invia()
        {
            LetteraSubmit letteraBE = SetLetteraSubmit();
            letteraBE.IdRichiesta = IdRichiesta;
            LetteraResponse letteraResult = null;

            CE.Body = SerializationUtility.SerializeToXmlElement(letteraBE);


            try
            {
                var ce = CE; ;
                WsCEClient.SubmitRequest(ref ce);
                try
                {
                    letteraResult = SerializationUtility.Deserialize<LetteraResponse>(ce.Body);

                    return letteraResult.CreateResult(ce);
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            finally
            {
                WsCEClient.InnerChannel.Close();
            }
        }

    }
}
