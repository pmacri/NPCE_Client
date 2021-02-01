using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.Common.Serialization;
using ComunicazioniElettroniche.PostaEvo.Assembly.External.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE.DataModel;
using NPCE_Client.Test;
using System;
using System.Diagnostics;
using System.Linq;

namespace NPCE_Client.UnitTest
{
    [TestClass]
    public class PIL_MOL : TestBase
    {
        public PIL_MOL(): base(Test.Environment.Collaudo)
        {

        }

        [TestMethod]
        public void MOL1_AutoConfirmTrue_RitiroDigitale_CF_Errato()
        {
            var guid = System.Guid.NewGuid();
            string xmlBase = Envelopes.PostaEvoPil.Replace("%GUID%", string.Concat("", guid.ToString(), ""));

            var postaEvoRequest = Helper.GetPostaEvoSubmitFromXml(xmlBase);

            PostaEvoResponse postaEvoResponse;

            postaEvoRequest.AutoConferma = true;
            postaEvoRequest.Opzioni.OpzioniServizio.ModalitaPricing = "ZONA";
            postaEvoRequest.Destinatari[0].RitiroDigitale = true;
            postaEvoRequest.Destinatari[0].RitiroDigitaleSpecified = true;
            postaEvoRequest.Destinatari[0].Nominativo.CodiceFiscale = "xxxxxxxxxxxxxxxx";

            postaEvoRequest.Documenti[0].URI = ambiente.PathDocument;
            postaEvoRequest.Documenti[0].HashMD5 = ambiente.HashMD5Document;

            postaEvoRequest.Documenti[1].URI = ambiente.PathCov;
            postaEvoRequest.Documenti[1].HashMD5 = ambiente.HashMD5Cov;

            var result = Helper.PublishToBizTalk<PostaEvoSubmit, PostaEvoResponse>(postaEvoRequest, ambiente.UrlEntryPoint, out postaEvoResponse);
            Assert.AreEqual(TResultResType.I, result.ResType);
        }

        [TestMethod]
        public void MOL1_Cover()
        {
            var guid = System.Guid.NewGuid();

            string xmlBase = Envelopes.PostaEvoPil.Replace("%GUID%", string.Concat("", guid.ToString(), ""));

            var postaEvoRequest = SerializationUtility.Deserialize<PostaEvoSubmit>(xmlBase); ;

            postaEvoRequest.AutoConferma = false;
            postaEvoRequest.Opzioni.OpzioniServizio.ModalitaPricing = "ZONA";
            postaEvoRequest.Opzioni.OpzioniServizio.AttestazioneConsegna = true;
            postaEvoRequest.Opzioni.OpzioniServizio.SecondoTentativoRecapito = true;

            PostaEvoResponse postaEvoResponse;

            postaEvoRequest.Documenti[0].URI = ambiente.PathDocument;
            postaEvoRequest.Documenti[0].HashMD5 = ambiente.HashMD5Document;

            postaEvoRequest.Documenti[1].URI = ambiente.PathCov;

            
            postaEvoRequest.Documenti[1].HashMD5 = ambiente.HashMD5Cov;

            var result = Helper.PublishToBizTalk<PostaEvoSubmit, PostaEvoResponse>(postaEvoRequest, ambiente.UrlEntryPoint, out postaEvoResponse);
            Assert.AreEqual(TResultResType.I, result.ResType);

            Debug.WriteLine(postaEvoResponse.IdRichiesta);

            string idRichiesta = postaEvoResponse.IdRichiesta;

            Assert.IsTrue(CheckStatusPostaEvo(idRichiesta, "K", TimeSpan.FromMinutes(2), TimeSpan.FromSeconds(10)));
        }

        [TestMethod]
        public void MOL1_Cover_Archiviazione_Storica_3_Anni()
        {
            var guid = System.Guid.NewGuid();

            string xmlBase = Envelopes.PostaEvoPil.Replace("%GUID%", string.Concat("", guid.ToString(), ""));

            var postaEvoRequest = SerializationUtility.Deserialize<PostaEvoSubmit>(xmlBase); ;

            postaEvoRequest.AutoConferma = false;
            postaEvoRequest.Opzioni.OpzioniServizio.ModalitaPricing = "ZONA";
            postaEvoRequest.Opzioni.OpzioniServizio.AttestazioneConsegna = true;
            postaEvoRequest.Opzioni.OpzioniServizio.SecondoTentativoRecapito = true;

            PostaEvoResponse postaEvoResponse;

            postaEvoRequest.Documenti[0].URI = ambiente.PathDocument;
            postaEvoRequest.Documenti[0].HashMD5 = ambiente.HashMD5Document;
            postaEvoRequest.Documenti[1].URI = ambiente.PathCov;
            postaEvoRequest.Documenti[1].HashMD5 = ambiente.HashMD5Cov;

            postaEvoRequest.Opzioni.OpzioniServizio.ArchiviazioneDocumenti = "STORICA";
            postaEvoRequest.Opzioni.OpzioniServizio.AnniArchiviazione = 3;
            postaEvoRequest.Opzioni.OpzioniServizio.AnniArchiviazioneSpecified = true;

            var result = Helper.PublishToBizTalk<PostaEvoSubmit, PostaEvoResponse>(postaEvoRequest, ambiente.UrlEntryPoint, out postaEvoResponse);
            Assert.AreEqual(TResultResType.I, result.ResType);

            Debug.WriteLine(postaEvoResponse.IdRichiesta);

            string idRichiesta = postaEvoResponse.IdRichiesta;

            Assert.IsTrue(CheckStatusPostaEvo(idRichiesta, "K", TimeSpan.FromMinutes(2), TimeSpan.FromSeconds(10)));
        }

        [TestMethod]
        public void MOL1_No_Cover()
        {
            var guid = System.Guid.NewGuid();

            string xmlBase = Envelopes.PostaEvoPil.Replace("%GUID%", string.Concat("", guid.ToString(), ""));

            var postaEvoRequest = SerializationUtility.Deserialize<PostaEvoSubmit>(xmlBase); ;

            postaEvoRequest.AutoConferma = false;
            postaEvoRequest.Opzioni.OpzioniServizio.ModalitaPricing = "ZONA";
            postaEvoRequest.Opzioni.OpzioniServizio.AttestazioneConsegna = true;
            postaEvoRequest.Opzioni.OpzioniServizio.SecondoTentativoRecapito = true;

            PostaEvoResponse postaEvoResponse;

            postaEvoRequest.Documenti[0].URI = ambiente.PathDocument;
            postaEvoRequest.Documenti[0].HashMD5 = ambiente.HashMD5Document;

            postaEvoRequest.Documenti[1] = null;

            var result = Helper.PublishToBizTalk<PostaEvoSubmit, PostaEvoResponse>(postaEvoRequest, ambiente.UrlEntryPoint, out postaEvoResponse);
            Assert.AreEqual(TResultResType.I, result.ResType);

            Debug.WriteLine(postaEvoResponse.IdRichiesta);
        }

        [TestMethod]
        public void MOL1_Autoconfirm()
        {
            var guid = System.Guid.NewGuid();

            string xmlBase = Envelopes.PostaEvoPil.Replace("%GUID%", string.Concat("", guid.ToString(), ""));

            var postaEvoRequest = Helper.GetPostaEvoSubmitFromXml(xmlBase);

            postaEvoRequest.AutoConferma = true;

            postaEvoRequest.Opzioni.OpzioniServizio.ModalitaPricing = "ZONA";
            postaEvoRequest.Opzioni.OpzioniServizio.AttestazioneConsegna = true;

            PostaEvoResponse postaEvoResponse;

            postaEvoRequest.Documenti[0].URI = ambiente.PathDocument;
            postaEvoRequest.Documenti[0].HashMD5 = ambiente.HashMD5Document;

            postaEvoRequest.Documenti[1].URI = ambiente.PathCov;
            postaEvoRequest.Documenti[1].HashMD5 = ambiente.HashMD5Cov;

            var result = Helper.PublishToBizTalk<PostaEvoSubmit, PostaEvoResponse>(postaEvoRequest, ambiente.UrlEntryPoint, out postaEvoResponse);
            Assert.AreEqual(TResultResType.I, result.ResType);

            Debug.WriteLine(postaEvoResponse.IdRichiesta);

            string idRichiesta = postaEvoResponse.IdRichiesta;

            Assert.IsTrue(CheckStatusPostaEvo(idRichiesta, "L", TimeSpan.FromMinutes(2), TimeSpan.FromSeconds(10)));


        }

        [TestMethod]
        public void MOL1_Autoconfirm_Archiviazione_Storica_3_Anni()
        {
            var guid = System.Guid.NewGuid();

            string xmlBase = Envelopes.PostaEvoPil.Replace("%GUID%", string.Concat("", guid.ToString(), ""));

            var postaEvoRequest = Helper.GetPostaEvoSubmitFromXml(xmlBase);

            postaEvoRequest.AutoConferma = true;

            postaEvoRequest.Opzioni.OpzioniServizio.ModalitaPricing = "ZONA";
            postaEvoRequest.Opzioni.OpzioniServizio.AttestazioneConsegna = true;

            postaEvoRequest.Opzioni.OpzioniServizio.ArchiviazioneDocumenti = "STORICA";
            postaEvoRequest.Opzioni.OpzioniServizio.AnniArchiviazione =3;
            postaEvoRequest.Opzioni.OpzioniServizio.AnniArchiviazioneSpecified = true;


            PostaEvoResponse postaEvoResponse;

            postaEvoRequest.Documenti[0].URI = ambiente.PathDocument;
            postaEvoRequest.Documenti[0].HashMD5 = ambiente.HashMD5Document;

            postaEvoRequest.Documenti[1].URI = ambiente.PathCov;
            postaEvoRequest.Documenti[1].HashMD5 = ambiente.HashMD5Cov;

            var result = Helper.PublishToBizTalk<PostaEvoSubmit, PostaEvoResponse>(postaEvoRequest, ambiente.UrlEntryPoint, out postaEvoResponse);
            Assert.AreEqual(TResultResType.I, result.ResType);

            using (var ctx = new PostaEvoEntities())
            {
                ctx.Database.Connection.ConnectionString = ambiente.PostaEvoConnectionString;

                var opzione = ctx.Opzioni.Where(o => o.IdRichiesta.ToString() == guid.ToString()).FirstOrDefault();

                if (opzione != null)
                {
                    Assert.AreEqual(opzione.TipoArchiviazione, 3);
                    Assert.AreEqual(opzione.AnniArchiviazione, 3);
                }
            }
                
            }
    }

}
