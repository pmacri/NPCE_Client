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
    public class PIL_ROL
    {
        Test.Environment env = Test.Environment.Collaudo;

        Helper helper ;

        [TestInitialize]
        public void testInit()
        {
            helper = new Helper(new Configs(env));
        }



        [TestMethod]
        public void Invio()
        {
            var guid = Guid.NewGuid();
            string xml = Envelopes.RolPil.Replace("%GUID%", string.Concat("", guid.ToString(), ""));
            var raccomandataSubmitRequest = Helper.GetRaccomandataSubmitFromXml(xml);
            Helper helper = new Helper(new Configs(env));
            RaccomandataResponse invioresult;
            var ceHeader = Helper.GetCeHeader();
            ceHeader.SenderSystem = "H2H";
            ceHeader.IDSender = "999988";
            ceHeader.IdCRM = string.Empty;
            ceHeader.UserId = "nello.citta.npce";
            ceHeader.ContractId = string.Empty;

            raccomandataSubmitRequest.Documenti[0].Uri = Helper.Config.PathDocument;
            raccomandataSubmitRequest.Documenti[0].FileHash = Helper.Config.HashMD5Document;

            raccomandataSubmitRequest.Documenti[1].Uri = Helper.Config.PathCov;
            raccomandataSubmitRequest.Documenti[1].FileHash = Helper.Config.HashMD5Cov;

            var result = helper.PublishToBizTalk<RaccomandataSubmit, RaccomandataResponse>(raccomandataSubmitRequest, ceHeader, out invioresult);
            Assert.AreEqual(TResultResType.I, result.ResType);
        }
    }
}
