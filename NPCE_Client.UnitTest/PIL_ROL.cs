using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.ROL.Web.BusinessEntities.InvioSubmitResponse;
using ComunicazioniElettroniche.ROL.Web.BusinessEntities.InvioSubmitROL;
using ComunicazioniElettroniche.ROL.Web.DataContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE_Client.Test;
using System;

namespace NPCE_Client.UnitTest
{
    [TestClass]
    public class PIL_ROL: TestBase
    {

        public PIL_ROL():  base(Test.Environment.Collaudo)
        {

        }


        [TestMethod]
        public void Invio()
        {
            var guid = Guid.NewGuid();
            string xml = Envelopes.RolPil.Replace("%GUID%", string.Concat("", guid.ToString(), ""));
            var raccomandataSubmitRequest = Helper.GetRaccomandataSubmitFromXml(xml);
            RaccomandataResponse invioresult;
            var ceHeader = Helper.GetCeHeader();
            ceHeader.SenderSystem = "H2H";
            ceHeader.IDSender = "999988";
            ceHeader.IdCRM = string.Empty;
            ceHeader.UserId = "nello.citta.npce";
            ceHeader.ContractId = string.Empty;

            raccomandataSubmitRequest.Documenti[0].Uri = ambiente.PathDocument;
            raccomandataSubmitRequest.Documenti[0].FileHash = ambiente.HashMD5Document;

            raccomandataSubmitRequest.Documenti[1].Uri = ambiente.PathCov;
            raccomandataSubmitRequest.Documenti[1].FileHash = ambiente.HashMD5Cov;

            var result = Helper.PublishToBizTalk<RaccomandataSubmit, RaccomandataResponse>(raccomandataSubmitRequest, ceHeader, ambiente.UrlEntryPoint, out invioresult);
            Assert.AreEqual(TResultResType.I, result.ResType);
        }
    }
}
