using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitResponse;
using NPCE.Library.ServiceReference.LOL;
using System;
using System.Collections.Generic;
using System.Text;

namespace NPCE.Library
{
    public static class Extensions
    {

        public static NPCEResult CreateResult(this InvioResult result)
        {
            return new NPCEResult
            {
                Code = result.CEResult.Code,
                Description = result.CEResult.Description,
                IdRichiesta = result.IDRichiesta
            };
        }

        public static NPCEResult CreateResult(this LetteraResponse result, CE ce)
        {
            return new NPCEResult
            {
                Code = ce.Result.ResType.ToString(),
                Description = ce.Result.Description,
                IdRichiesta = result.IdRichiesta
            };
        }
    }
}
