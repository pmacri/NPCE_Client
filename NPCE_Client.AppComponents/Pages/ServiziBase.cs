using Microsoft.AspNetCore.Components;
using NPCE_Client.AppComponents.Services;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Pages
{
    public class ServiziBase : ComponentBase
    {

        [Inject]
        public IServiziDataService ServiziDataService { get; set; }

        //protected AddAmbienteDialog AddAmbienteDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Servizi = await ServiziDataService.GetAllServiziAsync();
        }
        //public ServiziBase()
        //{
        //    Servizi = new List<Servizio>
        //    {
        //        new Servizio{
        //            Id= 1, TipoServizioId=(int) TipoServizioId.COL1, DataCreazione = DateTime.Now, StatoServizio= new StatoServizio{ Id = 1, Description="Da_Inviare"},
        //            Ambiente = new Ambiente{ Id=1, Description="Collaudo"}
        //        },
        //         new Servizio{
        //            Id= 2, TipoServizioId= (int)TipoServizioId.ROL, DataCreazione = DateTime.Now, StatoServizio= new StatoServizio{ Id = 1, Description="Da_Inviare"},
        //             Ambiente = new Ambiente{ Id=1, Description="Staging"}
        //        }
        //};
        //}
        public IEnumerable<Servizio> Servizi { get; set; }

    }
}
