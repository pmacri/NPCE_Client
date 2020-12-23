using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.Common.Proxy;
using ComunicazioniElettroniche.Common.Serialization;
using ComunicazioniElettroniche.PostaEvo.Assembly.External.Serialization;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace NPCE_Client.Test
{
    public class Helper
    {
        private Configs _config;

        public Configs Config { get { return _config; } }

        public Helper(Configs config)
        {
            _config = config;
        }
        public static CEHeader GetCeHeader()
        {
            CEHeader result = new CEHeader();
            result.BillingCenter = "BillingCenter";
            result.ContractId = "ContractId";
            result.BillingCenter = "BillingCenter";
            result.Customer = "Customer";
            result.IdCRM = "IdCRM";
            result.IDSender = "IDSender";
            result.SenderSystem = "SenderSystem";
            result.UserId = "UserId";
            result.UserType = "UserType";
            result.CodiceFiscale = "CodiceFiscale";
            result.PartitaIva = "PartitaIva";
            result.CostCenter = "CostCenter";
            result.ContractType = "ContractType";
            return result;
        }
        public static PostaEvoSubmit GetPostaEvoSubmitFromXml(string xml)
        {
            var result = SerializationUtility.Deserialize<PostaEvoSubmit>(xml);

            return result;

        }
        public TResult PublishToBizTalk<T, V>(T request, out V response) where V : class
        {
            CE ce = new CE();
            ce.Header = Helper.GetCeHeader();
            ce.Body = ComunicazioniElettroniche.Common.Serialization.SerializationUtility.SerializeToXmlElement(request);

            BasicHttpBinding httpBinding = new BasicHttpBinding();
            httpBinding.MaxReceivedMessageSize = 64000000;
            httpBinding.SendTimeout = TimeSpan.FromMinutes(20);
            EndpointAddress endpointAddress = new EndpointAddress(_config.UrlEntryPoint);


            var client = new WsCEClient(httpBinding, endpointAddress);
            string guidMessage = string.Empty;
            if (ce.Body.GetElementsByTagName("IdRichiesta").Count > 0)
            {
                guidMessage = ce.Body.GetElementsByTagName("IdRichiesta")[0].InnerText;
            }
            if (string.IsNullOrEmpty(guidMessage))
            {
                guidMessage = ce.Body.GetAttribute("IdRichiesta");
            }
            if (string.IsNullOrEmpty(guidMessage))
            {
                throw new ArgumentException("IdRichiesta not found in request to submit");
            }
            ce.Header.GUIDMessage = guidMessage;

            try
            {
                client.SubmitRequest(ref ce);
                response = SerializationUtility.Deserialize<V>(ce.Body);
            }
            catch (Exception ex)
            {

                throw;
            }

            return ce.Result;
        }

        
        //public bool VerifyDb(string PostaEvoConnectionString, TimeSpan retryInterval, int retryNum, Guid IdRichiesta, TipoProdotto tipoProdotto)
        //{
        //    int currentAttempt = 1;
        //    bool result = false;

        //    while (currentAttempt <= retryNum && !result)
        //    {
        //        using (PostaEvoEntities ctx = new PostaEvoEntities(PostaEvoConnectionString))
        //        {
        //            var richiesta = ctx.fn_Richieste(1).Where(r => r.IdRichiesta == IdRichiesta).FirstOrDefault();
        //            if (richiesta == null)
        //            {
        //                Thread.Sleep(retryInterval);
        //                currentAttempt++;
        //                continue;
        //            }

        //            // Charging
        //            var statoCorrente = richiesta.StatoCorrente;
        //            var statoConsumo = richiesta.StatoConsumo;
        //            result = (statoCorrente != null && statoCorrente == "L") && (statoConsumo != null && statoConsumo == "DaInviare");
        //            if (!result) continue;

        //            //Tracking

        //            if (tipoProdotto == TipoProdotto.MOL1 || tipoProdotto == TipoProdotto.MOL4)
        //            {
        //                ctx.fn_Destinatario(1).Where(d => d.IdRichiesta == IdRichiesta).All(d => d.CodiceTracciatura != null);
        //                if (!result) continue;
        //            }
        //        }
        //    }
        //    return result;
        //}
    }

}
