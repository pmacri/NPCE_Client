using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE_Client.Test;
using NPCE_Client.UnitTest.ServiceReference.LOL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using Environment = NPCE_Client.Test.Environment;

namespace NPCE_Client.UnitTest
{
    [TestClass]
    public class FE_LOL : TestBase
    {
        public FE_LOL() : base(Environment.Collaudo)
        {

        }

        [TestMethod]
        public void Lol_RecuperaIdRichiesta_Massive()
        {
            Stopwatch sw = new Stopwatch();

            Dictionary<string, int> errors = new Dictionary<string, int>();

            int errorCount = 0;
            int errorCountTimeout = 0;

            string lastError = string.Empty;

            string lastErrorTimeout = string.Empty;

            sw.Start();
            for (int i = 0; i < 10; i++)
            {

                try
                {
                    string idRichiesta = RecuperaIdRichiesta();
                    Assert.IsNotNull(idRichiesta);
                }

                catch (TimeoutException tex)
                {
                    lastErrorTimeout = tex.StackTrace;
                    errorCountTimeout += 1;
                }

                catch (CommunicationException cex)
                {
                    lastError = cex.InnerException.Message;
                    if (errors.ContainsKey(lastError))
                    {
                        errors[lastError] = errors[lastError] + 1;
                    }
                    else
                    {
                        errors[lastError] = 1;
                    }
                    errorCount += 1;
                }

            }

            Debug.WriteLine(sw.Elapsed.ToString());
            Debug.WriteLine(errorCount.ToString());
            Debug.WriteLine(lastError);
             Debug.WriteLine(lastErrorTimeout);

            // var invioLol = GetLolInvio(idRichiesta);
        }

        [TestMethod]
        public void Lol_RecuperaIdRichiesta()
        {
            try
            {
                string idRichiesta = RecuperaIdRichiesta();
                Assert.IsNotNull(idRichiesta);
            }

            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]

        public void Lol_Invio()
        {
            string idRichiesta = RecuperaIdRichiesta();
            Assert.IsNotNull(idRichiesta);

            var invioLol = GetLolInvio(idRichiesta);
            var proxy = GetProxy<LOLServiceSoap>(ambiente.LolUri);
            var fake = new OperationContextScope((IContextChannel)proxy);
            HttpRequestMessageProperty headers = GetHttpHeaders(ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            var result = proxy.Invio(idRichiesta, "CLIENTE", invioLol);

            Assert.AreEqual(result.CEResult.Type, "I");
        }

        private string RecuperaIdRichiesta()
        {
            try
            {
                var proxy = GetProxy<LOLServiceSoap>(ambiente.LolUri);
                var fake = new OperationContextScope((IContextChannel)proxy);
                HttpRequestMessageProperty headers = GetHttpHeaders(ambiente);
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;
                var result = proxy.RecuperaIdRichiesta();
                var idRichiesta = result.IDRichiesta;
                return idRichiesta;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
