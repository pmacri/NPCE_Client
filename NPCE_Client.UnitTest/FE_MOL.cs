using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE_Client.Model;
using NPCE_Client.Test;
using NPCE_Client.UnitTest.ServiceReference.Mol;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml.Serialization;

namespace NPCE_Client.UnitTest
{
    [TestClass]
    public class FE_MOL : TestBase
    {

        public FE_MOL(): base(Test.Environment.Collaudo)
        {
            
        }

       

        [TestMethod]
        public void Invio_MOL1_Autoconferma_True()
        {

            InvioRequest molSubmit = GetMolInvio();

            molSubmit.MarketOnline.AutoConferma = true;

            IRaccomandataMarketService _proxy = GetProxy<IRaccomandataMarketService>(ambiente.MolUri);

            var fake = new OperationContextScope((IContextChannel)_proxy);

            HttpRequestMessageProperty headers = GetHttpHeaders(ambiente);

            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            molSubmit.MarketOnline.Bollettini = null;

            var invioResult = _proxy.Invio(molSubmit);

            Assert.IsTrue(invioResult.Esito == EsitoPostaEvo.OK);

            string idRichiesta = invioResult.IdRichiesta;

        }

        [TestMethod]
        public void Invio_MOL1_Autoconferma_False()
        {

            InvioRequest molSubmit = GetMolInvio();

            molSubmit.MarketOnline.AutoConferma = false;

            IRaccomandataMarketService _proxy = GetProxy<IRaccomandataMarketService>(ambiente.MolUri);

            var fake = new OperationContextScope((IContextChannel)_proxy);

            HttpRequestMessageProperty headers = GetHttpHeaders(ambiente);

            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            molSubmit.MarketOnline.Bollettini = null;

            var invioResult = _proxy.Invio(molSubmit);

            Assert.IsTrue(invioResult.Esito == EsitoPostaEvo.OK);

            string idRichiesta = invioResult.IdRichiesta;

            Assert.IsTrue(CheckStatusPostaEvo(idRichiesta, "K", TimeSpan.FromMinutes(2), TimeSpan.FromSeconds(10)));
        }

        [TestMethod]
        public void SerializeMolInvio()
        {
            InvioRequest invio = GetMolInvio();
            var types = new Type[] { typeof(Bollettino896) };
            XmlSerializer serializer = new XmlSerializer(typeof(InvioRequest), types);

            serializer.Serialize(new FileStream("\\temp\\invio.xml", FileMode.Create, FileAccess.ReadWrite), invio);
        }

    }
}
