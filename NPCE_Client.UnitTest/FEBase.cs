using ComunicazioniElettroniche.Common.Security.Cryptography;
using NPCE.DataModel;
using NPCE_Client.Model;
using NPCE_Client.UnitTest.ServiceReference.LOL;
using NPCE_Client.UnitTest.ServiceReference.Mol;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;


namespace NPCE_Client.Test
{
    public class FEBase
    {

        protected Ambiente ambiente;

        public FEBase(Environment env)
        {
            ambiente = SetAmbiente(env);
        }

        private Ambiente SetAmbiente(Environment environment)
        {
            Ambiente result = null;

            switch (environment)
            {
                case Environment.BTS1023:
                    break;
                case Environment.Collaudo:
                    result = new Ambiente
                    {
                        MolUri = "http://10.60.19.13/RaccomandataMarket/MOLService.svc",
                        LolUri = "http://10.60.19.12/LOLGC/LolService.svc",
                        customerid = "3908983576",
                        smuser = "HH800117",
                        ContrattoMOL = "40000015982",
                        PostaEvoConnectionString = "data source=10.60.19.22\\TPCESQLINST02;initial catalog=PostaEvo;persist security info=True;user id=pasquale;password=pasquale;MultipleActiveResultSets=True;App=EntityFramework;",
                        NomeProprioMol = "DITTA MARKET"
                    };
                    break;
                case Environment.Certificazione:
                    result = new Ambiente
                    {
                        MolUri = "http://10.60.24.36/RaccomandataMarket/MOLService.svc",
                        customerid = "SNPCE002",
                        smuser = "CEPROB01",
                        ContrattoMOL = "00000000049999999999",
                        PostaEvoConnectionString = "data source=10.60.19.22\\TPCESQLINST02;initial catalog=PostaEvo;persist security info=True;user id=pasquale;password=pasquale;MultipleActiveResultSets=True;App=EntityFramework;",
                        NomeProprioMol = "CLIENTE TEST PROB01"
                    };
                    break;
                case Environment.Staging:
                    break;
                case Environment.Bts2016:
                    break;
                case Environment.Produzione:
                    result = new Ambiente
                    {
                        MolUri = "http://10.60.20.52/RaccomandataMarket/MOLService.svc",
                        LolUri = "http://10.60.20.56/LOLGC/LolService.svc",
                        customerid = "SNPCE002",
                        smuser = "CEPROB01",
                        ContrattoMOL = "00000000049999999999",
                        PostaEvoConnectionString = "data source=10.60.19.22\\TPCESQLINST02;initial catalog=PostaEvo;persist security info=True;user id=pasquale;password=pasquale;MultipleActiveResultSets=True;App=EntityFramework;",
                        NomeProprioMol = "CLIENTE TEST PROB01",
                        Username = "CEPROB01",
                        Password = "Cewspr01"
                    };
                    break;
                case Environment.ProduzioneIAM:
                    result = new Ambiente
                    {
                        MolUri = "http://10.60.20.52/RaccomandataMarket/MOLService.svc",
                        LolUri = "https://cewebservices.posteitaliane.it/LOLGC/LolService.svc",
                        customerid = "SNPCE002",
                        smuser = "CEPROB01",
                        ContrattoMOL = "00000000049999999999",
                        PostaEvoConnectionString = "data source=10.60.19.22\\TPCESQLINST02;initial catalog=PostaEvo;persist security info=True;user id=pasquale;password=pasquale;MultipleActiveResultSets=True;App=EntityFramework;",
                        NomeProprioMol= "CLIENTE TEST PROB01",
                        Username= "CEPROB01",
                        Password= "Cewspr01",
                        FromIAM = true
                    };
                    break;
                default:
                    break;
            }

            return result;
        }

        protected InvioRequest GetMolInvio()
        {

            string codiceContratto = ambiente.ContrattoMOL;
            var invio = new InvioRequest();
            invio.MarketOnline = new MarketOnline();
            invio.MarketOnline.AutoConferma = true;

            invio.MarketOnline.Mittente = new UnitTest.ServiceReference.Mol.Mittente
            {
                Cap = "05100",
                ComplementoIndirizzo = "Complemento Indirizzo",
                ComplementoNominativo = "ComplementoNominativo",
                Comune = "ROMA",
                Indirizzo = "Viale Europa 187",
                Nazione = "ITALIA",
                Nominativo = "DITTA MARKET",
                Provincia = "RM"
            };

            invio.MarketOnline.Mittente.Nominativo = ambiente.NomeProprioMol;


            var destinatario = new UnitTest.ServiceReference.Mol.Destinatario
            {
                Cap = "05100",
                ComplementoIndirizzo = "Complemento Indirizzo",
                ComplementoNominativo = "ComplementoNominativo",
                Comune = "ROMA",
                Indirizzo = "Viale Europa 187",
                Nazione = "ITALIA",
                Nominativo = "Nominativo",
                Provincia = "RM"
            };

            invio.MarketOnline.Destinatari = new UnitTest.ServiceReference.Mol.Destinatario[] { destinatario };

            var docPath = Path.Combine(Directory.GetCurrentDirectory(), "Documenti", "Docx_1_Pagina.docx");
            var content = File.ReadAllBytes(docPath);

            invio.MarketOnline.Documenti = new UnitTest.ServiceReference.Mol.Documento[]{
                new UnitTest.ServiceReference.Mol.Documento
                {
                      Estensione ="docx",
                      PercorsoDocumentoLotto= null,
                      Contenuto =  File.ReadAllBytes(docPath),
                      MD5 = Crypto.GetMD5HashString(content)
                }
            };

            invio.MarketOnline.Opzioni = new Opzioni();

            invio.MarketOnline.Opzioni.Servizio = new OpzioniServizio
            {
                ArchiviazioneDocumenti = ModalitaArchiviazione.NESSUNA,
                AnniArchiviazione = "0",
                AttestazioneConsegna = false,
                Consegna = ModalitaConsegna.S,
                SecondoTentativoRecapito = false

            };

            invio.MarketOnline.Opzioni.Stampa = new OpzioniStampa
            {
                FronteRetro = false,
                TipoColore = TipoColore.BW
            };

            invio.MarketOnline.Bollettini = new UnitTest.ServiceReference.Mol.PaginaBollettino[] { GetPaginaBollettino896() };


            invio.Intestazione = new Intestazione
            {
                CodiceContratto = codiceContratto,
                Prodotto = ProdottoPostaEvo.MOL1
            };

            return invio;
        }

        protected LOLSubmit GetLolInvio(string idRichiesta)
        {
            string xmlInvioLol = @"
    <Invio xmlns='http://ComunicazioniElettroniche.LOL.WS' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
      <IDRichiesta>cfb12c35-de69-47ff-9cc7-aaf945e2f710</IDRichiesta>
      <Cliente></Cliente>
      <LOLSubmit ForzaInvioDestinazioniValide='false' PrezzaturaSincrona='true'>
        <Mittente InviaStampa='false'>
          <ns1:Nominativo
            CAP='00144'
            CasellaPostale=''
            Citta='ROMA'
            Cognome=''
            ComplementoIndirizzo=''
            ComplementoNominativo=''
            ForzaDestinazione='false'
            Frazione=''
            Nome=''
            Provincia='RM'
            RagioneSociale='Aldo Petri'
            Stato='ITALIA'
            Telefono=''
            TipoIndirizzo='NORMALE'
            UfficioPostale=''
            Zona=''
            xmlns:ns1='http://ComunicazioniElettroniche.XOL'><ns1:Indirizzo DUG='' Esponente='' NumeroCivico='' Toponimo='EUROPA'/></ns1:Nominativo>
        </Mittente>
        <Destinatari>
          <Destinatario IdDestinatario='70799'>
            <ns2:Nominativo
              CAP='90123'
              CasellaPostale=''
              Citta='PALERMO'
              Cognome=''
              ComplementoIndirizzo=''
              ComplementoNominativo=''
              ForzaDestinazione='true'
              Frazione=''
              Nome=''
              Provincia='pa'
              RagioneSociale='Prova prova'
              Stato='ITALIA'
              Telefono=''
              TipoIndirizzo='NORMALE'
              UfficioPostale=''
              Zona=''
              xmlns:ns2='http://ComunicazioniElettroniche.XOL'><ns2:Indirizzo DUG='' Esponente='' NumeroCivico='' Toponimo='Via Palermo'/></ns2:Nominativo>
          </Destinatario>
        </Destinatari>
        <NumeroDestinatari>1</NumeroDestinatari>
        <Documento TipoDocumento='doc'>
          <ns3:Immagine xmlns:ns3='http://ComunicazioniElettroniche.XOL'>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eXBlL0NhdGFsb2cvUGFnZXMgMyAwIFI+PgplbmRvYmoKMTIgMCBvYmoKPDwvUHJvZHVjZXIoaVRleHSuIDUuNS43IKkyMDAwLTIwMTUgaVRleHQgR3JvdXAgTlYgXChBR1BMLXZlcnNpb25cKSkvTW9kRGF0ZShEOjIwMjAxMTIzMTc0MTM4KzAxJzAwJykvQ3JlYXRpb25EYXRlKEQ6MjAyMDExMjMxNzQxMzgrMDEnMDAnKT4+CmVuZG9iagp4cmVmCjAgMTMKMDAwMDAwMDAwMCA2NTUzNSBmIAowMDAwMDAwMzU2IDAwMDAwIG4gCjAwMDAwMDAwMTUgMDAwMDAgbiAKMDAwMDAxODgzNiAwMDAwMCBuIAowMDAwMDAwMTMzIDAwMDAwIG4gCjAwMDAwMDA2OTIgMDAwMDAgbiAKMDAwMDAwMDcyNSAwMDAwMCBuIAowMDAwMDAxMTA1IDAwMDAwIG4gCjAwMDAwMDEzOTIgMDAwMDAgbiAKMDAwMDAwMTc2NyAwMDAwMCBuIAowMDAwMDAyMDI1IDAwMDAwIG4gCjAwMDAwMTg4ODcgMDAwMDAgbiAKMDAwMDAxODkzMyAwMDAwMCBuIAp0cmFpbGVyCjw8L1Jvb3QgMTEgMCBSL0lEIFs8OGZjMTliMjYzZjY2MzI2OWZjNDcyOTI1NzhkMjQ0ZWI+PDhmYzE5YjI2M2Y2NjMyNjlmYzQ3MjkyNTc4ZDI0NGViPl0vSW5mbyAxMiAwIFIvU2l6ZSAxMz4+CiVpVGV4dC01LjUuNwpzdGFydHhyZWYKMTkwOTEKJSVFT0YKMTEgMCBvYmoKPDwvVHlwZS9DYXRhbG9nL1BhZ2VzIDMgMCBSL01ldGFkYXRhIDE0IDAgUj4+CmVuZG9iagoxMiAwIG9iago8PC9Qcm9kdWNlcihBZG9iZSBMaXZlQ3ljbGUgRVM0KS9Nb2REYXRlKEQ6MjAyMDExMjMxNzQxMzgrMDEnMDAnKS9DcmVhdGlvbkRhdGUoRDoyMDIwMTEyMzE3NDEzOCswMScwMCcpL0NyZWF0b3IoQWRvYmUgTGl2ZUN5Y2xlIEVTNCk+PgplbmRvYmoKMTQgMCBvYmoKPDwvTGVuZ3RoIDUxNzAvVHlwZS9NZXRhZGF0YS9TdWJ0eXBlL1hNTD4+c3RyZWFtCjw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+Cjx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC4yMCI+CiAgIDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+CiAgICAgIDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiCiAgICAgICAgICAgIHhtbG5zOnBkZj0iaHR0cDovL25zLmFkb2JlLmNvbS9wZGYvMS4zLyIKICAgICAgICAgICAgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIgogICAgICAgICAgICB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIKICAgICAgICAgICAgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIj4KICAgICAgICAgPHBkZjpQcm9kdWNlcj5BZG9iZSBMaXZlQ3ljbGUgRVM0PC9wZGY6UHJvZHVjZXI+CiAgICAgICAgIDx4bXA6Q3JlYXRlRGF0ZT4yMDIwLTExLTIzVDE3OjQxOjM4KzAxOjAwPC94bXA6Q3JlYXRlRGF0ZT4KICAgICAgICAgPHhtcDpNb2RpZnlEYXRlPjIwMjAtMTEtMjNUMTc6NDE6MzgrMDE6MDA8L3htcDpNb2RpZnlEYXRlPgogICAgICAgICA8eG1wOk1ldGFkYXRhRGF0ZT4yMDIwLTExLTIzVDE3OjQxOjM4KzAxOjAwPC94bXA6TWV0YWRhdGFEYXRlPgogICAgICAgICA8eG1wOkNyZWF0b3JUb29sPkFkb2JlIExpdmVDeWNsZSBFUzQ8L3htcDpDcmVhdG9yVG9vbD4KICAgICAgICAgPHhtcE1NOkRvY3VtZW50SUQ+dXVpZDpkYWZlN2FiMC0zMGY2LTJlMGEtN2MxOC0zMmQ3ODA2MzAwMTY8L3htcE1NOkRvY3VtZW50SUQ+CiAgICAgICAgIDx4bXBNTTpJbnN0YW5jZUlEPnV1aWQ6YTQ0ZDY3YzktMWY4ZC0yYzBmLTcxMjQtMzJkNzgwNjMwMDE2PC94bXBNTTpJbnN0YW5jZUlEPgogICAgICAgICA8ZGM6Zm9ybWF0PmFwcGxpY2F0aW9uL3BkZjwvZGM6Zm9ybWF0PgogICAgICA8L3JkZjpEZXNjcmlwdGlvbj4KICAgPC9yZGY6UkRGPgo8L3g6eG1wbWV0YT4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgIAo8P3hwYWNrZXQgZW5kPSJ3Ij8+ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKZW5kc3RyZWFtCmVuZG9iagp4cmVmCjExIDIgCjAwMDAwMTk1MTUgMDAwMDAgbg0KMDAwMDAxOTU3NyAwMDAwMCBuDQoxNCAxIAowMDAwMDE5NzI4IDAwMDAwIG4NCnRyYWlsZXIKPDwvUm9vdCAxMSAwIFIvSW5mbyAxMiAwIFIvSURbPDhGQzE5QjI2M0Y2NjMyNjlGQzQ3MjkyNTc4RDI0NEVCPjw2ODcxNTBFMzRFMjM2QjkwQkVFNEQ4NEREOEMyMUJERj5dL1NpemUgMTUvUHJldiAxOTA5MT4+CnN0YXJ0eHJlZgoyNDk3NAolJUVPRgoxNCAwIG9iago8PC9MZW5ndGggNTE3MC9UeXBlL01ldGFkYXRhL1N1YnR5cGUvWE1MPj5zdHJlYW0KPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4KPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS4wLjIwIj4KICAgPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4KICAgICAgPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIKICAgICAgICAgICAgeG1sbnM6cGRmPSJodHRwOi8vbnMuYWRvYmUuY29tL3BkZi8xLjMvIgogICAgICAgICAgICB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOnhtcE1NPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vIgogICAgICAgICAgICB4bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iPgogICAgICAgICA8cGRmOlByb2R1Y2VyPkFkb2JlIExpdmVDeWNsZSBFUzQ8L3BkZjpQcm9kdWNlcj4KICAgICAgICAgPHhtcDpDcmVhdGVEYXRlPjIwMjAtMTEtMjNUMTc6NDE6MzgrMDE6MDA8L3htcDpDcmVhdGVEYXRlPgogICAgICAgICA8eG1wOk1vZGlmeURhdGU+MjAyMC0xMS0yM1QxNzo0MTozOCswMTowMDwveG1wOk1vZGlmeURhdGU+CiAgICAgICAgIDx4bXA6TWV0YWRhdGFEYXRlPjIwMjAtMTEtMjNUMTc6NDE6MzgrMDE6MDA8L3htcDpNZXRhZGF0YURhdGU+CiAgICAgICAgIDx4bXA6Q3JlYXRvclRvb2w+QWRvYmUgTGl2ZUN5Y2xlIEVTNDwveG1wOkNyZWF0b3JUb29sPgogICAgICAgICA8eG1wTU06RG9jdW1lbnRJRD51dWlkOmRhZmU3YWIwLTMwZjYtMmUwYS03YzE4LTMyZDc4MDYzMDAxNjwveG1wTU06RG9jdW1lbnRJRD4KICAgICAgICAgPHhtcE1NOkluc3RhbmNlSUQ+dXVpZDozNGY4N2Q2ZS02MWY3LTJkZTktNWM2Yi0zMmQ3ODA2MzAwMTY8L3htcE1NOkluc3RhbmNlSUQ+CiAgICAgICAgIDxkYzpmb3JtYXQ+YXBwbGljYXRpb24vcGRmPC9kYzpmb3JtYXQ+CiAgICAgIDwvcmRmOkRlc2NyaXB0aW9uPgogICA8L3JkZjpSREY+CjwveDp4bXBtZXRhPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgCjw/eHBhY2tldCBlbmQ9InciPz4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAplbmRzdHJlYW0KZW5kb2JqCnhyZWYKMTQgMSAKMDAwMDAyNTIwMiAwMDAwMCBuDQp0cmFpbGVyCjw8L1Jvb3QgMTEgMCBSL0luZm8gMTIgMCBSL0lEWzw4RkMxOUIyNjNGNjYzMjY5RkM0NzI5MjU3OEQyNDRFQj48QzRGNDQ4MDU3MTg5NzFCREU2M0FDODUyRDUyNjVERTg+XS9TaXplIDE2L1ByZXYgMjQ5NzQ+PgpzdGFydHhyZWYKMzA0NDgKJSVFT0YK</ns3:Immagine>
          <ns4:MD5 xmlns:ns4='http://ComunicazioniElettroniche.XOL'>c335aae545e310cb8e3efa493a5d0ab3</ns4:MD5><ns5:Firmatari xmlns:ns5='http://ComunicazioniElettroniche.XOL'/></Documento>
        <Opzioni DPM='false' Archiviazione='false' DataStampa='2020-11-23T16:17:49.265Z' FirmaElettronica='false' InserisciMittente='true' SecurPaper='false'><Inserti InserisciMittente='false'/>
          <OpzionidiStampa BW='true' FronteRetro='false'>
            <ns6:OpzioniAggiuntiveStampa xmlns:ns6='http://ComunicazioniElettroniche.XOL'><ns6:ArrayOfServizioAggiuntivo ID='0' Descrizione='Numero Pagine' Valore='6'/></ns6:OpzioniAggiuntiveStampa>
          </OpzionidiStampa><OpzioniAggiuntive/></Opzioni><ServiziAggiuntivi/>
        <DescrizioneLettera>
          <TracciaturaLettera>false</TracciaturaLettera>
          <TipoLettera>Posta4</TipoLettera>
        </DescrizioneLettera>
      </LOLSubmit>
    </Invio>
";
            var invioLOL = ComunicazioniElettroniche.Common.Serialization.SerializationUtility.Deserialize<LOLSubmit>(xmlInvioLol);


            return invioLOL;


        }

        protected T GetProxy<T>(string endpointAddress)
        {

            BasicHttpsBinding secureBinding;
            BasicHttpBinding nonSecureBinding;
            ChannelFactory<T> myChannelFactory;
            // Create the binding.
            EndpointAddress ea = new EndpointAddress(endpointAddress);

            if (ambiente.FromIAM)
            {
                secureBinding = new BasicHttpsBinding();
                secureBinding.Security.Mode = BasicHttpsSecurityMode.Transport;
                secureBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
                secureBinding.SendTimeout = TimeSpan.FromMinutes(3);
                secureBinding.MaxReceivedMessageSize = 2147483647;
                myChannelFactory = new ChannelFactory<T>(secureBinding, ea);
                myChannelFactory.Credentials.UserName.UserName = ambiente.Username;
                myChannelFactory.Credentials.UserName.Password = ambiente.Password;
            }

            else
            {
                nonSecureBinding = new BasicHttpBinding();
                nonSecureBinding.SendTimeout = TimeSpan.FromMinutes(3);
                nonSecureBinding.MaxReceivedMessageSize = 2147483647;
                myChannelFactory = new ChannelFactory<T>(nonSecureBinding, ea);
            }
           
            // Create a channel
            T wcfClient = myChannelFactory.CreateChannel();
            return wcfClient;
        }


        protected UnitTest.ServiceReference.Mol.PaginaBollettino GetPaginaBollettino896()
        {
            return new UnitTest.ServiceReference.Mol.PaginaBollettino
            {
                Bollettino = new UnitTest.ServiceReference.Mol.Bollettino896
                {
                    AdditionalInfo = "AdditionalInfo",
                    Causale = "Causale",
                    EseguitoDa = new UnitTest.ServiceReference.Mol.BollettinoEseguitoDa
                    {
                        CAP = "00168",
                        Localita = "Roma",
                        Nominativo = "Mario Rossi",
                        Indirizzo = "Via Alberto Manzi 36"
                    },
                    FormatoStampa = UnitTest.ServiceReference.Mol.FormatoStampaBollettino.BIS,
                    IntestatoA = "GANASSA GIULIANA",
                    Logo = Encoding.ASCII.GetBytes("UEsDBBQABgAIAAAAIQDEY+UY7wEAAL0K"),
                    NumeroContoCorrente = "001000000420",
                    Template = "896",
                    Codice = "CodiceCliente",
                    ImportoEuro = 20M,
                    CodiceCliente = "CodiceCliente"

                }
            };
        }

        protected HttpRequestMessageProperty GetHttpHeaders(Ambiente ambiente)
        {
            var property = new HttpRequestMessageProperty();
            property.Headers.Add("customerid", ambiente.customerid);
            property.Headers.Add("smuser", ambiente.smuser);
            property.Headers.Add("costcenter", ambiente.costcenter);
            property.Headers.Add("billingcenter", ambiente.billingcenter);
            property.Headers.Add("idsender", ambiente.idsender);
            property.Headers.Add("contracttype", ambiente.contracttype);
            property.Headers.Add("sendersystem", ambiente.sendersystem);
            property.Headers.Add("contractid", ambiente.contractid);
            property.Headers.Add("customer", ambiente.customer);
            property.Headers.Add("usertype", ambiente.usertype);
            return property;
        }

        protected bool CheckStatusPostaEvo(string idRichiesta , string status, TimeSpan timeout , TimeSpan retryInterval )
        {
            bool result = false;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            while(sw.Elapsed <= timeout)
            {
                using (var ctx = new PostaEvoEntities())
                {
                    ctx.Database.Connection.ConnectionString = ambiente.PostaEvoConnectionString;
                    var richiesta = ctx.Richieste.Where(r => r.IdRichiesta == new Guid(idRichiesta)).FirstOrDefault();

                    if (richiesta != null)
                    {
                        if (richiesta.StatoCorrente == status)
                        {
                            result = true;
                            break;
                        }
                        else Thread.Sleep(retryInterval);
                    }
                }
            }
            return result;
        }



    }
}
