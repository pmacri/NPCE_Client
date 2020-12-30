using ComunicazioniElettroniche.Common.Security.Cryptography;
using NPCE.DataModel;
using NPCE_Client.Test;
using NPCE_Client.UnitTest.ServiceReference.LOL;
using NPCE_Client.UnitTest.ServiceReference.Mol;
using NPCE_Client.UnitTest.ServiceReference.ROL;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using Environment = NPCE_Client.Test.Environment;

namespace NPCE_Client.UnitTest
{
    public class TestBase
    {

        protected Ambiente ambiente;

        public string PathDocument { get; private set; }
        public string HashMD5Document { get; private set; }
        public string HashMD5Cov { get; private set; }

        public TestBase(Environment env)
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
                        LolUri = "http://10.60.19.13/LOLGC/LolService.svc",
                        RolUri = "http://10.60.19.13/ROLGC/RolService.svc",
                        customerid = "3908983576",
                        smuser = "HH800117",
                        ContrattoMOL = "40000015982",
                        PostaEvoConnectionString = "data source=10.60.19.22\\TPCESQLINST02;initial catalog=PostaEvo;persist security info=True;user id=pasquale;password=pasquale;MultipleActiveResultSets=True;App=EntityFramework;",
                        NomeProprioMol = "DITTA MARKET",
                        UrlEntryPoint = "http://10.60.19.37/NPCE_EntryPoint/WsCE.svc",
                        PathDocument = @"\\FSSVIL-b451.rete.testposte\ShareFS\inputdocument\20201127\80ac0000-3d01-c952-0000-000000019a68-879C931DE4FC8E1A48B284747C2B1C99.docx",
                        HashMD5Document = "879C931DE4FC8E1A48B284747C2B1C99",
                        PathCov = @"\\FSSVIL-b451.rete.testposte\ShareFS\inputdocument\20201127\80ac0000-3d01-c952-0000-000000019a68.cov",
                        HashMD5Cov = "5FBA263B3420664720BB6A15F92ED247",
                        PathLoggingFile = "\\\\10.60.19.20\\c$\\NPCE V6\\Logging"
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
                    result = new Ambiente
                    {
                        MolUri = "http://10.60.17.155/RaccomandataMarket/MOLService.svc",
                        LolUri = "http://10.60.17.155/LOLGC/LolService.svc",
                        RolUri = "http://10.60.17.155/ROLGC/RolService.svc",
                        sendersystem = "H2H",
                        customerid = "3909990431",
                        smuser = "H2HSTG06",
                        ContrattoMOL = "00000000040000017267",
                        PostaEvoConnectionString = "data source=10.60.17.150\\STGNPCESQLINST02;initial catalog=PostaEvo;persist security info=True;user id=pce_user;password=Qwerty12;MultipleActiveResultSets=True;App=EntityFramework;",
                        NomeProprioMol = "DITTA MARKET",
                        PathLoggingFile = "\\\\10.60.17.151\\c$\\NPCE V6\\Logging"
                    };
                    break;
                case Environment.Bts2016:
                    break;
                case Environment.Produzione:
                    result = new Ambiente
                    {
                        MolUri = "http://10.60.20.52/RaccomandataMarket/MOLService.svc",
                        LolUri = "http://10.60.20.56/LOLGC/LolService.svc",
                        RolUri = "http://10.60.20.56/ROLGC/RolService.svc",
                        UrlEntryPoint = "http://10.60.20.132/NPCE_EntryPoint/WsCE.svc",
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
                        RolUri = "https://cewebservices.posteitaliane.it/ROLGC/RolService.svc",
                        customerid = "SNPCE002",
                        smuser = "CEPROB01",
                        ContrattoMOL = "00000000049999999999",
                        PostaEvoConnectionString = "data source=10.60.19.22\\TPCESQLINST02;initial catalog=PostaEvo;persist security info=True;user id=pasquale;password=pasquale;MultipleActiveResultSets=True;App=EntityFramework;",
                        NomeProprioMol = "CLIENTE TEST PROB01",
                        Username = "CEPROB01",
                        Password = "Cewspr01",
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
                Cap = "00144",
                ComplementoIndirizzo = "Complemento Indirizzo",
                ComplementoNominativo = "ComplementoNominativo",
                Comune = "ROMA",
                Indirizzo = "Viale Europa",
                Nazione = "ITALIA",
                Nominativo = "DITTA MARKET",
                Provincia = "RM"
            };

            invio.MarketOnline.Mittente.Nominativo = ambiente.NomeProprioMol;


            var destinatario = new UnitTest.ServiceReference.Mol.Destinatario
            {
                Cap = "00144",
                ComplementoIndirizzo = "Complemento Indirizzo",
                ComplementoNominativo = "ComplementoNominativo",
                Comune = "ROMA",
                Indirizzo = "Viale Europa",
                Nazione = "ITALIA",
                Nominativo = "Nominativo",
                Provincia = "RM"
            };

            invio.MarketOnline.Destinatari = new UnitTest.ServiceReference.Mol.Destinatario[] { destinatario };

            ServiceReference.Mol.Documento doc = GetMolDocumento("Docx_1_Pagina", "docx");



            invio.MarketOnline.Documenti = new ServiceReference.Mol.Documento[] { doc };

            invio.MarketOnline.Opzioni = new ServiceReference.Mol.Opzioni();

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

        private ServiceReference.Mol.Documento GetMolDocumento(string docName, string estensioneSenzaPunto)
        {
            var content = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "Documenti", docName));
            return new UnitTest.ServiceReference.Mol.Documento
            {
                Estensione = estensioneSenzaPunto,
                PercorsoDocumentoLotto = null,
                Contenuto = content,
                MD5 = Crypto.GetMD5HashString(content)
            };
        }

        private ServiceReference.LOL.Documento GetLolDocumento(string docName, string estensioneSenzaPunto)
        {
            var content = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "Documenti", docName));
            return new ServiceReference.LOL.Documento
            {
                Firmatari = null,
                Immagine = content,
                TipoDocumento = estensioneSenzaPunto,
                MD5 = Crypto.GetMD5HashString(content)
            };
        }

        protected LOLSubmit GetLolInvio(string idRichiesta, string tipoDocumento, string docName = "Docx_1_Pagina.docx")
        {
            string xmlInvioLol = Envelopes.LolBase;
            var invioLOL = ComunicazioniElettroniche.Common.Serialization.SerializationUtility.Deserialize<LOLSubmit>(xmlInvioLol);
            invioLOL.Documento = null;
            invioLOL.Documento = new ServiceReference.LOL.Documento[] { GetLolDocumento(docName, tipoDocumento) };
            return invioLOL;
        }


        protected ROLSubmit GetRolInvio(string idRichiesta)
        {
            string xmlInvioRol = Envelopes.RolBase;
            var invioROL = ComunicazioniElettroniche.Common.Serialization.SerializationUtility.Deserialize<ROLSubmit>(xmlInvioRol);
            return invioROL;
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

        protected bool CheckStatusPostaEvo(string idRichiesta, string status, TimeSpan timeout, TimeSpan retryInterval)
        {
            bool result = false;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed <= timeout)
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
