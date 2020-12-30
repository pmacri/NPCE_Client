﻿using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.PostaEvo.Assembly.External.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE.DataModel;
using NPCE_Client.Test;
using System.Linq;

namespace NPCE_Client.UnitTest
{
    [TestClass]
    public class MOL_Pil : TestBase
    {



        public MOL_Pil(): base(Environment.Collaudo)
        {

        }

        [TestMethod]
        public void MOL1_Base_AutoConfirmTrue_RitiroDigitale_CF_Errato()
        {
            var guid = System.Guid.NewGuid();
            string xmlBase = Envelopes.PostaEvoBase.Replace("%GUID%", string.Concat("", guid.ToString(), ""));

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
        public void MOL1_Base_No_Autoconfirm()
        {
            var guid = System.Guid.NewGuid();

            string xmlBase = Envelopes.PostaEvoBase.Replace("%GUID%", string.Concat("", guid.ToString(), ""));

            var postaEvoRequest = Helper.GetPostaEvoSubmitFromXml(xmlBase);

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
        }

        [TestMethod]
        public void MOL1_Base_Autoconfirm()
        {
            var guid = System.Guid.NewGuid();

            string xmlBase = Envelopes.PostaEvoBase.Replace("%GUID%", string.Concat("", guid.ToString(), ""));

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
        }

        [TestMethod]
        public void MOL1_Base_Autoconfirm_Archiviazione_Storica_3_Anni()
        {
            var guid = System.Guid.NewGuid();

            string xmlBase = Envelopes.PostaEvoBase.Replace("%GUID%", string.Concat("", guid.ToString(), ""));

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