using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.Common.SchemaDefinition;
using ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL;
using ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitResponse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE_Client.Test;
using System;
using System.Diagnostics;
using System.Threading;

namespace NPCE_Client.UnitTest
{
    [TestClass]
    public class PIL_LOL : TestBase
    {

        public PIL_LOL() : base(Test.Environment.Collaudo)
        {

        }

        [TestMethod]
        public void Invio_Cover()
        {
            var guid = Guid.NewGuid();
            string xml = Envelopes.LolPil.Replace("%GUID%", string.Concat("", guid.ToString(), ""));
            var letteraSubmitRequest = Helper.GetLetteraSubmitFromXml(xml);
            LetteraResponse invioresult;
            var ceHeader = Helper.GetCeHeader();
            ceHeader.SenderSystem = "H2H";
            ceHeader.IDSender = "999988";
            ceHeader.IdCRM = string.Empty;
            ceHeader.UserId = "nello.citta.npce";
            ceHeader.ContractId = string.Empty;

            letteraSubmitRequest.Documenti[0].Uri = ambiente.PathCov;
            letteraSubmitRequest.Documenti[0].FileHash = ambiente.HashMD5Cov;

            letteraSubmitRequest.Documenti[1].Uri = ambiente.PathDocument;
            letteraSubmitRequest.Documenti[1].FileHash = ambiente.HashMD5Document;


            var result = Helper.PublishToBizTalk<LetteraSubmit, LetteraResponse>(letteraSubmitRequest, ceHeader, ambiente.UrlEntryPoint, out invioresult);
            Assert.AreEqual(TResultResType.I, result.ResType);

            Debug.WriteLine(invioresult.IdRichiesta.ToString());

        }


        [TestMethod]
        public void Invio_No_Cover()
        {
            var guid = Guid.NewGuid();
            string xml = Envelopes.LolPil.Replace("%GUID%", string.Concat("", guid.ToString(), ""));
            var LetteraSubmitRequest = Helper.GetLetteraSubmitFromXml(xml);
            LetteraResponse invioresult;
            var ceHeader = Helper.GetCeHeader();
            ceHeader.SenderSystem = "H2H";
            ceHeader.IDSender = "999988";
            ceHeader.IdCRM = string.Empty;
            ceHeader.UserId = "nello.citta.npce";
            ceHeader.ContractId = string.Empty;

            LetteraSubmitRequest.Documenti[0].Uri = ambiente.PathDocument;
            LetteraSubmitRequest.Documenti[0].FileHash = ambiente.HashMD5Document;

            LetteraSubmitRequest.Documenti[1] = null;

            var result = Helper.PublishToBizTalk<LetteraSubmit, LetteraResponse>(LetteraSubmitRequest, ceHeader, ambiente.UrlEntryPoint, out invioresult);
            Assert.AreEqual(TResultResType.I, result.ResType);
            Debug.WriteLine(invioresult.IdRichiesta.ToString());
        }

        [TestMethod]
        public void Invio_Archiviazione_Check_Parametri_Prezzatura_Archiviazione()
        {
            var guid = Guid.NewGuid();
            string xml = Envelopes.LolPil.Replace("%GUID%", string.Concat("", guid.ToString(), ""));
            var LetteraSubmitRequest = Helper.GetLetteraSubmitFromXml(xml);
            LetteraResponse invioresult;
            var ceHeader = Helper.GetCeHeader();
            ceHeader.SenderSystem = "H2H";
            ceHeader.IDSender = "999988";
            ceHeader.IdCRM = string.Empty;
            ceHeader.UserId = "nello.citta.npce";
            ceHeader.ContractId = string.Empty;

            LetteraSubmitRequest.Documenti[0].Uri = ambiente.PathCov;
            LetteraSubmitRequest.Documenti[0].FileHash = ambiente.HashMD5Cov;

            LetteraSubmitRequest.Documenti[1].Uri = ambiente.PathDocument;
            LetteraSubmitRequest.Documenti[1].FileHash = ambiente.HashMD5Document;



            LetteraSubmitRequest.Opzioni.ArchiviazioneDocumenti = "STORICA";
            LetteraSubmitRequest.Opzioni.AnniArchiviazione = 3;
            LetteraSubmitRequest.Opzioni.AnniArchiviazioneSpecified = true;

            var result = Helper.PublishToBizTalk<LetteraSubmit, LetteraResponse>(LetteraSubmitRequest, ceHeader, ambiente.UrlEntryPoint, out invioresult);
            Assert.AreEqual(TResultResType.I, result.ResType);

            int numeroFogli = 0;
            int mesiArchiviazione = 0;
            Thread.Sleep(20000);


            Helper.GetParametriPrezzatura("Lol", out numeroFogli, out mesiArchiviazione, ambiente.PathLoggingFile);

            Assert.AreEqual(2, numeroFogli);

            Assert.AreEqual(mesiArchiviazione, 36);
        }

        [TestMethod]
        public void Invio_Archiviazione_PosteIt_Check_Parametri_Prezzatura_Archiviazione()
        {
            var guid = Guid.NewGuid();
            string xml = Envelopes.LolPil.Replace("%GUID%", string.Concat("", guid.ToString(), ""));
            var LetteraSubmitRequest = Helper.GetLetteraSubmitFromXml(xml);
            LetteraResponse invioresult;
            var ceHeader = Helper.GetCeHeader();
            ceHeader.SenderSystem = "H2H";
            ceHeader.IDSender = "999988";
            ceHeader.IdCRM = string.Empty;
            ceHeader.UserId = "nello.citta.npce";
            ceHeader.ContractId = string.Empty;

            LetteraSubmitRequest.Documenti[0].Uri = ambiente.PathCov;
            LetteraSubmitRequest.Documenti[0].FileHash = ambiente.HashMD5Cov;

            LetteraSubmitRequest.Documenti[1].Uri = ambiente.PathDocument;
            LetteraSubmitRequest.Documenti[1].FileHash = ambiente.HashMD5Document;



            LetteraSubmitRequest.Opzioni.ArchiviazioneDocumenti = "STORICA";
            LetteraSubmitRequest.Opzioni.AnniArchiviazione = 3;
            LetteraSubmitRequest.Opzioni.AnniArchiviazioneSpecified = true;

            LetteraSubmitRequest.DocStampabile = true;
            LetteraSubmitRequest.DocPrezzabile = true;

            LetteraSubmitRequest.Opzioni.NumeroPagine = 8;




            var result = Helper.PublishToBizTalk<LetteraSubmit, LetteraResponse>(LetteraSubmitRequest, ceHeader, ambiente.UrlEntryPoint, out invioresult);
            Assert.AreEqual(TResultResType.I, result.ResType);

            int numeroFogli = 0;
            int mesiArchiviazione = 0;
            Thread.Sleep(20000);


            Helper.GetParametriPrezzatura("Lol", out numeroFogli, out mesiArchiviazione, ambiente.PathLoggingFile);

            Assert.AreEqual(9, numeroFogli);

            Assert.AreEqual(mesiArchiviazione, 36);
        }

        [TestMethod]
        public void Confirm_AbortOrPostalizza()
        {
            string guidMessage = "5447fb1c-77d5-4c92-8eb0-97e0bbb8db66";

            string xmlConfirmMessage = @"<ns0:ConfirmService GUIDMessage='%GUID%' IdOrdine='171C371E-B00A-4737-9B38-0524DCD7777E' PaymentTypeId='6' xmlns:ns0='http://posteitaliane.it/ordermanagement/schemas' />";

            xmlConfirmMessage = xmlConfirmMessage.Replace("%GUID%", guidMessage);
            var ceHeader = Helper.GetCeHeader();
            ceHeader.SenderSystem = "H2H";
            ceHeader.IDSender = "999988";
            ceHeader.IdCRM = string.Empty;
            ceHeader.UserId = "nello.citta.npce";
            ceHeader.ContractId = string.Empty;
            ConfirmOrderResponse confirmResponse = null;
            ConfirmOrder confirmRequest = null;
            confirmRequest = ComunicazioniElettroniche.Common.Serialization.SerializationUtility.Deserialize<ConfirmOrder>(xmlConfirmMessage);

            var result = Helper.PublishToBizTalk<ConfirmOrder, ConfirmOrderResponse>(confirmRequest, ceHeader, ambiente.UrlEntryPoint, out confirmResponse);
            Assert.AreEqual(TResultResType.I, result.ResType);
        }
    }
}
