using ComunicazioniElettroniche.Common.Security.Cryptography;
using NPCE.DataModel;
using NPCE.LIbrary.ServiceReference.MOL;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
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
                        customerid = "3908983576",
                        smuser = "HH800117",
                        ContrattoMOL = "40000015982",
                        PostaEvoConnectionString = "data source=10.60.19.22\\TPCESQLINST02;initial catalog=PostaEvo;persist security info=True;user id=pasquale;password=pasquale;MultipleActiveResultSets=True;App=EntityFramework;"
                    };
                    break;
                case Environment.Certificazione:
                    break;
                case Environment.Staging:
                    break;
                case Environment.Bts2016:
                    break;
                case Environment.Produzione:
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

            invio.MarketOnline.Mittente = new Mittente
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

            var destinatario = new Destinatario
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

            invio.MarketOnline.Destinatari = new Destinatario[] { destinatario };

            var docPath = Path.Combine(Directory.GetCurrentDirectory(), "Documenti", "Docx_1_Pagina.docx");
            var content = File.ReadAllBytes(docPath);

            invio.MarketOnline.Documenti = new NPCE.LIbrary.ServiceReference.MOL.Documento[]{
                new NPCE.LIbrary.ServiceReference.MOL.Documento
                {
                      Estensione ="docx",
                      PercorsoDocumentoLotto= null,
                      Contenuto =  File.ReadAllBytes(docPath),
                      MD5 = Crypto.GetMD5HashString(content)
                }
            };

            invio.MarketOnline.Opzioni = new NPCE.LIbrary.ServiceReference.MOL.Opzioni();

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

            invio.MarketOnline.Bollettini = new PaginaBollettino[] { GetPaginaBollettino896() };


            invio.Intestazione = new Intestazione
            {
                CodiceContratto = codiceContratto,
                Prodotto = ProdottoPostaEvo.MOL1
            };

            return invio;
        }

        protected T GetProxy<T>(string endpointAddress, string username, string password)
        {
            BasicHttpBinding myBinding = new BasicHttpBinding();
            myBinding.SendTimeout = TimeSpan.FromMinutes(3);
            myBinding.MaxReceivedMessageSize = 2147483647;
            EndpointAddress myEndpoint = new EndpointAddress(endpointAddress);



            ChannelFactory<T> myChannelFactory = new ChannelFactory<T>(myBinding, myEndpoint);

            myChannelFactory.Credentials.UserName.UserName = username;
            myChannelFactory.Credentials.UserName.Password = password;
            // Create a channel
            T wcfClient = myChannelFactory.CreateChannel();



            return wcfClient;
        }


        protected PaginaBollettino GetPaginaBollettino896()
        {
            return new PaginaBollettino
            {
                Bollettino = new Bollettino896
                {
                    AdditionalInfo = "AdditionalInfo",
                    Causale = "Causale",
                    EseguitoDa = new BollettinoEseguitoDa
                    {
                        CAP = "00168",
                        Localita = "Roma",
                        Nominativo = "Mario Rossi",
                        Indirizzo = "Via Alberto Manzi 36"
                    },
                    FormatoStampa = FormatoStampaBollettino.BIS,
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
