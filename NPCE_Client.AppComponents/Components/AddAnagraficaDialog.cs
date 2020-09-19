using Microsoft.AspNetCore.Components;
using NPCE_Client.AppComponents.Services;
using NPCE_Client.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_Client.AppComponents.Components
{
    public partial class AddAnagraficaDialog
    {
        public Anagrafica Anagrafica { get; set; } = new Anagrafica { Stato="ITALIA" };

        [Inject]
        public IAnagraficheDataService AnagraficaDataService { get; set; }

        [Parameter]
        public EventCallback<bool> CloseEventCallback { get; set; }

        public bool ShowDialog { get; set; }

        public void Show()
        {
            ShowDialog = true;
            StateHasChanged();
        }

        public void Close()
        {
            ShowDialog = false;
            StateHasChanged();
        }

        protected async Task HandleValidSubmit()
        {
            await AnagraficaDataService.AddAnagrafica(Anagrafica);
            ShowDialog = false;

            await CloseEventCallback.InvokeAsync(true);
            StateHasChanged();
        }
    }
}
