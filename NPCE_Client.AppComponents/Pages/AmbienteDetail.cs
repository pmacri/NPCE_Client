using Microsoft.AspNetCore.Components;
using NPCE_Client.AppComponents.Services;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Pages
{
    public partial class AmbienteDetail: ComponentBase
    {
        public Ambiente Ambiente { get; set; }

        [Inject]
        public IAmbientiDataService AmbientiDataService { get; set; }

        [Parameter]
        public string AmbienteId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Ambiente = await AmbientiDataService.GetAmbienteDetail((int.Parse(AmbienteId)));

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
