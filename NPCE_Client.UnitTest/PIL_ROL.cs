using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.ROL.Web.BusinessEntities.InvioSubmitResponse;
using ComunicazioniElettroniche.ROL.Web.BusinessEntities.InvioSubmitROL;
using ComunicazioniElettroniche.ROL.Web.DataContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE_Client.Test;
using System;
using System.Threading;

namespace NPCE_Client.UnitTest
{
    [TestClass]
    public class PIL_ROL : TestBase
    {

        public PIL_ROL() : base(Test.Environment.Collaudo)
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

        [TestMethod]
        public void Invio_Archiviazione_Check_Parametri_Prezzatura_Archiviazione()
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

            raccomandataSubmitRequest.Documenti[0].Uri = ambiente.PathCov;
            raccomandataSubmitRequest.Documenti[0].FileHash = ambiente.HashMD5Cov;

            raccomandataSubmitRequest.Documenti[1].Uri = ambiente.PathDocument;
            raccomandataSubmitRequest.Documenti[1].FileHash = ambiente.HashMD5Document;



            raccomandataSubmitRequest.Opzioni.ArchiviazioneDocumenti = "STORICA";
            raccomandataSubmitRequest.Opzioni.AnniArchiviazione = 3;
            raccomandataSubmitRequest.Opzioni.AnniArchiviazioneSpecified = true;

            var result = Helper.PublishToBizTalk<RaccomandataSubmit, RaccomandataResponse>(raccomandataSubmitRequest, ceHeader, ambiente.UrlEntryPoint, out invioresult);
            Assert.AreEqual(TResultResType.I, result.ResType);

            int numeroFogli = 0;
            int mesiArchiviazione = 0;
            Thread.Sleep(20000);


            Helper.GetParametriPrezzatura("ROL", out numeroFogli, out mesiArchiviazione, ambiente.PathLoggingFile);

            Assert.AreEqual(2, numeroFogli);

            Assert.AreEqual(mesiArchiviazione, 36);
        }

        [TestMethod]
        public void Invio_Archiviazione_PosteIt_Check_Parametri_Prezzatura_Archiviazione()
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

            raccomandataSubmitRequest.Documenti[0].Uri = ambiente.PathCov;
            raccomandataSubmitRequest.Documenti[0].FileHash = ambiente.HashMD5Cov;

            raccomandataSubmitRequest.Documenti[1].Uri = ambiente.PathDocument;
            raccomandataSubmitRequest.Documenti[1].FileHash = ambiente.HashMD5Document;



            raccomandataSubmitRequest.Opzioni.ArchiviazioneDocumenti = "STORICA";
            raccomandataSubmitRequest.Opzioni.AnniArchiviazione = 3;
            raccomandataSubmitRequest.Opzioni.AnniArchiviazioneSpecified = true;

            raccomandataSubmitRequest.DocStampabile = true;
            raccomandataSubmitRequest.DocPrezzabile = true;

            raccomandataSubmitRequest.Opzioni.NumeroPagine = 8;




            var result = Helper.PublishToBizTalk<RaccomandataSubmit, RaccomandataResponse>(raccomandataSubmitRequest, ceHeader, ambiente.UrlEntryPoint, out invioresult);
            Assert.AreEqual(TResultResType.I, result.ResType);

            int numeroFogli = 0;
            int mesiArchiviazione = 0;
            Thread.Sleep(20000);


            Helper.GetParametriPrezzatura("ROL", out numeroFogli, out mesiArchiviazione, ambiente.PathLoggingFile);

            Assert.AreEqual(9, numeroFogli);

            Assert.AreEqual(mesiArchiviazione, 36);
        }
    }
}
