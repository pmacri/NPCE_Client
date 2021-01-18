using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE_Client.Test;
using NPCE_Client.UnitTest.ServiceReference.LOL;
using NPCE_Client.UnitTest.ServiceReference.ROL;
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
    public class FE_ROL : TestBase
    {
        public FE_ROL() : base(Environment.Collaudo)
        {

        }

        [TestMethod]
        public void Rol_RecuperaIdRichiesta_Massive()
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
        public void Rol_RecuperaIdRichiesta()
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

        public void Rol_Invio()
        {
            string idRichiesta = RecuperaIdRichiesta();
            Assert.IsNotNull(idRichiesta);

            var invioRol = GetRolInvio(idRichiesta);
            var proxy = GetProxy<ROLServiceSoap>(ambiente.RolUri);
            var fake = new OperationContextScope((IContextChannel)proxy);
            HttpRequestMessageProperty headers = GetHttpHeaders(ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            var result = proxy.Invio(idRichiesta, "CLIENTE", invioRol);

            Assert.AreEqual(result.CEResult.Type, "I");
        }

        [TestMethod]

        public void Invio_PreConferma_PosteIt()
        {
            string idRichiesta = "B0DDB22A-E21B-4A06-961B-E945176D2617";
            string guidUtente = "ROL202101000000720";
            var userId = "salvatore.fratoni";
            var canale = "WEB";

            var listRichieste = new List<ServiceReference.ROL.Richiesta>();
            listRichieste.Add(new ServiceReference.ROL.Richiesta() { GuidUtente = guidUtente, IDRichiesta = idRichiesta });
            ServiceReference.ROL.PreConfermaRequest request = new ServiceReference.ROL.PreConfermaRequest { Richieste = listRichieste.ToArray(), autoConferma = true };
            var proxy = GetProxy<ROLServiceSoap>(ambiente.RolUri);
            var fake = new OperationContextScope((IContextChannel)proxy);
            var headers = GetHttpHeaders(ambiente);
            headers.Headers["smuser"] = userId;
            headers.Headers["sendersystem"] = canale;
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;
            var preConfermaResult = proxy.PreConferma(request);
            Assert.AreEqual(preConfermaResult.PreConfermaResult.CEResult.Type, "I");
        }

        private string RecuperaIdRichiesta()
        {
            try
            {
                var proxy = GetProxy<ROLServiceSoap>(ambiente.RolUri);
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
