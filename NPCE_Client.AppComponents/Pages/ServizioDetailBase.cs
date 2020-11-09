using Microsoft.AspNetCore.Components;
using NPCE_Client.AppComponents.Services;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Pages
{
    public class ServizioDetailBase :ComponentBase
    {

        public Servizio Servizio { get; set; }

        [Parameter]
        public string ServizioId { get; set; }

        [Inject]
        public IServiziDataService ServizioDataService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var servizio = await ServizioDataService.GetServizioDetailAsync(int.Parse(ServizioId));
            Servizio = servizio;
            StateHasChanged();
        }

    }
}
