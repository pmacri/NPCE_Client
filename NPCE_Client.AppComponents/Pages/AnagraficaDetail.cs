using Microsoft.AspNetCore.Components;
using NPCE_Client.AppComponents.Services;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Pages
{
    public partial class AnagraficaDetail
    {
        public Anagrafica Anagrafica { get; set; }

        [Inject]
        public IAnagraficheDataService AnagraficheDataService { get; set; }

        [Parameter]
        public string AnagraficaId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Anagrafica = await AnagraficheDataService.GetAnagraficaDetail((int.Parse(AnagraficaId)));

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
