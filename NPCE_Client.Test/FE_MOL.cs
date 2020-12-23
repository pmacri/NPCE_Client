using ComunicazioniElettroniche.Common.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE.LIbrary.ServiceReference.MOL;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NPCE_Client.Test
{
    [TestClass]
    public class FE_MOL : FEBase
    {

        public FE_MOL(): base(Environment.Collaudo)
        {
            
        }

        private static Ambiente _ambiente = null;
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
                        ContrattoMOL = "40000015982"
                    };
                    break;
                case Environment.Certificazione:
                    break;
                case Environment.Staging:
                    break;
                case Environment.Bts2016:
                    break;
                case Environment.Produzione:
                    result = new Ambiente
                    {
                        MolUri = "http://10.60.19.13/RaccomandataMarket/MOLService.svc",
                        customerid = "3908983576",
                        smuser = "HH800117",
                        ContrattoMOL = "40000015982"
                    };
                    break;
                default:
                    break;
            }

            return result;
        }

        [TestMethod]
        public void Invio_MOL1_Autoconferma_False()
        {

            InvioRequest molSubmit = GetMolInvio();
           
            IRaccomandataMarketService _proxy = GetProxy<IRaccomandataMarketService>(ambiente.MolUri, ambiente.Username, ambiente.Password);

            var fake = new OperationContextScope((IContextChannel)_proxy);

            HttpRequestMessageProperty headers = GetHttpHeaders(ambiente);

            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            var invioResult = _proxy.Invio(molSubmit);

            Assert.IsTrue(invioResult.Esito == EsitoPostaEvo.OK);

            string idRichiesta = invioResult.IdRichiesta;

            Assert.IsTrue(CheckStatusPostaEvo(idRichiesta, "K", TimeSpan.FromMinutes(2), TimeSpan.FromSeconds(20)));
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
