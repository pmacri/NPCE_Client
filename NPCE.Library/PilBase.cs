using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.Common.Proxy;
using NPCE_Client.Model;
using PosteItaliane.OrderManagement.Schema.SchemaDefinition;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace NPCE.Library
{
    public abstract class PilBase : INPCEService
    {
        public Ambiente Ambiente { get; set; }
        public Servizio Servizio { get; set; }

        protected HttpRequestMessageProperty _httpHeaders;

        public CEHeader CEHeader { get; set; }

        protected CE CE { get; set; }

        public string IdRichiesta { get; set; }

        protected WsCEClient WsCEClient;

        public PilBase(Servizio servizio, Ambiente ambiente)
        {
            Ambiente = ambiente;
            Servizio = servizio;
            IdRichiesta = Guid.NewGuid().ToString();
            Init();
        }
        public PilBase(Servizio servizio, Ambiente ambiente, string idRichiesta) : this(servizio, ambiente)
        {
            IdRichiesta = idRichiesta;
            Init();
        }

        public abstract Task<NPCEResult> ConfermaAsync(string idRichiesta);


        public abstract Task<NPCEResult> InviaAsync();


        protected void Init()
        {
            CEHeader = new CEHeader
            {
                BillingCenter = Ambiente.billingcenter,
                CodiceFiscale = Ambiente.codicefiscale,
                ContractId = Ambiente.contractid,
                ContractType = Ambiente.contracttype,
                CostCenter = Ambiente.costcenter,
                Customer = Ambiente.customer,
                IdCRM = string.Empty,
                SenderSystem = Ambiente.sendersystem,
                UserId = Ambiente.smuser,
                PartitaIva = Ambiente.partitaiva,
                IDSender = string.Empty,
                UserType = Ambiente.usertype
            };

            CE = new CE();
            CE.Header = CEHeader;

            CE.Header.GUIDMessage = IdRichiesta;

            _httpHeaders = GetHttpHeaders(Ambiente);

            string uri = null;

            switch (Servizio.TipoServizioId)
            {
                case (int)TipoServizioId.POSTA1:
                case (int)TipoServizioId.POSTA4:
                    uri = Ambiente.LolUri;
                    break;

                case (int)TipoServizioId.ROL:
                    uri = Ambiente.RolUri;
                    break;

                default:
                    break;
            }

            var enpointAddress = new System.ServiceModel.EndpointAddress(uri);

            BasicHttpBinding myBinding = new BasicHttpBinding();
            myBinding.SendTimeout = TimeSpan.FromMinutes(3);
            myBinding.MaxReceivedMessageSize = 2147483647;

            WsCEClient = new WsCEClient(myBinding, enpointAddress);
        }

        protected virtual HttpRequestMessageProperty GetHttpHeaders(Ambiente ambiente)
        {
            var property = new HttpRequestMessageProperty();
            property.Headers.Add("customerid", ambiente.customerid);
            property.Headers.Add("smuser", ambiente.smuser);
            property.Headers.Add("costcenter", ambiente.costcenter);
            property.Headers.Add("billingcenter", ambiente.billingcenter);
            property.Headers.Add("idsender", ambiente.idsender);
            property.Headers.Add("contracttype", ambiente.contracttype);
            property.Headers.Add("sendersystem", ambiente.sendersystem);
            property.Headers.Add("contractid", ambiente.contractid);
            property.Headers.Add("customer", ambiente.customer);
            property.Headers.Add("usertype", ambiente.usertype);

            return property;
        }
        public abstract NPCEResult Invia();

        protected virtual OrderRequest GetPreConfirmRequest(string idRichiesta)
        {
            OrderRequest orderRequest = new OrderRequest();
            orderRequest.ServiceInstance = new OrderRequestServiceInstance[1];
            orderRequest.ForceOrderCreation = true;

            orderRequest.ServiceInstance[0] = new OrderRequestServiceInstance();
            orderRequest.ServiceInstance[0].GUIDMessage = idRichiesta;

            return orderRequest;

        }

        protected virtual ConfirmOrder GetConfirmRequest(string idOrdine, string typeDescription)
        {
            ConfirmOrder confirmOrder = new ConfirmOrder();
            confirmOrder.IdOrder = idOrdine;
            confirmOrder.PaymentType = new PaymentType
            {
                PostPayment = true,
                PostPaymentSpecified = true,
                TypeDescription = typeDescription,
                TypeId = "6"                 
            };

            return confirmOrder;
        }

    }
}
