using NPCE.ServiceReference;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace NPCE.Library
{
    public abstract class ServiceBase : INPCEService
    {

        protected  LOLServiceSoap _proxy;

        protected  HttpRequestMessageProperty _httpHeaders;

        public ServiceBase(Servizio servizio, Ambiente ambiente)
        {
            Ambiente = ambiente;
            Servizio = servizio;
            Init();
        }
        public ServiceBase(Servizio servizio, Ambiente ambiente, string idRichiesta): this(servizio, ambiente)
        {
            IdRichiesta = IdRichiesta;
            Init();
        }

        protected void Init()
        {
            _proxy = GetProxy<LOLServiceSoap>(Ambiente.LolUri, Ambiente.Username, Ambiente.Password);
            _httpHeaders = GetHttpHeaders(Ambiente);
        }

        public Ambiente Ambiente { get ; set ; }
        public Servizio Servizio { get ; set ; }

        public string IdRichiesta { get; set; }

        public abstract Task ConfermaAsync();
        public abstract Task InviaAsync();

        protected T GetProxy<T>(string endpointAddress, string username, string password)
        {
            BasicHttpBinding myBinding = new BasicHttpBinding();
            myBinding.SendTimeout = TimeSpan.FromMinutes(3);
            myBinding.MaxReceivedMessageSize = 2147483647;
            EndpointAddress myEndpoint = new EndpointAddress(endpointAddress);

            ChannelFactory<T> myChannelFactory = new ChannelFactory<T>(myBinding, myEndpoint);

            myChannelFactory.Credentials.UserName.UserName = username;
            myChannelFactory.Credentials.UserName.Password = password;
            // Create a channel
            T wcfClient = myChannelFactory.CreateChannel();

            return wcfClient;
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

        protected string GetFronteRetroDescription(bool fronteRetro)
        {
            if (fronteRetro) return "true"; else return "false";
        }

        protected string GetMD5(NPCE_Client.Model.Documento documento)
        {
            using (System.Security.Cryptography.MD5CryptoServiceProvider cryptoService = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                byte[] Ret = cryptoService.ComputeHash(documento.Content);
                return BitConverter.ToString(Ret).Replace("-", "");
            }
        }
    }
}
