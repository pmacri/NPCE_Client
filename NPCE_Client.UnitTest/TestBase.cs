using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.Common.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE.DataModel;
using NPCE_Client.Test;
using NPCE_Client.UnitTest.ServiceReference.Col;
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
using ConfirmOrder = PosteItaliane.OrderManagement.Schema.SchemaDefinition.ConfirmOrder;
using ConfirmOrderResponse = PosteItaliane.OrderManagement.Schema.SchemaDefinition.ConfirmOrderResponse;
using OrderRequest = PosteItaliane.OrderManagement.Schema.SchemaDefinition.OrderRequest;
using OrderRequestServiceInstance = PosteItaliane.OrderManagement.Schema.SchemaDefinition.OrderRequestServiceInstance;
using OrderResponse = PosteItaliane.OrderManagement.Schema.SchemaDefinition.OrderResponse;

using Environment = NPCE_Client.Test.Environment;
using PosteItaliane.OrderManagement.Schema.SchemaDefinition;

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
                        ColUri = "http://10.60.19.13/PostaContest/COLService.svc",
                        LolUri = "http://10.60.19.13/LOLGC/LolService.svc",
                        RolUri = "http://10.60.19.13/ROLGC/RolService.svc",
                        customeridMOL = "3908983576",
                        customeridCOL = "3908983583",
                        smuserMOL = "HH800112",
                        smuserCOL = "HH800133",
                        sendersystem = "H2H",
                        ContrattoMOL = "40000015977",
                        ContrattoCOL = "00000000040000015998",
                        PostaEvoConnectionString = "data source=10.60.19.22\\TPCESQLINST02;initial catalog=PostaEvo;persist security info=True;user id=pasquale;password=pasquale;MultipleActiveResultSets=True;App=EntityFramework;",
                        NomeProprioMol = "DITTA MARKET",
                        NomeProprioCol = "DITTA POSTAONLINE",
                        UrlEntryPoint = "http://10.60.19.36/NPCE_EntryPoint/WsCE.svc",
                        PathDocument = @"\\FSSVIL-b451.rete.testposte\ShareFS\inputdocument\DocPil.doc",
                        HashMD5Document = "AB8EF323B64C85C8DFCCCD4356E4FB9B",
                        PathCov = @"\\FSSVIL-b451.rete.testposte\ShareFS\inputdocument\CovPil.cov",
                        HashMD5Cov = "60125C6E43E0C596565B1D35B728F795",
                        PathLoggingFile = "\\\\10.60.19.20\\c$\\NPCE V6\\Logging"
                    };
                    break;
                case Environment.Certificazione:
                    result = new Ambiente
                    {
                        
                        MolUri = "http://10.60.24.36/RaccomandataMarket/MOLService.svc",
                        ColUri = "http://10.60.24.36/PostaContest/COLService.svc",
                        LolUri = "http://10.60.24.36/LOLGC/LolService.svc",
                        RolUri = "http://10.60.24.36/ROLGC/RolService.svc",
                        customeridMOL = "SNPCE002",
                        sendersystem = "H2H",
                        smuserMOL = "CEPROB01",
                        ContrattoMOL = "00000000049999999999",
                        PostaEvoConnectionString = "data source=10.60.19.22\\TPCESQLINST02;initial catalog=PostaEvo;persist security info=True;user id=pasquale;password=pasquale;MultipleActiveResultSets=True;App=EntityFramework;",
                        NomeProprioMol = "CLIENTE TEST PROB01",
                        UrlEntryPoint = "http://10.60.25.228/NPCE_EntryPoint/WsCE.svc",
                        PathDocument = @"\\FSCERT4-a127.retecert.postecert\ShareFS\InputDocument\DocPil.doc",
                        HashMD5Document = "AB8EF323B64C85C8DFCCCD4356E4FB9B",
                        PathCov = @"\\FSCERT4-a127.retecert.postecert\ShareFS\InputDocument\CovPil.cov",
                        HashMD5Cov = "60125C6E43E0C596565B1D35B728F795"
                    };
                    break;
                case Environment.Staging:
                    result = new Ambiente
                    {
                        MolUri = "http://10.60.17.155/RaccomandataMarket/MOLService.svc",
                        ColUri = "http://10.60.17.155/PostaContest/COLService.svc",
                        LolUri = "http://10.60.17.155/LOLGC/LolService.svc",
                        RolUri = "http://10.60.17.155/ROLGC/RolService.svc",
                        UrlEntryPoint = "http://10.60.17.154/NPCE_EntryPoint/WsCE.svc",
                        sendersystem = "H2H",
                        customeridMOL = "3909990431",
                        customeridCOL= "3909991340",
                        smuserMOL = "H2HSTG06",
                        smuserCOL = "H2HSTG01",
                        ContrattoMOL = "00000000040000017267",
                        ContrattoCOL= "40000020188",
                        PostaEvoConnectionString = "data source=10.60.17.150\\STGNPCESQLINST02;initial catalog=PostaEvo;persist security info=True;user id=pce_user;password=Qwerty12;MultipleActiveResultSets=True;App=EntityFramework;",
                        NomeProprioMol = "DITTA MARKET",
                        NomeProprioCol = "Posta Contest",
                        PathLoggingFile = "\\\\10.60.17.151\\c$\\NPCE V6\\Logging",
                        PathDocument = @"\\FSSVIL-b451.rete.testposte\ShareFS\inputdocument\20201127\80ac0000-3d01-c952-0000-000000019a68-879C931DE4FC8E1A48B284747C2B1C99.docx",
                        HashMD5Document = "879C931DE4FC8E1A48B284747C2B1C99",
                        PathCov = @"\\FSSVIL-b451.rete.testposte\ShareFS\inputdocument\20201127\80ac0000-3d01-c952-0000-000000019a68.cov",
                        HashMD5Cov = "5FBA263B3420664720BB6A15F92ED247",
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
                        customeridMOL = "SNPCE002",
                        smuserMOL = "CEPROB01",
                        ContrattoMOL = "00000000049999999999",
                        PostaEvoConnectionString = "data source=10.60.19.22\\TPCESQLINST02;initial catalog=PostaEvo;persist security info=True;user id=pasquale;password=pasquale;MultipleActiveResultSets=True;App=EntityFramework;",
                        NomeProprioMol = "CLIENTE TEST PROB01",
                        Username = "CEPROB01",
                        Password = "Cewspr01",
                        PathDocument = @"\\FSPROD2-56ab.rete.poste\NPCE-ShareR\InputDocuments\DocPil.doc",
                        HashMD5Document = "AB8EF323B64C85C8DFCCCD4356E4FB9B",
                        PathCov = @"\\FSPROD2-56ab.rete.poste\NPCE-ShareR\InputDocuments\CovPil.cov",
                        HashMD5Cov = "60125C6E43E0C596565B1D35B728F795",
                    };
                    break;
                case Environment.ProduzioneIAM:
                    result = new Ambiente
                    {
                        MolUri = "http://10.60.20.52/RaccomandataMarket/MOLService.svc",
                        LolUri = "https://cewebservices.posteitaliane.it/LOLGC/LolService.svc",
                        RolUri = "https://cewebservices.posteitaliane.it/ROLGC/RolService.svc",
                        customeridMOL = "SNPCE002",
                        smuserMOL = "CEPROB01",
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

        protected ServiceReference.Mol.InvioRequest GetMolFEInvio()
        {

            string codiceContratto = ambiente.ContrattoMOL;
            var invio = new ServiceReference.Mol.InvioRequest();
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

            ServiceReference.Mol.Documento doc = GetMolDocumento("Docx_1_Pagina.docx", "docx");

            invio.MarketOnline.Documenti = new ServiceReference.Mol.Documento[] { doc };

            invio.MarketOnline.Opzioni = new ServiceReference.Mol.Opzioni();

            invio.MarketOnline.Opzioni.Servizio = new ServiceReference.Mol.OpzioniServizio
            {
                ArchiviazioneDocumenti = ServiceReference.Mol.ModalitaArchiviazione.NESSUNA,
                AnniArchiviazione = "0",
                AttestazioneConsegna = false,
                Consegna = ServiceReference.Mol.ModalitaConsegna.S,
                SecondoTentativoRecapito = false

            };

            invio.MarketOnline.Opzioni.Stampa = new ServiceReference.Mol.OpzioniStampa
            {
                FronteRetro = false,
                TipoColore = ServiceReference.Mol.TipoColore.BW
            };

            invio.MarketOnline.Bollettini = new UnitTest.ServiceReference.Mol.PaginaBollettino[] { GetPaginaBollettino896Mol() };

            invio.MarketOnline.BollettinoPA = GetFEBollettinoPAMol();

            invio.Intestazione = new ServiceReference.Mol.Intestazione
            {
                CodiceContratto = codiceContratto,
                Prodotto = ServiceReference.Mol.ProdottoPostaEvo.MOL1
            };

            return invio;
        }


        protected ServiceReference.Col.InvioRequest GetColFEInvio()
        {

            string codiceContratto = ambiente.ContrattoCOL;
            var invio = new ServiceReference.Col.InvioRequest();
            invio.PostaContest = new PostaContest();
            invio.PostaContest.AutoConferma = true;

            invio.PostaContest.Mittente = new ServiceReference.Col.Mittente
            {
                Cap = "00144",
                ComplementoIndirizzo = "Complemento Indirizzo",
                ComplementoNominativo = "ComplementoNominativo",
                Comune = "ROMA",
                Indirizzo = "Viale Europa",
                Nazione = "ITALIA",
                Nominativo = "DITTA POSTAONLINE",
                Provincia = "RM"
            };

            invio.PostaContest.Mittente.Nominativo = ambiente.NomeProprioCol;

            var destinatario = new UnitTest.ServiceReference.Col.Destinatario
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

            invio.PostaContest.Destinatari = new UnitTest.ServiceReference.Col.Destinatario[] { destinatario };

            ServiceReference.Col.Documento doc = GetColDocumento("Docx_1_Pagina.docx", "docx");

            invio.PostaContest.Documenti = new ServiceReference.Col.Documento[] { doc };

            invio.PostaContest.Opzioni = new ServiceReference.Col.Opzioni();

            invio.PostaContest.Opzioni.Servizio = new ServiceReference.Col.OpzioniServizio
            {
                TipoArchiviazioneDocumenti = "NESSUNA",
                AnniArchiviazione = "0",
                AttestazioneConsegna = false,
                Consegna = ServiceReference.Col.ModalitaConsegna.S,
                SecondoTentativoRecapito = false

            };

            invio.PostaContest.Opzioni.Stampa = new ServiceReference.Col.OpzioniStampa
            {
                FronteRetro = false,
                TipoColore = ServiceReference.Col.TipoColore.BW
            };

            invio.PostaContest.Bollettini = new UnitTest.ServiceReference.Col.PaginaBollettino[] { GetPaginaBollettino896Col() };

            invio.PostaContest.BollettinoPA = GetFEBollettinoPACol();

            invio.Intestazione = new ServiceReference.Col.Intestazione
            {
                CodiceContratto = codiceContratto,
                Prodotto = ServiceReference.Col.ProdottoPostaEvo.COL1
            };

            return invio;
        }

        private ServiceReference.Mol.AvvisoPagamentoPagoPA GetFEBollettinoPAMol()
        {
            var result = new ServiceReference.Mol.AvvisoPagamentoPagoPA();

            result.Destinatario = new ServiceReference.Mol.AvvisoPagamentoPagoPADestinatario
            {
                CodiceFiscaleDestinatario = "0000045127750318",
                IndirizzoDestinatario = "VIALE EUROPA 190 00144 ROMA SCALA F int1",
                NomeCognomeDestinatario = "PIERMARIA FERDINANDO PASSALAQUAGLIA"
            };

            result.EnteCreditore = new ServiceReference.Mol.AvvisoPagamentoPagoPAEnteCreditore
            {
                AutorizzazioneStampaInProprio = "AUT. DB/xxxx/xxx xxxxx DEL xx/xx/xxxx",
                DenominazioneEnte = "MINISTERO DELLE POLITICHE  AGRICOLE, ALIMENTARI PA",
                CodiceFiscaleEnte = "90942650327",
                InfoEnte = "INFOENTEABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789ABCDEFGHIJLMNOPQRST",
                LogoEnte = Encoding.ASCII.GetBytes("UEsDBBQABgAIAAAAIQDEY+UY7wEAAL0K"),
                SettoreEnte = "SETTOREENTEABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890123"

            };

            result.Pagamento = new ServiceReference.Mol.AvvisoPagamentoPagoPAPagamento
            {
                AllaRata = "alle rate e",
                CBILL = "CBILLABCDE",
                CodiceAvviso = "038209010594456876",
                DelTuoEnte = "Del Tuo Ente Creditore",
                DiPoste = "di Poste Italiane",
                Importo = 600M,
                IntestatarioCCPostale = "GANASSA GIULIANA",
                NumeroCCPostale = "001000000420",
                Oggetto = "OGGETTO DEL PAGAMENTO: PROVE DI COLLAUDO DEL BOLLETTINO PA 5",
                PagamentoRateale = "Puoi pagare anche a rate",
                RateTesto = "oppure in 5 rate",
                Scadenza = new DateTime(2019, 01, 01),
                Rate = new ServiceReference.Mol.AvvisoPagamentoPagoPAPagamentoRata[0]

            };

            return result;
        }

        private ServiceReference.Col.AvvisoPagamentoPagoPA GetFEBollettinoPACol()
        {
            var result = new ServiceReference.Col.AvvisoPagamentoPagoPA();

            result.Destinatario = new ServiceReference.Col.AvvisoPagamentoPagoPADestinatario
            {
                CodiceFiscaleDestinatario = "0000045127750318",
                IndirizzoDestinatario = "VIALE EUROPA 190 00144 ROMA SCALA F int1",
                NomeCognomeDestinatario = "PIERMARIA FERDINANDO PASSALAQUAGLIA"
            };

            result.EnteCreditore = new ServiceReference.Col.AvvisoPagamentoPagoPAEnteCreditore
            {
                AutorizzazioneStampaInProprio = "AUT. DB/xxxx/xxx xxxxx DEL xx/xx/xxxx",
                DenominazioneEnte = "MINISTERO DELLE POLITICHE  AGRICOLE, ALIMENTARI PA",
                CodiceFiscaleEnte = "90942650327",
                InfoEnte = "INFOENTEABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789ABCDEFGHIJLMNOPQRST",
                LogoEnte = Encoding.ASCII.GetBytes("UEsDBBQABgAIAAAAIQDEY+UY7wEAAL0K"),
                SettoreEnte = "SETTOREENTEABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890123"

            };

            result.Pagamento = new ServiceReference.Col.AvvisoPagamentoPagoPAPagamento
            {
                AllaRata = "alle rate e",
                CBILL = "CBILLABCDE",
                CodiceAvviso = "038209010594456876",
                DelTuoEnte = "Del Tuo Ente Creditore",
                DiPoste = "di Poste Italiane",
                Importo = 600M,
                IntestatarioCCPostale = "GANASSA GIULIANA",
                NumeroCCPostale = "001000000420",
                Oggetto = "OGGETTO DEL PAGAMENTO: PROVE DI COLLAUDO DEL BOLLETTINO PA 5",
                PagamentoRateale = "Puoi pagare anche a rate",
                RateTesto = "oppure in 5 rate",
                Scadenza = new DateTime(2019, 01, 01),
                Rate = new ServiceReference.Col.AvvisoPagamentoPagoPAPagamentoRata[0]

            };

            return result;
        }

        protected ServiceReference.Mol.Documento GetMolDocumento(string docName, string estensioneSenzaPunto)
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

        protected ServiceReference.Col.Documento GetColDocumento(string docName, string estensioneSenzaPunto)
        {
            var content = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "Documenti", docName));
            return new UnitTest.ServiceReference.Col.Documento
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
            string xmlInvioLol = Envelopes.LolFE;
            var invioLOL = ComunicazioniElettroniche.Common.Serialization.SerializationUtility.Deserialize<LOLSubmit>(xmlInvioLol);
            invioLOL.Documento = null;
            invioLOL.Documento = new ServiceReference.LOL.Documento[] { GetLolDocumento(docName, tipoDocumento) };
            return invioLOL;
        }
        protected ROLSubmit GetRolInvio(string idRichiesta)
        {
            string xmlInvioRol = Envelopes.RolFE;
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
        protected UnitTest.ServiceReference.Mol.PaginaBollettino GetPaginaBollettino896Mol()
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

        protected UnitTest.ServiceReference.Col.PaginaBollettino GetPaginaBollettino896Col()
        {
            return new UnitTest.ServiceReference.Col.PaginaBollettino
            {
                Bollettino = new UnitTest.ServiceReference.Col.Bollettino896
                {
                    AdditionalInfo = "AdditionalInfo",
                    Causale = "Causale",
                    EseguitoDa = new UnitTest.ServiceReference.Col.BollettinoEseguitoDa
                    {
                        CAP = "00168",
                        Localita = "Roma",
                        Nominativo = "Mario Rossi",
                        Indirizzo = "Via Alberto Manzi 36"
                    },
                    FormatoStampa = UnitTest.ServiceReference.Col.FormatoStampaBollettino.BIS,
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
        protected HttpRequestMessageProperty GetHttpHeaders(Ambiente ambiente, string prodotto ="MOL")
        {
            var property = new HttpRequestMessageProperty();
            string customerId = prodotto.Equals("MOL") ?  ambiente.customeridMOL :  ambiente.customeridCOL;
            string smUser = prodotto.Equals("MOL") ? ambiente.smuserMOL :  ambiente.smuserCOL;

            property.Headers.Add("customerid", customerId);
            property.Headers.Add("smuser", smUser);

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

        protected ConfirmOrderResponse ConfirmServicePIL(string idRichiesta)
        {
            // Prima PreConferma e poi Conferma
            OrderRequest preconfirmRequest = GetPreConfirmRequest(idRichiesta);
            var ceHeader = Helper.GetCeHeader();
            ceHeader.SenderSystem = "H2H";
            ceHeader.IDSender = "999988";
            ceHeader.IdCRM = string.Empty;
            ceHeader.UserId = "nello.citta.npce";
            ceHeader.ContractId = string.Empty;
            ceHeader.GUIDMessage = idRichiesta;
            OrderResponse preConfirmResponse = null;
            var preConfirmResult = Helper.PublishToBizTalk<OrderRequest, OrderResponse>(preconfirmRequest, ceHeader, ambiente.UrlEntryPoint, out preConfirmResponse);
            Assert.AreEqual(TResultResType.I, preConfirmResult.ResType);
            ConfirmOrder confirmRequest = GetConfirmRequest(preConfirmResponse.IdOrder, preConfirmResponse.PaymentTypes[0].TypeDescription);
            ConfirmOrderResponse confirmResponse = null;
            var result = Helper.PublishToBizTalk<ConfirmOrder, ConfirmOrderResponse>(confirmRequest, ceHeader, ambiente.UrlEntryPoint, out confirmResponse);
            Assert.AreEqual(TResultResType.I, result.ResType);
            return confirmResponse;
        }

        protected virtual OrderRequest GetPreConfirmRequest(string idRichiesta)
        {
            OrderRequest orderRequest = new OrderRequest();
            orderRequest.ServiceInstance = new OrderRequestServiceInstance[1];
            orderRequest.ForceOrderCreation = true;

            orderRequest.ServiceInstance[0] = new OrderRequestServiceInstance();
            orderRequest.ServiceInstance[0].GUIDMessage = idRichiesta;

            return orderRequest;

        }

        protected virtual ConfirmOrder GetConfirmRequest(string idOrdine, string typeDescription)
        {
            ConfirmOrder confirmOrder = new ConfirmOrder();
            confirmOrder.IdOrder = idOrdine;
            confirmOrder.PaymentType = new PaymentType
            {
                PostPayment = true,
                PostPaymentSpecified = true,
                TypeDescription = typeDescription,
                TypeId = "6"
            };

            return confirmOrder;
        }


    }
}
