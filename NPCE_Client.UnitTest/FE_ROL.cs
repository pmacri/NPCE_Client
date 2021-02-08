using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE_Client.UnitTest.ServiceReference.ROL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using Environment = NPCE_Client.Test.Environment;
using Richiesta = NPCE_Client.UnitTest.ServiceReference.ROL.Richiesta;
using PreConfermaRequest = NPCE_Client.UnitTest.ServiceReference.ROL.PreConfermaRequest;
using NPCE.DataModel;
using System.Linq;
using System.Threading.Tasks;

namespace NPCE_Client.UnitTest
{
    [TestClass]
    public class FE_ROL : TestBase
    {
        public FE_ROL() : base(Environment.Collaudo)
        {

        }

        [TestMethod]
        public void Rol_RecuperaIdRichiesta_Massive()
        {
            Stopwatch sw = new Stopwatch();

            Dictionary<string, int> errors = new Dictionary<string, int>();

            int errorCount = 0;
            int errorCountTimeout = 0;

            string lastError = string.Empty;

            string lastErrorTimeout = string.Empty;

            sw.Start();
            for (int i = 0; i < 10; i++)
            {

                try
                {
                    string idRichiesta = RecuperaIdRichiesta();
                    Assert.IsNotNull(idRichiesta);
                }

                catch (TimeoutException tex)
                {
                    lastErrorTimeout = tex.StackTrace;
                    errorCountTimeout += 1;
                }

                catch (CommunicationException cex)
                {
                    lastError = cex.InnerException.Message;
                    if (errors.ContainsKey(lastError))
                    {
                        errors[lastError] = errors[lastError] + 1;
                    }
                    else
                    {
                        errors[lastError] = 1;
                    }
                    errorCount += 1;
                }

            }

            Debug.WriteLine(sw.Elapsed.ToString());
            Debug.WriteLine(errorCount.ToString());
            Debug.WriteLine(lastError);
            Debug.WriteLine(lastErrorTimeout);

            // var invioLol = GetLolInvio(idRichiesta);
        }

        [TestMethod]
        public void Rol_RecuperaIdRichiesta()
        {
            try
            {
                string idRichiesta = RecuperaIdRichiesta();
                Assert.IsNotNull(idRichiesta);
            }

            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public void Rol_Invio()
        {
            string idRichiesta = RecuperaIdRichiesta();
            Assert.IsNotNull(idRichiesta);

            var invioRol = GetRolInvio(idRichiesta);
            var proxy = GetProxy<ROLServiceSoap>(ambiente.RolUri);
            var fake = new OperationContextScope((IContextChannel)proxy);
            HttpRequestMessageProperty headers = GetHttpHeaders(ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            var result = proxy.Invio(idRichiesta, "CLIENTE", invioRol);

            var guidUtente = result.GuidUtente;

            Assert.AreEqual(result.CEResult.Type, "I");

            Debug.WriteLine(idRichiesta);
        }

        [TestMethod]
        public void Rol_Invio_Inesito_Digitale()
        {
            string idRichiesta = RecuperaIdRichiesta();
            Assert.IsNotNull(idRichiesta);

            var invioRol = GetRolInvio(idRichiesta);
            invioRol.Destinatari[0].Nominativo.InesitateDigitali = true;
            invioRol.Destinatari[0].Nominativo.CodiceFiscale = "MSSDRA83C63B626C";

            var proxy = GetProxy<ROLServiceSoap>(ambiente.RolUri);
            var fake = new OperationContextScope((IContextChannel)proxy);
            HttpRequestMessageProperty headers = GetHttpHeaders(ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            var result = proxy.Invio(idRichiesta, "CLIENTE", invioRol);

            var guidUtente = result.GuidUtente;

            Assert.AreEqual(result.CEResult.Type, "I");

            Debug.WriteLine(idRichiesta);


        }

        [TestMethod]
        public void Confirm_Service()
        {
            string idRichiesta = "549551eb-4c76-4e6a-aba1-c37e4c83d8b4";
            string guidUtente = string.Empty;

            using (var ctx = new NPCEROLEntities())
            {
                ctx.Database.Connection.ConnectionString = ambiente.RolConnectionString;
                var rol= ctx.ROL.Where(r => r.IdRichiesta.ToString() == idRichiesta).FirstOrDefault();
                guidUtente = rol.GuidUtente;
            }

            var proxy = GetProxy<ROLServiceSoap>(ambiente.RolUri);
            var fake = new OperationContextScope((IContextChannel)proxy);
            HttpRequestMessageProperty headers = GetHttpHeaders(ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;
            var listRichieste = new List<ServiceReference.ROL.Richiesta>();
            listRichieste.Add(new Richiesta() { GuidUtente = guidUtente, IDRichiesta = idRichiesta });
            PreConfermaRequest request = new PreConfermaRequest { Richieste = listRichieste.ToArray(), autoConferma = true };
            proxy = GetProxy<ROLServiceSoap>(ambiente.RolUri);
            fake = new OperationContextScope((IContextChannel)proxy);
            headers = GetHttpHeaders(ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;
            var preConfermaResult = proxy.PreConferma(request);
            Assert.AreEqual(preConfermaResult.PreConfermaResult.CEResult.Type, "I");

            CheckStatusRol(idRichiesta, "L", TimeSpan.FromMinutes(3), TimeSpan.FromSeconds(20));

        }

        [TestMethod]

        public void Rol_Invio_Preconferma_AutoConferma()
        {
            string idRichiesta = RecuperaIdRichiesta();
            Assert.IsNotNull(idRichiesta);

            var invioRol = GetRolInvio(idRichiesta);
            var proxy = GetProxy<ROLServiceSoap>(ambiente.RolUri);
            var fake = new OperationContextScope((IContextChannel)proxy);
            HttpRequestMessageProperty headers = GetHttpHeaders(ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            var result = proxy.Invio(idRichiesta, "CLIENTE", invioRol);

            var guidUtente = result.GuidUtente;

            Assert.AreEqual(result.CEResult.Type, "I");

            Thread.Sleep(30000);

            var listRichieste = new List<ServiceReference.ROL.Richiesta>();
            listRichieste.Add(new Richiesta() { GuidUtente = guidUtente, IDRichiesta = idRichiesta });
            PreConfermaRequest request = new PreConfermaRequest { Richieste = listRichieste.ToArray(), autoConferma = true };
            proxy = GetProxy<ROLServiceSoap>(ambiente.RolUri);
            fake = new OperationContextScope((IContextChannel)proxy);
            headers = GetHttpHeaders(ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;
            var preConfermaResult = proxy.PreConferma(request);
            Assert.AreEqual(preConfermaResult.PreConfermaResult.CEResult.Type, "I");
        }

        private void RolInvioConferma()
        {
            string idRichiesta = RecuperaIdRichiesta();
            Assert.IsNotNull(idRichiesta);

            var invioRol = GetRolInvio(idRichiesta);
            var proxy = GetProxy<ROLServiceSoap>(ambiente.RolUri);
            var fake = new OperationContextScope((IContextChannel)proxy);
            HttpRequestMessageProperty headers = GetHttpHeaders(ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            var result = proxy.Invio(idRichiesta, "CLIENTE", invioRol);

            var guidUtente = result.GuidUtente;

            Assert.AreEqual(result.CEResult.Type, "I");

            CheckStatusRol(idRichiesta,"R", TimeSpan.FromMinutes(2), TimeSpan.FromSeconds(20));
            var listRichieste = new List<ServiceReference.ROL.Richiesta>();
            listRichieste.Add(new Richiesta() { GuidUtente = guidUtente, IDRichiesta = idRichiesta });
            PreConfermaRequest request = new PreConfermaRequest { Richieste = listRichieste.ToArray(), autoConferma = true };
            proxy = GetProxy<ROLServiceSoap>(ambiente.RolUri);
            fake = new OperationContextScope((IContextChannel)proxy);
            headers = GetHttpHeaders(ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;
            var preConfermaResult = proxy.PreConferma(request);
            Assert.AreEqual(preConfermaResult.PreConfermaResult.CEResult.Type, "I");
        }

        [TestMethod]
        public void Rol_Invio_2_Destinatari_Indirizzo_Minuscolo_Preconferma_AutoConferma()
        {
            string idRichiesta = RecuperaIdRichiesta();
            Assert.IsNotNull(idRichiesta);

            var invioRol = GetRolInvio(idRichiesta);
            invioRol.Destinatari[0].Nominativo.ComplementoNominativo = "comp nonimativo";
            invioRol.Destinatari[0].Nominativo.Nome = "nome";
            invioRol.Destinatari[0].Nominativo.Cognome = "cognome";
            invioRol.Destinatari[0].Nominativo.Indirizzo.DUG = "via";
            invioRol.Destinatari[0].Nominativo.Indirizzo.Toponimo = "delle rose";
            invioRol.Destinatari[0].Nominativo.Indirizzo.NumeroCivico = "74";
            invioRol.Destinatari[0].Nominativo.Indirizzo.Esponente = "c";
            invioRol.Destinatari[0].Nominativo.Provincia = "tr";
            invioRol.Destinatari[0].Nominativo.Citta = "terni";
            var proxy = GetProxy<ROLServiceSoap>(ambiente.RolUri);
            var fake = new OperationContextScope((IContextChannel)proxy);
            HttpRequestMessageProperty headers = GetHttpHeaders(ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            var result = proxy.Invio(idRichiesta, "CLIENTE", invioRol);

            var guidUtente = result.GuidUtente;

            Assert.AreEqual(result.CEResult.Type, "I");

            Thread.Sleep(30000);

            var listRichieste = new List<ServiceReference.ROL.Richiesta>();
            listRichieste.Add(new Richiesta() { GuidUtente = guidUtente, IDRichiesta = idRichiesta });
            PreConfermaRequest request = new PreConfermaRequest { Richieste = listRichieste.ToArray(), autoConferma = true };
            proxy = GetProxy<ROLServiceSoap>(ambiente.RolUri);
            fake = new OperationContextScope((IContextChannel)proxy);
            headers = GetHttpHeaders(ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;
            var preConfermaResult = proxy.PreConferma(request);
            Assert.AreEqual(preConfermaResult.PreConfermaResult.CEResult.Type, "I");

            CheckStatusRol(idRichiesta, "L", TimeSpan.FromMinutes(3), TimeSpan.FromSeconds(20));
        }


        [TestMethod]
        public void Rol_Invio_Archiviazione_Storica()
        {
            string idRichiesta = RecuperaIdRichiesta();
            Assert.IsNotNull(idRichiesta);

            var invioRol = GetRolInvio(idRichiesta);

            invioRol.Opzioni.ArchiviazioneDocumenti = "STORICA";
            invioRol.Opzioni.AnniArchiviazione = 3;
            invioRol.Opzioni.AnniArchiviazioneSpecified = true;

            var proxy = GetProxy<ROLServiceSoap>(ambiente.RolUri);
            var fake = new OperationContextScope((IContextChannel)proxy);
            HttpRequestMessageProperty headers = GetHttpHeaders(ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            var result = proxy.Invio(idRichiesta, "CLIENTE", invioRol);

            Assert.AreEqual(result.CEResult.Type, "I");

            CheckStatusRol(idRichiesta, "R", TimeSpan.FromMinutes(3), TimeSpan.FromSeconds(20));
        }

        [TestMethod]
        public void Multiple_Invio_Conferma()
        {
            int N=30;

            List<Task> TaskList = new List<Task>();
            for (int i = 0; i < N; i++)
            {
                TaskList.Add(Task.Run(() => RolInvioConferma())); 
            }
            
            Task t = Task.WhenAll(TaskList.ToArray());
            t.Wait();
        }

        [TestMethod]
        public void Invio_PreConferma_PosteIt()
        {
            string idRichiesta = "B0DDB22A-E21B-4A06-961B-E945176D2617";
            string guidUtente = "ROL202101000000720";
            var userId = "salvatore.fratoni";
            var canale = "WEB";

            var listRichieste = new List<ServiceReference.ROL.Richiesta>();
            listRichieste.Add(new ServiceReference.ROL.Richiesta() { GuidUtente = guidUtente, IDRichiesta = idRichiesta });
            ServiceReference.ROL.PreConfermaRequest request = new ServiceReference.ROL.PreConfermaRequest { Richieste = listRichieste.ToArray(), autoConferma = true };
            var proxy = GetProxy<ROLServiceSoap>(ambiente.RolUri);
            var fake = new OperationContextScope((IContextChannel)proxy);
            var headers = GetHttpHeaders(ambiente);
            headers.Headers["smuser"] = userId;
            headers.Headers["sendersystem"] = canale;
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;
            var preConfermaResult = proxy.PreConferma(request);
            Assert.AreEqual(preConfermaResult.PreConfermaResult.CEResult.Type, "I");
        }

        private string RecuperaIdRichiesta()
        {
            try
            {
                var proxy = GetProxy<ROLServiceSoap>(ambiente.RolUri);
                var fake = new OperationContextScope((IContextChannel)proxy);
                HttpRequestMessageProperty headers = GetHttpHeaders(ambiente);
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;
                var result = proxy.RecuperaIdRichiesta();
                var idRichiesta = result.IDRichiesta;
                return idRichiesta;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
