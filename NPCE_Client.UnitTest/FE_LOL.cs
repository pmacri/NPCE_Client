using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE_Client.Test;
using NPCE_Client.UnitTest.ServiceReference.LOL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using Environment = NPCE_Client.Test.Environment;

namespace NPCE_Client.UnitTest
{
    [TestClass]
    public class FE_LOL : TestBase
    {
        public FE_LOL() : base(Environment.Collaudo)
        {

        }

        [TestMethod]
        public void Multiple_Invio_Conferma()
        {
            int N = 30;

            List<Task> TaskList = new List<Task>();
            for (int i = 0; i < N; i++)
            {
                TaskList.Add(Task.Run(() => LolInvioConferma()));
            }

            Task t = Task.WhenAll(TaskList.ToArray());
            t.Wait();
        }

        private void LolInvioConferma()
        {
            string idRichiesta = RecuperaIdRichiesta();
            Assert.IsNotNull(idRichiesta);
            InvioResult result = InvioLOL(idRichiesta);
            var guidUtente = result.GuidUtente;
            Assert.AreEqual(result.CEResult.Type, "I");

            CheckStatusLol(idRichiesta, "R", TimeSpan.FromMinutes(3), TimeSpan.FromSeconds(20));

            var listRichieste = new List<Richiesta>();
            listRichieste.Add(new Richiesta() { GuidUtente = guidUtente, IDRichiesta = idRichiesta });
            PreConfermaRequest request = new PreConfermaRequest { Richieste = listRichieste.ToArray(), autoConferma = true };
            var proxy = GetProxy<LOLServiceSoap>(ambiente.LolUri);
            var fake = new OperationContextScope((IContextChannel)proxy);
            var headers = GetHttpHeaders(ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;
            var preConfermaResult = proxy.PreConferma(request);
            Assert.AreEqual(preConfermaResult.PreConfermaResult.CEResult.Type, "I");

            CheckStatusLol(idRichiesta, "L", TimeSpan.FromMinutes(3), TimeSpan.FromSeconds(20));
        }

        [TestMethod]
        public void Lol_RecuperaIdRichiesta_Massive()
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
        public void Lol_RecuperaIdRichiesta()
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
        public void Lol_Invio()
        {
            string idRichiesta = RecuperaIdRichiesta();
            Assert.IsNotNull(idRichiesta);

            InvioResult result = InvioLOL(idRichiesta);

            Assert.AreEqual(result.CEResult.Type, "I");

            Debug.WriteLine(result.IDRichiesta);

            CheckStatusLol(idRichiesta, "R", TimeSpan.FromMinutes(3), TimeSpan.FromSeconds(20));
        }

        [Ignore]
        [TestMethod]
        public void Lol_Invio_Check_Parametri_Prezzatura_Archiviazione()
        {
            string idRichiesta = RecuperaIdRichiesta();

            Assert.IsNotNull(idRichiesta);

            InvioResult result = InvioLOL(idRichiesta, "STORICA", 3, "Docx_5_Pagine.docx");

            Assert.AreEqual(result.CEResult.Type, "I");

            int numeroFogli = 0;
            int mesiArchiviazione = 0;
            Thread.Sleep(20000);


            Helper.GetParametriPrezzatura("LOL", out numeroFogli, out mesiArchiviazione, ambiente.PathLoggingFile);
            Assert.AreEqual(6, numeroFogli);

            Assert.AreEqual(mesiArchiviazione, 36);
        }

        private InvioResult InvioLOL(string idRichiesta, string tipoArchiviazione = "NESSUNA", int anniArchiviazione =0, string docName = "Docx_1_Pagina.docx", string tipoDocumento ="docx")
        {
            var invioLol = GetLolInvio(idRichiesta, tipoDocumento, docName);
            //invioLol.Opzioni.ArchiviazioneDocumenti = tipoArchiviazione;
            //invioLol.Opzioni.AnniArchiviazione = anniArchiviazione;
            //invioLol.Opzioni.AnniArchiviazioneSpecified = true;

            
            var proxy = GetProxy<LOLServiceSoap>(ambiente.LolUri);
            var fake = new OperationContextScope((IContextChannel)proxy);
            HttpRequestMessageProperty headers = GetHttpHeaders(ambiente);
            
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            var result = proxy.Invio(idRichiesta, "CLIENTE", invioLol);
            return result;
        }

        [TestMethod]
        public void Invio_PreConferma_Con_AutoConferma()
        {
            string idRichiesta = RecuperaIdRichiesta();
            Assert.IsNotNull(idRichiesta);
            InvioResult result = InvioLOL(idRichiesta);
            var guidUtente = result.GuidUtente;
            Assert.AreEqual(result.CEResult.Type, "I");

            Thread.Sleep(30000);

            var listRichieste = new List<Richiesta>();
            listRichieste.Add(new Richiesta() { GuidUtente = guidUtente, IDRichiesta = idRichiesta });
            PreConfermaRequest request = new PreConfermaRequest { Richieste = listRichieste.ToArray(), autoConferma = true };
            var proxy = GetProxy<LOLServiceSoap>(ambiente.LolUri);
            var fake = new OperationContextScope((IContextChannel)proxy);
            var headers = GetHttpHeaders(ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;
            var preConfermaResult = proxy.PreConferma(request);
            Assert.AreEqual(preConfermaResult.PreConfermaResult.CEResult.Type, "I");

            CheckStatusLol(idRichiesta, "L", TimeSpan.FromMinutes(3), TimeSpan.FromSeconds(20));
        }

        private string RecuperaIdRichiesta()
        {
            try
            {
                var proxy = GetProxy<LOLServiceSoap>(ambiente.LolUri);
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
