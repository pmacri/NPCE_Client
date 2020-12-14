using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.PostaEvo.Assembly.External.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace NPCE_Client.Test
{
    [TestClass]
    public class MOL_Pil
    {

        Environment env = Environment.Collaudo;

        private string xmlBase = @"
<PostaEvoSubmit TipoProdotto='MOL1' IdRichiesta='%GUID%' AutoConferma='false' CodiceContratto='00000000040000015982' xmlns='http://ComunicazioniElettroniche.PostaEvo.Schema'>
          <Destinatari xmlns=''>
            <Destinatario>
              <Nominativo>
                <Nominativo>Rossi Paolo</Nominativo>
              </Nominativo>
              <Indirizzo>
                <Indirizzo>Via dei ciclamini 180 </Indirizzo>
              </Indirizzo>
              <Destinazione>
                <CAP>00144</CAP>
                <Comune>ROMA</Comune>
                <Provincia>RM</Provincia>
              </Destinazione>
            </Destinatario>
          </Destinatari>
          <Mittente xmlns=''>
            <Nominativo>
              <Nominativo>Rossi Paolo</Nominativo>
            </Nominativo>
            <Indirizzo>
              <Indirizzo>Viadei ciclamini 180</Indirizzo>
            </Indirizzo>
            <Destinazione>
              <CAP>00144</CAP>
              <Comune>ROMA</Comune>
              <Provincia>RM</Provincia>
            </Destinazione>
          </Mittente>
          <Documenti xmlns=''>
            <Documento>
              <URI>\\FSSVIL-b451.rete.testposte\ShareFS\InputDocument\ROL_db56a17c-12b2-402a-ad51-9e309f895e79.doc</URI>
              <HashMD5>AB8EF323B64C85C8DFCCCD4356E4FB9B</HashMD5>
              <Estensione>doc</Estensione>
            </Documento>
           <Documento>
              <URI>\\FSSVIL-b451.rete.testposte\ShareFS\InputDocument\ROL_276f67ac-9157-4feb-b8f2-0b89a742aad5_01.cov</URI>
              <HashMD5>55EF669FDE954F6B74B218ECC5C91B5D</HashMD5>
              <Estensione>cov</Estensione>
            </Documento>
          </Documenti>
          <Opzioni xmlns=''>
            <OpzioniStampa TipoColore='BW' FronteRetro='false'/>
            <OpzioniServizio>
              <ModalitaConsegna>S</ModalitaConsegna>
              <AttestazioneConsegna>false</AttestazioneConsegna>
              <SecondoTentativoRecapito>false</SecondoTentativoRecapito>
              <TipoNomeProprio>false</TipoNomeProprio>
              <ModalitaPricing>NAZ</ModalitaPricing>
            </OpzioniServizio>
          </Opzioni>
        </PostaEvoSubmit>
";


        [TestMethod]
        public void MOL1_Base_AutoConfirmTrue_RitiroDigitale_CF_Errato()
        {
            var guid = System.Guid.NewGuid();
            xmlBase = xmlBase.Replace("%GUID%", string.Concat("", guid.ToString(), ""));

            var postaEvoRequest = Helper.GetPostaEvoSubmitFromXml(xmlBase);
            Helper helper = new Helper(new Configs(env));

            PostaEvoResponse postaEvoResponse;

            postaEvoRequest.AutoConferma = true;
            postaEvoRequest.Opzioni.OpzioniServizio.ModalitaPricing = "ZONA";
            postaEvoRequest.Destinatari[0].RitiroDigitale = true;
            postaEvoRequest.Destinatari[0].RitiroDigitaleSpecified = true;
            postaEvoRequest.Destinatari[0].Nominativo.CodiceFiscale = "xxxxxxxxxxxxxxxx";

            postaEvoRequest.Documenti[0].URI = helper.Config.PathDocument;
            postaEvoRequest.Documenti[0].HashMD5 = helper.Config.HashMD5Document;

            postaEvoRequest.Documenti[1].URI = helper.Config.PathCov;
            postaEvoRequest.Documenti[1].HashMD5 = helper.Config.HashMD5Cov;

            var result = helper.PublishToBizTalk<PostaEvoSubmit, PostaEvoResponse>(postaEvoRequest, out postaEvoResponse);
            Assert.AreEqual(TResultResType.I, result.ResType);
        }

        [TestMethod]
        public void MOL1_Base_No_Autoconfirm()
        {
            var guid = System.Guid.NewGuid();

            xmlBase = xmlBase.Replace("%GUID%", string.Concat("", guid.ToString(), ""));

            var postaEvoRequest = Helper.GetPostaEvoSubmitFromXml(xmlBase);

            postaEvoRequest.Opzioni.OpzioniServizio.ModalitaPricing = "ZONA";
            postaEvoRequest.Opzioni.OpzioniServizio.AttestazioneConsegna = true;
            postaEvoRequest.Opzioni.OpzioniServizio.SecondoTentativoRecapito = true;

            Helper helper = new Helper(new Configs(env));

            PostaEvoResponse postaEvoResponse;

            postaEvoRequest.Documenti[0].URI = helper.Config.PathDocument;
            postaEvoRequest.Documenti[0].HashMD5 = helper.Config.HashMD5Document;

            postaEvoRequest.Documenti[1].URI = helper.Config.PathCov;
            postaEvoRequest.Documenti[1].HashMD5 = helper.Config.HashMD5Cov;

            var result = helper.PublishToBizTalk<PostaEvoSubmit, PostaEvoResponse>(postaEvoRequest, out postaEvoResponse);
            Assert.AreEqual(TResultResType.I, result.ResType);
        }
    }
}
