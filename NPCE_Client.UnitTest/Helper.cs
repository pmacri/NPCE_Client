using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.Common.Proxy;
using ComunicazioniElettroniche.Common.Serialization;
using ComunicazioniElettroniche.PostaEvo.Assembly.External.Serialization;
using ComunicazioniElettroniche.ROL.Web.BusinessEntities.InvioSubmitROL;
using NPCE.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace NPCE_Client.Test
{
    public class Helper
    {
        private static Configs _config;

        public static Configs Config { get { return _config; } }

        public Helper(Configs config)
        {
            _config = config;
        }

        public static CEHeader GetCeHeader(string senderSystem)
        {
            var temp = GetCeHeader();
            temp.SenderSystem = senderSystem;
            return temp;
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

        public static RaccomandataSubmit GetRaccomandataSubmitFromXml(string xml)
        {
            var result = SerializationUtility.Deserialize<RaccomandataSubmit>(xml);

            return result;

        }

        public static TResult PublishToBizTalk<T, V>(T request, CEHeader ceHeader, string entryPointuri, out V response) where V : class
        {
            CE ce = new CE();
            ce.Header = ceHeader;
            ce.Body = SerializationUtility.SerializeToXmlElement(request);

            BasicHttpBinding httpBinding = new BasicHttpBinding();
            httpBinding.MaxReceivedMessageSize = 64000000;
            httpBinding.SendTimeout = TimeSpan.FromMinutes(20);
            EndpointAddress endpointAddress = new EndpointAddress(entryPointuri);


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

        public static TResult PublishToBizTalk<T, V>(T request, string entryPointuri, out V response) where V : class
        {
            CE ce = new CE();
            ce.Header = GetCeHeader();
            ce.Body = SerializationUtility.SerializeToXmlElement(request);

            BasicHttpBinding httpBinding = new BasicHttpBinding();
            httpBinding.MaxReceivedMessageSize = 64000000;
            httpBinding.SendTimeout = TimeSpan.FromMinutes(20);
            EndpointAddress endpointAddress = new EndpointAddress(entryPointuri);


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


        static bool CheckStatusPostaEvo(string idRichiesta, string status, TimeSpan timeout, TimeSpan retryInterval)
        {
            bool result = false;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed <= timeout)
            {
                using (var ctx = new PostaEvoEntities())
                {
                    ctx.Database.Connection.ConnectionString = Config.PostaEvoConnectionString;
                    var richiesta = ctx.Richieste.Where(r => r.IdRichiesta == new Guid(idRichiesta)).FirstOrDefault();

                    if (richiesta != null)
                    {
                        if (richiesta.StatoCorrente == status)
                        {
                            result = true;
                            break;
                        }
                        else Thread.Sleep(retryInterval);
                    }
                }
            }
            return result;
        }

        public static void GetParametriPrezzatura(string servizio, out int numeroFogli, out int mesiArchiviazione, string pathLoggingFile)
        {
            string fileName = string.Empty;

            if (servizio.ToLower()=="lol")
            {
                fileName = pathLoggingFile + @"\NPCELOL.log";
            }
            else
            {
                fileName = pathLoggingFile + @"\NPCEROL.log";
            };


            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(fs, Encoding.UTF8);
            try
            {
                string line = String.Empty;
                string lastLineFound = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {

                    if (Regex.IsMatch(line, $@"\bARCHIV\b"))
                    {
                        lastLineFound = line;
                    }

                    //<Parameter ParamName="ARCHIV" ParamValue="00020024" />
                }
                var paramValue = lastLineFound.Substring(lastLineFound.LastIndexOf("ParamValue") + 12, 8);
                numeroFogli = int.Parse(paramValue.Substring(0, 4));
                mesiArchiviazione = int.Parse(paramValue.Substring(4, 4));
            }
            catch (Exception)
            {
                sr.Dispose();
                fs.Dispose();
                throw;
            }
            
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
