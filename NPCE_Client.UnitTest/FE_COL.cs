using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE_Client.UnitTest.ServiceReference.Col;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace NPCE_Client.UnitTest
{
    [TestClass]
    public class FE_COL: TestBase
    {

        public FE_COL(): base(Test.Environment.Collaudo)
        {

        }

        [TestMethod]
        public void Multiple_Invio_AutoConferma()
        {
            int N = 30;

            List<Task> TaskList = new List<Task>();
            for (int i = 0; i < N; i++)
            {
                TaskList.Add(Task.Run(() => ColInvioConferma()));
            }

            Task t = Task.WhenAll(TaskList.ToArray());
            t.Wait();
        }

        private void ColInvioConferma()
        {
            InvioRequest colSubmit = GetColFEInvio();

            colSubmit.PostaContest.AutoConferma = true;

            IPostaContestService _proxy = GetProxy<IPostaContestService>(ambiente.ColUri);

            var fake = new OperationContextScope((IContextChannel)_proxy);

            HttpRequestMessageProperty headers = GetHttpHeaders(ambiente, "COL");

            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            colSubmit.PostaContest.Bollettini = null;
            colSubmit.PostaContest.BollettinoPA = null;

            //colSubmit.PostaContest.Opzioni = new Opzioni();

            //colSubmit.PostaContest.Opzioni.Servizio = new OpzioniServizio { TipoArchiviazioneDocumenti = "STORICA", AnniArchiviazione = "6" };

            var invioResult = _proxy.Invio(colSubmit);

            Assert.IsTrue(invioResult.Esito == EsitoPostaEvo.OK);

            string idRichiesta = invioResult.IdRichiesta;

            Assert.IsTrue(CheckStatusPostaEvo(idRichiesta, "L", TimeSpan.FromMinutes(3), TimeSpan.FromSeconds(10)));
        }

        [TestMethod]
        public void Invio_COL1_AutoConferma_False()
        {

            InvioRequest colSubmit = GetColFEInvio();

            colSubmit.PostaContest.AutoConferma = false;

            IPostaContestService _proxy = GetProxy<IPostaContestService>(ambiente.ColUri);

            var fake = new OperationContextScope((IContextChannel)_proxy);

           HttpRequestMessageProperty headers = GetHttpHeaders(ambiente,"COL");

            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            colSubmit.PostaContest.Bollettini = null;
            colSubmit.PostaContest.BollettinoPA = null;

            var invioResult = _proxy.Invio(colSubmit);

            Assert.IsTrue(invioResult.Esito == EsitoPostaEvo.OK);

            string idRichiesta = invioResult.IdRichiesta;

            Assert.IsTrue(CheckStatusPostaEvo(idRichiesta, "K", TimeSpan.FromMinutes(2), TimeSpan.FromSeconds(10)));
        }

        [TestMethod]
        public void Invio_COL1_AutoConferma_True_Archiviazione_Storica_6_Anni()
        {

            InvioRequest colSubmit = GetColFEInvio();

            colSubmit.PostaContest.AutoConferma = true;

            IPostaContestService _proxy = GetProxy<IPostaContestService>(ambiente.ColUri);

            var fake = new OperationContextScope((IContextChannel)_proxy);

            HttpRequestMessageProperty headers = GetHttpHeaders(ambiente, "COL");

            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            colSubmit.PostaContest.Bollettini = null;
            colSubmit.PostaContest.BollettinoPA = null;

            //colSubmit.PostaContest.Opzioni = new Opzioni();

            colSubmit.PostaContest.Opzioni.Servizio = new OpzioniServizio { TipoArchiviazioneDocumenti = "STORICA", AnniArchiviazione = "6" };

            var invioResult = _proxy.Invio(colSubmit);

            Assert.IsTrue(invioResult.Esito == EsitoPostaEvo.OK);

            string idRichiesta = invioResult.IdRichiesta;

            Assert.IsTrue(CheckStatusPostaEvo(idRichiesta, "L", TimeSpan.FromMinutes(2), TimeSpan.FromSeconds(10)));
        }
    }
}
