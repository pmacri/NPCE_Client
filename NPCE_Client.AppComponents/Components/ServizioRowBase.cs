using Microsoft.AspNetCore.Components;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace NPCE_Client.AppComponents.Components
{
   public  class ServizioRowBase : ComponentBase
    {

        [Parameter]
        public Servizio Servizio  { get; set; }
    }
}
