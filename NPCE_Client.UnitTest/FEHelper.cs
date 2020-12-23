using ComunicazioniElettroniche.Common.Security.Cryptography;
using NPCE_Client.Model;
using NPCE_Client.UnitTest.ServiceReference.Mol;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace NPCE_Client.Test
{
    internal class FEHelper
    {

        public static T GetProxy<T>(string endpointAddress, string username, string password)
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

        public static InvioRequest GetMolInvio(Ambiente ambiente)
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

            invio.MarketOnline.Bollettini = new PaginaBollettino[] { GetPaginaBollettino896() };


            invio.Intestazione = new Intestazione
            {
                CodiceContratto = codiceContratto,
                Prodotto = ProdottoPostaEvo.MOL1
            };

            return invio;
        }

        public static PaginaBollettino GetPaginaBollettino896()
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

        public static HttpRequestMessageProperty GetHttpHeaders(Ambiente ambiente)
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
    }
}