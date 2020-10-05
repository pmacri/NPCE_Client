﻿using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.Common.Proxy;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace NPCE.Library
{
    public abstract class PilBase : INPCEService
    {
        public Ambiente Ambiente { get; set; }
        public Servizio Servizio { get ; set ; }

        protected HttpRequestMessageProperty _httpHeaders;

        public CEHeader CEHeader { get; set; }

        protected CE CE { get; set; }

        public string IdRichiesta { get; set; }

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

        public abstract Task ConfermaAsync();


        public abstract Task<NPCEResult> InviaAsync();
       
            
        protected void Init()
        {
           CEHeader= new CEHeader
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
       
    }
}