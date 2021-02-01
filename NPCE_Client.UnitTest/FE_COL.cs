using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE_Client.UnitTest.ServiceReference.Col;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace NPCE_Client.UnitTest
{
    [TestClass]
    public class FE_COL: TestBase
    {

        public FE_COL(): base(Test.Environment.Collaudo)
        {

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

            Assert.IsTrue(CheckStatusPostaEvo(idRichiesta, "K", TimeSpan.FromMinutes(2), TimeSpan.FromSeconds(10)));
        }
    }
}
